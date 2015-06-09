#region

using wServer.networking.svrPackets;
using wServer.realm;

#endregion

namespace wServer.logic.behaviors
{
    public class Flash : Behavior
    {
        //State storage: none

        private readonly uint color;
        private readonly float flashPeriod;
        private readonly int flashRepeats;

        public Flash(uint color, double flashPeriod, int flashRepeats)
        {
            this.color = color;
            this.flashPeriod = (float) flashPeriod;
            this.flashRepeats = flashRepeats;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            host.Owner.BroadcastPacket(new ShowEffectPacket
            {
                EffectType = EffectType.Flashing,
                PosA = new Position {X = flashPeriod, Y = flashRepeats},
                TargetId = host.Id,
                Color = new ARGB(color)
            }, null);
        }
    }
}