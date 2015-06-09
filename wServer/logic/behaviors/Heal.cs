#region

using System.Linq;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;

#endregion

namespace wServer.logic.behaviors
{
    public class Heal : Behavior
    {
        //State storage: cooldown timer

        private readonly string group;
        private readonly double range;
        private Cooldown coolDown;

        public Heal(double range, string group, Cooldown coolDown = new Cooldown())
        {
            this.range = (float) range;
            this.group = group;
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
                if (group == "Self")
                {
                    Enemy entity = host as Enemy;
                    int newHp = (int)entity.ObjectDesc.MaxHP;
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
                        entity.Owner.BroadcastPacket(new NotificationPacket
                        {
                            ObjectId = entity.Id,
                            Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"+" + n + "\"}}",
                            Color = new ARGB(0xff00ff00)
                        }, null);
                    }
                }
                else
                {
                    foreach (Enemy entity in host.GetNearestEntitiesByGroup(range, group).OfType<Enemy>())
                    {
                        int newHp = (int)entity.ObjectDesc.MaxHP;
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
                                PosA = new Position { X = entity.X, Y = entity.Y },
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
                }
                cool = coolDown.Next(Random);
            }
            else
                cool -= time.thisTickTimes;

            state = cool;
        }
    }
}