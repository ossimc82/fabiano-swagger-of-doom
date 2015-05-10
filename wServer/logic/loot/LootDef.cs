using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.logic.loot
{
    class LootDef
    {
        public static readonly LootDef Empty = new LootDef(0, 0, 0, 0);

        Tuple<double, ILoot>[] loots;
        public LootDef(
            int baseLootCount,
            int personMult,
            int minLootCount,
            int maxLootCount, 
            params Tuple<double, ILoot>[] loots)
        {
            this.loots = loots;
            BaseLootCount = baseLootCount;
            PersonMultiplier = personMult;
            MinLootCount = minLootCount;
            MaxLootCount = maxLootCount;
        }

        public int BaseLootCount { get; private set; }
        public int PersonMultiplier { get; private set; }
        public int MinLootCount { get; private set; }
        public int MaxLootCount { get; private set; }

        public Item GetRandomLoot(Random rand)
        {
            foreach (var loot in loots)
            {
                if (rand.NextDouble() < loot.Item1)
                    return loot.Item2.GetLoot(rand);
            }
            return null;
        }
    }
}
