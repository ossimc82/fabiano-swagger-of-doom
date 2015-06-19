#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using db.data;
using Ionic.Zlib;
using MySql.Data.MySqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace db
{
    public partial class Database : IDisposable
    {
        private static readonly List<string> emails = new List<string>();
        private static readonly string[] Names =
        {
            "Darq", "Deyst", "Drac", "Drol",
            "Eango", "Eashy", "Eati", "Eendi", "Ehoni",
            "Gharr", "Iatho", "Iawa", "Idrae", "Iri", "Issz", "Itani",
            "Laen", "Lauk", "Lorz",
            "Oalei", "Odaru", "Oeti", "Orothi", "Oshyu",
            "Queq", "Radph", "Rayr", "Ril", "Rilr", "Risrr",
            "Saylt", "Scheev", "Sek", "Serl", "Seus",
            "Tal", "Tiar", "Uoro", "Urake", "Utanu",
            "Vorck", "Vorv", "Yangu", "Yimi", "Zhiar"
        };

        private static string _host, _databaseName, _user, _password;
        private readonly MySqlConnection _con;
        public MySqlConnection Connection { get { return _con; } }

        public Database(string host, string database, string user, string password)
        {
            _host = host;
            _databaseName = database;
            _user = user;
            _password = password;

            _con = new MySqlConnection(
                String.Format("Server={0};Database={1};uid={2};password={3};convert zero datetime=True;",
                    host, database ?? "rotmgprod", user ?? "root", password ?? ""));
            _con.Open();

            if (File.Exists("UnlockedAccounts.txt"))
            {
                using (StreamReader rdr = new StreamReader("UnlockedAccounts.txt"))
                {
                    string s;
                    do
                    {
                        s = rdr.ReadLine();
                        if (s != null && !s.StartsWith("#"))
                            if(!emails.Contains(s))
                                emails.Add(s);
                    } while (s != null);
                }
            }
        }

        public Database()
        {
            _con = new MySqlConnection(
                String.Format("Server={0};Database={1};uid={2};password={3};convert zero datetime=True;",
                    _host, _databaseName, _user, _password));
            _con.Open();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_con.State == System.Data.ConnectionState.Open)
                {
                    _con.Close();
                    _con.Dispose();
                }
            }
            //GC.SuppressFinalize(this);//Updated
        }

        private static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public MySqlCommand CreateQuery()
        {
            return _con.CreateCommand();
        }

        public static int DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (int)(dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        public List<NewsItem> GetNews(XmlData data, Account acc)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT icon, title, text, link, date FROM news ORDER BY date LIMIT 10;";
            List<NewsItem> ret = new List<NewsItem>();
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                    ret.Add(new NewsItem
                    {
                        Icon = rdr.GetString("icon"),
                        Title = rdr.GetString("title"),
                        TagLine = rdr.GetString("text"),
                        Link = rdr.GetString("link"),
                        Date = DateTimeToUnixTimestamp(rdr.GetDateTime("date")),
                    });
            }
            if (acc != null)
            {
                cmd.CommandText = @"SELECT charId, characters.charType, level, death.totalFame, time
FROM characters, death
WHERE dead = TRUE AND
characters.accId=@accId AND death.accId=@accId
AND characters.charId=death.chrId;";
                cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                        ret.Add(new NewsItem
                        {
                            Icon = "fame",
                            Title = string.Format("Your {0} died at level {1}",
                                data.ObjectTypeToId[(ushort)rdr.GetInt32("charType")],
                                rdr.GetInt32("level")),
                            TagLine = string.Format("You earned {0} glorious Fame",
                                rdr.GetInt32("totalFame")),
                            Link = "fame:" + rdr.GetInt32("charId"),
                            Date = DateTimeToUnixTimestamp(rdr.GetDateTime("time")),
                        });
                }
            }
            ret.Sort((a, b) => -Comparer<int>.Default.Compare(a.Date, b.Date));
            return ret.Take(10).ToList();
        }

        public static Account CreateGuestAccount(string uuid)
        {
            return new Account
            {
                Name = Names[(uint)uuid.GetHashCode() % Names.Length],
                AccountId = "0",
                Admin = false,
                Banned = false,
                Rank = 0,
                BeginnerPackageTimeLeft = 0,
                Converted = false,
                Credits = 0,
                PetYardType = 1,
                Guild = new Guild
                {
                    Name = "",
                    Id = 0,
                    Rank = 0
                },
                NameChosen = false,
                NextCharSlotPrice = 600,
                VerifiedEmail = false,
                Stats = new Stats
                {
                    BestCharFame = 0,
                    ClassStates = new List<ClassStats>(),
                    Fame = 0,
                    TotalFame = 0
                },
                Vault = new VaultData
                {
                    Chests = new List<VaultChest>()
                },
                Gifts = new List<int>(),
                OwnedSkins = new List<int>(),
                IsGuestAccount = true
            };
        }

        public Account Verify(string uuid, string password, XmlData data)
        {
            var cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM accounts WHERE uuid=@uuid AND password=SHA1(@password);";
            cmd.Parameters.AddWithValue("@uuid", uuid);
            cmd.Parameters.AddWithValue("@password", password);
            string accId = String.Empty;
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) return null;
                rdr.Read();
                accId = rdr.GetString("id");
            }
            return GetAccount(accId, data);
        }

        public QuestItem GetDailyQuest(string accId, XmlData data)
        {
            var cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM dailyQuests WHERE accId=@id;";
            cmd.Parameters.AddWithValue("@id", accId);
            QuestItem quest = null;
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    int[] goals = Utils.FromCommaSepString32(rdr.GetString("goals"));
                    if (goals.Length < DailyQuestConstants.QuestsPerDay) break;
                    quest = new QuestItem
                    {
                        Description = DailyQuestConstants.GetDescriptionByTier(rdr.GetInt32("tier")),
                        Image = DailyQuestConstants.GetImageByTier(rdr.GetInt32("tier")),
                        Goal = goals[rdr.GetInt32("tier") < 0 ? 0 : rdr.GetInt32("tier") - 1].ToString(),
                        Tier = rdr.GetInt32("tier"),
                        Time = rdr.GetDateTime("time")
                    };
                }
            }

            DateTime converted;
            if (TimeZoneInfo.Local.Id != TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time").Id)
                converted = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            else
                converted = DateTime.Now;

            var fixedTime = new DateTime(converted.Year, converted.Month, converted.Day, 17, 0, 0, 0, DateTimeKind.Unspecified);

            if (quest == null || ((converted.Hour >= 17 && converted.Day-1 == quest.Time.Day) || quest.Time.AddDays(1) <= converted))
                    quest = GenerateDailyQuest(accId, data, fixedTime);
            return quest;
        }

        public bool IsMuted(string accId)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT muted FROM accounts WHERE id=@accId";
            cmd.Parameters.AddWithValue("@accId", accId);
            if ((int)cmd.ExecuteNonQuery() == 1) return true;
            return false;
        }

        public void MuteAccount(string accId)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "UPDATE accounts SET muted=1 WHERE id=@accId";
            cmd.Parameters.AddWithValue("@accId", accId);
            cmd.ExecuteNonQuery();

        }
        public void UnmuteAccount(string accId)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "UPDATE accounts SET muted=0 WHERE id=@accId";
            cmd.Parameters.AddWithValue("@accId", accId);
            cmd.ExecuteNonQuery();
        }

        public Account Register(string uuid, string password, bool isGuest, XmlData data)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT COUNT(id) FROM accounts WHERE uuid=@uuid;";
            cmd.Parameters.AddWithValue("@uuid", uuid);
            if ((int)(long)cmd.ExecuteScalar() > 0) return null;

            cmd = CreateQuery();
            cmd.CommandText =
                "INSERT INTO accounts(uuid, password, name, rank, namechosen, verified, guild, guildRank, guildFame, vaultCount, maxCharSlot, regTime, guest, banned, locked, ignored, gifts, isAgeVerified, authToken) VALUES(@uuid, SHA1(@password), @randomName, @rank, 0, 0, 0, 0, 0, 1, 2, @regTime, @guest, 0, @empty, @empty, @empty, 1, @authToken);";
            cmd.Parameters.AddWithValue("@uuid", uuid);
            cmd.Parameters.AddWithValue("@randomName", Names[new Random().Next(0, Names.Length)]);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@name", Names[(uint)uuid.GetHashCode() % Names.Length]);
            cmd.Parameters.AddWithValue("@guest", isGuest);
            cmd.Parameters.AddWithValue("@regTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@authToken", GenerateRandomString(128));
            cmd.Parameters.AddWithValue("@empty", "");

            if (emails.Contains(uuid))
                cmd.Parameters.AddWithValue("@rank", 1);
            else
                cmd.Parameters.AddWithValue("@rank", 0);

            var success = cmd.ExecuteNonQuery() > 0;
            var accId = cmd.LastInsertedId;

            if (success)
            {
                cmd = CreateQuery();
                cmd.CommandText =
                    "INSERT INTO stats(accId, fame, totalFame, credits, totalCredits) VALUES(@accId, 1000, 1000, 20000, 20000);";
                cmd.Parameters.AddWithValue("@accId", accId);
                cmd.ExecuteNonQuery();

                cmd = CreateQuery();
                cmd.CommandText = "INSERT INTO vaults(accId, items) VALUES(@accId, '-1, -1, -1, -1, -1, -1, -1, -1');";
                cmd.Parameters.AddWithValue("@accId", accId);
                cmd.ExecuteNonQuery();
            }
            return Verify(uuid, password, data);
        }

        public QuestItem GenerateDailyQuest(string accId, XmlData data, DateTime pst5pm)
        {
            if (accId == "0") return null;
            Random rand = new Random();
            List<int> items = new List<int>(DailyQuestConstants.QuestsPerDay);

            List<Item> candidates = data.Items.Where(_ =>
                _.Value.SlotType == 1 || _.Value.SlotType == 2 ||
                _.Value.SlotType == 3 || _.Value.SlotType == 6 ||
                _.Value.SlotType == 7 || _.Value.SlotType == 8 ||
                _.Value.SlotType == 14 || _.Value.SlotType == 17 ||
                _.Value.SlotType == 24).Where(_ =>
                _.Value.Tier == 6 || _.Value.Tier == 7 ||
                _.Value.Tier == 8 || _.Value.Tier == 9 ||
                _.Value.Tier == 10).Select(_ => _.Value).ToList();
            candidates.AddRange(data.Items.Where(_ => 
                _.Value.SlotType == 4 || _.Value.SlotType == 5 ||
                _.Value.SlotType == 11 || _.Value.SlotType == 12 ||
                _.Value.SlotType == 13 || _.Value.SlotType == 15 ||
                _.Value.SlotType == 16 ||  _.Value.SlotType == 18 ||
                _.Value.SlotType == 19 || _.Value.SlotType == 20 ||
                _.Value.SlotType == 21 || _.Value.SlotType == 22 ||
                _.Value.SlotType == 23 || _.Value.SlotType == 25)
                .Where(_ => _.Value.Tier == 3 || _.Value.Tier == 4).Select(_ => _.Value));

            do
            {
                int r = -1;
                int item = candidates[(r = rand.Next(candidates.Count))].ObjectType;
                while (items.Contains(item)) item = candidates[(r = rand.Next(candidates.Count))].ObjectType;
                items.Add(item);
            }
            while(items.Count < DailyQuestConstants.QuestsPerDay);

            var cmd = CreateQuery();
            cmd.CommandText = "INSERT INTO dailyQuests(accId, goals, tier, time) VALUES(@accId, @goals, @tier, @time) ON DUPLICATE KEY UPDATE accId=@accId, goals=@goals, tier=@tier, time=@time;";
            cmd.Parameters.AddWithValue("@tier", 1);
            cmd.Parameters.AddWithValue("@accId", accId);
            cmd.Parameters.AddWithValue("@goals", Utils.GetCommaSepString(items.ToArray()));
            cmd.Parameters.AddWithValue("@time", pst5pm.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
            return GetDailyQuest(accId, data);
        }

        public static string GenerateRandomString(int size, Random rand=null)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var builder = new StringBuilder();
            var random = rand ?? new Random();
            char ch;
            for (var i = 0; i < size; i++)
            {
                ch = chars[random.Next(0, chars.Length - 1)];
                builder.Append(ch);
            }
            return builder.ToString();
        }

        public bool HasUuid(string uuid)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT COUNT(id) FROM accounts WHERE uuid=@uuid;";
            cmd.Parameters.AddWithValue("@uuid", uuid);
            return (int)(long)cmd.ExecuteScalar() > 0;
        }

        public Account GetAccountByName(string name, XmlData data)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText =
                "SELECT * FROM accounts WHERE name=@name;";
            cmd.Parameters.AddWithValue("@name", name);
            string accId = String.Empty;
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) return null;
                rdr.Read();
                accId = rdr.GetString("id");
            }
            return GetAccount(accId, data);
        }

        public Account GetAccount(string accId, XmlData data, string uuid=null, string password=null)
        {
            if (String.IsNullOrWhiteSpace(accId)) return CreateGuestAccount(accId ?? String.Empty);
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText =
                "SELECT * FROM accounts WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", accId);
            Account ret;
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) return null;
                rdr.Read();
                ret = new Account
                {
                    Name = rdr.GetString(UppercaseFirst("name")),
                    AccountId = rdr.GetString("id"),
                    Admin = rdr.GetInt32("rank") >= 2,
                    Email = uuid ?? rdr.GetString("uuid"),
                    Password = password ?? rdr.GetString("password"),
                    VisibleMuledump = rdr.GetInt32("publicMuledump") == 1,
                    Rank = rdr.GetInt32("rank"),
                    Banned = rdr.GetBoolean("banned"),
                    BeginnerPackageTimeLeft = 0,
                    PetYardType = rdr.GetInt32("petYardType"),
                    Converted = false,
                    IsProdAccount = rdr.GetInt32("prodAcc") == 1,
                    Guild = new Guild
                    {
                        Id = rdr.GetInt64("guild"),
                        Rank = rdr.GetInt32("guildRank"),
                        Fame = rdr.GetInt32("guildFame")
                    },
                    NameChosen = rdr.GetBoolean("namechosen"),
                    NextCharSlotPrice = rdr.GetInt32("maxCharSlot") == 1 ? 600 : rdr.GetInt32("maxCharSlot") == 2 ? 800 : 1000,
                    VerifiedEmail = rdr.GetBoolean("verified"),
                    Locked = rdr.GetString("locked").Split(',').ToList(),
                    Ignored = rdr.GetString("ignored").Split(',').ToList(),
                    _Gifts = rdr.GetString("gifts"),
                    IsAgeVerified = rdr.GetString("isAgeVerified").ToLower() == "true" ? 1 : 0,
                    AuthToken = rdr.GetString("authToken"),
                    NotAcceptedNewTos = rdr.GetInt32("acceptedNewTos") == 1 ? null : String.Empty,
                    OwnedSkins = Utils.FromCommaSepString32(rdr.GetString("ownedSkins")).ToList()
                };
            }
            ReadStats(ret);
            ReadGiftCodes(ret);
            ret.Guild.Name = GetGuildName(ret.Guild.Id);
            ret.DailyQuest = GetDailyQuest(ret.AccountId, data);
            return ret;
        }

        public int UpdateCredit(Account acc, int amount)
        {
            MySqlCommand cmd = CreateQuery();
            if (amount > 0)
            {
                cmd.CommandText = "UPDATE stats SET totalCredits = totalCredits + @amount WHERE accId=@accId;";
                cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.ExecuteNonQuery();
                cmd = CreateQuery();
            }
            cmd.CommandText = @"UPDATE stats SET credits = credits + (@amount) WHERE accId=@accId;
SELECT credits FROM stats WHERE accId=@accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            cmd.Parameters.AddWithValue("@amount", amount);
            return (int)cmd.ExecuteScalar();
        }

        public int UpdateFame(Account acc, int amount)
        {
            MySqlCommand cmd = CreateQuery();
            if (amount > 0)
            {
                cmd.CommandText = "UPDATE stats SET totalFame = totalFame + @amount WHERE accId=@accId;";
                cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.ExecuteNonQuery();
                cmd = CreateQuery();
            }
            cmd.CommandText = @"UPDATE stats SET fame = fame + (@amount) WHERE accId=@accId;
SELECT fame FROM stats WHERE accId=@accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            cmd.Parameters.AddWithValue("@amount", amount);
            return (int)cmd.ExecuteScalar();
        }

        public int UpdateFortuneToken(Account acc, int amount)
        {
            MySqlCommand cmd = CreateQuery();
            if (amount > 0)
            {
                cmd.CommandText = "UPDATE stats SET totalFortuneTokens = totalFortuneTokens + @amount WHERE accId=@accId;";
                cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.ExecuteNonQuery();
                cmd = CreateQuery();
            }
            cmd.CommandText = @"UPDATE stats SET fortuneTokens = fortuneTokens + (@amount) WHERE accId=@accId;
SELECT credits FROM stats WHERE accId=@accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            cmd.Parameters.AddWithValue("@amount", amount);
            return (int)cmd.ExecuteScalar();
        }

        public void ReadStats(Account acc)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM stats WHERE accId=@accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    rdr.Read();
                    acc.Credits = rdr.GetInt32("credits");
                    acc.FortuneTokens = rdr.GetInt32("fortuneTokens");
                    acc.Stats = new Stats
                    {
                        Fame = rdr.GetInt32("fame"),
                        TotalFame = rdr.GetInt32("totalFame")
                    };
                }
                else
                {
                    acc.Credits = 0;
                    acc.Stats = new Stats
                    {
                        Fame = 0,
                        TotalFame = 0,
                        BestCharFame = 0,
                        ClassStates = new List<ClassStats>()
                    };
                }
            }

            acc.Stats.ClassStates = ReadClassStates(acc);
            if (acc.Stats.ClassStates.Count > 0)
                acc.Stats.BestCharFame = acc.Stats.ClassStates.Max(_ => _.BestFame);
            acc.Vault = ReadVault(acc);
        }

        public void ReadGiftCodes(Account acc)
        {
            var cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM giftcodes WHERE accId=@accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            acc.GiftCodes = new List<string>();
            using (var rdr = cmd.ExecuteReader())
                while (rdr.Read())
                    acc.GiftCodes.Add(rdr.GetString("code"));
        }

        public List<ClassStats> ReadClassStates(Account acc)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT objType, bestLv, bestFame FROM classstats WHERE accId=@accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            List<ClassStats> ret = new List<ClassStats>();
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                    ret.Add(new ClassStats
                    {
                        ObjectType = Utils.To2Hex((short)rdr.GetInt32("objType")),
                        BestFame = rdr.GetInt32("bestFame"),
                        BestLevel = rdr.GetInt32("bestLv")
                    });
            }
            return ret;
        }

        public VaultData ReadVault(Account acc)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT chestId, items FROM vaults WHERE accId=@accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    VaultData ret = new VaultData { Chests = new List<VaultChest>() };
                    while (rdr.Read())
                    {
                        ret.Chests.Add(new VaultChest
                        {
                            ChestId = rdr.GetInt32("chestId"),
                            _Items = rdr.GetString("items")
                        });
                    }
                    return ret;
                }
                return new VaultData
                {
                    Chests = new List<VaultChest>()
                };
            }
        }

        public void SaveChest(string accId, VaultChest chest)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "UPDATE vaults SET items=@items WHERE accId=@accId AND chestId=@chestId;";
            cmd.Parameters.AddWithValue("@accId", accId);
            cmd.Parameters.AddWithValue("@chestId", chest.ChestId);
            cmd.Parameters.AddWithValue("@items", chest._Items);
            cmd.ExecuteNonQuery();
        }

        public VaultChest CreateChest(Account acc)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = @"INSERT INTO vaults(accId, items) VALUES(@accId, '-1, -1, -1, -1, -1, -1, -1, -1');
