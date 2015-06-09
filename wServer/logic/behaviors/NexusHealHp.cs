#region

using System;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities.player;

#endregion

namespace wServer.logic.behaviors
{
    public class NexusHealHp : Behavior
    {
        //State storage: cooldown timer

        private readonly int amount;
        private readonly double range;
        private Cooldown coolDown;

        public NexusHealHp(double range, int amount, Cooldown coolDown = new Cooldown())
        {
            this.range = (float) range;
            this.amount = amount;
            this.coolDown = coolDown.Normalize();
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = 0;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            int cool = (int) state;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffectIndex.Sick)) return;

                Player entity = host.GetNearestEntity(range, null) as Player;

                if (entity != null)
                {
                    int maxHp = entity.Stats[0] + entity.Boost[0];
                    int newHp = Math.Min(maxHp, entity.HP + amount);
                    if (newHp != entity.HP)
                    {
                        int n = newHp - entity.HP;
                        entity.HP = newHp;
                        entity.UpdateCount++;
                        entity.Owner.BroadcastPacket(new ShowEffectPacket
                        {
                            EffectType = EffectType.Potion,
                            TargetId = entity.Id,
                            Color = new ARGB(0xffffffff)
                        }, null);
                        entity.Owner.BroadcastPacket(new ShowEffectPacket
                        {
                            EffectType = EffectType.Trail,
                            TargetId = host.Id,
                            PosA = new Position {X = entity.X, Y = entity.Y},
                            Color = new ARGB(0xffffffff)
                        }, null);
                        entity.Owner.BroadcastPacket(new NotificationPacket
                        {
                            ObjectId = entity.Id,
                            Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"+" + n + "\"}}",
                            Color = new ARGB(0xff00ff00)
                        }, null);
                    }
                }
                cool = coolDown.Next(Random);
            }
            else
                cool -= time.thisTickTimes;

            state = cool;
        }
    }
}