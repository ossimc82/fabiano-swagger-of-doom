#region

using Mono.Game;
using wServer.realm;

#endregion

namespace wServer.logic.behaviors
{
    public class Buzz : CycleBehavior
    {
        //State storage: direction & remain
        private readonly float dist;
        private readonly float speed;
        private Cooldown coolDown;

        public Buzz(double speed = 2, double dist = 0.5, Cooldown coolDown = new Cooldown())
        {
            this.speed = (float) speed;
            this.dist = (float) dist;
            this.coolDown = coolDown.Normalize(1);
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = new BuzzStorage();
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            //var en = host.GetNearestEntity(20, null);
            //var player = en as Player;

            //if (en == null)
            //{
            //    return;
            //}

            var storage = (BuzzStorage) state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed)) return;

            if (storage.RemainingTime > 0)
            {
                storage.RemainingTime -= time.thisTickTimes;
                Status = CycleStatus.NotStarted;
            }
            else
            {
                Status = CycleStatus.InProgress;
                if (storage.RemainingDistance <= 0)
                {
                    do
                    {
                        storage.Direction = new Vector2(Random.Next(-1, 2), Random.Next(-1, 2));
                    } while (storage.Direction.X == 0 && storage.Direction.Y == 0);
                    storage.Direction.Normalize();
                    storage.RemainingDistance = this.dist;
                    Status = CycleStatus.Completed;
                }
                float dist = host.GetSpeed(speed)*(time.thisTickTimes/1000f);
                host.ValidateAndMove(host.X + storage.Direction.X*dist, host.Y + storage.Direction.Y*dist);
                host.UpdateCount++;

                storage.RemainingDistance -= dist;
            }

            state = storage;
        }

        private class BuzzStorage
        {
            public Vector2 Direction;
            public float RemainingDistance;
            public int RemainingTime;
        }
    }
}