using db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.entities.player;
using wServer.realm.worlds;

namespace wServer.networking.handlers
{
    internal class PetYardCommandHandler : PacketHandlerBase<PetYardCommandPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.PETYARDCOMMAND; }
        }

        protected override void HandlePacket(Client client, PetYardCommandPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
            {
                switch (packet.CommandId)
                {
                    case PetYardCommandPacket.UPGRADE_PET_YARD:
                        UpgradePetYard(client, packet);
                        break;
                    case PetYardCommandPacket.FEED_PET:
                        FeedPet(client, packet);
                        break;
                    case PetYardCommandPacket.FUSE_PET:
                        FusePet(client, packet);
                        break;
                }

                if (client.Player.Pet != null)
                    client.Player.Pet.PlayerOwner = client.Player;
            });
        }

        private void UpgradePetYard(Client client, PetYardCommandPacket packet)
        {
            if (client.Account.PetYardType > 4)
            {
                client.Player.SendError("Your PetYard is already at max.");
                return;
            }
            using (Database db = new Database())
            {
                if (packet.Currency == CurrencyType.Fame)
                {
                    switch (client.Account.PetYardType)
                    {
                        case 1:
                            if (!TryDeduct(packet.Currency, client.Player, 500)) return;
                            break;
                        case 2:
                            if (!TryDeduct(packet.Currency, client.Player, 2000)) return;
                            break;
                        case 3:
                            if (!TryDeduct(packet.Currency, client.Player, 25000)) return;
                            break;
                        case 4:
                            if (!TryDeduct(packet.Currency, client.Player, 50000)) return;
                            break;
                    }
                }

                if (packet.Currency == CurrencyType.Gold)
                {
                    switch (client.Account.PetYardType)
                    {
                        case 1:
                            if (!TryDeduct(packet.Currency, client.Player, 150)) return;
                            break;
                        case 2:
                            if (!TryDeduct(packet.Currency, client.Player, 400)) return;
                            break;
                        case 3:
                            if (!TryDeduct(packet.Currency, client.Player, 1200)) return;
                            break;
                        case 4:
                            if (!TryDeduct(packet.Currency, client.Player, 2000)) return;
                            break;
                    }
                }
                client.Account.PetYardType++;
                var cmd = db.CreateQuery();
                cmd.CommandText = "UPDATE accounts SET petYardType=@type WHERE id=@accId;";
                cmd.Parameters.AddWithValue("@type", client.Account.PetYardType);
                cmd.Parameters.AddWithValue("@accId", client.Account.AccountId);
                cmd.ExecuteNonQuery();
                client.SendPacket(new UpgradePetYardResultPacket
                {
                    Type = client.Account.PetYardType
                });
            }
        }

        private void FeedPet(Client client, PetYardCommandPacket packet)
        {
            try
            {
                Pet pet = (client.Player.Owner as PetYard).FindPetById(packet.PetId1);

                if (packet.Currency == CurrencyType.Fame)
                {
                    switch (pet.PetRarity)
                  	{
                  		case Rarity.Common:
                            if (!TryDeduct(packet.Currency, client.Player, 10)) return;
                            break;
                        case Rarity.Uncommon:
                            if (!TryDeduct(packet.Currency, client.Player, 30)) return;
                            break;
                        case Rarity.Rare:
                            if (!TryDeduct(packet.Currency, client.Player, 100)) return;
                            break;
                        case Rarity.Legendary:
                            if (!TryDeduct(packet.Currency, client.Player, 350)) return;
                            break;
                        case Rarity.Divine:
                            if (!TryDeduct(packet.Currency, client.Player, 1000)) return;
                            break;
                        default:
                            throw new Exception("Invalid pet rarity");
                  	}
                }

                if (packet.Currency == CurrencyType.Gold)
                {
                    switch (pet.PetRarity)
                    {
                        case Rarity.Common:
                            if (!TryDeduct(packet.Currency, client.Player, 5)) return;
                            break;
                        case Rarity.Uncommon:
                            if (!TryDeduct(packet.Currency, client.Player, 12)) return;
                            break;
                        case Rarity.Rare:
                            if (!TryDeduct(packet.Currency, client.Player, 30)) return;
                            break;
                        case Rarity.Legendary:
                            if (!TryDeduct(packet.Currency, client.Player, 60)) return;
                            break;
                        case Rarity.Divine:
                            if (!TryDeduct(packet.Currency, client.Player, 150)) return;
                            break;
                        default:
                            throw new Exception("Invalid pet rarity");
                    }
                }

                client.SendPacket(new BuyResultPacket
                {
                    Result = 0,
                    Message = "{\"key\":\"server.buy_success\"}"
                });
                IFeedable tofeed = client.Player.Inventory[packet.ObjectSlot.SlotId];
                client.Player.Inventory[packet.ObjectSlot.SlotId] = null;
                pet.Feed(tofeed);
                client.Player.UpdateCount++;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                client.Player.SendError("Internal server error: " + ex.Message);
            }
        }

        private void FusePet(Client client, PetYardCommandPacket packet)
        {
            Pet pet1 = (client.Player.Owner as PetYard).FindPetById(packet.PetId1);
            Pet pet2 = (client.Player.Owner as PetYard).FindPetById(packet.PetId2);

            if (pet1.PetRarity != pet2.PetRarity) return;

            if (packet.Currency == CurrencyType.Fame)
            {
                switch (pet1.PetRarity)
                {
                    case Rarity.Common:
                        if (!TryDeduct(packet.Currency, client.Player, 300)) return;
                        break;
                    case Rarity.Uncommon:
                        if (!TryDeduct(packet.Currency, client.Player, 1000)) return;
                        break;
                    case Rarity.Rare:
                        if (!TryDeduct(packet.Currency, client.Player, 4000)) return;
                        break;
                    case Rarity.Legendary:
                        if (!TryDeduct(packet.Currency, client.Player, 15000)) return;
                        break;
                    case Rarity.Divine:
                        return;
                    default:
                        throw new Exception("Invalid pet rarity");
                }
            }

            if (packet.Currency == CurrencyType.Gold)
            {
                switch (pet1.PetRarity)
                {
                    case Rarity.Common:
                        if (!TryDeduct(packet.Currency, client.Player, 100)) return;
                        break;
                    case Rarity.Uncommon:
                        if (!TryDeduct(packet.Currency, client.Player, 240)) return;
                        break;
                    case Rarity.Rare:
                        if (!TryDeduct(packet.Currency, client.Player, 600)) return;
                        break;
                    case Rarity.Legendary:
                        if (!TryDeduct(packet.Currency, client.Player, 1800)) return;
                        break;
                    case Rarity.Divine:
                        return;
                    default:
                        throw new Exception("Invalid pet rarity");
                }
            }

            client.SendPacket(new BuyResultPacket
            {
                Result = 0,
                Message = "{\"key\":\"server.buy_success\"}"
            });

            if (AbilityUnlock(pet1, pet2))
            {
                Fuse(client, pet1, pet2);

                client.SendPacket(new RemovePetFromListPacket
                {
                    PetId = pet2.PetId
                });

                if (pet1.PetRarity == Rarity.Uncommon)
                {
                    client.SendPacket(new NewAbilityUnlockedPacket
                    {
                        Type = pet1.SecondPetLevel.Ability
                    });
                }
                else
                {
                    client.SendPacket(new NewAbilityUnlockedPacket
                    {
                        Type = pet1.ThirdPetLevel.Ability
                    });
                }
            }
            else
            {
                int oldSkin = (client.Player.Owner as PetYard).FindPetById(packet.PetId1).SkinId;
                Evolve(client, pet1, pet2);

                client.SendPacket(new RemovePetFromListPacket
                {
                    PetId = pet2.PetId
                });

                client.SendPacket(new PetEvolveResultPacket
                {
                    PetId1 = packet.PetId1,
                    SkinId1 = oldSkin,
                    SkinId2 = (client.Player.Owner as PetYard).FindPetById(packet.PetId1).SkinId
                });
            }

            pet1.UpdateCount++;
            pet1.UpdateNeeded = true;
            pet2.Owner.LeaveWorld(pet2);
        }

        private void Evolve(Client client, Pet pet1, Pet pet2)
        {
            int l1 = pet1.FirstPetLevel.Level == 1 ? 1 : pet1.FirstPetLevel.Level / 2;
            int l2 = pet2.FirstPetLevel.Level == 1 ? 1 : pet2.FirstPetLevel.Level / 2;
            int level = l1 + l2 + 20;

            PetStruct s = null;
            pet1.EvolveResult(level, (int)pet1.PetRarity + 1, ref s);

            if (s == null) return;
            PetSkin skin = client.Manager.GameData.IdToPetSkin[s.DefaultSkin];

            client.Manager.Database.DoActionAsync(db =>
            {
                var cmd = db.CreateQuery();
                cmd.CommandText = "UPDATE pets SET rarity=rarity+1, maxLevel=@level, skinName=@skinName, skin=@skinId, objType=@objType WHERE petId=@petId1 AND accId=@accId;";
                cmd.Parameters.AddWithValue("@accId", client.Account.AccountId);
                cmd.Parameters.AddWithValue("@petId1", pet1.PetId);
                cmd.Parameters.AddWithValue("@level", level);
                cmd.Parameters.AddWithValue("@skinName", skin.DisplayId);
                cmd.Parameters.AddWithValue("@skinId", skin.ObjectType);
                cmd.Parameters.AddWithValue("@objType", s.ObjectType);
                cmd.ExecuteNonQuery();

                cmd = db.CreateQuery();
                cmd.CommandText = "DELETE FROM pets WHERE accId=@accId AND petId=@petId2;";
                cmd.Parameters.AddWithValue("@accId", client.Account.AccountId);
                cmd.Parameters.AddWithValue("@petId2", pet2.PetId);
                cmd.ExecuteNonQuery();
            });
        }

        private void Fuse(Client client, Pet pet1, Pet pet2)
        {
            int l1 = pet1.FirstPetLevel.Level == 1 ? 1 : pet1.FirstPetLevel.Level / 2;
            int l2 = pet2.FirstPetLevel.Level == 1 ? 1 : pet2.FirstPetLevel.Level / 2;
            int level = l1 + l2 + 20;

            if (level > 100) level = 100;

            client.Manager.Database.DoActionAsync(db =>
            {
                var cmd = db.CreateQuery();
                cmd.CommandText = "UPDATE pets SET rarity=rarity+1, maxLevel=@level WHERE petId=@petId1 AND accId=@accId;";
                cmd.Parameters.AddWithValue("@accId", client.Account.AccountId);
                cmd.Parameters.AddWithValue("@petId1", pet1.PetId);
                cmd.Parameters.AddWithValue("@level", level);
                cmd.ExecuteNonQuery();

                cmd = db.CreateQuery();
                cmd.CommandText = "DELETE FROM pets WHERE accId=@accId AND petId=@petId2;";
                cmd.Parameters.AddWithValue("@accId", client.Account.AccountId);
                cmd.Parameters.AddWithValue("@petId2", pet2.PetId);
                cmd.ExecuteNonQuery();
            });

            pet1.FuseResult(level, (int)pet1.PetRarity + 1);
        }

        private bool AbilityUnlock(Pet pet1, Pet pet2)
        {
            return pet1.PetRarity == Rarity.Common && pet2.PetRarity == Rarity.Common || pet1.PetRarity == Rarity.Rare && pet2.PetRarity == Rarity.Rare;
        }

        private bool TryDeduct(CurrencyType currency, Player player, int price)
        {
            using (Database db = new Database())
            {
                Account acc = player.Client.Account;
                db.ReadStats(acc);

                if (currency == CurrencyType.Fame)
                {
                    if (acc.Stats.Fame < price)
                    {
                        player.SendError("{\"key\":\"server.not_enough_fame\"}");
                        return false;
                    }

                    db.UpdateFame(acc, -price);
                }

                if (currency == CurrencyType.Gold)
                {
                    if (acc.Credits < price)
                    {
                        player.SendError("{\"key\":\"server.not_enough_gold\"}");
                        return false;
                    }

                    db.UpdateCredit(acc, -price);
                }

                return true;
            }
        }
    }
}
