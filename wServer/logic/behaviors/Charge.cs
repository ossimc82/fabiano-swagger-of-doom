#region

using Mono.Game;
using wServer.realm;
using wServer.realm.entities.player;

#endregion

namespace wServer.logic.behaviors
{
    public class Charge : CycleBehavior
    {
        //State storage: charge state
        private readonly float range;
        private readonly float speed;
        private Cooldown coolDown;

        public Charge(double speed = 4, float range = 10, Cooldown coolDown = new Cooldown())
        {
            this.speed = (float) speed;
            this.range = range;
            this.coolDown = coolDown.Normalize(2000);
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            ChargeState s;
            if (state == null) s = new ChargeState();
            else s = (ChargeState) state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed)) return;

            if (s.RemainingTime <= 0)
            {
                if (s.Direction == Vector2.Zero)
                {
                    var player = (Player) host.GetNearestEntity(range, null);
                    if (player != null && player.X != host.X && player.Y != host.Y)
                    {
                        s.Direction = new Vector2(player.X - host.X, player.Y - host.Y);
                        float d = s.Direction.Length;
                        s.Direction.Normalize();
                        s.RemainingTime = coolDown.Next(Random);
                        if (d/host.GetSpeed(speed) < s.RemainingTime)
                            s.RemainingTime = (int) (d/host.GetSpeed(speed)*1000);
                        Status = CycleStatus.InProgress;
                    }
                }
                else
                {
                    s.Direction = Vector2.Zero;
                    s.RemainingTime = coolDown.Next(Random);
                    Status = CycleStatus.Completed;
                }
            }
            if (s.Direction != Vector2.Zero)
            {
                float dist = host.GetSpeed(speed)*(time.thisTickTimes/1000f);
                host.ValidateAndMove(host.X + s.Direction.X*dist, host.Y + s.Direction.Y*dist);
                host.UpdateCount++;
                Status = CycleStatus.InProgress;
            }
            s.RemainingTime -= time.thisTickTimes;

            state = s;
        }

        private class ChargeState
        {
            public Vector2 Direction;
            public int RemainingTime;
        }
    }
}