using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    public class CopyLootState : Behavior
    {
        private readonly ushort children;
        private readonly double radius;

        public CopyLootState(string children, double radius = 10)
        {
            this.children = BehaviorDb.InitGameData.IdToObjectType[children];
            this.radius = radius;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            if (host is Enemy)
            {
                foreach (Entity i in host.GetNearestEntities(radius, children))
                    if(i is Enemy)
                        if ((i as Enemy).LootState != (host as Enemy).LootState)
                            (i as Enemy).LootState = (host as Enemy).LootState;
            }
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state) { }
    }
}
