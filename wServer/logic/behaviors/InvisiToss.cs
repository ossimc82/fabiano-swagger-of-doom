#region

using System;
using wServer.realm;
using wServer.realm.entities;

#endregion

namespace wServer.logic.behaviors
{
    public class InvisiToss : Behavior
    {
        //State storage: cooldown timer

        private readonly ushort child;
        private readonly int coolDownOffset;
        private readonly double range;
        private double? angle;
        private Cooldown coolDown;

        public InvisiToss(string child, double range = 5, double? angle = null,
            Cooldown coolDown = new Cooldown(), int coolDownOffset = 0)
        {
            this.child = BehaviorDb.InitGameData.IdToObjectType[child];
            this.range = range;
            this.angle = angle*Math.PI/180;
            this.coolDown = coolDown.Normalize();
            this.coolDownOffset = coolDownOffset;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = coolDownOffset;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            int cool = (int) state;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffectIndex.Stunned)) return;

                Position target = new Position
                {
                    X = host.X + (float) (range*Math.Cos(angle.Value)),
                    Y = host.Y + (float) (range*Math.Sin(angle.Value)),
                };
                host.Owner.Timers.Add(new WorldTimer(0, (world, t) =>
                {
                    Entity entity = Entity.Resolve(world.Manager, child);
                    entity.Move(target.X, target.Y);
                    (entity as Enemy).Terrain = (host as Enemy).Terrain;
                    world.EnterWorld(entity);
                }));
                cool = coolDown.Next(Random);
            }
            else
                cool -= time.thisTickTimes;

            state = cool;
        }
    }
}