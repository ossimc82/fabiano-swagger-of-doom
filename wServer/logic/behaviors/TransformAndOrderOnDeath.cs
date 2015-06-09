using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    public class TransformAndOrderOnDeath : Behavior
    {
        private readonly int max;
        private readonly int min;
        private readonly float probability;
        private readonly ushort target;
        private readonly bool returnToSpawn;
        private readonly double range;
        private readonly string targetStateName;
        private State targetState;

        public TransformAndOrderOnDeath(string target, double range, string targetState, int min = 1, int max = 1, double probability = 1, bool returnToSpawn = false)
        {
            this.target = BehaviorDb.InitGameData.IdToObjectType[target];
            this.min = min;
            this.max = max;
            this.probability = (float) probability;
            this.returnToSpawn = returnToSpawn;
            this.targetStateName = targetState;
            this.range = range;
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

        private static State FindState(State state, string name)
        {
            if (state.Name == name) return state;
            State ret;
            foreach (State i in state.States)
            {
                if ((ret = FindState(i, name)) != null)
                    return ret;
            }
            return null;
        }


        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if (targetState == null)
                targetState = FindState(host.Manager.Behaviors.Definitions[(ushort)target].Item1, targetStateName);
            foreach (Entity i in host.GetNearestEntities(range, target))
                if (!i.CurrentState.Is(targetState))
                    i.SwitchTo(targetState);
        }
    }
}
