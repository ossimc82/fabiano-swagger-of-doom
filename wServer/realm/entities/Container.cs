#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

#endregion

namespace wServer.realm.entities
{
    public interface IContainer
    {
        int[] SlotTypes { get; }
        Item[] Inventory { get; }
    }

    public class Container : StaticObject, IContainer
    {
        public Container(RealmManager manager, ushort objType, int? life, bool dying)
            : base(manager, objType, life, false, dying, false)
        {
            Inventory = new Item[8];
            SlotTypes = new int[8];

            for (int i = 0; i < SlotTypes.Length; i++)
                if (SlotTypes[i] == 0) SlotTypes[i] = 10;
        }

        public Container(RealmManager manager, XElement node)
            : base(manager, (ushort) Utils.FromString(node.Attribute("type").Value), null, false, false, false)
        {
            SlotTypes = Utils.FromCommaSepString32(node.Element("SlotTypes").Value);
            XElement eq = node.Element("Equipment");
            if (eq != null)
            {
                Item[] inv =
                    Utils.FromCommaSepString16(eq.Value)
                        .Select(_ => _ == -1 ? null : manager.GameData.Items[(ushort)_])
                        .ToArray();
                Array.Resize(ref inv, 8);
                Inventory = inv;
            }
        }

        public string[] BagOwners { get; set; }
        public int[] SlotTypes { get; private set; }
        public Item[] Inventory { get; set; }
        public bool BoostedBag { get; set; }

        public override ObjectDef ToDefinition()
        {
            var ret = base.ToDefinition();
            if (BoostedBag) ret.ObjectType = 0x510;
            return ret;
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            if (Inventory == null) Inventory = new Item[8];
            stats[StatsType.Inventory0] = (Inventory[0] != null ? Inventory[0].ObjectType : -1);
            stats[StatsType.Inventory1] = (Inventory[1] != null ? Inventory[1].ObjectType : -1);
            stats[StatsType.Inventory2] = (Inventory[2] != null ? Inventory[2].ObjectType : -1);
            stats[StatsType.Inventory3] = (Inventory[3] != null ? Inventory[3].ObjectType : -1);
            stats[StatsType.Inventory4] = (Inventory[4] != null ? Inventory[4].ObjectType : -1);
            stats[StatsType.Inventory5] = (Inventory[5] != null ? Inventory[5].ObjectType : -1);
            stats[StatsType.Inventory6] = (Inventory[6] != null ? Inventory[6].ObjectType : -1);
            stats[StatsType.Inventory7] = (Inventory[7] != null ? Inventory[7].ObjectType : -1);
            if(BagOwners != null)
                stats[StatsType.OwnerAccountId] = BagOwners.Length == 1 ? BagOwners[0] : "-1";
            base.ExportStats(stats);
        }

        public override void Tick(RealmTime time)
        {
            if (ObjectType == 0x0504 || ObjectType == 0x0502) //Vault chest
                return;
            bool hasItem = false;
            foreach (Item i in Inventory)
                if (i != null)
                {
                    hasItem = true;
                    break;
                }
            if (!hasItem)
                Owner.LeaveWorld(this);
            base.Tick(time);
        }

        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            return false;
        }
    }
}