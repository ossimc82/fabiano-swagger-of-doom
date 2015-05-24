using Mono.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.networking.svrPackets;
using wServer.realm;

namespace wServer.logic.behaviors.Drakes
{
    internal class WhiteDrakeAttack : Behavior
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
                var entity = host.GetPlayerOwner();

                if (!entity.HasConditionEffect(ConditionEffectIndex.Sick))
                {
                    try
                    {
                        var distance = Vector2.Distance(new Vector2(host.X, host.Y),
                            new Vector2(entity.X, entity.Y));

                        if (distance < 10)
                        {
                            var hp = entity.HP;
                            var maxHp = entity.Stats[0] + entity.Boost[0];
                            hp = Math.Min(hp + 25, maxHp);

                            if (hp != entity.HP)
                            {
                                var n = hp - entity.HP;
                                entity.HP = hp;
                                entity.UpdateCount++;
                                entity.Owner.BroadcastPacket(new ShowEffectPacket
                                {
                                    EffectType = EffectType.Potion,
                                    TargetId = entity.Id,
                                    Color = new ARGB(0xFFFFFF)
                                }, null);
                                entity.Owner.BroadcastPacket(new ShowEffectPacket
                                {
                                    EffectType = EffectType.Trail,
                                    TargetId = host.Id,
                                    PosA = new Position { X = entity.X, Y = entity.Y },
                                    Color = new ARGB(0xFFFFFF)
                                }, null);
                                entity.Owner.BroadcastPacket(new NotificationPacket
                                {
                                    ObjectId = entity.Id,
                                    Text = "+" + n,
                                    Color = new ARGB(0xff00ff00)
                                }, null);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                cool = 500;
            }
            else
                cool -= time.thisTickTimes;

            state = cool;
        }
    }
}
