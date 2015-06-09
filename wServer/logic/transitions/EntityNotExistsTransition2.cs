#region

using wServer.realm;

#endregion

namespace wServer.logic.transitions
{
    public class EntityNotExistsTransition2 : Transition
    {
        //State storage: none

        private readonly double dist;
        private readonly ushort target;
        private readonly ushort target2;

        public EntityNotExistsTransition2(string target, string target2, double dist, string targetState)
            : base(targetState)
        {
            this.dist = dist;
            this.target = BehaviorDb.InitGameData.IdToObjectType[target];
            this.target2 = BehaviorDb.InitGameData.IdToObjectType[target2];
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            if (host.GetNearestEntity(dist, target) == null && host.GetNearestEntity(dist, target2) == null)
            {
                return true;
            }
            return false;
        }
    }
}