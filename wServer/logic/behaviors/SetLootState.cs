using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    public class SetLootState : Behavior
    {
        private readonly string lootState;

        public SetLootState(string lootState = "")
        {
            this.lootState = lootState;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            if (host is Enemy)
                (host as Enemy).LootState = lootState;
            else throw new InvalidOperationException("Not an Enemy.");
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state) { }
    }
}
