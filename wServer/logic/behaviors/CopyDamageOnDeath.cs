using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    public class CopyDamageOnDeath : Behavior
    {
        private float dist;
        private string child;

        public CopyDamageOnDeath(string child, float dist = 50)
        {
            this.dist = dist;
            this.child = child;
        }

        protected internal override void Resolve(State parent)
        {
            parent.Death += (sender, e) =>
            {
                Enemy en;
                if ((en = e.Host.GetNearestEntity(dist, e.Host.Manager.GameData.IdToObjectType[child]) as Enemy) != null)
                {
                    en.SetDamageCounter((e.Host as Enemy).DamageCounter, en);
                }
            };
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state) { }
    }
}