SELECT MAX(chestId) FROM vaults WHERE accId = @accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            return new VaultChest
            {
                ChestId = (int)cmd.ExecuteScalar(),
                _Items = "-1, -1, -1, -1, -1, -1, -1, -1"
            };
        }

        public void GetCharData(Account acc, Chars chrs)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT IFNULL(MAX(charId), 0) + 1 FROM characters WHERE accId=@accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            chrs.NextCharId = (int)(long)cmd.ExecuteScalar();

            cmd = CreateQuery();
            cmd.CommandText = "SELECT maxCharSlot FROM accounts WHERE id=@accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            chrs.MaxNumChars = (int)cmd.ExecuteScalar();
        }

        public int GetNextCharId(Account acc)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT IFNULL(MAX(charId), 0) + 1 FROM characters WHERE accId=@accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            int ret = (int)(long)cmd.ExecuteScalar();
            return ret;
        }

        public void LoadCharacters(Account acc, Chars chrs)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM characters WHERE accId=@accId AND dead = FALSE;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    int[] stats = Utils.FromCommaSepString32(rdr.GetString("stats"));
                    chrs.Characters.Add(new Char
                    {
                        ObjectType = (ushort)rdr.GetInt32("charType"),
                        CharacterId = rdr.GetInt32("charId"),
                        Level = rdr.GetInt32("level"),
                        Exp = rdr.GetInt32("exp"),
                        CurrentFame = rdr.GetInt32("fame"),
                        _Equipment = rdr.GetString("items"),
                        MaxHitPoints = stats[0],
                        HitPoints = rdr.GetInt32("hp"),
                        MaxMagicPoints = stats[1],
                        MagicPoints = rdr.GetInt32("mp"),
                        Attack = stats[2],
                        Defense = stats[3],
                        Speed = stats[4],
                        Dexterity = stats[7],
                        HpRegen = stats[5],
                        MpRegen = stats[6],
                        HealthStackCount = rdr.GetInt32("hpPotions"),
                        MagicStackCount = rdr.GetInt32("mpPotions"),
                        HasBackpack = rdr.GetInt32("hasBackpack"),
                        Tex1 = rdr.GetInt32("tex1"),
                        Tex2 = rdr.GetInt32("tex2"),
                        Dead = false,
                        PCStats = rdr.GetString("fameStats"),
                        Pet = GetPet(rdr.GetInt32("petId"), acc),
                        Skin = rdr.GetInt32("skin")
                    });
                }
            }
            foreach (Char i in chrs.Characters)
            {
                if (i.HasBackpack == 1)
                    i._Equipment += ", " + Utils.GetCommaSepString(GetBackpack(i, acc));
            }
        }

        public PetItem GetPet(int petId, Account acc)
        {
            using (Database db = new Database())
            {
                MySqlCommand cmd = db.CreateQuery();
                cmd.CommandText = "SELECT * FROM pets WHERE petId=@petId AND accId=@accId";
                cmd.Parameters.AddWithValue("@petId", petId);
                cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        return new PetItem
                        {
                            Abilities = GetPetAbilities(rdr),
                            Rarity = rdr.GetInt32("rarity"),
                            MaxAbilityPower = rdr.GetInt32("maxLevel"),
                            InstanceId = petId,
                            SkinName = rdr.GetString("skinName"),
                            Skin = rdr.GetInt32("skin"),
                            Type = rdr.GetInt32("objType")
                        };
                    }
                }
            }
            return null;
        }
        public List<AbilityItem> GetPetAbilities(MySqlDataReader rdr)
        {
            List<AbilityItem> ret = new List<AbilityItem>();
            int lenght = rdr.GetString("levels").Split(',').Length;
            for (int i = 0; i < lenght; i++)
            {
                ret.Add(new AbilityItem
                {
                    Points = Utils.FromString(rdr.GetString("xp").Split(',')[i]),
                    Power = Utils.FromString(rdr.GetString("levels").Split(',')[i]),
                    Type = Utils.FromString(rdr.GetString("abilities").Split(',')[i])
                });
            }
            return ret;
        }

        public static Char CreateCharacter(XmlData data, ushort type, int chrId)
        {
            XElement cls = data.ObjectTypeToElement[type];
            if (cls == null) return null;
            Char ret = new Char
            {
                ObjectType = type,
                CharacterId = chrId,
                Level = 1,
                Exp = 0,
                CurrentFame = 0,
                HasBackpack = 0,
                _Equipment = cls.Element("Equipment").Value.Replace("0xa22", "-1"),
                MaxHitPoints = int.Parse(cls.Element("MaxHitPoints").Value),
                HitPoints = int.Parse(cls.Element("MaxHitPoints").Value),
                MaxMagicPoints = int.Parse(cls.Element("MaxMagicPoints").Value),
                MagicPoints = int.Parse(cls.Element("MaxMagicPoints").Value),
                Attack = int.Parse(cls.Element("Attack").Value),
                Defense = int.Parse(cls.Element("Defense").Value),
                Speed = int.Parse(cls.Element("Speed").Value),
                Dexterity = int.Parse(cls.Element("Dexterity").Value),
                HpRegen = int.Parse(cls.Element("HpRegen").Value),
                MpRegen = int.Parse(cls.Element("MpRegen").Value),
                HealthStackCount = 1,
                Tex1 = 0,
                Tex2 = 0,
                Dead = false,
                PCStats = "",
                FameStats = new FameStats(),
                Pet = null,
                Skin = 0
            };
            return ret;
        }

        public string GetEmail(string uuid, string password)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT email WHERE uuid=@uuid AND password=@pass LIMIT 1";
            cmd.Parameters.AddWithValue("@uuid", uuid);
            cmd.Parameters.AddWithValue("@pass", password);
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) return "";
                rdr.Read();
                return rdr.GetString("email");
            }
        }

        public void InsertEmail(string uuid, string password, string hash)
        {
            Console.WriteLine("Adding Email!");

            MySqlCommand cmd = CreateQuery();
            cmd.CommandText =
                "INSERT INTO emails(accId, name, email, accessKey) VALUES(@accId, @name, @email, @accessKey);";
            cmd.Parameters.AddWithValue("@accId", GetAccInfo(uuid, 1));
            cmd.Parameters.AddWithValue("@name", GetAccInfo(uuid, 2));
            cmd.Parameters.AddWithValue("@email", GetAccInfo(uuid, 3));
            cmd.Parameters.AddWithValue("@accessKey", hash);

            cmd.ExecuteNonQuery();
        }

        public string GetAccInfo(string guid, int type)
        {
            string info = "";
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT id, name, email FROM accounts WHERE uuid=@uuid LIMIT 1";
            cmd.Parameters.AddWithValue("@uuid", guid);
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) return "";
                rdr.Read();
                if (type == 1)
                    info = rdr.GetInt32("id").ToString();
                if (type == 2)
                    info = rdr.GetString("name");
                if (type == 3)
                    info = rdr.GetString("email");
                return info;
            }
        }

        public bool HasEmail(string email)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT COUNT(id) FROM accounts WHERE email=@email;";
            cmd.Parameters.AddWithValue("@email", email);
            return (int)(long)cmd.ExecuteScalar() > 0;
        }

        public Char LoadCharacter(Account acc, int charId)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM characters WHERE accId=@accId AND charId=@charId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            cmd.Parameters.AddWithValue("@charId", charId);
            Char ret;
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) return null;
                rdr.Read();
                int[] stats = Utils.FromCommaSepString32(rdr.GetString("stats"));
                ret = new Char
                {
                    ObjectType = (ushort)rdr.GetInt32("charType"),
                    CharacterId = rdr.GetInt32("charId"),
                    Exp = rdr.GetInt32("exp"),
                    Level = rdr.GetInt32("level"),
                    LDTimer = rdr.GetInt32("ldTimer"),
                    LTTimer = rdr.GetInt32("ltTimer"),
                    XpTimer = rdr.GetInt32("xpBoosterTime"),
                    CurrentFame = rdr.GetInt32("fame"),
                    _Equipment = rdr.GetString("items"),
                    HasBackpack = rdr.GetInt32("hasBackpack"),
                    MaxHitPoints = stats[0],
                    HitPoints = rdr.GetInt32("hp"),
                    MaxMagicPoints = stats[1],
                    MagicPoints = rdr.GetInt32("mp"),
                    Attack = stats[2],
                    Defense = stats[3],
                    Speed = stats[4],
                    HpRegen = stats[5],
                    MpRegen = stats[6],
                    Dexterity = stats[7],
                    HealthStackCount = rdr.GetInt32("hpPotions"),
                    MagicStackCount = rdr.GetInt32("mpPotions"),
                    Tex1 = rdr.GetInt32("tex1"),
                    Tex2 = rdr.GetInt32("tex2"),
                    Dead = rdr.GetBoolean("dead"),
                    Pet = GetPet(rdr.GetInt32("petId"), acc),
                    PCStats = rdr.GetString("fameStats"),
                    FameStats = new FameStats(),
                    Skin = rdr.GetInt32("skin")
                };
                if (!string.IsNullOrEmpty(ret.PCStats) && ret.PCStats != "FameStats")
                    try
                    {
                        ret.FameStats.Read(
                            Convert.FromBase64String(ret.PCStats.Replace('-', '+').Replace('_', '/')));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[" + DateTime.Now.ToString("h:mm:ss tt") + "] " + e);
                    }
            }
            ret.XpBoosted = ret.XpTimer == 0 ? false : true;
            if (ret.HasBackpack == 1)
                ret.Backpack = GetBackpack(ret, acc);
            return ret;
        }

        public void SaveCharacter(Account acc, Char chr)
        {
            if (acc == null || chr == null) return;
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = @"UPDATE characters SET 
level=@level, 
exp=@exp, 
fame=@fame, 
items=@items,
hpPotions=@hpPots,
mpPotions=@mpPots,
stats=@stats, 
hp=@hp, 
mp=@mp, 
tex1=@tex1, 
tex2=@tex2, 
petId=@pet,
fameStats=@fameStats,
hasBackpack=@hasBackpack,
skin=@skin,
xpBoosterTime=@xpTime,
ldTimer=@lootDropTime,
ltTimer=@lootTierTime
WHERE accId=@accId AND charId=@charId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            cmd.Parameters.AddWithValue("@charId", chr.CharacterId);

            cmd.Parameters.AddWithValue("@level", chr.Level);
            cmd.Parameters.AddWithValue("@exp", chr.Exp);
            cmd.Parameters.AddWithValue("@fame", chr.CurrentFame);
            cmd.Parameters.AddWithValue("@hpPots", chr.HealthStackCount);
            cmd.Parameters.AddWithValue("@mpPots", chr.MagicStackCount);
            cmd.Parameters.AddWithValue("@items", Utils.GetCommaSepString(chr.EquipSlots()));
            cmd.Parameters.AddWithValue("@stats", Utils.GetCommaSepString(new[]
            {
                chr.MaxHitPoints,
                chr.MaxMagicPoints,
                chr.Attack,
                chr.Defense,
                chr.Speed,
                chr.HpRegen,
                chr.MpRegen,
                chr.Dexterity
            }));
            cmd.Parameters.AddWithValue("@hp", chr.HitPoints);
            cmd.Parameters.AddWithValue("@mp", chr.MagicPoints);
            cmd.Parameters.AddWithValue("@hasBackpack", chr.HasBackpack);
            cmd.Parameters.AddWithValue("@tex1", chr.Tex1);
            cmd.Parameters.AddWithValue("@tex2", chr.Tex2);
            cmd.Parameters.AddWithValue("@pet", chr.Pet == null ? -1 : chr.Pet.InstanceId);
            cmd.Parameters.AddWithValue("@skin", chr.Skin);
            cmd.Parameters.AddWithValue("@xpTime", chr.XpTimer);
            cmd.Parameters.AddWithValue("@lootDropTime", chr.LDTimer);
            cmd.Parameters.AddWithValue("@lootTierTime", chr.LTTimer);
            chr.PCStats =
                Convert.ToBase64String(chr.FameStats.Write())
                    .Replace('+', '-')
                    .Replace('/', '_');
            cmd.Parameters.AddWithValue("@fameStats", chr.PCStats);
            cmd.ExecuteNonQuery();

            cmd = CreateQuery();
            cmd.CommandText = @"INSERT INTO classstats(accId, objType, bestLv, bestFame) 
VALUES(@accId, @objType, @bestLv, @bestFame) 
ON DUPLICATE KEY UPDATE 
bestLv = GREATEST(bestLv, @bestLv), 
bestFame = GREATEST(bestFame, @bestFame);";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            cmd.Parameters.AddWithValue("@objType", chr.ObjectType);
            cmd.Parameters.AddWithValue("@bestLv", chr.Level);
            cmd.Parameters.AddWithValue("@bestFame", chr.CurrentFame);
            cmd.ExecuteNonQuery();

            SaveBackpacks(chr, acc);
        }

        public int[] GetBackpack(Char chr, Account acc)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM backpacks WHERE charId=@charId AND accId=@accId";
            cmd.Parameters.AddWithValue("@charId", chr.CharacterId);
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            var ret = new int[8];
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows)
                    return new [] { -1, -1, -1, -1, -1, -1, -1, -1 };
                while (rdr.Read())
                    ret = Utils.FromCommaSepString32(rdr.GetString("items"));
            }
            return ret;
        }

        public void SaveBackpacks(Char chr, Account acc)
        {
            if (chr.HasBackpack == 1)
            {
                MySqlCommand cmd = CreateQuery();
                cmd.CommandText = @"INSERT INTO backpacks(accId, charId, items)
VALUES(@accId, @charId, @items)
ON DUPLICATE KEY UPDATE
items = @items;";
                cmd.Parameters.AddWithValue("@charId", chr.CharacterId);
                cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                cmd.Parameters.AddWithValue("@items", Utils.GetCommaSepString(chr.Backpack));
                cmd.ExecuteNonQuery();
            }
        }

        public void Death(XmlData data, Account acc, Char chr, string killer) //Save first
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = @"UPDATE characters SET 
dead=TRUE, 
deathTime=NOW() 
WHERE accId=@accId AND charId=@charId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            cmd.Parameters.AddWithValue("@charId", chr.CharacterId);
            cmd.ExecuteNonQuery();

            bool firstBorn;
            int finalFame = chr.FameStats.CalculateTotal(data, acc, chr, chr.CurrentFame, out firstBorn);

            cmd = CreateQuery();
            cmd.CommandText = @"UPDATE stats SET 
