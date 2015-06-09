#region

using System;
using wServer.realm;
using wServer.realm.entities;

#endregion

namespace wServer.logic.behaviors
{
    public class Reproduce : Behavior
    {
        //State storage: cooldown timer

        private readonly ushort? children;
        private readonly int densityMax;
        private readonly double densityRadius;
        private readonly double spawnRadius;
        private Cooldown coolDown;

        public Reproduce(string children = null, double densityRadius = 10, int densityMax = 5, double spawnRadius = -1,
            Cooldown coolDown = new Cooldown())
        {
            this.children = children == null ? null : (ushort?) BehaviorDb.InitGameData.IdToObjectType[children];
            this.densityRadius = densityRadius;
            this.densityMax = densityMax;
            this.coolDown = coolDown.Normalize(60000);
            this.spawnRadius = spawnRadius == -1 ? densityRadius : spawnRadius;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            int cool;
            if (state == null) cool = coolDown.Next(Random);
            else cool = (int) state;

            if (cool <= 0)
            {
                int count = host.CountEntity(densityRadius, children ?? host.ObjectType);
                if (count < densityMax)
                {
                    Entity entity = Entity.Resolve(host.Manager, children ?? host.ObjectType);

                    double targetX = host.X;
                    double targetY = host.Y;

                    int i = 0;
                    do
                    {
                        double angle = Random.NextDouble()*2*Math.PI;
                        targetX = host.X + spawnRadius*0.5*Math.Cos(angle);
                        targetY = host.Y + spawnRadius* 0.5*Math.Sin(angle);
                        i++;
                    } while (targetX < host.Owner.Map.Width &&
                             targetY < host.Owner.Map.Height &&
                             targetX > 0 && targetY > 0 &&
                             host.Owner.Map[(int) targetX, (int) targetY].Terrain !=
                             host.Owner.Map[(int) host.X, (int) host.Y].Terrain &&
                             i < 10);

                    entity.Move((float) targetX, (float) targetY);
                    (entity as Enemy).Terrain = (host as Enemy).Terrain;
                    host.Owner.EnterWorld(entity);
                }
                cool = coolDown.Next(Random);
            }
            else
                cool -= time.thisTickTimes;

            state = cool;
        }
    }
}