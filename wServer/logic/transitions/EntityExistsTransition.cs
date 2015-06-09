using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm;

namespace wServer.logic.transitions
{
    public class EntityExistsTransition : Transition
    {
        //State storage: none

        private readonly double dist;
        private readonly ushort target;

        public EntityExistsTransition(string target, double dist, string targetState)
            : base(targetState)
        {
            this.dist = dist;
            this.target = BehaviorDb.InitGameData.IdToObjectType[target];
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            return host.GetNearestEntity(dist, target) != null;
        }
    }
}
