using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm;
using wServer.realm.setpieces;

namespace wServer.logic.behaviors
{
    public class ApplySetpiece : Behavior
    {
        private readonly string name;

        public ApplySetpiece(string name)
        {
            this.name = name;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            var piece = (ISetPiece)Activator.CreateInstance(Type.GetType(
                "wServer.realm.setpieces." + name, true, true));
            piece.RenderSetPiece(host.Owner, new IntPoint((int)host.X, (int)host.Y));
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state) { }
    }
}