fame=fame+@amount, 
totalFame=totalFame+@amount 
WHERE accId=@accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            cmd.Parameters.AddWithValue("@amount", finalFame);
            cmd.ExecuteNonQuery();

            cmd = CreateQuery();
            cmd.CommandText = @"INSERT INTO classstats(accId, objType, bestLv, bestFame) 
VALUES(@accId, @objType, @bestLv, @bestFame) 
ON DUPLICATE KEY UPDATE 
bestLv = GREATEST(bestLv, @bestLv), 
bestFame = GREATEST(bestFame, @bestFame);";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            cmd.Parameters.AddWithValue("@objType", chr.ObjectType);
            cmd.Parameters.AddWithValue("@bestLv", chr.Level);
            cmd.Parameters.AddWithValue("@bestFame", finalFame);
            cmd.ExecuteNonQuery();

            if (acc.Guild.Id != 0)
            {
                cmd = CreateQuery();
                cmd.CommandText = @"UPDATE guilds SET
guildFame=guildFame+@amount,
totalGuildFame=totalGuildFame+@amount
WHERE name=@name;";
                cmd.Parameters.AddWithValue("@amount", finalFame);
                cmd.Parameters.AddWithValue("@name", acc.Guild.Name);
                cmd.ExecuteNonQuery();

                cmd = CreateQuery();
                cmd.CommandText = @"UPDATE accounts SET
guildFame=guildFame+@amount
WHERE id=@id;";
                cmd.Parameters.AddWithValue("@amount", finalFame);
                cmd.Parameters.AddWithValue("@id", acc.AccountId);
                cmd.ExecuteNonQuery();
            }

            cmd = CreateQuery();
            cmd.CommandText =
                @"INSERT INTO death(accId, chrId, name, charType, tex1, tex2, skin, items, fame, exp, fameStats, totalFame, firstBorn, killer) 
