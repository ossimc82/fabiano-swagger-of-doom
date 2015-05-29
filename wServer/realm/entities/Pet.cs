#region

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using wServer.realm.entities.player;
using wServer.networking.svrPackets;
using wServer.realm.worlds;

#endregion

namespace wServer.realm.entities
{
    public class Pet : Entity, IPlayer
    {
        private Random rand;
        private Position? spawn;

        public Pet(RealmManager manager, PetItem petData, Player playerOwner)
            : base(manager, (ushort)petData.Type, true, true)
        {
            rand = new Random();
            PlayerOwner = playerOwner;
            Info = petData;

            try
            {
                if (petData.InstanceId != -1)
                {
                    FirstPetLevel = new PetLevel(AbilityType.First,
                        Utils.GetEnumByName<Ability>(Utils.GetEnumName<Ability>(petData.Abilities[0].Type)),
                        petData.Abilities[0].Points, petData.Abilities[0].Power, this);

                    SecondPetLevel = new PetLevel(AbilityType.Second,
                        Utils.GetEnumByName<Ability>(Utils.GetEnumName<Ability>(petData.Abilities[1].Type)),
                        petData.Abilities[1].Points, petData.Abilities[1].Power, this);

                    ThirdPetLevel = new PetLevel(AbilityType.Third,
                        Utils.GetEnumByName<Ability>(Utils.GetEnumName<Ability>(petData.Abilities[2].Type)),
                        petData.Abilities[2].Points, petData.Abilities[2].Power, this);

                    Size = manager.GameData.TypeToPet[(ushort)petData.Type].Size;
                    PetRarity = (Rarity)petData.Rarity;
                    PetFamily = manager.GameData.TypeToPet[(ushort)petData.Type].PetFamily;
                    MaximumLevel = petData.MaxAbilityPower;
                    UpdateNeeded = true;
                }
                Skin = petData.SkinName;
                SkinId = petData.Skin;
                PetId = petData.InstanceId;
                IsPet = true;
            }
            catch (Exception e)
            {
                if (PlayerOwner != null)
                    PlayerOwner.SendError(
                        String.Format(
                            "An error ocurred while loading your pet data, please report this to an Admin: {0}",
                            e.Message));
            }
        }

        public int PetId { get; private set; }
        public int SkinId { get; private set; }
        public int MaximumLevel { get; private set; }

        public string Skin { get; private set; }

        public bool UpdateNeeded { get; set; }

        public PetItem Info { get; private set; }
        public PetLevel FirstPetLevel { get; private set; }
        public PetLevel SecondPetLevel { get; private set; }
        public PetLevel ThirdPetLevel { get; private set; }
        public Player PlayerOwner { get; set; }

        public Position SpawnPoint
        {
            get { return spawn ?? new Position(X, Y); }
        }

        public Rarity PetRarity { get; private set; }
        public Family PetFamily { get; private set; }

        public void Feed(IFeedable petFoodNOMNOMNOM)
        {
            FirstPetLevel.Incease(petFoodNOMNOMNOM);
            SecondPetLevel.Incease(petFoodNOMNOMNOM);
            ThirdPetLevel.Incease(petFoodNOMNOMNOM);

            Manager.Database.DoActionAsync(db =>
            {
                MySqlCommand cmd = db.CreateQuery();
                cmd.CommandText =
                    "UPDATE pets SET levels=@newLevels, xp=@newXp WHERE petId=@petId AND accId=@accId";
                cmd.Parameters.AddWithValue("@petId", PetId);
                cmd.Parameters.AddWithValue("@accId", Owner.Players.ToArray()[0].Value.AccountId);
                cmd.Parameters.AddWithValue("@newLevels",
                    String.Format("{0}, {1}, {2}", FirstPetLevel.Level, SecondPetLevel.Level, ThirdPetLevel.Level));
                cmd.Parameters.AddWithValue("@newXp",
                    String.Format("{0}, {1}, {2}", FirstPetLevel.Power, SecondPetLevel.Power, ThirdPetLevel.Power));
                cmd.ExecuteNonQuery();
            });

            UpdateNeeded = true;
            UpdateCount++;

            this.Owner.Players.ToArray()[0].Value.Client.SendPacket(new UpdatePacket
            {
                Tiles = new UpdatePacket.TileData[0],
                NewObjects = new ObjectDef[1] { this.ToDefinition() },
                RemovedObjectIds = new int[0]
            });
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            if (Owner is PetYard)
            {
                if (UpdateNeeded)
                {
                    base.ExportStats(stats);
                    stats[StatsType.Skin] = SkinId;
                    stats[StatsType.PetId] = PetId;
                    stats[StatsType.PetSkin] = Skin;
                    stats[StatsType.PetType] = (int)ObjectType;
                    stats[StatsType.PetRarity] = (int)PetRarity;
                    stats[StatsType.PetMaximumLevel] = MaximumLevel;
                    stats[StatsType.PetNothing] = Skin.GetHashCode();
                    stats[StatsType.PetPoints0] = FirstPetLevel.Power;
                    stats[StatsType.PetPoints1] = SecondPetLevel.Power;
                    stats[StatsType.PetPoints2] = ThirdPetLevel.Power;
                    stats[StatsType.PetLevel0] = FirstPetLevel.Level;
                    stats[StatsType.PetLevel1] = SecondPetLevel.Level;
                    stats[StatsType.PetLevel2] = ThirdPetLevel.Level;
                    stats[StatsType.PetAbilityType0] = (int)FirstPetLevel.Ability;
                    stats[StatsType.PetAbilityType1] = (int)SecondPetLevel.Ability;
                    stats[StatsType.PetAbilityType2] = (int)ThirdPetLevel.Ability;
                    UpdateNeeded = false;
                }
            }
            else
            {
                base.ExportStats(stats);
                stats[StatsType.Skin] = SkinId;
                stats[StatsType.PetId] = PetId;
                stats[StatsType.PetSkin] = Skin;
                stats[StatsType.PetType] = (int)ObjectType;
                stats[StatsType.PetRarity] = (int)PetRarity;
                stats[StatsType.PetMaximumLevel] = MaximumLevel;
                stats[StatsType.PetNothing] = Skin.GetHashCode();
                stats[StatsType.PetPoints0] = FirstPetLevel.Power;
                stats[StatsType.PetPoints1] = SecondPetLevel.Power;
                stats[StatsType.PetPoints2] = ThirdPetLevel.Power;
                stats[StatsType.PetLevel0] = FirstPetLevel.Level;
                stats[StatsType.PetLevel1] = SecondPetLevel.Level;
                stats[StatsType.PetLevel2] = ThirdPetLevel.Level;
                stats[StatsType.PetAbilityType0] = (int)FirstPetLevel.Ability;
                stats[StatsType.PetAbilityType1] = (int)SecondPetLevel.Ability;
                stats[StatsType.PetAbilityType2] = (int)ThirdPetLevel.Ability;
            }
        }

