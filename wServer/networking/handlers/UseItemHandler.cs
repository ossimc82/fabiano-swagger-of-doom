#region

using db;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
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

            client.Manager.Logic.AddPendingAction(t =>
            {
                var container = client.Player.Owner.GetEntity(packet.SlotObject.ObjectId) as IContainer;
                if(container == null) return;
                Item item;
                switch (packet.SlotObject.SlotId)
                {
                    case 254:
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
                            if (client.Account.Credits > client.Player.HpPotionPrice)
                            {
                                switch (client.Player.HpPotionPrice)
                                {
                                    case 5:
                                        if (client.Player.HpFirstPurchaseTime)
                                        {
                                            client.Player.HpPotionPrice = 5;
                                            client.Player.HpFirstPurchaseTime = false;
                                        }
                                        client.Player.HpPotionPrice = 10;
                                        break;
                                    case 10:
                                        client.Player.HpPotionPrice = 20;
                                        break;
                                    case 20:
                                        client.Player.HpPotionPrice = 40;
                                        break;
                                    case 40:
                                        client.Player.HpPotionPrice = 80;
                                        break;
                                    case 80:
                                        client.Player.HpPotionPrice = 120;
                                        break;
                                    case 120:
                                        client.Player.HpPotionPrice = 200;
                                        break;
                                    case 200:
                                        client.Player.HpPotionPrice = 300;
                                        break;
                                    case 300:
                                        client.Player.HpPotionPrice = 450;
                                        break;
                                    case 450:
                                        client.Player.HpPotionPrice = 600;
                                        break;
                                    case 600:
                                        break;
                                }
                                client.Player.Owner.Timers.Add(new WorldTimer(8000, (world, j) =>
                                {
                                    switch (client.Player.HpPotionPrice)
                                    {
                                        case 5:
                                            break;
                                        case 10:
                                            client.Player.HpFirstPurchaseTime = true;
                                            client.Player.HpPotionPrice = 5;
                                            break;
                                        case 20:
                                            client.Player.HpPotionPrice = 10;
                                            break;
                                        case 40:
                                            client.Player.HpPotionPrice = 20;
                                            break;
                                        case 80:
                                            client.Player.HpPotionPrice = 40;
                                            break;
                                        case 120:
                                            client.Player.HpPotionPrice = 80;
                                            break;
                                        case 200:
                                            client.Player.HpPotionPrice = 120;
                                            break;
                                        case 300:
                                            client.Player.HpPotionPrice = 200;
                                            break;
                                        case 450:
                                            client.Player.HpPotionPrice = 300;
                                            break;
                                        case 600:
                                            client.Player.HpPotionPrice = 450;
                                            break;
                                    }
                                }));
                                using (var db = new Database())
                                {
                                    client.Player.Credits = client.Account.Credits = db.UpdateCredit(client.Account, -client.Player.HpPotionPrice);
                                }
                                client.Character.HitPoints += 100;
                                client.Player.SaveToCharacter();
                            }
                        }
                        break;
                    case 255:
                        item = client.Player.Manager.GameData.Items[packet.SlotObject.ObjectType];

                        if (item.ObjectId != "Magic Potion")
                        {
                            log.FatalFormat("Cheat engine detected for player {0},\nItem should be a Magic Potion, but its {1}.",
                                client.Player.Name, item.ObjectId);
                            foreach (var player in client.Player.Owner.Players.Values.Where(player => player.Client.Account.Rank >= 2))
                                player.SendInfo($"Cheat engine detected for player {client.Player.Name},\nItem should be a Magic Potion, but its {item.ObjectId}.");
                            client.Disconnect();
                            return;
                        }

                        if (client.Player.MagicPotions > 0)
                            client.Player.MagicPotions--;
                        else
                        {
                            if (client.Account.Credits > client.Player.MpPotionPrice)
                            {
                                switch (client.Player.MpPotionPrice)
                                {
                                    case 5:
                                        if (client.Player.MpFirstPurchaseTime)
                                        {
                                            client.Player.MpPotionPrice = 5;
                                            client.Player.MpFirstPurchaseTime = false;
                                        }
                                        client.Player.MpPotionPrice = 10;
                                        break;
                                    case 10:
                                        client.Player.MpPotionPrice = 20;
                                        break;
                                    case 20:
                                        client.Player.MpPotionPrice = 40;
                                        break;
                                    case 40:
                                        client.Player.MpPotionPrice = 80;
                                        break;
                                    case 80:
                                        client.Player.MpPotionPrice = 120;
                                        break;
                                    case 120:
                                        client.Player.MpPotionPrice = 200;
                                        break;
                                    case 200:
                                        client.Player.MpPotionPrice = 300;
                                        break;
                                    case 300:
                                        client.Player.MpPotionPrice = 450;
                                        break;
                                    case 450:
                                        client.Player.MpPotionPrice = 600;
                                        break;
                                    case 600:
                                        break;
                                }
                                client.Player.Owner.Timers.Add(new WorldTimer(10000, (world, j) =>
                                {
                                    switch (client.Player.MpPotionPrice)
                                    {
                                        case 5:
                                            break;
                                        case 10:
                                            client.Player.MpFirstPurchaseTime = true;
                                            client.Player.MpPotionPrice = 5;
                                            break;
                                        case 20:
                                            client.Player.MpPotionPrice = 10;
                                            break;
                                        case 40:
                                            client.Player.MpPotionPrice = 20;
                                            break;
                                        case 80:
                                            client.Player.MpPotionPrice = 40;
                                            break;
                                        case 120:
                                            client.Player.MpPotionPrice = 80;
                                            break;
                                        case 200:
                                            client.Player.MpPotionPrice = 120;
                                            break;
                                        case 300:
                                            client.Player.MpPotionPrice = 200;
                                            break;
                                        case 450:
                                            client.Player.MpPotionPrice = 300;
                                            break;
                                        case 600:
                                            client.Player.MpPotionPrice = 450;
                                            break;
                                    }
                                }));
                                using (var db = new Database())
                                    client.Player.Credits = client.Account.Credits = db.UpdateCredit(client.Account, -client.Player.MpPotionPrice);
                                client.Character.MagicPoints += 100;
                                client.Player.SaveToCharacter();
                            }
                        }
                        break;
                    default:
                        item = container.Inventory[packet.SlotObject.SlotId];
                        break;
                }
                if (item != null)
                {
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
                }
                if (packet.SlotObject.SlotId != 254 && packet.SlotObject.SlotId != 255)
                    if (container.SlotTypes[packet.SlotObject.SlotId] != -1)
                        client.Player.FameCounter.UseAbility();

                ((Entity)container).UpdateCount++;
                client.Player.UpdateCount++;
                client.Player.SaveToCharacter();
                client.Save();
            }, PendingPriority.Networking);
        }
    }
}