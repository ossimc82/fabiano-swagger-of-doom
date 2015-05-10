#region

using db;
using MySql.Data.MySqlClient;
using System;
using wServer.networking.cliPackets;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.entities.player;

#endregion

namespace wServer.networking.handlers
{
    internal class UseItemHandler : PacketHandlerBase<UseItemPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.USEITEM; }
        }

        protected override void HandlePacket(Client client, UseItemPacket packet)
        {
            if (client.Player.Owner == null) return;

            client.Manager.Logic.AddPendingAction(async t =>
            {
                IContainer container = client.Player.Owner.GetEntity(packet.SlotObject.ObjectId) as IContainer;
                Item item;
                if (packet.SlotObject.SlotId == 254)
                {
                    item = client.Player.Manager.GameData.Items[packet.SlotObject.ObjectType];

                    if (item.ObjectId != "Health Potion")
                    {
                        log.FatalFormat("Cheat engine detected for player {0},\nItem should be a Health Potion, but its {1}.",
                            client.Player.Name, item.ObjectId);
                        foreach (Player player in client.Player.Owner.Players.Values)
                            if (player.Client.Account.Rank >= 2)
                                player.SendInfo(String.Format("Cheat engine detected for player {0},\nItem should be a Health Potion, but its {1}.",
                            client.Player.Name, item.ObjectId));
                        client.Disconnect();
                        return;
                    }

                    if (client.Player.HealthPotions > 0)
                        client.Player.HealthPotions--;
                    else
                    {
                        if (client.Account.Credits > client.Player.HPPotionPrice)
                        {
                            switch (client.Player.HPPotionPrice)
                            {
                                case 5:
                                    if (client.Player.HPFirstPurchaseTime)
                                    {
                                        client.Player.HPPotionPrice = 5;
                                        client.Player.HPFirstPurchaseTime = false;
                                    }
                                    client.Player.HPPotionPrice = 10;
                                    break;
                                case 10:
                                    client.Player.HPPotionPrice = 20;
                                    break;
                                case 20:
                                    client.Player.HPPotionPrice = 40;
                                    break;
                                case 40:
                                    client.Player.HPPotionPrice = 80;
                                    break;
                                case 80:
                                    client.Player.HPPotionPrice = 120;
                                    break;
                                case 120:
                                    client.Player.HPPotionPrice = 200;
                                    break;
                                case 200:
                                    client.Player.HPPotionPrice = 300;
                                    break;
                                case 300:
                                    client.Player.HPPotionPrice = 450;
                                    break;
                                case 450:
                                    client.Player.HPPotionPrice = 600;
                                    break;
                                case 600:
                                    break;
                            }
                            client.Player.Owner.Timers.Add(new WorldTimer(8000, (world, j) =>
                            {
                                switch (client.Player.HPPotionPrice)
                                {
                                    case 5:
                                        break;
                                    case 10:
                                        client.Player.HPFirstPurchaseTime = true;
                                        client.Player.HPPotionPrice = 5;
                                        break;
                                    case 20:
                                        client.Player.HPPotionPrice = 10;
                                        break;
                                    case 40:
                                        client.Player.HPPotionPrice = 20;
                                        break;
                                    case 80:
                                        client.Player.HPPotionPrice = 40;
                                        break;
                                    case 120:
                                        client.Player.HPPotionPrice = 80;
                                        break;
                                    case 200:
                                        client.Player.HPPotionPrice = 120;
                                        break;
                                    case 300:
                                        client.Player.HPPotionPrice = 200;
                                        break;
                                    case 450:
                                        client.Player.HPPotionPrice = 300;
                                        break;
                                    case 600:
                                        client.Player.HPPotionPrice = 450;
                                        break;
                                }
                            }));
                            using (var db = new Database())
                            {
                                client.Player.Credits = client.Account.Credits = db.UpdateCredit(client.Account, -client.Player.HPPotionPrice);
                            }
                            client.Character.HitPoints += 100;
                            client.Player.SaveToCharacter();
                        }
                    }
                }
                else if (packet.SlotObject.SlotId == 255)
                {
                    item = client.Player.Manager.GameData.Items[packet.SlotObject.ObjectType];

                    if (item.ObjectId != "Magic Potion")
                    {
                        log.FatalFormat("Cheat engine detected for player {0},\nItem should be a Magic Potion, but its {1}.",
                            client.Player.Name, item.ObjectId);
                        foreach (Player player in client.Player.Owner.Players.Values)
                            if (player.Client.Account.Rank >= 2)
                                player.SendInfo(String.Format("Cheat engine detected for player {0},\nItem should be a Magic Potion, but its {1}.",
                            client.Player.Name, item.ObjectId));
                        client.Disconnect();
                        return;
                    }

                    if (client.Player.MagicPotions > 0)
                        client.Player.MagicPotions--;
                    else
                    {
                        if (client.Account.Credits > client.Player.MPPotionPrice)
                        {
                            switch (client.Player.MPPotionPrice)
                            {
                                case 5:
                                    if (client.Player.MPFirstPurchaseTime)
                                    {
                                        client.Player.MPPotionPrice = 5;
                                        client.Player.MPFirstPurchaseTime = false;
                                    }
                                    client.Player.MPPotionPrice = 10;
                                    break;
                                case 10:
                                    client.Player.MPPotionPrice = 20;
                                    break;
                                case 20:
                                    client.Player.MPPotionPrice = 40;
                                    break;
                                case 40:
                                    client.Player.MPPotionPrice = 80;
                                    break;
                                case 80:
                                    client.Player.MPPotionPrice = 120;
                                    break;
                                case 120:
                                    client.Player.MPPotionPrice = 200;
                                    break;
                                case 200:
                                    client.Player.MPPotionPrice = 300;
                                    break;
                                case 300:
                                    client.Player.MPPotionPrice = 450;
                                    break;
                                case 450:
                                    client.Player.MPPotionPrice = 600;
                                    break;
                                case 600:
                                    break;
                            }
                            client.Player.Owner.Timers.Add(new WorldTimer(10000, (world, j) =>
                            {
                                switch (client.Player.MPPotionPrice)
                                {
                                    case 5:
                                        break;
                                    case 10:
                                        client.Player.MPFirstPurchaseTime = true;
                                        client.Player.MPPotionPrice = 5;
                                        break;
                                    case 20:
                                        client.Player.MPPotionPrice = 10;
                                        break;
                                    case 40:
                                        client.Player.MPPotionPrice = 20;
                                        break;
                                    case 80:
                                        client.Player.MPPotionPrice = 40;
                                        break;
                                    case 120:
                                        client.Player.MPPotionPrice = 80;
                                        break;
                                    case 200:
                                        client.Player.MPPotionPrice = 120;
                                        break;
                                    case 300:
                                        client.Player.MPPotionPrice = 200;
                                        break;
                                    case 450:
                                        client.Player.MPPotionPrice = 300;
                                        break;
                                    case 600:
                                        client.Player.MPPotionPrice = 450;
                                        break;
                                }
                            }));
                            using (var db = new Database())
                            {
                                client.Player.Credits = client.Account.Credits = db.UpdateCredit(client.Account, -client.Player.MPPotionPrice);
                            }
                            client.Character.MagicPoints += 100;
                            client.Player.SaveToCharacter();
                        }
                    }
                }
                else
                    item = container.Inventory[packet.SlotObject.SlotId];

                if (!client.Player.Activate(t, item, packet))
                {
                    if (item.Consumable)
                    {
                        if (item.SuccessorId != null)
                        {
                            if (item.SuccessorId != item.ObjectId)
                            {
                                if (packet.SlotObject.SlotId != 254 && packet.SlotObject.SlotId != 255)
                                {
                                    container.Inventory[packet.SlotObject.SlotId] =
                                        client.Player.Manager.GameData.Items[
                                            client.Player.Manager.GameData.IdToObjectType[item.SuccessorId]];
                                    client.Player.Owner.GetEntity(packet.SlotObject.ObjectId).UpdateCount++;
                                }
                            }
                        }
                        else
                        {
                            if (packet.SlotObject.SlotId != 254 && packet.SlotObject.SlotId != 255)
                            {
                                container.Inventory[packet.SlotObject.SlotId] = null;
                                client.Player.Owner.GetEntity(packet.SlotObject.ObjectId).UpdateCount++;
                            }
                        }

                        if (container is OneWayContainer)
                        {
                            using (Database db = new Database())
                            {
                                Account acc = db.GetAccount(client.Account.AccountId, Manager.GameData);
                                acc.Gifts.Remove(packet.SlotObject.ObjectType);

                                MySqlCommand cmd = db.CreateQuery();
                                cmd.CommandText = @"UPDATE accounts SET gifts=@gifts WHERE id=@accId;";
                                cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                                cmd.Parameters.AddWithValue("@gifts", Utils.GetCommaSepString(acc.Gifts.ToArray()));
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                if (packet.SlotObject.SlotId != 254 && packet.SlotObject.SlotId != 255)
                    if (container.SlotTypes[packet.SlotObject.SlotId] != -1)
                        client.Player.FameCounter.UseAbility();

                client.Player.UpdateCount++;
                client.Player.SaveToCharacter();
                await client.Save();
            }, PendingPriority.Networking);
        }
    }
}