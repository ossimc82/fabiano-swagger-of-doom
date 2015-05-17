#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using log4net;
using wServer.networking.svrPackets;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.entities.merchant
{
    public class Merchants : SellableObject
    {
        private const int BUY_NO_GOLD = 3;
        private const int BUY_NO_FAME = 6;
        private const int BUY_NO_FORTUNETOKENS = 9;
        private const int MERCHANT_SIZE = 100;
        private static readonly ILog log = LogManager.GetLogger(typeof(Merchants));

        private readonly Dictionary<int, Tuple<int, CurrencyType>> prices = MerchantLists.prices;

        private bool closing;
        private bool newMerchant;
        private int tickcount;

        public static Random Random { get; private set; }

        public Merchants(RealmManager manager, ushort objType, World owner = null)
            : base(manager, objType)
        {
            MType = -1;
            Size = MERCHANT_SIZE;
            if (owner != null)
                Owner = owner;

            if (Random == null) Random = new Random();
            if (AddedTypes == null) AddedTypes = new List<KeyValuePair<string, int>>();
            if(owner != null) ResolveMType();
        }

        private static List<KeyValuePair<string, int>> AddedTypes { get; set; }

        public bool Custom { get; set; }

        public int MType { get; set; }
        public int MRemaining { get; set; }
        public int MTime { get; set; }
        public int Discount { get; set; }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.MerchantMerchandiseType] = MType;
            stats[StatsType.MerchantRemainingCount] = MRemaining;
            stats[StatsType.MerchantRemainingMinute] = newMerchant ? Int32.MaxValue : MTime;
            stats[StatsType.MerchantDiscount] = Discount;
            stats[StatsType.SellablePrice] = Price;
            stats[StatsType.SellableRankRequirement] = RankReq;
            stats[StatsType.SellablePriceCurrency] = Currency;

            base.ExportStats(stats);
        }

        public override void Init(World owner)
        {
            base.Init(owner);
            ResolveMType();
            UpdateCount++;
            if (MType == -1) Owner.LeaveWorld(this);
        }

        protected override bool TryDeduct(Player player)
        {
            var acc = player.Client.Account;
            if (player.Stars < RankReq) return false;

            if (Currency == CurrencyType.Fame)
                if (acc.Stats.Fame < Price) return false;

            if (Currency == CurrencyType.Gold)
                if (acc.Credits < Price) return false;

            if (Currency == CurrencyType.FortuneTokens)
                if (acc.FortuneTokens < Price) return false;
            return true;
        }

        public override void Buy(Player player)
        {
            Manager.Database.DoActionAsync(db =>
            {
                if (ObjectType == 0x01ca) //Merchant
                {
                    if (TryDeduct(player))
                    {
                        for (var i = 0; i < player.Inventory.Length; i++)
                        {
                            try
                            {
                                XElement ist;
                                Manager.GameData.ObjectTypeToElement.TryGetValue((ushort)MType, out ist);
                                if (player.Inventory[i] == null &&
                                    (player.SlotTypes[i] == 10 ||
                                     player.SlotTypes[i] == Convert.ToInt16(ist.Element("SlotType").Value)))
                                // Exploit fix - No more mnovas as weapons!
                                {
                                    player.Inventory[i] = Manager.GameData.Items[(ushort)MType];

                                    if (Currency == CurrencyType.Fame)
                                        player.CurrentFame =
                                            player.Client.Account.Stats.Fame =
                                                db.UpdateFame(player.Client.Account, -Price);
                                    if (Currency == CurrencyType.Gold)
                                        player.Credits =
                                            player.Client.Account.Credits =
                                                db.UpdateCredit(player.Client.Account, -Price);
                                    if (Currency == CurrencyType.FortuneTokens)
                                        player.Tokens =
                                            player.Client.Account.FortuneTokens =
                                                db.UpdateFortuneToken(player.Client.Account, -Price);

                                    player.Client.SendPacket(new BuyResultPacket
                                    {
                                        Result = 0,
                                        Message = "{\"key\":\"server.buy_success\"}"
                                    });
                                    MRemaining--;
                                    player.UpdateCount++;
                                    player.SaveToCharacter();
                                    UpdateCount++;
                                    return;
                                }
                            }
                            catch (Exception e)
                            {
                                log.Error(e);
                            }
                        }
                        player.Client.SendPacket(new BuyResultPacket
                        {
                            Result = 0,
                            Message = "{\"key\":\"server.inventory_full\"}"
                        });
                    }
                    else
                    {
                        if (player.Stars < RankReq)
                        {
                            player.Client.SendPacket(new BuyResultPacket
                            {
                                Result = 0,
                                Message = "Not enough stars!"
                            });
                            return;
                        }
                        switch (Currency)
                        {
                            case CurrencyType.Gold:
                                player.Client.SendPacket(new BuyResultPacket
                                {
                                    Result = BUY_NO_GOLD,
                                    Message = "{\"key\":\"server.not_enough_gold\"}"
                                });
                                break;
                            case CurrencyType.Fame:
                                player.Client.SendPacket(new BuyResultPacket
                                {
                                    Result = BUY_NO_FAME,
                                    Message = "{\"key\":\"server.not_enough_fame\"}"
                                });
                                break;
                            case CurrencyType.FortuneTokens:
                                player.Client.SendPacket(new BuyResultPacket
                                {
                                    Result = BUY_NO_FORTUNETOKENS,
                                    Message = "{\"key\":\"server.not_enough_fortunetokens\"}"
                                });
                                break;
                        }
                    }
                }
            });
        }

        public override void Tick(RealmTime time)
        {
            try
            {
                if (Size == 0 && MType != -1)
                {
                    Size = MERCHANT_SIZE;
                    UpdateCount++;
                }

                if (!closing)
                {
                    tickcount++;
                    if (tickcount % (Manager?.TPS * 60) == 0) //once per minute after spawning
                    {
                        MTime--;
                        UpdateCount++;
                    }
                }

                if (MRemaining == 0 &&  MType != -1)
                {
                    if (AddedTypes.Contains(new KeyValuePair<string, int>(Owner.Name, MType)))
                        AddedTypes.Remove(new KeyValuePair<string, int>(Owner.Name, MType));
                    Recreate(this);
                    UpdateCount++;
                }

                if (MTime == -1 && Owner != null)
                {
                    if (AddedTypes.Contains(new KeyValuePair<string, int>(Owner.Name, MType)))
                        AddedTypes.Remove(new KeyValuePair<string, int>(Owner.Name, MType));
                    Recreate(this);
                    UpdateCount++;
                }

                if (MTime == 1 && !closing)
                {
                    closing = true;
                    Owner?.Timers.Add(new WorldTimer(30 * 1000, (w1, t1) =>
                    {
                        MTime--;
                        UpdateCount++;
                        w1.Timers.Add(new WorldTimer(30 * 1000, (w2, t2) =>
                        {
                            MTime--;
                            UpdateCount++;
                        }));
                    }));
                }

                if (MType == -1) Owner?.LeaveWorld(this);

                base.Tick(time);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public void Recreate(Merchants x)
        {
            try
            {
                var mrc = new Merchants(Manager, x.ObjectType, x.Owner);
                mrc.Move(x.X, x.Y);
                var w = Owner;
                Owner.LeaveWorld(this);
                w.Timers.Add(new WorldTimer(Random.Next(30, 60) * 1000, (world, time) => w.EnterWorld(mrc)));
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        public void ResolveMType()
        {
            MType = -1;
            var list = new int[0];
            if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_1)
                list = MerchantLists.store1List;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_2)
                list = MerchantLists.store2List;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_3)
                list = MerchantLists.store3List;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_4)
                list = MerchantLists.store4List;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_5)
                list = MerchantLists.store5List;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_6)
                list = MerchantLists.store6List;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_7)
                list = MerchantLists.store7List;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_8)
                list = MerchantLists.store8List;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_9)
                list = MerchantLists.store9List;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_10)
                list = MerchantLists.store10List;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_11)
                list = MerchantLists.store11List;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_12)
                list = MerchantLists.AccessoryDyeList;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_13)
                list = MerchantLists.ClothingClothList;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_14)
                list = MerchantLists.AccessoryClothList;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_15)
                list = MerchantLists.ClothingDyeList;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_16)
                list = MerchantLists.store5List;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_17)
                list = MerchantLists.store7List;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_18)
                list = MerchantLists.store6List;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_19)
                list = MerchantLists.store2List;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_20)
                list = MerchantLists.store20List;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_21)
                list = MerchantLists.AccessoryClothList;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_22)
                list = MerchantLists.AccessoryDyeList;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_23)
                list = MerchantLists.ClothingClothList;
            else if (Owner.Map[(int) X, (int) Y].Region == TileRegion.Store_24)
                list = MerchantLists.ClothingDyeList;

            if (AddedTypes == null) AddedTypes = new List<KeyValuePair<string, int>>();
            list.Shuffle();
            foreach (var t1 in list.Where(t1 => !AddedTypes.Contains(new KeyValuePair<string, int>(Owner.Name, t1))))
            {
                AddedTypes.Add(new KeyValuePair<string, int>(Owner.Name, t1));
                MType = t1;
                MTime = Random.Next(6, 15);
                MRemaining = Random.Next(6, 11);
                newMerchant = true;
                Owner.Timers.Add(new WorldTimer(30000, (w, t) =>
                {
                    newMerchant = false;
                    UpdateCount++;
                }));

                var s = Random.Next(0, 100);

                if(s < 2)
                    Discount = 50;
                else if(s < 5)
                    Discount = 25;
                else if (s < 10)
                    Discount = 15;
                else if(s < 15)
                    Discount = 10;
                else Discount = 0;

                Tuple<int, CurrencyType> price;
                if (prices.TryGetValue(MType, out price))
                {
                    if (Discount != 0)
                        Price = (int)(price.Item1 - (price.Item1 * ((double)Discount / 100))) < 1 ?
                            price.Item1 : (int)(price.Item1 - (price.Item1 * ((double)Discount / 100)));
                    else
                        Price = price.Item1;
                    Currency = price.Item2;
                }

                break;
            }
            UpdateCount++;
        }
    }
}