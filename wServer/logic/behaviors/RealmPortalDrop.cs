using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm;

namespace wServer.logic.behaviors
{
    public class RealmPortalDrop : Behavior
    {

        protected internal override void Resolve(State parent)
        {
            parent.Death += (e, s) =>
            {
                Entity en = s.Host.GetNearestEntity(100, 0x5e4b);
                Entity portal = Entity.Resolve(s.Host.Manager, "Realm Portal");

                if (en != null)
                    portal.Move(en.X, en.Y);
                else
                    portal.Move(s.Host.X, s.Host.Y);

                s.Host.Owner.EnterWorld(portal);
            };
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            if (host.GetNearestEntity(100, 0x5e4b) != null) return;
            Entity opener = Entity.Resolve(host.Manager, "Realm Portal Opener");
            host.Owner.EnterWorld(opener);
            opener.Move(host.X, host.Y);
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state) { }
    }
}
