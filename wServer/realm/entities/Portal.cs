#region

using System;
using System.Collections.Generic;
using System.Globalization;

#endregion

namespace wServer.realm.entities
{
    public class Portal : StaticObject
    {
        public Portal(RealmManager manager, ushort objType, int? life)
            : base(manager, objType, life, false, true, false)
        {
            Usable = objType != 0x0721;
            ObjectDesc = Manager.GameData.Portals[objType];
            Name = manager.GameData.Portals[objType].DisplayId;
        }

        private Portal(RealmManager manager, PortalDesc desc, int? life)
            : base(manager, desc.ObjectType, life, false, true, false)
        {
            ObjectDesc = desc;
            Name = desc.DisplayId;
        }

        public string PortalName { get; set; }
        public new PortalDesc ObjectDesc { get; }
        public new ushort ObjectType => ObjectDesc.ObjectType;
        public new World WorldInstance { get; set; }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            if (ObjectType != 0x072f)
                stats[StatsType.PortalUsable] = Usable ? 1 : 0;
            base.ExportStats(stats);
            stats[StatsType.Name] = ObjectDesc.DungeonName ?? Name;
        }

        public override ObjectDef ToDefinition()
        {
            return new ObjectDef
            {
                ObjectType = ObjectDesc.ObjectType,
                Stats = ExportStats()
            };
        }

        public override void Tick(RealmTime time)
        {
            if(WorldInstance != null && IsRealmPortal)
                Usable = !(WorldInstance.Players.Count >= RealmManager.MAX_REALM_PLAYERS);
            base.Tick(time);
        }

        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            return false;
        }

        public bool IsRealmPortal
        {
            get { return Owner.Id == -2 && Name.StartsWith("NexusPortal."); }
        }

        public Portal Unlock(string dungeonName)
        {
            var desc = Manager.GameData.Portals[0x0700];
            desc.DungeonName = dungeonName;
            var portal = new Portal(Manager, desc, desc.TimeoutTime * 1000);
            portal.Move(X, Y);
            portal.Usable = true;
            Owner.EnterWorld(portal);
            Owner.LeaveWorld(this);
            return portal;
        }

        public override void Dispose()
        {
            WorldInstance = null;
            base.Dispose();
        }
    }
}