        public PetLevel GetPetLevelFromAbility(Ability ability, bool checkRarity)
        {
            if (checkRarity)
            {
                if (FirstPetLevel.Ability == ability) return FirstPetLevel;
                else if (SecondPetLevel.Ability == ability && PetRarity >= Rarity.Uncommon) return SecondPetLevel;
                else if (ThirdPetLevel.Ability == ability && PetRarity >= Rarity.Legendary) return ThirdPetLevel;
                return null;
            }
            else
            {
                if (FirstPetLevel.Ability == ability) return FirstPetLevel;
                else if (SecondPetLevel.Ability == ability) return SecondPetLevel;
                else if (ThirdPetLevel.Ability == ability) return ThirdPetLevel;
                return null;
            }
        }

        public override void Tick(RealmTime time)
        {
            if (spawn == null) spawn = new Position(X, Y);
            base.Tick(time);
        }

        public static async void Create(RealmManager manager, Player player, Item egg)
        {
            await manager.Database.DoActionAsync(db =>
            {
                PetStruct petStruct = GetPetStruct(manager, egg.Family, (Rarity)egg.Rarity);
                PetSkin skin = manager.GameData.IdToPetSkin[petStruct.DefaultSkin];

                PetItem item = new PetItem
                {
                    InstanceId = db.GetNextPetId(player.AccountId),
                    Rarity = (int)egg.Rarity,
                    SkinName = skin.DisplayId,
                    Skin = skin.ObjectType,
                    Type = petStruct.ObjectType,
                    Abilities = GetPetAbilites(egg, petStruct),
                };

                switch (item.Rarity)
                {
                    case 1:
                        item.MaxAbilityPower = 50;
                        item.Abilities[0].Power = 30;
                        item.Abilities[0].Points = 2080;
                        item.Abilities[1].Power = 11;
                        item.Abilities[1].Points = 290;
                        item.Abilities[2].Power = 1;
                        item.Abilities[2].Points = 0;
                        break;
                    case 2:
                        item.MaxAbilityPower = 70;
                        item.Abilities[0].Power = 50;
                        item.Abilities[0].Points = 10607;
                        item.Abilities[1].Power = 30;
                        item.Abilities[1].Points = 2080;
                        item.Abilities[2].Power = 1;
                        item.Abilities[2].Points = 0;
                        break;
                    case 3:
                        item.MaxAbilityPower = 90;
                        item.Abilities[0].Power = 70;
                        item.Abilities[0].Points = 50355;
                        item.Abilities[1].Power = 50;
                        item.Abilities[1].Points = 10607;
                        item.Abilities[2].Power = 30;
                        item.Abilities[2].Points = 2080;
                        break;
                    case 4:
                        item.MaxAbilityPower = 100;
                        item.Abilities[0].Power = 90;
                        item.Abilities[0].Points = 235610;
                        item.Abilities[1].Power = 70;
                        item.Abilities[1].Points = 50354;
                        item.Abilities[2].Power = 50;
                        item.Abilities[2].Points = 10607;
                        break;
                    default:
                        item.MaxAbilityPower = 30;
                        item.Abilities[0].Power = 1;
                        item.Abilities[0].Points = 0;
                        item.Abilities[1].Power = 1;
                        item.Abilities[1].Points = 0;
                        item.Abilities[2].Power = 1;
                        item.Abilities[2].Points = 0;
                        break;
                }

                Pet pet = new Pet(manager, item, null);
                int x;
                int y;
                Random rand = new Random((int)DateTime.Now.Ticks);
                do
                {
                    x = rand.Next(0, player.Owner.Map.Width);
                    y = rand.Next(0, player.Owner.Map.Height);
                } while (player.Owner.Map[x, y].Region != TileRegion.Spawn);
                pet.Move(x + 0.5f, y + 0.5f);
                db.CreatePet(player.Client.Account, item);
                player.Owner.EnterWorld(pet);
                player.Client.SendPacket(new HatchEggPacket
                {
                    PetName = skin.DisplayId,
                    PetSkinId = skin.ObjectType
                });
                player.Client.SendPacket(new UpdatePacket
                {
                    Tiles = new UpdatePacket.TileData[0],
                    NewObjects = new ObjectDef[1] { pet.ToDefinition() },
                    RemovedObjectIds = new int[0]
                });
            });
        }

