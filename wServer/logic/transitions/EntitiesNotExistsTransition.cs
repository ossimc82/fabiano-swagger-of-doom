using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm;

namespace wServer.logic.transitions
{
    public class EntitiesNotExistsTransition : Transition
    {
        private readonly double dist;
        private readonly string[] childrens;

        public EntitiesNotExistsTransition(double dist, string targetState, params string[] childrens)
            : base(targetState)
        {
            this.dist = dist;
            this.childrens = childrens;
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            foreach (string children in childrens)
                if (host.GetNearestEntity(dist, host.Manager.GameData.IdToObjectType[children]) != null) return false;
            return true;
        }
    }
}
