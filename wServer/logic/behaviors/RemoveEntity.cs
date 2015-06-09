using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm;

namespace wServer.logic.behaviors
{
    public class RemoveEntity : Behavior
    {
        private readonly float dist;
        private readonly string children;

        public RemoveEntity(double dist, string children)
        {
            this.dist = (float)dist;
            this.children = children;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            Entity[] ens = host.GetNearestEntities(dist).ToArray();
            foreach (Entity e in ens)
                if (e.ObjectType == host.Manager.GameData.IdToObjectType[children])
                    host.Owner.LeaveWorld(e);
        } 

        protected override void TickCore(Entity host, RealmTime time, ref object state) { }
    }
}