        private static List<AbilityItem> GetPetAbilites(Item egg, PetStruct petStruct)
        {
            List<Ability> abilities = new List<Ability>()
            {
                Ability.AttackClose,
                Ability.AttackFar,
                Ability.AttackMid,
                Ability.Decoy,
                Ability.Electric,
                Ability.Heal,
                Ability.MagicHeal,
                Ability.RisingFury,
                Ability.Savage
            };

            Random rand = new Random((int)DateTime.Now.Ticks);
            List<AbilityItem> ret = new List<AbilityItem>(3);
            int power = 1;

            switch (egg.Rarity)
            {
                case Rarity.Uncommon:
                    power = 30;
                    break;
                case Rarity.Rare:
                    power = 50;
                    break;
                case Rarity.Legendary:
                    power = 70;
                    break;
            }

            for (int i = 0; i < 3; i++)
			{
                if (i == 0 && petStruct.FirstAbility != null)
                {
                    ret.Add(new AbilityItem
                    {
                        Power = power,
                        Points = 0,
                        Type = (int)petStruct.FirstAbility
                    });

                    abilities.Remove((Ability)petStruct.FirstAbility);
                }
                else
                {
                    Ability ability = abilities[rand.Next(abilities.Count)];
                    ret.Add(new AbilityItem
                    {
                        Power = power,
                        Points = 0,
                        Type = (int)ability
                    });

                    if (ability == Ability.AttackClose || ability == Ability.AttackFar || ability == Ability.AttackMid)
                    {
                        abilities.Remove(Ability.AttackClose);
                        abilities.Remove(Ability.AttackFar);
                        abilities.Remove(Ability.AttackMid);
                    }
                    else
                        abilities.Remove(ability);
                }
			}
            

            return ret;
        }

        public static PetStruct GetPetStruct(RealmManager manager, Family? petFamily, Rarity rarity)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            List<PetStruct> structs = new List<PetStruct>();

            Rarity petRarity = rarity;

            if (rarity == Rarity.Uncommon)
                petRarity = Rarity.Common;
            else if (rarity == Rarity.Legendary)
                petRarity = Rarity.Rare;

            foreach (var x in manager.GameData.TypeToPet)
            {
                if (petFamily == null && x.Value.PetRarity == petRarity)
                {
                    structs.Add(x.Value);
                    continue;
                }
                if (x.Value.PetFamily == petFamily && x.Value.PetRarity == petRarity)
                    structs.Add(x.Value);
            }

            PetStruct petStruct = structs[rand.Next(structs.Count)];
            return petStruct;
        }

        public void FuseResult(int level, int rarity)
        {
            this.MaximumLevel = level;
            this.PetRarity = (Rarity)rarity;
        }

        public void EvolveResult(int level, int rarity, ref PetStruct newPetStruct)
        {
            FuseResult(level, rarity);
            PetStruct s = GetPetStruct(Manager, PetFamily, (Rarity)rarity);
            this.Skin = Manager.GameData.IdToPetSkin[s.DefaultSkin].DisplayId;
            this.SkinId = Manager.GameData.IdToPetSkin[s.DefaultSkin].ObjectType;
            newPetStruct = s;
        }

        public void Damage(int dmg, Entity chr) { }

        public bool IsVisibleToEnemy()
        {
            //Todo: Implement decoy here
            if (HasConditionEffect(global::ConditionEffectIndex.Stasis)) return false;
            return true;
        }

        public PetLevel[] GetLevels()
        {
            return new[]
            {
                FirstPetLevel,
                SecondPetLevel,
                ThirdPetLevel
            };
        }
    }
}