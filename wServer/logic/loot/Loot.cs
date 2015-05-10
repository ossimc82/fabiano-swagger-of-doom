using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.logic.loot
{
    public interface ILoot
    {
        Item GetLoot(Random rand);
    }
    public enum ItemType
    {
        Weapon,
        Ability,
        Armor,
        Ring
    }
    class TierLoot : ILoot
    {
        public static readonly int[] WeaponsT = new int[] { 1, 2, 3, 8, 17, };
        public static readonly int[] AbilityT = new int[] { 4, 5, 11, 12, 13, 15, 16, 18, 19, 20, 21, 22, 23, };
        public static readonly int[] ArmorsT = new int[] { 6, 7, 14, };
        public static readonly int[] RingT = new int[] { 9 };

        public static readonly Dictionary<int, Item[]> WeaponItems;
        public static readonly Dictionary<int, Item[]> AbilityItems;
        public static readonly Dictionary<int, Item[]> ArmorItems;
        public static readonly Dictionary<int, Item[]> RingItems;

        static TierLoot()
        {
            WeaponItems = new Dictionary<int, Item[]>();
            for (int tier = 1; tier < 20; tier++)
            {
                List<Item> items = new List<Item>();
                foreach (var i in WeaponsT)
                    items.AddRange(XmlDatas.ItemDescs.Select(_ => _.Value).Where(_ => _.Tier == tier && _.SlotType == i));
                if (items.Count == 0)
                    break;
                else
                    WeaponItems[tier] = items.ToArray();
            }
            AbilityItems = new Dictionary<int, Item[]>();
            for (int tier = 1; tier < 20; tier++)
            {
                List<Item> items = new List<Item>();
                foreach (var i in AbilityT)
                    items.AddRange(XmlDatas.ItemDescs.Select(_ => _.Value).Where(_ => _.Tier == tier && _.SlotType == i));
                if (items.Count == 0)
                    break;
                else
                    AbilityItems[tier] = items.ToArray();
            }
            ArmorItems = new Dictionary<int, Item[]>();
            for (int tier = 1; tier < 20; tier++)
            {
                List<Item> items = new List<Item>();
                foreach (var i in ArmorsT)
                    items.AddRange(XmlDatas.ItemDescs.Select(_ => _.Value).Where(_ => _.Tier == tier && _.SlotType == i));
                if (items.Count == 0)
                    break;
                else
                    ArmorItems[tier] = items.ToArray();
            }
            RingItems = new Dictionary<int, Item[]>();
            for (int tier = 1; tier < 20; tier++)
            {
                List<Item> items = new List<Item>();
                foreach (var i in RingT)
                    items.AddRange(XmlDatas.ItemDescs.Select(_ => _.Value).Where(_ => _.Tier == tier && _.SlotType == i));
                if (items.Count == 0)
                    break;
                else
                    RingItems[tier] = items.ToArray();
            }
        }

        public TierLoot(byte tier, ItemType type)
        {
            this.Tier = tier;
            this.Type = type;
        }

        public byte Tier { get; private set; }
        public ItemType Type { get; private set; }
        public Item GetLoot(Random rand)
        {
            Item[] candidates;
            switch (Type)
            {
                case ItemType.Weapon: candidates = WeaponItems[Tier]; break;
                case ItemType.Ability: candidates = AbilityItems[Tier]; break;
                case ItemType.Armor: candidates = ArmorItems[Tier]; break;
                case ItemType.Ring:
                default: candidates = RingItems[Tier]; break;
            }
            var idx = rand.Next(0, candidates.Length);
            return candidates[idx];
        }
    }
    public class ItemLoot : ILoot
    {
        public ItemLoot(string loot) : this(XmlDatas.IdToType[loot]) { }
        protected ItemLoot(short objType) { Item = XmlDatas.ItemDescs[objType]; }

        public Item Item { get; private set; }
        public Item GetLoot(Random rand)
        {
            return Item;
        }
    }
    class HpPotionLoot : ItemLoot
    {
        private HpPotionLoot() : base(0x0a22) { }
        public static readonly HpPotionLoot Instance = new HpPotionLoot();
    }
    class MpPotionLoot : ItemLoot
    {
        private MpPotionLoot() : base(0x0a23) { }
        public static readonly MpPotionLoot Instance = new MpPotionLoot();
    }
    class PotionLoot : ILoot
    {
        private PotionLoot() { }
        public static readonly PotionLoot Instance = new PotionLoot();
        public Item GetLoot(Random rand)
        {
            return rand.Next() % 2 == 0 ? XmlDatas.ItemDescs[0x0a22] : XmlDatas.ItemDescs[0x0a23];
        }
    }
    class IncLoot : ItemLoot
    {
        private IncLoot() : base(0x722) { }
        public static readonly IncLoot Instance = new IncLoot();
    }

    enum StatPotion : short
    {
        Att = 0xa1f,
        Def = 0xa20,
        Spd = 0xa21,
        Vit = 0xa34,
        Wis = 0xa35,
        Dex = 0xa4c,
        Life = 0xae9,
        Mana = 0xaea,
    }
    class StatPotionLoot : ItemLoot
    {
        public StatPotionLoot(StatPotion pot) : base((short)pot) { }
    }
    class StatPotionsLoot : ILoot
    {
        static StatPotion[][] Tiers = new StatPotion[][]
        {
            new StatPotion[] { StatPotion.Att, StatPotion.Def, StatPotion.Spd },
            new StatPotion[] { StatPotion.Vit, StatPotion.Wis, StatPotion.Dex },
            new StatPotion[] { StatPotion.Life, StatPotion.Mana }
        };

        StatPotion[] pots;
        public StatPotionsLoot(params int[] tiers)
        {
            List<StatPotion> p = new List<StatPotion>();
            foreach (var i in tiers)
                p.AddRange(Tiers[i - 1]);
            pots = p.Distinct().ToArray();
        }

        public Item GetLoot(Random rand)
        {
            return XmlDatas.ItemDescs[(short)pots[rand.Next(0, pots.Length)]];
        }
    }
}
