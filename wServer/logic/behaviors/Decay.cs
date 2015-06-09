#region

using wServer.realm;

#endregion

namespace wServer.logic.behaviors
{
    public class Decay : Behavior
    {
        //State storage: timer

        private readonly int time;

        public Decay(int time = 10000)
        {
            this.time = time;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = this.time;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            int cool = (int) state;

            if (cool <= 0)
                host.Owner.LeaveWorld(host);
            else
                cool -= time.thisTickTimes;

            state = cool;
        }
    }
}