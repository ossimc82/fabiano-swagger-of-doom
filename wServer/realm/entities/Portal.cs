#region

using System.Collections.Generic;

#endregion

namespace wServer.realm.entities
{
    public class Portal : StaticObject
    {
        public Portal(RealmManager manager, ushort objType, int? life)
            : base(manager, objType, life, false, true, false)
        {
            if (objType == 0x0721)
                Usable = false;
            else
                Usable = true;
            EWorld = "";

            Name = manager.GameData.Portals[objType].DisplayId;
        }

        public string PortalName { get; set; }
        public string EWorld { get; set; }
        public new World WorldInstance { get; set; }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            if (ObjectType != 0x072f)
                stats[StatsType.PortalUsable] = Usable ? 1 : 0;
            base.ExportStats(stats);
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

        public void Unlock()
        {
            this.Name = "{objects.Wine_Cellar}";
            this.UpdateCount++;
        }

        public override void Dispose()
        {
            WorldInstance = null;
            base.Dispose();
        }
    }
}