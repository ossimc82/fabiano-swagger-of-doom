#region

using System;
using System.Collections.Generic;
using System.Linq;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.entities.player;

#endregion

namespace wServer.logic.loot
{
    public struct LootDef
    {
        public readonly Item Item;
        public readonly double Probabilty;
        public readonly string LootState;

        public LootDef(Item item, double probabilty, string lootState)
        {
            Item = item;
            Probabilty = probabilty;
            LootState = lootState;
        }
    }

    public class Loot : List<ILootDef>
    {
        private static readonly Random rand = new Random();

        public Loot(params ILootDef[] lootDefs) //For independent loots(e.g. chests)
        {
            AddRange(lootDefs);
        }

        public IEnumerable<Item> GetLoots(RealmManager manager, int min, int max) //For independent loots(e.g. chests)
        {
            List<LootDef> consideration = new List<LootDef>();
            foreach (ILootDef i in this)
                i.Populate(manager, null, null, rand, "", consideration);

            int retCount = rand.Next(min, max);
            foreach (LootDef i in consideration)
            {
                if (rand.NextDouble() < i.Probabilty)
                {
                    yield return i.Item;
                    retCount--;
                }
                if (retCount == 0)
                    yield break;
            }
        }

        public void Handle(Enemy enemy, RealmTime time)
        {
            if (enemy.Owner.Name == "Arena") return;
            List<LootDef> consideration = new List<LootDef>();

            List<Item> sharedLoots = new List<Item>();
            foreach (ILootDef i in this)
                i.Populate(enemy.Manager, enemy, null, rand, i.Lootstate, consideration);
            foreach (LootDef i in consideration)
            {
                if (i.LootState == enemy.LootState || i.LootState == null)
                {
                    if (rand.NextDouble() < i.Probabilty)
                        sharedLoots.Add(i.Item);
                }
            }

            Tuple<Player, int>[] dats = enemy.DamageCounter.GetPlayerData();
            Dictionary<Player, IList<Item>> loots = enemy.DamageCounter.GetPlayerData().ToDictionary(
                d => d.Item1, d => (IList<Item>)new List<Item>());

            foreach (Item loot in sharedLoots.Where(item => item.Soulbound))
                loots[dats[rand.Next(dats.Length)].Item1].Add(loot);

            foreach (Tuple<Player, int> dat in dats)
            {
                consideration.Clear();
                foreach (ILootDef i in this)
                    i.Populate(enemy.Manager, enemy, dat, rand, i.Lootstate, consideration);

                IList<Item> playerLoot = loots[dat.Item1];
                foreach (LootDef i in consideration)
                {
                    if (i.LootState == enemy.LootState || i.LootState == null)
                    {
                        double prob = dat.Item1.LootDropBoost ? i.Probabilty * 1.5 : i.Probabilty;
                        if (rand.NextDouble() < prob)
                        {
                            if (dat.Item1.LootTierBoost)
                                playerLoot.Add(IncreaseTier(enemy.Manager, i.Item, consideration));
                            else
                                playerLoot.Add(i.Item);
                        }
                    }
                }
            }

            AddBagsToWorld(enemy, sharedLoots, loots);
        }

        private Item IncreaseTier(RealmManager manager, Item item, List<LootDef> consideration)
        {
            if (item.SlotType == 10) return item;
            Item[] tier = manager.GameData.Items
                 .Where(i => item.SlotType == i.Value.SlotType)
                 .Where(i => i.Value.Tier >= item.Tier + 3)
                 .Where(i => consideration.Select(_ => _.Item).Contains(i.Value))
                 .Select(i => i.Value).ToArray();

            return tier.Length > 0 ? tier[rand.Next(1, tier.Length)] : item;
        }

        private void AddBagsToWorld(Enemy enemy, IList<Item> shared, IDictionary<Player, IList<Item>> soulbound)
        {
            List<Player> pub = new List<Player>(); //only people not getting soulbound
            foreach (KeyValuePair<Player, IList<Item>> i in soulbound)
            {
                if (i.Value.Count > 0)
                    ShowBags(enemy, i.Value, i.Key);
                else
                    pub.Add(i.Key);
            }
            if (pub.Count > 0 && shared.Count > 0)
                ShowBags(enemy, shared, null);
        }

        private void ShowBags(Enemy enemy, IEnumerable<Item> loots, params Player[] owners)
        {
            string[] ownerIds = owners?.Select(x => x.AccountId).ToArray();
            int bagType = 0;
            Item[] items = new Item[8];
            int idx = 0;

            foreach (Item i in loots)
            {
                if (i.BagType > bagType) bagType = i.BagType;
                items[idx] = i;
                idx++;

                if (idx == 8)
                {
                    ShowBag(enemy, ownerIds, bagType, items);

                    bagType = 0;
                    items = new Item[8];
                    idx = 0;
                }
            }

            if (idx > 0)
                ShowBag(enemy, ownerIds, bagType, items);
        }

        private static void ShowBag(Enemy enemy, string[] owners, int bagType, Item[] items)
        {
            ushort bag = 0x500;
            switch (bagType)
            {
                case 0:
                    bag = 0x500;
                    break;
                case 1:
                    bag = 0x506;
                    break;
                case 2:
                    bag = 0x503;
                    break;
                case 3:
                    bag = 0x508;
                    break;
                case 4:
                    bag = 0x509;
                    break;
                case 5:
                    bag = 0x050B;
                    break;
                case 6:
                    bag = 0x050C;
                    break;
                case 7:
                    bag = 0xfff;
                    break;
            }

            Container container = new Container(enemy.Manager, bag, 1000 * 30, true);
            for (int j = 0; j < 8; j++)
                container.Inventory[j] = items[j];
            container.BagOwners = owners;
            container.Move(
                enemy.X + (float)((rand.NextDouble() * 2 - 1) * 0.5),
                enemy.Y + (float)((rand.NextDouble() * 2 - 1) * 0.5));
            container.Size = 80;
            enemy.Owner.EnterWorld(container);
        }
    }
}