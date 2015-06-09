#region

using System;
using Mono.Game;
using wServer.realm;

#endregion

namespace wServer.logic.behaviors
{
    public class Swirl : CycleBehavior
    {
        //State storage: swirl state
        private readonly float acquireRange;
        private readonly float radius;
        private readonly float speed;
        private readonly bool targeted;

        public Swirl(double speed = 1, double radius = 8, double acquireRange = 10, bool targeted = true)
        {
            this.speed = (float) speed;
            this.radius = (float) radius;
            this.acquireRange = (float) acquireRange;
            this.targeted = targeted;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = new SwirlState
            {
                Center = targeted ? Vector2.Zero : new Vector2(host.X, host.Y),
                Acquired = !targeted,
            };
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            SwirlState s = (SwirlState) state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed)) return;

            int period = (int) (1000*radius/host.GetSpeed(speed)*(2*Math.PI));
            if (!s.Acquired &&
                s.RemainingTime <= 0 &&
                targeted)
            {
                Entity entity = host.GetNearestEntity(acquireRange, null);
                if (entity != null && entity.X != host.X && entity.Y != host.Y)
                {
                    //find circle which pass through host and player pos
                    double l = entity.Dist(host);
                    float hx = (host.X + entity.X)/2;
                    float hy = (host.Y + entity.Y)/2;
                    double c = Math.Sqrt(Math.Abs(radius*radius - l*l)/4);
                    s.Center = new Vector2(
                        (float) (hx + c*(host.Y - entity.Y)/l),
                        (float) (hy + c*(entity.X - host.X)/l));

                    s.RemainingTime = period;
                    s.Acquired = true;
                }
                else
                    s.Acquired = false;
            }
            else if (s.RemainingTime <= 0 || (s.RemainingTime - period > 200 && host.GetNearestEntity(2, null) != null))
            {
                if (targeted)
                {
                    s.Acquired = false;
                    Entity entity = host.GetNearestEntity(acquireRange, null);
                    if (entity != null)
                        s.RemainingTime = 0;
                    else
                        s.RemainingTime = 5000;
                }
                else
                    s.RemainingTime = 5000;
            }
            else
                s.RemainingTime -= time.thisTickTimes;

            double angle;
            if (host.Y == s.Center.Y && host.X == s.Center.X) //small offset
                angle = Math.Atan2(host.Y - s.Center.Y + (Random.NextDouble()*2 - 1),
                    host.X - s.Center.X + (Random.NextDouble()*2 - 1));
            else
                angle = Math.Atan2(host.Y - s.Center.Y, host.X - s.Center.X);

            double spd = host.GetSpeed(speed)*(s.Acquired ? 1 : 0.2);
            double angularSpd = spd/radius;
            angle += angularSpd*(time.thisTickTimes/1000f);

            double x = s.Center.X + Math.Cos(angle)*radius;
            double y = s.Center.Y + Math.Sin(angle)*radius;
            Vector2 vect = new Vector2((float) x, (float) y) - new Vector2(host.X, host.Y);
            vect.Normalize();
            vect *= (float) spd*(time.thisTickTimes/1000f);

            host.ValidateAndMove(host.X + vect.X, host.Y + vect.Y);
            host.UpdateCount++;

            Status = CycleStatus.InProgress;

            state = s;
        }

        private class SwirlState
        {
            public bool Acquired;
            public Vector2 Center;
            public int RemainingTime;
        }
    }
}