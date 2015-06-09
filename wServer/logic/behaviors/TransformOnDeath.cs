#region

using wServer.realm;
using wServer.realm.entities;

#endregion

namespace wServer.logic.behaviors
{
    public class TransformOnDeath : Behavior
    {
        private readonly int max;
        private readonly int min;
        private readonly float probability;
        private readonly ushort target;
        private readonly bool returnToSpawn;

        public TransformOnDeath(string target, int min = 1, int max = 1, double probability = 1, bool returnToSpawn = false)
        {
            this.target = BehaviorDb.InitGameData.IdToObjectType[target];
            this.min = min;
            this.max = max;
            this.probability = (float) probability;
            this.returnToSpawn = returnToSpawn;
        }

        protected internal override void Resolve(State parent)
        {
            parent.Death += (sender, e) =>
            {
                if (e.Host.CurrentState.Is(parent) &&
                    Random.NextDouble() < probability)
                {
                    int count = Random.Next(min, max + 1);
                    for (int i = 0; i < count; i++)
                    {
                        Entity entity = Entity.Resolve(e.Host.Manager, target);

                        if (returnToSpawn)
                            entity.Move((e.Host as Enemy).SpawnPoint.X, (e.Host as Enemy).SpawnPoint.Y);
                        else
                            entity.Move(e.Host.X, e.Host.Y);
                        e.Host.Owner.EnterWorld(entity);
                    }
                }
            };
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
        }
    }
}