#region

using wServer.realm;

#endregion

namespace wServer.logic.behaviors
{
    public class Transform : Behavior
    {
        //State storage: none

        private readonly ushort target;

        public Transform(string target)
        {
            this.target = BehaviorDb.InitGameData.IdToObjectType[target];
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            Entity entity = Entity.Resolve(host.Manager, target);

            entity.Move(host.X, host.Y);
            host.Owner.EnterWorld(entity);
            host.Owner.LeaveWorld(host);
        }
    }
}