using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm;

namespace wServer.logic.behaviors
{
    public class JumpToRandomOffset : CycleBehavior
    {
        private readonly int minX;
        private readonly int maxX;
        private readonly int minY;
        private readonly int maxY;

        public JumpToRandomOffset(int minX, int maxX, int minY, int maxY)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            host.Move(host.X + Random.Next(minX, maxX), host.Y + Random.Next(minY, maxY));
            host.UpdateCount++;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
        }
    }
}
