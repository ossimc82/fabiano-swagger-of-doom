#region

using System.Collections.Generic;
using System.Linq;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;

#endregion

namespace wServer.realm.entities.player
{
    partial class Player
    {
        public TradeManager TradeHandler { get; private set; }

        public void RequestTrade(RealmTime time, RequestTradePacket pkt)
        {
            var target = Owner.GetPlayerByName(pkt.Name);

            if (target == null)
            {
                SendInfo("{\"key\":\"server.player_not_found\",\"tokens\":{\"player\":\"" + pkt.Name + "\"}}");
                return;
            }
            if (!NameChosen || !target.NameChosen)
            {
                SendInfo("{\"key\":\"server.trade_needs_their_name\"}");
                return;
            }
            if (Client.Player == target)
            {
                SendInfo("{\"key\":\"server.self_trade\"}");
                return;
            }

            if (TradeManager.TradingPlayers.Count(_ => _.AccountId == target.AccountId) > 0)
            {
                SendInfo("{\"key\":\"server.they_already_trading\",\"tokens\":{\"player\":\"" + target.Name + "\"}}");
                return;
            }
            if (TradeManager.CurrentRequests.Count(_ => _.Value.AccountId == AccountId && _.Key.AccountId == target.AccountId) > 0)
            {
                var myItems = new TradeItem[12];
                var yourItems = new TradeItem[12];

                for (var i = 0; i < myItems.Length; i++)
                {
                    myItems[i] = new TradeItem
                    {
                        Item = Inventory[i] == null ? -1 : Inventory[i].ObjectType,
                        SlotType = SlotTypes[i],
                        Tradeable = (Inventory[i] != null && i >= 4) && (!Inventory[i].Soulbound),
                        Included = false
                    };
                }

                for (var i = 0; i < yourItems.Length; i++)
                {
                    yourItems[i] = new TradeItem
                    {
                        Item = target.Inventory[i] == null ? -1 : target.Inventory[i].ObjectType,
                        SlotType = SlotTypes[i],
                        Tradeable = (target.Inventory[i] != null && i >= 4) && (!target.Inventory[i].Soulbound),
                        Included = false
                    };
                }

                Client.SendPacket(new TradeStartPacket
                {
                    MyItems = myItems,
                    YourItems = yourItems,
                    YourName = target.Name
                });

                target.Client.SendPacket(new TradeStartPacket
                {
                    MyItems = yourItems,
                    YourItems = myItems,
                    YourName = Name
                });

                var t = new TradeManager(this, target);
                target.TradeHandler = t;
                TradeHandler = t;
            }
            else
            {
                SendInfo("{\"key\":\"server.trade_requested\",\"tokens\":{\"player\":\"" + target.Name + "\"}}");
                if (target.Ignored.Contains(Client.Account.AccountId)) return;
                target.Client.SendPacket(new TradeRequestedPacket
                {
                    Name = Name
                });
                var format = new KeyValuePair<Player, Player>(this, target);
                TradeManager.CurrentRequests.Add(format);

                Owner.Timers.Add(new WorldTimer(60 * 1000, (w, t) =>
                {
                    if (!TradeManager.CurrentRequests.Contains(format)) return;
                    TradeManager.CurrentRequests.Remove(format);
                    SendInfo("{\"key\":\"server.trade_timeout\"}");
                }));
            }
        }

        public void ChangeTrade(RealmTime time, ChangeTradePacket pkt)
        {
            TradeHandler?.TradeChanged(this, pkt.Offers);
        }

        public void AcceptTrade(RealmTime time, AcceptTradePacket pkt)
        {
            TradeHandler?.AcceptTrade(this, pkt);
        }

        public void CancelTrade(RealmTime time, CancelTradePacket pkt)
        {
            TradeHandler?.CancelTrade(this);
        }

        public void TradeCanceled()
        {
            TradeHandler = null;
        }
    }
}