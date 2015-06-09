#region

using Mono.Game;
using wServer.realm;

#endregion

namespace wServer.logic.behaviors
{
    public class StayAbove : CycleBehavior
    {
        //State storage: none

        private readonly int altitude;
        private readonly float speed;

        public StayAbove(double speed, int altitude)
        {
            this.speed = (float) speed;
            this.altitude = altitude;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed)) return;

            Wmap map = host.Owner.Map;
            WmapTile tile = map[(int) host.X, (int) host.Y];
            if (tile.Elevation != 0 && tile.Elevation < altitude)
            {
                Vector2 vect;
                vect = new Vector2(map.Width/2 - host.X, map.Height/2 - host.Y);
                vect.Normalize();
                float dist = host.GetSpeed(speed)*(time.thisTickTimes/1000f);
                host.ValidateAndMove(host.X + vect.X*dist, host.Y + vect.Y*dist);
                host.UpdateCount++;

                Status = CycleStatus.InProgress;
            }
            else
                Status = CycleStatus.Completed;
        }
    }
}