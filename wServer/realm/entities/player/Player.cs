#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using db;
using log4net;
using wServer.logic;
using wServer.networking;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;

#endregion

namespace wServer.realm.entities.player
{
    internal interface IPlayer
    {
        void Damage(int dmg, Entity chr);
        bool IsVisibleToEnemy();
    }

    public static class IComparableExtension
    {
        public static bool InRange<T>(this T value, T from, T to) where T : IComparable<T>
        {
            return value.CompareTo(from) >= 1 && value.CompareTo(to) <= -1;
        }
    }

    public partial class Player : Character, IContainer, IPlayer
    {
        private static readonly new ILog log = LogManager.GetLogger(typeof(Player));

        private bool dying;

        private Item[] _inventory;

        private float hpRegenCounter;
        private float mpRegenCounter;
        private bool resurrecting;

        private byte[,] tiles;
        public bool vanished = false;
        private int pingSerial;
        private SetTypeSkin setTypeSkin;

        public Player(RealmManager manager, Client psr)
            : base(manager, (ushort)psr.Character.ObjectType, psr.Random)
        {
            try
            {
                Client = psr;
                Manager = psr.Manager;
                StatsManager = new StatsManager(this);
                Name = psr.Account.Name;
                AccountId = psr.Account.AccountId;
                FameCounter = new FameCounter(this);
                Tokens = psr.Account.FortuneTokens;
                HPPotionPrice = 5;
                MPPotionPrice = 5;

                Level = psr.Character.Level == 0 ? 1 : psr.Character.Level;
                Experience = psr.Character.Exp;
                ExperienceGoal = GetExpGoal(Level);
                Stars = GetStars();
                Texture1 = psr.Character.Tex1;
                Texture2 = psr.Character.Tex2;
                Credits = psr.Account.Credits;
                NameChosen = psr.Account.NameChosen;
                CurrentFame = psr.Account.Stats.Fame;
                Fame = psr.Character.CurrentFame;
                XpBoosted = psr.Character.XpBoosted;
                XpBoostTimeLeft = psr.Character.XpTimer;
                xpFreeTimer = XpBoostTimeLeft == -1.0 ? false : true;
                LootDropBoostTimeLeft = psr.Character.LDTimer;
                lootDropBoostFreeTimer = LootDropBoost;
                LootTierBoostTimeLeft = psr.Character.LTTimer;
                lootTierBoostFreeTimer = LootTierBoost;
                ClassStats state =
                    psr.Account.Stats.ClassStates.SingleOrDefault(_ => Utils.FromString(_.ObjectType) == ObjectType);
                FameGoal = GetFameGoal(state != null ? state.BestFame : 0);
                Glowing = IsUserInLegends();
                Guild = GuildManager.Add(this, psr.Account.Guild);
                if (psr.Character.HitPoints <= 0)
                    HP = psr.Character.MaxHitPoints;
                else
                    HP = psr.Character.HitPoints;
                MP = psr.Character.MagicPoints;
                ConditionEffects = 0;
                OxygenBar = 100;
                HasBackpack = psr.Character.HasBackpack;
                PlayerSkin = Client.Account.OwnedSkins.Contains(Client.Character.Skin) ? Client.Character.Skin : 0;
                HealthPotions = psr.Character.HealthStackCount < 0 ? 0 : psr.Character.HealthStackCount;
                MagicPotions = psr.Character.MagicStackCount < 0 ? 0 : psr.Character.MagicStackCount;

                Locked = psr.Account.Locked ?? new List<string>();
                Ignored = psr.Account.Ignored ?? new List<string>();
                try
                {
                    Manager.Database.DoActionAsync(db =>
                    {
                        Locked = db.GetLockeds(AccountId);
                        Ignored = db.GetIgnoreds(AccountId);
                        Muted = db.IsMuted(AccountId);
                        DailyQuest = psr.Account.DailyQuest;
                    });
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }

                if (HasBackpack == 1)
                {
                    Item[] inv =
                        psr.Character.Equipment.Select(
                            _ =>
                                _ == -1
                                    ? null
                                    : (Manager.GameData.Items.ContainsKey((ushort)_) ? Manager.GameData.Items[(ushort)_] : null))
                            .ToArray();
                    Item[] backpack =
                        psr.Character.Backpack.Select(
                            _ =>
                                _ == -1
                                    ? null
                                    : (Manager.GameData.Items.ContainsKey((ushort)_) ? Manager.GameData.Items[(ushort)_] : null))
                            .ToArray();

                    Inventory = inv.Concat(backpack).ToArray();
                    int[] slotTypes =
                        Utils.FromCommaSepString32(
                            Manager.GameData.ObjectTypeToElement[ObjectType].Element("SlotTypes").Value);
                    Array.Resize(ref slotTypes, 20);
                    SlotTypes = slotTypes;
                }
                else
                {
                    Inventory =
                        psr.Character.Equipment.Select(
                            _ =>
                                _ == -1
                                    ? null
                                    : (Manager.GameData.Items.ContainsKey((ushort)_) ? Manager.GameData.Items[(ushort)_] : null))
                            .ToArray();
                    SlotTypes =
                        Utils.FromCommaSepString32(
                            Manager.GameData.ObjectTypeToElement[ObjectType].Element("SlotTypes").Value);
                }
                Stats = new[]
                {
                    psr.Character.MaxHitPoints,
                    psr.Character.MaxMagicPoints,
                    psr.Character.Attack,
                    psr.Character.Defense,
                    psr.Character.Speed,
                    psr.Character.HpRegen,
                    psr.Character.MpRegen,
                    psr.Character.Dexterity
                };

                Pet = null;

                for (int i = 0; i < SlotTypes.Length; i++)
                    if (SlotTypes[i] == 0) SlotTypes[i] = 10;

                if (Client.Account.Rank < 2)
                    for (int i = 0; i < 4; i++)
                        if (Inventory[i]?.SlotType != SlotTypes[i])
                            Inventory[i] = null;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        ~Player()
        {
            WorldInstance = null;
            questEntity = null;
        }

        //Stats
        public string AccountId { get; private set; }

        public int[] Boost { get; private set; }

        public Client Client { get; }

        public int Credits { get; set; }
        public int Tokens { get; set; }
        public int CurrentFame { get; set; }

        public int Experience { get; set; }
        public int ExperienceGoal { get; set; }

        public int Fame { get; set; }

        public FameCounter FameCounter { get; }

        public QuestItem DailyQuest { get; set; }

        public int FameGoal { get; set; }

        public bool Glowing { get; set; }

        public int HasBackpack { get; set; }

        public int HealthPotions { get; set; }

        public List<string> Ignored { get; set; }

        public bool Invited { get; set; }
        public bool Muted { get; set; }

        public int Level { get; set; }

        public List<string> Locked { get; set; }

        public bool LootDropBoost
        {
            get { return LootDropBoostTimeLeft > 0; }
            set { LootDropBoostTimeLeft = value ? LootDropBoostTimeLeft : 0.0f; }
        }
        public float LootDropBoostTimeLeft { get; set; }

        public bool LootTierBoost
        {
            get { return LootTierBoostTimeLeft > 0; }
            set { LootTierBoostTimeLeft = value ? LootTierBoostTimeLeft : 0.0f; }
        }
        public float LootTierBoostTimeLeft { get; set; }

        public bool XpBoosted { get; set; }
        public float XpBoostTimeLeft { get; set; }

        public int MagicPotions { get; set; }

        public ushort HPPotionPrice { get; set; }
        public ushort MPPotionPrice { get; set; }

        public bool HPFirstPurchaseTime { get; set; }
        public bool MPFirstPurchaseTime { get; set; }

        public new RealmManager Manager { get; }

        public int MaxHP { get; set; }

        public int MaxMP { get; set; }

        public int MP { get; set; }

        public bool NameChosen { get; set; }

        public int OxygenBar { get; set; }

        public Pet Pet { get; set; }

        public int PlayerSkin { get; set; }

        public int Stars { get; set; }

        public int[] Stats { get; private set; }

        public StatsManager StatsManager { get; }

        public int Texture1 { get; set; }

        public int Texture2 { get; set; }

        public Item[] Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        public GuildManager Guild { get; set; }

        public int[] SlotTypes { get; private set; }

        public void Damage(int dmg, Entity chr)
        {
            try
            {
                if (HasConditionEffect(ConditionEffects.Paused) ||
                    HasConditionEffect(ConditionEffects.Stasis) ||
                    HasConditionEffect(ConditionEffects.Invincible))
                    return;

                dmg = (int)StatsManager.GetDefenseDamage(dmg, false);
                if (!HasConditionEffect(ConditionEffects.Invulnerable))
                    HP -= dmg;
                UpdateCount++;
                Owner.BroadcastPacket(new DamagePacket
                {
                    TargetId = Id,
                    Effects = 0,
                    Damage = (ushort)dmg,
                    Killed = HP <= 0,
                    BulletId = 0,
                    ObjectId = chr.Id
                }, this);
                SaveToCharacter();

                if (HP <= 0)
                    Death(chr.ObjectDesc.DisplayId, chr.ObjectDesc);
            }
            catch (Exception e)
            {
                log.Error("Error while processing playerDamage: ", e);
            }
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            base.ExportStats(stats);
            stats[StatsType.AccountId] = AccountId;
            stats[StatsType.Name] = Name;

            stats[StatsType.Experience] = Experience - GetLevelExp(Level);
            stats[StatsType.ExperienceGoal] = ExperienceGoal;
            stats[StatsType.Level] = Level;

            stats[StatsType.CurrentFame] = CurrentFame;
            stats[StatsType.Fame] = Fame;
            stats[StatsType.FameGoal] = FameGoal;
            stats[StatsType.Stars] = Stars;

            stats[StatsType.Guild] = Guild[AccountId].Name;
            stats[StatsType.GuildRank] = Guild[AccountId].Rank;

            stats[StatsType.Credits] = Credits;
            stats[StatsType.Tokens] = Tokens;
            stats[StatsType.NameChosen] = NameChosen ? 1 : 0;
            stats[StatsType.Texture1] = Texture1;
            stats[StatsType.Texture2] = Texture2;

            if (Glowing)
                stats[StatsType.Glowing] = 1;

            stats[StatsType.HP] = HP;
            stats[StatsType.MP] = MP;

            stats[StatsType.Inventory0] = (Inventory[0] != null ? Inventory[0].ObjectType : -1);
            stats[StatsType.Inventory1] = (Inventory[1] != null ? Inventory[1].ObjectType : -1);
            stats[StatsType.Inventory2] = (Inventory[2] != null ? Inventory[2].ObjectType : -1);
            stats[StatsType.Inventory3] = (Inventory[3] != null ? Inventory[3].ObjectType : -1);
            stats[StatsType.Inventory4] = (Inventory[4] != null ? Inventory[4].ObjectType : -1);
            stats[StatsType.Inventory5] = (Inventory[5] != null ? Inventory[5].ObjectType : -1);
            stats[StatsType.Inventory6] = (Inventory[6] != null ? Inventory[6].ObjectType : -1);
            stats[StatsType.Inventory7] = (Inventory[7] != null ? Inventory[7].ObjectType : -1);
            stats[StatsType.Inventory8] = (Inventory[8] != null ? Inventory[8].ObjectType : -1);
            stats[StatsType.Inventory9] = (Inventory[9] != null ? Inventory[9].ObjectType : -1);
            stats[StatsType.Inventory10] = (Inventory[10] != null ? Inventory[10].ObjectType : -1);
            stats[StatsType.Inventory11] = (Inventory[11] != null ? Inventory[11].ObjectType : -1);

            if (Boost == null) CalcBoost();

            stats[StatsType.MaximumHP] = Stats[0] + Boost[0];
            stats[StatsType.MaximumMP] = Stats[1] + Boost[1];
            stats[StatsType.Attack] = Stats[2] + Boost[2];
            stats[StatsType.Defense] = Stats[3] + Boost[3];
            stats[StatsType.Speed] = Stats[4] + Boost[4];
            stats[StatsType.Vitality] = Stats[5] + Boost[5];
            stats[StatsType.Wisdom] = Stats[6] + Boost[6];
            stats[StatsType.Dexterity] = Stats[7] + Boost[7];

            stats[StatsType.HPBoost] = Boost[0];
            stats[StatsType.MPBoost] = Boost[1];
            stats[StatsType.AttackBonus] = Boost[2];
            stats[StatsType.DefenseBonus] = Boost[3];
            stats[StatsType.SpeedBonus] = Boost[4];
            stats[StatsType.VitalityBonus] = Boost[5];
            stats[StatsType.WisdomBonus] = Boost[6];
            stats[StatsType.DexterityBonus] = Boost[7];

            stats[StatsType.Size] = setTypeSkin == null ? Size : setTypeSkin.Size;
            stats[StatsType.Has_Backpack] = HasBackpack;
            if (HasBackpack == 1)
            {
                stats[StatsType.Backpack0] = (Inventory[12] != null ? Inventory[12].ObjectType : -1);
                stats[StatsType.Backpack1] = (Inventory[13] != null ? Inventory[13].ObjectType : -1);
                stats[StatsType.Backpack2] = (Inventory[14] != null ? Inventory[14].ObjectType : -1);
                stats[StatsType.Backpack3] = (Inventory[15] != null ? Inventory[15].ObjectType : -1);
                stats[StatsType.Backpack4] = (Inventory[16] != null ? Inventory[16].ObjectType : -1);
                stats[StatsType.Backpack5] = (Inventory[17] != null ? Inventory[17].ObjectType : -1);
                stats[StatsType.Backpack6] = (Inventory[18] != null ? Inventory[18].ObjectType : -1);
                stats[StatsType.Backpack7] = (Inventory[19] != null ? Inventory[19].ObjectType : -1);
            }

            stats[StatsType.Skin] = setTypeSkin == null ? PlayerSkin : setTypeSkin.SkinType;
            stats[StatsType.HealStackCount] = HealthPotions;
            stats[StatsType.MagicStackCount] = MagicPotions;

            if (Owner != null && Owner.Name == "Ocean Trench")
                stats[StatsType.OxygenBar] = OxygenBar;

            stats[StatsType.XpBoosterActive] = XpBoosted ? 1 : 0;
            stats[StatsType.XpBoosterTime] = (int)XpBoostTimeLeft;
            stats[StatsType.LootDropBoostTimer] = (int)LootDropBoostTimeLeft;
            stats[StatsType.LootTierBoostTimer] = (int)LootTierBoostTimeLeft;
        }

        public void CalcBoost()
        {
            CheckSetTypeSkin();
            if (Boost == null) Boost = new int[12];
            else
                for (int i = 0; i < Boost.Length; i++) Boost[i] = 0;
            for (int i = 0; i < 4; i++)
            {
                if (Inventory.Length < i || Inventory.Length == 0) return;
                if (Inventory[i] == null) continue;
                foreach (KeyValuePair<int, int> b in Inventory[i].StatsBoost)
                {
                    if (b.Key == StatsType.MaximumHP) Boost[0] += b.Value;
                    if (b.Key == StatsType.MaximumMP) Boost[1] += b.Value;
                    if (b.Key == StatsType.Attack) Boost[2] += b.Value;
                    if (b.Key == StatsType.Defense) Boost[3] += b.Value;
                    if (b.Key == StatsType.Speed) Boost[4] += b.Value;
                    if (b.Key == StatsType.Vitality) Boost[5] += b.Value;
                    if (b.Key == StatsType.Wisdom) Boost[6] += b.Value;
                    if (b.Key == StatsType.Dexterity) Boost[7] += b.Value;
                }
            }

            if (setTypeBoosts != null)
                for (int i = 0; i < 8; i++)
                    Boost[i] += setTypeBoosts[i];
        }

        public bool CompareName(string name)
        {
            string rn = name.ToLower();
            if (rn.Split(' ')[0].StartsWith("[") || Name.Split(' ').Length == 1)
                if (Name.ToLower().StartsWith(rn)) return true;
                else return false;
            if (Name.Split(' ')[1].ToLower().StartsWith(rn)) return true;
            return false;
        }

        //public GlobalPlayerData PlayerData { get; private set; }

        public void Death(string killer, ObjectDesc desc = null)
        {
            if (!dying)
            {
                dying = true;
                switch (Owner.Name)
                {
                    case "Arena":
                        {
                            Client.SendPacket(new ArenaDeathPacket
                            {
                                RestartPrice = 100
                            });
                            HP = Client.Character.MaxHitPoints;
                            ApplyConditionEffect(new ConditionEffect
                            {
                                Effect = ConditionEffectIndex.Invulnerable,
                                DurationMS = -1
                            });
                            return;
                        }
                }

                if (Client.Stage == ProtocalStage.Disconnected || resurrecting)
                    return;
                if (CheckResurrection())
                    return;

                if (Client.Character.Dead)
                {
                    Client.Disconnect();
                    return;
                }
                GenerateGravestone();
                if (desc != null)
                    killer = desc.DisplayId;
                switch (killer)
                {
                    case "":
                    case "Unknown":
                        break;

                    default:
                        Owner.BroadcastPacket(new TextPacket
                        {
                            BubbleTime = 0,
                            Stars = -1,
                            Name = "",
                            Text = "{\"key\":\"server.death\",\"tokens\":{\"player\":\"" + Name + "\",\"level\":\"" + Level + "\",\"enemy\":\"" + killer + "\"}}"
                        }, null);
                        break;
                }

                try
                {
                    Manager.Database.DoActionAsync(db =>
                    {
                        Client.Character.Dead = true;
                        SaveToCharacter();
                        db.SaveCharacter(Client.Account, Client.Character);
                        db.Death(Manager.GameData, Client.Account, Client.Character, killer);
                    });
                    if (Owner.Id != -6)
                    {
                        Client.SendPacket(new DeathPacket
                        {
                            AccountId = AccountId.ToString(),
                            CharId = Client.Character.CharacterId,
                            Killer = killer,
                            obf0 = -1,
                            obf1 = -1,
                        });
                        Owner.Timers.Add(new WorldTimer(1000, (w, t) => Client.Disconnect()));
                        Owner.LeaveWorld(this);
                    }
                    else
                        Client.Disconnect();
                }
                catch (Exception e)
                {
                    log.Error(e);
                }
            }
        }

        public void GivePet(PetItem petInfo)
        {
            //if (Name == "ossimc82" || Name == "C453")
            //{
                Pet = new Pet(Manager, petInfo, this);
                Pet.Move(X, Y);
                Owner.EnterWorld(Pet);
            //}
        }

        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            if (projectile.ProjectileOwner is Player ||
                HasConditionEffect(ConditionEffects.Paused) ||
                HasConditionEffect(ConditionEffects.Stasis) ||
                HasConditionEffect(ConditionEffects.Invincible))
                return false;

            return base.HitByProjectile(projectile, time);
        }

        public override void Init(World owner)
        {
            WorldInstance = owner;
            Random rand = new Random();
            int x, y;
            do
            {
                x = rand.Next(0, owner.Map.Width);
                y = rand.Next(0, owner.Map.Height);
            } while (owner.Map[x, y].Region != TileRegion.Spawn);
            Move(x + 0.5f, y + 0.5f);
            tiles = new byte[owner.Map.Width, owner.Map.Height];
            SetNewbiePeriod();
            base.Init(owner);

            if (Client.Character.Pet != null)
                GivePet(Client.Character.Pet);

            if (owner.Id == World.NEXUS_ID || owner.Name == "Vault")
            {
                Client.SendPacket(new Global_NotificationPacket
                {
                    Type = 0,
                    Text = Client.Account.Gifts.Count > 0 ? "giftChestOccupied" : "giftChestEmpty"
                });
            }

            SendAccountList(Locked, Client.LOCKED_LIST_ID);
            SendAccountList(Ignored, Client.IGNORED_LIST_ID);

            WorldTimer accTimer = null;
            owner.Timers.Add(accTimer = new WorldTimer(5000, (w, t) =>
            {
                Manager.Database.DoActionAsync(db =>
                {
                    if (Client == null || Client.Account == null) return;
                    Client.Account = db.GetAccount(AccountId, Manager.GameData);
                    Credits = Client.Account.Credits;
                    CurrentFame = Client.Account.Stats.Fame;
                    Tokens = Client.Account.FortuneTokens;
                    accTimer.Reset();
                    Manager.Logic.AddPendingAction(_ => w.Timers.Add(accTimer), PendingPriority.Creation);
                });
            }));

            WorldTimer pingTimer = null;
            owner.Timers.Add(pingTimer = new WorldTimer(PING_PERIOD, (w, t) =>
            {
                Client.SendPacket(new PingPacket { Serial = pingSerial++ });
                pingTimer.Reset();
                Manager.Logic.AddPendingAction(_ => w.Timers.Add(pingTimer), PendingPriority.Creation);
            }));
            Manager.Database.DoActionAsync(db =>
            {
                db.UpdateLastSeen(Client.Account.AccountId, Client.Character.CharacterId, owner.Name);
                db.LockAccount(Client.Account);
            });

            if (Client.Account.IsGuestAccount)
            {
                owner.Timers.Add(new WorldTimer(1000, (w, t) => Client.Disconnect()));
                Client.SendPacket(new networking.svrPackets.FailurePacket
                {
                    ErrorId = 8,
                    ErrorDescription = "Registration needed."
                });
                Client.SendPacket(new PasswordPromtPacket
                {
                    CleanPasswordStatus = PasswordPromtPacket.REGISTER
                });
                return;
            }

            if (!Client.Account.VerifiedEmail && Program.Verify)
            {
                Client.SendPacket(new VerifyEmailDialogPacket());
                owner.Timers.Add(new WorldTimer(1000, (w, t) => Client.Disconnect()));
                return;
            }
            CheckSetTypeSkin();
        }

        public void SaveToCharacter()
        {
            Char chr = Client.Character;
            chr.Exp = Experience;
            chr.Level = Level;
            chr.Tex1 = Texture1;
            chr.Tex2 = Texture2;
            chr.Pet = Pet?.Info;
            chr.CurrentFame = Fame;
            chr.HitPoints = HP;
            chr.MagicPoints = MP;
            if (Inventory.Length == 12)
                chr.Equipment = Inventory.Select(_ => _ == null ? (short)-1 : _.ObjectType).ToArray();
            else if (Inventory.Length == 20)
            {
                short[] equip = Inventory.Select(_ => _ == null ? (short)-1 : _.ObjectType).ToArray();
                Array.Resize(ref equip, 12);
                chr.Equipment = equip;
                equip = Inventory.Select(_ => _ == null ? (short)-1 : _.ObjectType).Reverse().ToArray();
                Array.Resize(ref equip, 8);
                chr.Backpack = equip;
            }
            chr.MaxHitPoints = Stats[0];
            chr.MaxMagicPoints = Stats[1];
            chr.Attack = Stats[2];
            chr.Defense = Stats[3];
            chr.Speed = Stats[4];
            chr.HpRegen = Stats[5];
            chr.MpRegen = Stats[6];
            chr.Dexterity = Stats[7];
            chr.HealthStackCount = HealthPotions;
            chr.MagicStackCount = MagicPotions;
            chr.HasBackpack = HasBackpack;
            chr.Skin = PlayerSkin;
            chr.XpBoosted = XpBoosted;
            chr.XpTimer = (int)XpBoostTimeLeft;
            chr.LDTimer = (int)LootDropBoostTimeLeft;
            chr.LTTimer = (int)LootTierBoostTimeLeft;
        }

        public void Teleport(RealmTime time, TeleportPacket packet)
        {
            Entity obj = Client.Player.Owner.GetEntity(packet.ObjectId);
            try
            {
                if (obj == null) return;
                if (!TPCooledDown())
                {
                    SendError("Player.teleportCoolDown");
                    return;
                }
                if (obj.HasConditionEffect(ConditionEffects.Invisible))
                {
                    SendError("server.no_teleport_to_invisible");
                    return;
                }
                if (obj.HasConditionEffect(ConditionEffects.Paused))
                {
                    SendError("server.no_teleport_to_paused");
                    return;
                }
                if (!(obj as Player).NameChosen)
                {
                    SendError("server.teleport_needs_name");
                    return;
                }
                if (obj.Id == Id)
                {
                    SendError("server.teleport_to_self");
                    return;
                }
                if (!Owner.AllowTeleport)
                {
                    SendError(GetLanguageString("server.no_teleport_in_realm", new KeyValuePair<string, object>[1]
                    {
                        new KeyValuePair<string, object>("realm", Owner.Name)
                    }));
                    return;
                }

                SetTPDisabledPeriod();
                Move(obj.X, obj.Y);
                Pet?.Move(obj.X, obj.X);
                FameCounter.Teleport();
                SetNewbiePeriod();
                UpdateCount++;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SendError("player.cannotTeleportTo");
                return;
            }
            Owner.BroadcastPacket(new GotoPacket
            {
                ObjectId = Id,
                Position = new Position
                {
                    X = X,
                    Y = Y
                }
            }, null);
            Owner.BroadcastPacket(new ShowEffectPacket
            {
                EffectType = EffectType.Teleport,
                TargetId = Id,
                PosA = new Position
                {
                    X = X,
                    Y = Y
                },
                Color = new ARGB(0xFFFFFFFF)
            }, null);
        }

        public override void Tick(RealmTime time)
        {
            try
            {
                if (Manager.Clients.Count(_ => _.Value.Id == Client.Id) == 0)
                {
                    if (Owner != null)
                        Owner.LeaveWorld(this);
                    else
                        WorldInstance.LeaveWorld(this);
                    Manager.Database.DoActionAsync(db => db.UnlockAccount(Client.Account));
                    return;
                }
                if (Client.Stage == ProtocalStage.Disconnected || (!Client.Account.VerifiedEmail && Program.Verify))
                {
                    if (Owner != null)
                        Owner.LeaveWorld(this);
                    else
                        WorldInstance.LeaveWorld(this);
                    Manager.Database.DoActionAsync(db => db.UnlockAccount(Client.Account));
                    return;
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }

            if (Stats != null && Boost != null)
            {
                MaxHP = Stats[0] + Boost[0];
                MaxMP = Stats[1] + Boost[1];
            }

            if (!KeepAlive(time)) return;

            if (Boost == null) CalcBoost();

            TradeHandler?.Tick(time);
            HandleRegen(time);
            HandleQuest(time);
            HandleEffects(time);
            HandleGround(time);
            HandleBoosts();

            FameCounter.Tick(time);

            //if(pingSerial > 5)
            //    if (!Enumerable.Range(UpdatesSend, 5000).Contains(UpdatesReceived))
            //        Client.Disconnect();

            if (MP < 0) MP = 0;

            /* try
                * {
                *     psr.Database.SaveCharacter(psr.Account, psr.Character);
                *     UpdateCount++;
                * }
                * catch (ex)
                * {
                * }
            */

            try
            {
                if (Owner != null)
                {
                    SendUpdate(time);
                    if (!Owner.IsPassable((int)X, (int)Y) && Client.Account.Rank < 2)
                    {
                        log.Fatal($"Player {Name} No-Cliped at position: {X}, {Y}");
                        Client.Disconnect();
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            try
            {
                SendNewTick(time);
            }
            catch (Exception e)
            {
                log.Error(e);
            }

            if (HP < 0 && !dying)
            {
                Death("Unknown");
                return;
            }

            base.Tick(time);
        }

        private bool CheckResurrection()
        {
            for (int i = 0; i < 4; i++)
            {
                Item item = Inventory[i];
                if (item == null || !item.Resurrects) continue;

                HP = Stats[0] + Stats[0];
                MP = Stats[1] + Stats[1];
                Inventory[i] = null;
                Owner.BroadcastPacket(new TextPacket
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "",
                    Text = string.Format("{0}'s {1} breaks and he disappears", Name, item.ObjectId)
                }, null);
                Client.Reconnect(new ReconnectPacket
                {
                    Host = "",
                    Port = Program.Settings.GetValue<int>("port"),
                    GameId = World.NEXUS_ID,
                    Name = "Nexus",
                    Key = Empty<byte>.Array,
                });

                resurrecting = true;
                return true;
            }
            return false;
        }

        private void GenerateGravestone()
        {
            int maxed = 0;
            foreach (XElement i in Manager.GameData.ObjectTypeToElement[ObjectType].Elements("LevelIncrease"))
            {
                int limit =
                    int.Parse(Manager.GameData.ObjectTypeToElement[ObjectType].Element(i.Value).Attribute("max").Value);
                int idx = StatsManager.StatsNameToIndex(i.Value);
                if (Stats[idx] >= limit)
                    maxed++;
            }

            ushort objType;
            int? time;
            switch (maxed)
            {
                case 8:
                    objType = 0x0735;
                    time = null;
                    break;

                case 7:
                    objType = 0x0734;
                    time = null;
                    break;

                case 6:
                    objType = 0x072b;
                    time = null;
                    break;

                case 5:
                    objType = 0x072a;
                    time = null;
                    break;

                case 4:
                    objType = 0x0729;
                    time = null;
                    break;

                case 3:
                    objType = 0x0728;
                    time = null;
                    break;

                case 2:
                    objType = 0x0727;
                    time = null;
                    break;

                case 1:
                    objType = 0x0726;
                    time = null;
                    break;

                default:
                    if (Level <= 1)
                    {
                        objType = 0x0723;
                        time = 30 * 1000;
                    }
                    else if (Level < 20)
                    {
                        objType = 0x0724;
                        time = 60 * 1000;
                    }
                    else
                    {
                        objType = 0x0725;
                        time = 5 * 60 * 1000;
                    }
                    break;
            }
            StaticObject obj = new StaticObject(Manager, objType, time, true, time != null, false);
            obj.Move(X, Y);
            obj.Name = Name;
            Owner.EnterWorld(obj);
        }

        private void HandleRegen(RealmTime time)
        {
            if (HP == Stats[0] + Boost[0] || !CanHpRegen())
                hpRegenCounter = 0;
            else
            {
                hpRegenCounter += StatsManager.GetHPRegen() * time.thisTickTimes / 1000f;
                int regen = (int)hpRegenCounter;
                if (regen > 0)
                {
                    HP = Math.Min(Stats[0] + Boost[0], HP + regen);
                    hpRegenCounter -= regen;
                    UpdateCount++;
                }
            }

            if (MP == Stats[1] + Boost[1] || !CanMpRegen())
                mpRegenCounter = 0;
            else
            {
                mpRegenCounter += StatsManager.GetMPRegen() * time.thisTickTimes / 1000f;
                int regen = (int)mpRegenCounter;
                if (regen > 0)
                {
                    MP = Math.Min(Stats[1] + Boost[1], MP + regen);
                    mpRegenCounter -= regen;
                    UpdateCount++;
                }
            }
        }

        public new void Dispose()
        {
            tiles = null;
            Guild.Remove(this);
        }
    }
}