VALUES(@accId, @chrId, @name, @objType, @tex1, @tex2, @skin, @items, @fame, @exp, @fameStats, @totalFame, @firstBorn, @killer);";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            cmd.Parameters.AddWithValue("@chrId", chr.CharacterId);
            cmd.Parameters.AddWithValue("@name", acc.Name);
            cmd.Parameters.AddWithValue("@objType", chr.ObjectType);
            cmd.Parameters.AddWithValue("@tex1", chr.Tex1);
            cmd.Parameters.AddWithValue("@tex2", chr.Tex2);
            cmd.Parameters.AddWithValue("@skin", chr.Skin);
            cmd.Parameters.AddWithValue("@items", chr._Equipment);
            cmd.Parameters.AddWithValue("@fame", chr.CurrentFame);
            cmd.Parameters.AddWithValue("@exp", chr.Exp);
            cmd.Parameters.AddWithValue("@fameStats", chr.PCStats);
            cmd.Parameters.AddWithValue("@totalFame", finalFame);
            cmd.Parameters.AddWithValue("@firstBorn", firstBorn);
            cmd.Parameters.AddWithValue("@killer", killer);
            cmd.ExecuteNonQuery();
        }


        public void AddToArenaLb(int wave, List<string> participants)
        {
            string players = string.Join(", ", participants.ToArray());
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "INSERT INTO arenalb(wave, players) VALUES(@wave, @players)";
            cmd.Parameters.AddWithValue("@wave", wave);
            cmd.Parameters.AddWithValue("@players", players);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("h:mm:ss tt") + "] " + e);
            }
        }

        public string[][] GetArenaLeaderboards(string type, Account acc)
        {
            List<string[]> lbrankings = new List<string[]>();
            MySqlCommand cmd = CreateQuery();
            switch (type)
            {
                case "alltime":
                    cmd.CommandText = "SELECT * FROM arenalb ORDER BY wave DESC LIMIT 20";
                    break;
                case "weekly":
                    cmd.CommandText =
                        "SELECT * FROM arenalb WHERE date BETWEEN date_sub(now(), INTERVAL 1 WEEK) AND NOW() ORDER BY wave DESC LIMIT 20";
                    break;
                case "personal":
                    cmd.CommandText = "SELECT * FROM arenalb WHERE accid = @accid ORDER BY wave DESC LIMIT 20";
                    cmd.Parameters.AddWithValue("@acc", acc.AccountId);
                    break;
                default:
                    return null;
            }
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    List<string> ranking = new List<string>();
                    ranking.Add(rdr.GetInt32("wave").ToString());
                    ranking.Add(rdr.GetInt32("accid").ToString());
                    ranking.Add(rdr.GetInt32("charid").ToString());
                    ranking.Add(rdr.GetString("petid"));
                    ranking.Add(rdr.GetString("time"));
                    ranking.Add(rdr.GetString("date"));
                    lbrankings.Add(ranking.ToArray());
                }
            }

            return lbrankings.ToArray();
        }

        public string[] GetGuildLeaderboards()
        {
            List<string> guildrankings = new List<string>();

            MySqlCommand cmd = CreateQuery();

            cmd.CommandText = "SELECT * FROM guilds ORDER BY guildFame DESC LIMIT 10";

            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) guildrankings.Add("None");
                else
                {
                    while (rdr.Read())
                    {
                        guildrankings.Add(rdr.GetString("name") + " - " + rdr.GetInt32("guildFame") + " Fame");
                    }
                }
            }

            return guildrankings.ToArray();
        }


        public List<string> GetLockeds(string accId)
        {
            List<string> ret = new List<string>();
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT locked FROM accounts WHERE id=@accId";
            cmd.Parameters.AddWithValue("@accid", accId);
            try
            {
                string tmp = cmd.ExecuteScalar().ToString();
                if (!String.IsNullOrWhiteSpace(tmp))
                    ret = tmp.Split(',').ToList();
                for (int i = 0; i < ret.Count; i++)
                    ret[i] = ret[i].Trim();
                return ret;
            }
            catch
            {
                return new List<string>();
            }
        }

        public List<string> GetIgnoreds(string accId)
        {
            List<string> ret = new List<string>();
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT ignored FROM accounts WHERE id=@accId";
            cmd.Parameters.AddWithValue("@accid", accId);
            try
            {
                string tmp = cmd.ExecuteScalar().ToString();
                if (!String.IsNullOrWhiteSpace(tmp))
                    ret = tmp.Split(',').ToList();
                for (int i = 0; i < ret.Count; i++)
                    ret[i] = ret[i].Trim();
                return ret;
            }
            catch
            {
                return new List<string>();
            }
        }

        public bool AddLock(string accId, string lockId)
        {
            List<string> x = GetLockeds(accId);
            x.Add(lockId.ToString());
            string s = Utils.GetCommaSepString(x.ToArray());
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "UPDATE accounts SET locked=@newlocked WHERE id=@accId";
            cmd.Parameters.AddWithValue("@newlocked", s);
            cmd.Parameters.AddWithValue("@accId", accId);
            if (cmd.ExecuteNonQuery() == 0)
                return false;
            return true;
        }

        public bool RemoveLock(string accId, string lockId)
        {
            List<string> x = GetLockeds(accId);
            x.Remove(lockId.ToString());
            string s = Utils.GetCommaSepString(x.ToArray());
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "UPDATE accounts SET locked=@newlocked WHERE id=@accId";
            cmd.Parameters.AddWithValue("@newlocked", s);
            cmd.Parameters.AddWithValue("@accId", accId);
            if (cmd.ExecuteNonQuery() == 0)
                return false;
            return true;
        }

        public bool AddIgnore(string accId, string ignoreId)
        {
            List<string> x = GetIgnoreds(accId);
            x.Add(ignoreId.ToString());
            string s = Utils.GetCommaSepString(x.ToArray());
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "UPDATE accounts SET ignored=@newignored WHERE id=@accId";
            cmd.Parameters.AddWithValue("@newignored", s);
            cmd.Parameters.AddWithValue("@accId", accId);
            if (cmd.ExecuteNonQuery() == 0)
                return false;
            return true;
        }

        public bool RemoveIgnore(string accId, string ignoreId)
        {
            List<string> x = GetIgnoreds(accId);
            x.Remove(ignoreId.ToString());
            string s = Utils.GetCommaSepString(x.ToArray());
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "UPDATE accounts SET ignored=@newignored WHERE id=@accId";
            cmd.Parameters.AddWithValue("@newignored", s);
            cmd.Parameters.AddWithValue("@accId", accId);
            if (cmd.ExecuteNonQuery() == 0)
                return false;
            return true;
        }

        public void CreatePet(Account acc, PetItem item)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT COUNT(petId) FROM pets WHERE petId=@petId AND accId=@accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            cmd.Parameters.AddWithValue("@petId", item.InstanceId);
            if ((int)(long)cmd.ExecuteScalar() == 0)
            {
                //Not finished yet.
                cmd = CreateQuery();
                cmd.CommandText =
                    @"INSERT INTO pets(accId, petId, objType, skinName, skin, rarity, maxLevel, abilities, levels, xp) 
VALUES(@accId, @petId, @objType, @skinName, @skin, @rarity, @maxLevel, @abilities, @levels, @xp);";
                cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                cmd.Parameters.AddWithValue("@petId", item.InstanceId);
                cmd.Parameters.AddWithValue("@objType", item.Type);
                cmd.Parameters.AddWithValue("@skinName", item.SkinName);
                cmd.Parameters.AddWithValue("@skin", item.Skin);
                cmd.Parameters.AddWithValue("@rarity", item.Rarity);
                cmd.Parameters.AddWithValue("@maxLevel", item.MaxAbilityPower);
                cmd.Parameters.AddWithValue("@abilities",
                    String.Format("{0}, {1}, {2}", item.Abilities[0].Type, item.Abilities[1].Type, item.Abilities[2].Type));
                cmd.Parameters.AddWithValue("@levels",
                    String.Format("{0}, {1}, {2}", item.Abilities[0].Power, item.Abilities[1].Power, item.Abilities[2].Power));
                cmd.Parameters.AddWithValue("@xp",
                    String.Format("{0}, {1}, {2}", item.Abilities[0].Points, item.Abilities[1].Points, item.Abilities[2].Points));
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public int GetNextPetId(string accId)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT IFNULL(MAX(petId), 0) FROM pets WHERE accId=@accId;";
            cmd.Parameters.AddWithValue("@accId", accId);
            int ret = (int)(long)cmd.ExecuteScalar() + 1;
            return ret;
        }

        public void UpdateLastSeen(string accId, int charId, string location)
        {
            string currentDate = DateTime.UtcNow.ToString("yyyy-MM-dd:HH-mm-ss");
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "UPDATE accounts SET lastSeen=@lastSeen WHERE id=@accId;";
            cmd.Parameters.AddWithValue("@lastSeen", currentDate);
            cmd.Parameters.AddWithValue("@accId", accId);
            cmd.ExecuteScalar();

            cmd = CreateQuery();
            cmd.CommandText = "UPDATE characters SET lastSeen=@lastSeen, lastLocation=@location WHERE accId=@accId AND charId=@charId;";
            cmd.Parameters.AddWithValue("@lastSeen", currentDate);
            cmd.Parameters.AddWithValue("@location", location ?? String.Empty);
            cmd.Parameters.AddWithValue("@accId", accId);
            cmd.Parameters.AddWithValue("@charId", charId);
            cmd.ExecuteScalar();
        }

        public bool CheckAccountInUse(Account acc, ref int? timeout)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT lastSeen, accountInUse FROM accounts WHERE id=@accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    DateTime lastSeen = rdr.GetDateTime("lastSeen");
                    if(lastSeen == DateTime.MinValue)
                        return false;

                    int timeInSec = 600 - (int)(DateTime.UtcNow - lastSeen).TotalSeconds;
                    bool accInUse = rdr.GetInt32("accountInUse") == 1;
                    if (accInUse && timeInSec > 0)
                    {
                        timeout = timeInSec;
                        return true;
                    }
                }
            }
            UnlockAccount(acc);
            return false;
        }

        public void LockAccount(Account acc)
        {
            if (acc == null) return;
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "UPDATE accounts SET accountInUse=1 WHERE id=@accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            cmd.ExecuteScalar();
        }

        public void UnlockAccount(Account acc)
        {
            if (acc == null) return;
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "UPDATE accounts SET accountInUse=0 WHERE id=@accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            cmd.ExecuteScalar();
        }

        public string GenerateGiftcode(string contents, string accId)
        {
            var code = generateGiftCode(5, 5);
            while (giftCodeExists(code))
                code = generateGiftCode(5, 5);

            var cmd = CreateQuery();
            cmd.CommandText = "INSERT INTO giftCodes(code, content, accId) VALUES(@code, @contents, @accId);";
            cmd.Parameters.AddWithValue("@code", code);
            cmd.Parameters.AddWithValue("@contents", contents);
            cmd.Parameters.AddWithValue("@accId", accId);
            cmd.ExecuteNonQuery();
            return code;
        }

        private string generateGiftCode(int blocks, int blockLength)
        {
            var builder = new StringBuilder();
            var rand = new Random();
            for (var i = 0; i < blocks; i++)
            {
                builder.Append(GenerateRandomString(blockLength, rand));
                if(i < blocks-1)
                    builder.Append("-");
            }
            return builder.ToString();
        }

        private bool giftCodeExists(string code)
        {
            var cmd = CreateQuery();
            cmd.CommandText = "SELECT code FROM giftCodes WHERE code=@code";
            cmd.Parameters.AddWithValue("@code", code);
            using (var rdr = cmd.ExecuteReader())
                return rdr.HasRows;
        }

        public bool SaveChars(string oldAccId, Chars oldChars, Chars chrs, XmlData data)
        {
            try
            {
                chrs.Account.AccountId = oldAccId;

                var cmd = CreateQuery();
                cmd.CommandText = "UPDATE accounts SET prodAcc=1, name=@name, namechosen=@nameChoosen, vaultCount=@vaults, maxCharSlot=@maxChars, ownedSkins=@skins, gifts=@gifts WHERE id=@oldAccId;";
                cmd.Parameters.AddWithValue("@name", chrs.Account.Name);
                cmd.Parameters.AddWithValue("@nameChoosen", chrs.Account.NameChosen ? 1 : 0);
                cmd.Parameters.AddWithValue("@vaults", chrs.Account.Vault.Chests.Count);
                cmd.Parameters.AddWithValue("@maxChars", chrs.MaxNumChars);
                cmd.Parameters.AddWithValue("@oldAccId", oldAccId);
                cmd.Parameters.AddWithValue("@gifts", chrs.Account._Gifts);
                cmd.Parameters.AddWithValue("@skins", chrs.OwnedSkins);
                cmd.ExecuteNonQuery();

                cmd = CreateQuery();
                cmd.CommandText = "DELETE FROM characters WHERE accId=@accId AND dead=0;";
                cmd.Parameters.AddWithValue("@accId", oldAccId);
                cmd.ExecuteNonQuery();

                cmd = CreateQuery();
                cmd.CommandText = "DELETE FROM vaults WHERE accId=@accId;";
                cmd.Parameters.AddWithValue("@accId", oldAccId);
                cmd.ExecuteNonQuery();

                cmd = CreateQuery();
                cmd.CommandText = "DELETE FROM classstats WHERE accId=@accId";
                cmd.Parameters.AddWithValue("@accId", oldAccId);
                cmd.ExecuteNonQuery();

                foreach (var stat in chrs.Account.Stats.ClassStates)
                {
                    cmd = CreateQuery();
                    cmd.CommandText = @"INSERT INTO classstats(accId, objType, bestLv, bestFame) 
VALUES(@accId, @objType, @bestLv, @bestFame) 
ON DUPLICATE KEY UPDATE 
bestLv = GREATEST(bestLv, @bestLv), 
bestFame = GREATEST(bestFame, @bestFame);";
                    cmd.Parameters.AddWithValue("@accId", oldAccId);
                    cmd.Parameters.AddWithValue("@objType", Utils.FromString(stat.ObjectType.ToString()));
                    cmd.Parameters.AddWithValue("@bestLv", stat.BestLevel);
                    cmd.Parameters.AddWithValue("@bestFame", stat.BestFame);
                    cmd.ExecuteNonQuery();
                }

                cmd = CreateQuery();
                cmd.CommandText = "DELETE FROM stats WHERE accId=@accId";
                cmd.Parameters.AddWithValue("@accId", oldAccId);
                cmd.ExecuteNonQuery();

                cmd = CreateQuery();
                cmd.CommandText = "INSERT INTO stats (accId, fame, totalFame, credits, totalCredits, fortuneTokens, totalFortuneTokens) VALUES(@accId, @fame, @fame, @gold, @gold, @tokens, @tokens)";
                cmd.Parameters.AddWithValue("@accId", oldAccId);
                cmd.Parameters.AddWithValue("@fame", chrs.Account.Stats.Fame);
                cmd.Parameters.AddWithValue("@totalFame", chrs.Account.Stats.TotalFame);
                cmd.Parameters.AddWithValue("@gold", chrs.Account.Credits);
                cmd.Parameters.AddWithValue("@tokens", chrs.Account.FortuneTokens);
                cmd.ExecuteNonQuery();

                cmd = CreateQuery();
                cmd.CommandText = "DELETE FROM pets WHERE accId=@accId;";
                cmd.Parameters.AddWithValue("@accId", oldAccId);
                cmd.ExecuteNonQuery();

                foreach (var @char in chrs.Characters)
                {
                    var chr = CreateCharacter(data, (ushort)@char.ObjectType, @char.CharacterId);

                    int[] stats = new[]
                    {
                        @char.MaxHitPoints,
                        @char.MaxMagicPoints,
                        @char.Attack,
                        @char.Defense,
                        @char.Speed,
                        @char.Dexterity,
                        @char.HpRegen,
                        @char.MpRegen
                    };

                    cmd = CreateQuery();
                    cmd.Parameters.AddWithValue("@accId", chrs.Account.AccountId);
                    cmd.Parameters.AddWithValue("@charId", @char.CharacterId);
                    cmd.Parameters.AddWithValue("@charType", @char.ObjectType);
                    cmd.Parameters.AddWithValue("@items", Utils.GetCommaSepString(@char.EquipSlots()));
                    cmd.Parameters.AddWithValue("@stats", Utils.GetCommaSepString(stats));
                    cmd.Parameters.AddWithValue("@fameStats", @char.PCStats);
                    cmd.Parameters.AddWithValue("@skin", @char.Skin);
                    cmd.CommandText =
                        "INSERT INTO characters (accId, charId, charType, level, exp, fame, items, hp, mp, stats, dead, pet, fameStats, skin) VALUES (@accId, @charId, @charType, 1, 0, 0, @items, 100, 100, @stats, FALSE, -1, @fameStats, @skin);";
                    cmd.ExecuteNonQuery();

                    if (@char.Pet != null)
                        CreatePet(chrs.Account, @char.Pet);

                    @char.FameStats = new FameStats();
                    @char.FameStats.Read(
                        Convert.FromBase64String(@char.PCStats.Replace('-', '+').Replace('_', '/')));

                    if (@char.Equipment.Length > 12)
                    {
                        @char.Backpack = new int[8];
                        Array.Copy(@char.Equipment, 12, @char.Backpack, 0, 8);
                        var eq = @char.Equipment;
                        Array.Resize(ref eq, 12);
                        @char.Equipment = eq;
                        @char.HasBackpack = 1;
                    }
                    chr = @char;
                    SaveCharacter(chrs.Account, chr);
                }

                foreach (var chest in chrs.Account.Vault.Chests)
                {
                    chest.ChestId = CreateChest(chrs.Account).ChestId;
                    if (chest.Items.Length < 8)
                    {
                        var inv = chest.Items;
                        Array.Resize(ref inv, 8);
                        for (int i = 0; i < inv.Length; i++)
                            if (inv[i] == 0) inv[i] = -1;
                        chest.Items = inv;
                    }
                    SaveChest(chrs.Account.AccountId, chest);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }

        public void AddGifts(Account acc, IEnumerable<int> gifts)
        {
            var tmpGifts = Utils.FromCommaSepString32(acc._Gifts).ToList();
            tmpGifts.AddRange(gifts);
            var cmd = CreateQuery();
            cmd.CommandText = "UPDATE accounts SET gifts=@newGifts WHERE id=@accId;";
            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
            cmd.Parameters.AddWithValue("@newGifts", Utils.GetCommaSepString(tmpGifts.ToArray()));
            cmd.ExecuteNonQuery();
        }
    }
}