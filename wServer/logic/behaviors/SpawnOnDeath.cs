using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class SpawnOnDeath : Behavior
    {
        public string name;
        public double prob;
        public int amount;

        public SpawnOnDeath(string obj, double probability, int amount=1)
        {
            this.name = obj;
            this.prob = probability;
            this.amount = amount;
        }

        protected internal override void Resolve(State parent)
        {
            parent.Death += (sender, e) =>
            {
                if (Random.NextDouble() < this.prob)
                {
                    for (int i = 0; i < this.amount; i++)
                    {
                        Entity entity = Entity.Resolve(e.Host.Manager, this.name);
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
