using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors.Drakes
{
    internal class PurpleDrakeAttack : Behavior
    {
        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = 0;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            int cool = (int)state;
            if (cool <= 0)
            {
                var entities = host.GetNearestEntities(6);

                Enemy en = null;
                foreach (Entity e in entities)
                    if (e is Enemy)
                    {
                        en = e as Enemy;
                        break;
                    }

                if (en != null & en.ObjectDesc.Enemy)
                {
                    en.Owner.BroadcastPacket(new ShowEffectPacket
                    {
                        EffectType = EffectType.AreaBlast,
                        Color = new ARGB(0x3E3A78),
                        TargetId = en.Id,
                        PosA = new Position { X = 1, }
                    }, null);
                    en.Owner.BroadcastPacket(new ShowEffectPacket
                    {
                        EffectType = EffectType.Trail,
                        TargetId = host.Id,
                        PosA = new Position { X = en.X, Y = en.Y },
                        Color = new ARGB(0x3E3A78)
                    }, null);
                    en.Damage(host.GetPlayerOwner(), time, 35, false, new ConditionEffect[] { });
                }
                cool = 300;
            }
            else
                cool -= time.thisTickTimes;

            state = cool;
        }
    }
}
