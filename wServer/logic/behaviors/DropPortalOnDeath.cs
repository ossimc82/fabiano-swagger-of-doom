#region

using System;
using wServer.realm;
using wServer.realm.entities;

#endregion

namespace wServer.logic.behaviors
{
    public class DropPortalOnDeath : Behavior
    {
        private readonly int despawnTime;
        private readonly int dropDelay;
        private readonly ushort objType;
        private readonly int percent;
        private readonly string stringObjType;
        private readonly float xAdjustment;
        private readonly float yAdjustment;

        public DropPortalOnDeath(ushort objType, int percent, int dropDelaySec = 0, float XAdjustment = 0,
            float YAdjustment = 0, int PortalDespawnTimeSec = 30)
        {
            this.objType = objType;
            stringObjType = null;
            this.percent = percent;
            xAdjustment = XAdjustment;
            yAdjustment = YAdjustment;
            dropDelay = dropDelaySec;
            despawnTime = PortalDespawnTimeSec;
        }

        public DropPortalOnDeath(string objType, int percent, int dropDelaySec = 0, float XAdjustment = 0,
            float YAdjustment = 0, int PortalDespawnTimeSec = 30)
        {
            stringObjType = objType;
            this.percent = percent;
            xAdjustment = XAdjustment;
            yAdjustment = YAdjustment;
            dropDelay = dropDelaySec;
            despawnTime = PortalDespawnTimeSec;
        }

        protected internal override void Resolve(State parent)
        {
            parent.Death += (sender, e) =>
            {
                if (e.Host.Owner.Name == "Arena") return;
                if (new Random().Next(1, 100) <= percent)
                {
                    Portal entity = objType == 0
                        ? Entity.Resolve(e.Host.Manager, stringObjType) as Portal
                        : Entity.Resolve(e.Host.Manager, objType) as Portal;
                    Entity en = e.Host;
                    World w = e.Host.Manager.GetWorld(e.Host.Owner.Id);
                    entity.Move(en.X + xAdjustment, en.Y + yAdjustment);
                    w.Timers.Add(new WorldTimer(dropDelay*1000, (world, t) => { w.EnterWorld(entity); }));
                    w.Timers.Add(new WorldTimer(despawnTime*1000, (world, t) =>
                    {
                        try
                        {
                            w.LeaveWorld(entity);
                        }
                        catch (Exception ex)
                        {
                            log.ErrorFormat("Couldn't despawn portal.\n{0}", ex);
                        }
                    }));
                }
            };
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
        }
    }
}