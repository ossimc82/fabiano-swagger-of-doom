#region

using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

#endregion

namespace db
{
    partial class Database
    {
        public string GetGuildName(long id)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT name FROM guilds WHERE id=@gid;";
            cmd.Parameters.AddWithValue("@gid", id);
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) return "";
                rdr.Read();
                return rdr.GetString("name");
            }
        }

        public int GetGuildId(string name)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT id FROM guilds WHERE name=@name;";
            cmd.Parameters.AddWithValue("@name", name);
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) return 0;
                rdr.Read();
                return rdr.GetInt32("id");
            }
        }

        public List<GuildStruct> GetGuilds()
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM guilds";
            List<GuildStruct> guilds = new List<GuildStruct>();
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) return null;
                while (rdr.Read())
                {
                    guilds.Add(new GuildStruct
                    {
                        Id = rdr.GetInt32("id"),
                        Name = rdr.GetString("name"),
                        Level = rdr.GetInt32("level"),
                        Members = rdr.GetString("members").Split(','),
                        GuildFame = rdr.GetInt32("guildFame"),
                        TotalGuildFame = rdr.GetInt32("totalGuildFame")
                    });
                }
            }
            return guilds;
        }

        public GuildStruct GetGuild(string name)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM guilds WHERE name=@n";
            cmd.Parameters.AddWithValue("@n", name);
            GuildStruct guild;
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) return null;
                rdr.Read();
                guild = new GuildStruct
                {
                    Id = rdr.GetInt32("id"),
                    Name = rdr.GetString("name"),
                    Level = rdr.GetInt32("level"),
                    Members = rdr.GetString("members").Split(','),
                    GuildFame = rdr.GetInt32("guildFame"),
                    TotalGuildFame = rdr.GetInt32("totalGuildFame")
                };
            }
            return guild;
        }

        public GuildStruct GetGuild(long guildid)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM guilds WHERE id=@gid";
            cmd.Parameters.AddWithValue("@gid", guildid);
            GuildStruct guild;
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) return null;
                rdr.Read();
                guild = new GuildStruct
                {
                    Id = rdr.GetInt32("id"),
                    Name = rdr.GetString("name"),
                    Level = rdr.GetInt32("level"),
                    Members = rdr.GetString("members").Split(','),
                    GuildFame = rdr.GetInt32("guildFame"),
                    TotalGuildFame = rdr.GetInt32("totalGuildFame")
                };
            }
            return guild;
        }

        public Guild CreateGuild(Account acc, string name)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText =
                "INSERT INTO guilds (name, members, guildFame, totalGuildFame, level) VALUES (@name,@empty,0,0,0)";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@empty", "");
            if (cmd.ExecuteNonQuery() == 0)
            {
                throw new Exception("Could not add guild to SQL database!");
            }
            int id = GetGuildId(name);

            cmd = CreateQuery();
            cmd.CommandText = "INSERT INTO boards (guildId, text) VALUES (@id,@empty)";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@empty", "");
            if (cmd.ExecuteNonQuery() == 0)
            {
                throw new Exception("Could not add guild board to SQL database!");
            }

            return ChangeGuild(acc, GetGuildId(name), 40, 0, false);
        }

        public Guild ChangeGuild(Account acc, long guildid, int rank, int fame, bool renounce)
        {
            Guild guild;
            if (renounce)
            {
                guild = new Guild
                {
                    Name = "",
                    Id = 0,
                    Rank = 0,
                    Fame = 0
                };

                MySqlCommand cmd = CreateQuery();
                cmd.CommandText = "UPDATE accounts SET guild=0, guildRank=0, guildFame=0 WHERE id=@aid";
                cmd.Parameters.AddWithValue("@aid", acc.AccountId);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    Console.WriteLine("Could not change player's guild in the SQL!");
                }

                UpdateGuild(guildid);

                return guild;
            }
            else
            {
                guild = new Guild
                {
                    Id = guildid,
                    Name = GetGuildName(guildid),
                    Rank = rank,
                    Fame = fame
                };
                if (guild.Name == "")
                {
                    throw new Exception("Guild not found!");
                }
                MySqlCommand cmd = CreateQuery();
                cmd.CommandText = "UPDATE accounts SET guild=@gid, guildRank=@gr, guildFame=@gf WHERE id=@aid";
                cmd.Parameters.AddWithValue("@gid", guildid);
                cmd.Parameters.AddWithValue("@gr", rank);
                cmd.Parameters.AddWithValue("@gf", fame);
                cmd.Parameters.AddWithValue("@aid", acc.AccountId);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    Console.WriteLine("Could not change player's guild in the SQL!");
                }

                UpdateGuild(guildid);

                return guild;
            }
        }

        public Guild EditGuild(string name, int guildid, int rank, int fame, bool renounce)
        {
            Guild guild;
            if (renounce)
            {
                guild = new Guild
                {
                    Name = "",
                    Id = 0,
                    Rank = 0,
                    Fame = 0
                };

                MySqlCommand cmd = CreateQuery();
                cmd.CommandText = "UPDATE accounts SET guild=0, guildRank=0, guildFame=0 WHERE name=@name";
                cmd.Parameters.AddWithValue("@name", name);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    Console.WriteLine("Player not found!");
                }

                UpdateGuild(guildid);

                return guild;
            }
            else
            {
                guild = new Guild
                {
                    Id = guildid,
                    Name = GetGuildName(guildid),
                    Rank = rank,
                    Fame = fame
                };
                if (guild.Name == "")
                {
                    Console.WriteLine("Guild not found!");
                }
                MySqlCommand cmd = CreateQuery();
                cmd.CommandText = "UPDATE accounts SET guild=@gid, guildRank=@gr guildFame=@gf WHERE name=@name";
                cmd.Parameters.AddWithValue("@gid", guildid);
                cmd.Parameters.AddWithValue("@gr", rank);
                cmd.Parameters.AddWithValue("@gf", fame);
                cmd.Parameters.AddWithValue("@name", name);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    throw new Exception("Player not found!");
                }

                UpdateGuild(guildid);

                return guild;
            }
        }

        public void UpdateGuild(long id)
        {
            GuildStruct guild = GetGuild(id);
            if (guild == null)
                throw new Exception("Guild not found!");
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM accounts WHERE guild=@gid";
            cmd.Parameters.AddWithValue("@gid", id);
            string members = "";
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    members = members + rdr.GetInt32("id") + ",";
                }
            }

            if (members != "")
            {
                cmd = CreateQuery();
                cmd.CommandText = "UPDATE guilds SET members=@mem WHERE id=@gid";
                cmd.Parameters.AddWithValue("@gid", id);
                cmd.Parameters.AddWithValue("@mem", members);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    Console.WriteLine("Failed to edit members column!");
                }
            }
            else
            {
                cmd = CreateQuery();
                cmd.CommandText = "DELETE FROM guilds WHERE id=@gid";
                cmd.Parameters.AddWithValue("@gid", id);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    Console.WriteLine("Failed to delete empty guild!");
                }
            }
        }

        public string GetGuildBoard(Account acc)
        {
            if (acc != null)
            {
                long gid = acc.Guild.Id;
                MySqlCommand cmd = CreateQuery();
                cmd.CommandText = "SELECT * FROM boards WHERE guildId=@gid";
                cmd.Parameters.AddWithValue("@gid", gid);
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (!rdr.HasRows) return "Board error!";
                    rdr.Read();
                    return rdr.GetString("text");
                }
            }
            Console.WriteLine("Invalid account.");
            return null;
        }

        public string SetGuildBoard(string text, Account acc)
        {
            if (acc != null)
            {
                long gid = acc.Guild.Id;
                MySqlCommand cmd = CreateQuery();
                cmd.CommandText = "UPDATE boards SET text=@txt WHERE guildId=@gid";
                cmd.Parameters.AddWithValue("@gid", gid);
                cmd.Parameters.AddWithValue("@txt", text);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    return "Error";
                }
                return text;
            }
            return "Error";
        }

        public string HttpGetGuildMembers(int num, int offset, Account acc)
        {
            GuildStruct guild = GetGuild((int)acc.Guild.Id);
            string ret = "<Guild name=\"" + guild.Name + "\" id=\"" + guild.Id + "\"><TotalFame>" + guild.TotalGuildFame +
                         "</TotalFame><CurrentFame>" + guild.GuildFame + "</CurrentFame><HallType>Guild Hall " +
                         guild.Level + "</HallType>";
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM accounts WHERE guild = @gid";
            cmd.Parameters.AddWithValue("@gid", guild.Id);
            List<string> founders = new List<string>();
            List<string> leaders = new List<string>();
            List<string> officers = new List<string>();
            List<string> members = new List<string>();
            List<string> initiates = new List<string>();
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                int countLeft = num;
                int offsleft = offset;
                while (rdr.Read())
                {
                    if (offsleft == 0)
                    {
                        if (countLeft != 0)
                        {
                            string add = "<Member>";

                            add += string.Format("<Name>{0}</Name>", rdr.GetString("name"));
                            add += string.Format("<Rank>{0}</Rank>", rdr.GetInt32("guildRank"));
                            add += string.Format("<Fame>{0}</Fame>", rdr.GetInt32("guildFame"));

                            add += "</Member>";

                            switch (rdr.GetInt32("guildRank"))
                            {
                                case 40:
                                    founders.Add(add);
                                    break;
                                case 30:
                                    leaders.Add(add);
                                    break;
                                case 20:
                                    officers.Add(add);
                                    break;
                                case 10:
                                    members.Add(add);
                                    break;
                                case 0:
                                    initiates.Add(add);
                                    break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        offsleft--;
                    }
                    countLeft--;
                }
            }
            members.AddRange(initiates);
            officers.AddRange(members);
            leaders.AddRange(officers);
            founders.AddRange(leaders);
            ret = founders.Aggregate(ret, (current, i) => current + i);
            ret += "</Guild>";
            return ret;
        }

        public int GetGuildFame(int id)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT guildFame FROM guilds WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        public void DetractGuildFame(int id, int quantity)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "UPDATE guilds SET guildFame=@fame WHERE id=@id";
            cmd.Parameters.AddWithValue("@fame", GetGuildFame(id) - quantity);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public void ChangeGuildLevel(int id, int newLevel)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "UPDATE guilds SET level=@level WHERE id=@id";
            cmd.Parameters.AddWithValue("@level", newLevel);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public int GetGuildLevel(int id)
        {
            MySqlCommand cmd = CreateQuery();
            cmd.CommandText = "SELECT level FROM guilds WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            return int.Parse(cmd.ExecuteScalar().ToString());
        }
    }
}