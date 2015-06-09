#region

using wServer.realm;

#endregion

namespace wServer.logic.behaviors
{
    public class Order : Behavior
    {
        //State storage: none

        private readonly ushort children;
        private readonly double range;
        private readonly string targetStateName;
        private State targetState;

        public Order(double range, string children, string targetState)
        {
            this.range = range;
            this.children = BehaviorDb.InitGameData.IdToObjectType[children];
            targetStateName = targetState;
        }

        private static State FindState(State state, string name)
        {
            if (state.Name == name) return state;
            State ret;
            foreach (State i in state.States)
            {
                if ((ret = FindState(i, name)) != null)
                    return ret;
            }
            return null;
        }


        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if (targetState == null)
                targetState = FindState(host.Manager.Behaviors.Definitions[(ushort) children].Item1, targetStateName);
            foreach (Entity i in host.GetNearestEntities(range, children))
                if (!i.CurrentState.Is(targetState))
                    i.SwitchTo(targetState);
        }
    }
}