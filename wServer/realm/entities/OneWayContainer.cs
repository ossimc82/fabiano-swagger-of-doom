#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

#endregion

namespace wServer.realm.entities
{
    public class OneWayContainer : StaticObject, IContainer
    {
        public OneWayContainer(RealmManager manager, ushort objType, int? life, bool dying)
            : base(manager, objType, life, false, dying, false)
        {
            Inventory = new Item[8];
            SlotTypes = new int[8];

            for (int i = 0; i < SlotTypes.Length; i++)
                if (SlotTypes[i] == 0) SlotTypes[i] = 10;
        }

        public int[] SlotTypes { get; private set; }
        public Item[] Inventory { get; private set; }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.Inventory0] = (Inventory[0] != null ? Inventory[0].ObjectType : -1);
            stats[StatsType.Inventory1] = (Inventory[1] != null ? Inventory[1].ObjectType : -1);
            stats[StatsType.Inventory2] = (Inventory[2] != null ? Inventory[2].ObjectType : -1);
            stats[StatsType.Inventory3] = (Inventory[3] != null ? Inventory[3].ObjectType : -1);
            stats[StatsType.Inventory4] = (Inventory[4] != null ? Inventory[4].ObjectType : -1);
            stats[StatsType.Inventory5] = (Inventory[5] != null ? Inventory[5].ObjectType : -1);
            stats[StatsType.Inventory6] = (Inventory[6] != null ? Inventory[6].ObjectType : -1);
            stats[StatsType.Inventory7] = (Inventory[7] != null ? Inventory[7].ObjectType : -1);
            base.ExportStats(stats);
        }

        public override void Tick(RealmTime time)
        {
            bool hasItem = false;
            foreach (Item i in Inventory)
                if (i != null)
                {
                    hasItem = true;
                    break;
                }

            if (!hasItem)
            {
                StaticObject obj = new StaticObject(Manager, 0x0743, null, false, false, false);
                obj.Move(X, Y);
                World w = Owner;
                Owner.LeaveWorld(this);
                w.EnterWorld(obj);
            }
            base.Tick(time);
        }

        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            return false;
        }
    }
}