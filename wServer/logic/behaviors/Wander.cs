#region

using Mono.Game;
using wServer.realm;

#endregion

namespace wServer.logic.behaviors
{
    public class Wander : CycleBehavior
    {
        //State storage: direction & remain time


        private static Cooldown period = new Cooldown(500, 200);
        private readonly float speed;

        public Wander(double speed)
        {
            this.speed = (float) speed;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            WanderStorage storage;
            if (state == null) storage = new WanderStorage();
            else storage = (WanderStorage) state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed)) return;

            Status = CycleStatus.InProgress;
            if (storage.RemainingDistance <= 0)
            {
                storage.Direction = new Vector2(Random.Next(-1, 2), Random.Next(-1, 2));
                storage.Direction.Normalize();
                storage.RemainingDistance = period.Next(Random)/1000f;
                Status = CycleStatus.Completed;
            }

            //var en = host.GetNearestEntity(20, null);
            //var player = en as Player;

            //if(en == null)
            //{
            //    return;
            //}

            float dist = host.GetSpeed(speed)*(time.thisTickTimes/1000f);
            host.ValidateAndMove(host.X + storage.Direction.X*dist, host.Y + storage.Direction.Y*dist);
            host.UpdateCount++;

            storage.RemainingDistance -= dist;

            state = storage;
        }

        private class WanderStorage
        {
            public Vector2 Direction;
            public float RemainingDistance;
        }
    }
}