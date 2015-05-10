#region

using log4net;
using System;
using wServer.realm;

#endregion

namespace wServer.logic
{
    public abstract class Behavior : IStateChildren
    {
        [ThreadStatic] private static Random rand;
        private static int randomCount = 0;
        public static ILog log = LogManager.GetLogger(typeof(Behavior));

        protected static Random Random
        {
            get
            {
                if (rand == null) rand = new Random();
                randomCount++;
                if (randomCount >= 10)
                {
                    randomCount = 0;
                    rand = new Random();
                }
                return rand;
            }
        }

        public void Tick(Entity host, RealmTime time)
        {
            object state;
            if (!host.StateStorage.TryGetValue(this, out state))
                state = null;

            try
            {
                TickCore(host, time, ref state);

                if (state == null)
                    host.StateStorage.Remove(this);
                else
                    host.StateStorage[this] = state;
            }
            catch (Exception e)
            {
                log.ErrorFormat("BehaviorException:\nHost: {0}\nState: {1}\nInternalExeption:\n{2}",
                    host.Manager.GameData.ObjectTypeToId[host.ObjectType], state, e);
            }
        }

        protected abstract void TickCore(Entity host, RealmTime time, ref object state);

        public void OnStateEntry(Entity host, RealmTime time)
        {
            object state;
            if (!host.StateStorage.TryGetValue(this, out state))
                state = null;

            OnStateEntry(host, time, ref state);

            if (state == null)
                host.StateStorage.Remove(this);
            else
                host.StateStorage[this] = state;
        }

        protected virtual void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
        }

        public void OnStateExit(Entity host, RealmTime time)
        {
            object state;
            if (!host.StateStorage.TryGetValue(this, out state))
                state = null;

            OnStateExit(host, time, ref state);

            if (state == null)
                host.StateStorage.Remove(this);
            else
                host.StateStorage[this] = state;
        }

        protected virtual void OnStateExit(Entity host, RealmTime time, ref object state)
        {
        }

        protected internal virtual void Resolve(State parent)
        {
        }
    }
}