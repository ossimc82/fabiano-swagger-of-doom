#region

using log4net;
using System;

#endregion

namespace wServer.realm
{
    public class WorldTimer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WorldTimer));
        private readonly Action<World, RealmTime> cb;
        private readonly int total;
        private int remain;
        private bool destroy;

        public int Remaining { get { return remain; } }

        public WorldTimer(int tickMs, Action<World, RealmTime> callback)
        {
            remain = total = tickMs;
            cb = callback;
        }

        public void Reset()
        {
            remain = total;
        }

        public void Destroy()
        {
            destroy = true;
        }

        public int RemainingInSeconds()
        {
            return (int)TimeSpan.FromMilliseconds(remain).TotalSeconds;
        }

        public bool Tick(World world, RealmTime time)
        {
            if (destroy)
            {
                world.Timers.Remove(this);
                return true;
            }
            remain -= time.thisTickTimes;
            if (remain < 0)
            {
                try
                {
                    cb(world, time);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                return true;
            }
            return false;
        }
    }
}