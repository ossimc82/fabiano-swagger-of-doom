using Mono.Game;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.entities.player;
using wServer.realm.worlds;

namespace wServer.logic.behaviors.PetBehaviors
{
    internal class PetFollow : CycleBehavior
    {
        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if ((host as Pet)?.PlayerOwner == null) return;
            var pet = (Pet)host;
            FollowState s;
            if (state == null) s = new FollowState();
            else s = (FollowState)state;

            Status = CycleStatus.NotStarted;

            var player = host.GetEntity(pet.PlayerOwner.Id) as Player;
            if (player == null)
            {
                var tile = host.Owner.Map[(int)host.X, (int)host.Y].Clone();
                if (tile.Region != TileRegion.PetRegion)
                {
                    if (!(host.Owner is PetYard))
                    {
                        host.Owner.LeaveWorld(host);
                        return;
                    }
                    if (tile.Region != TileRegion.Spawn)
                    {
                        host.Owner.LeaveWorld(host);
                        return;
                    }
                }
            }

            switch (s.State)
            {
                case F.DontKnowWhere:
                    if (s.RemainingTime > 0)
                        s.RemainingTime -= time.thisTickTimes;
                    else
                        s.State = F.Acquired;
                    break;
                case F.Acquired:
                    if (player == null)
                    {
                        s.State = F.DontKnowWhere;
                        s.RemainingTime = 0;
                        break;
                    }
                    if (s.RemainingTime > 0)
                        s.RemainingTime -= time.thisTickTimes;

                    var vect = new Vector2(player.X - host.X, player.Y - host.Y);
                    if (vect.Length > 20)
                    {
                        host.Move(player.X, player.Y);
                        host.UpdateCount++;
                    }
                    else if (vect.Length > 1)
                    {
                        var dist = host.GetSpeed(0.5f) * (time.thisTickTimes / 1000f);
                        if (vect.Length > 2)
                            dist = host.GetSpeed(0.5f + ((float)player.Stats[4] / 100)) * (time.thisTickTimes / 1000f);
                        else if(vect.Length > 3.5)
                            dist = host.GetSpeed(0.5f + (player.Stats[4] + (float)player.Boost[4] / 100)) * (time.thisTickTimes / 1000f);
                        else if (vect.Length > 5)
                            dist = host.GetSpeed(1.0f + (player.Stats[4] + (float)player.Boost[4] / 100)) * (time.thisTickTimes / 1000f);
                        else if (vect.Length > 6)
                            dist = host.GetSpeed(1.35f + (player.Stats[4] + (float)player.Boost[4] / 100)) * (time.thisTickTimes / 1000f);
                        else if (vect.Length > 7)
                            dist = host.GetSpeed(1.5f + (player.Stats[4] + (float)player.Boost[4] / 100)) * (time.thisTickTimes / 1000f);
                        else if (vect.Length > 10)
                            dist = host.GetSpeed(2f + (player.Stats[4] + (float)player.Boost[4] / 100)) * (time.thisTickTimes / 1000f);

                        Status = CycleStatus.InProgress;
                        vect.Normalize();
                        host.ValidateAndMove(host.X + vect.X * dist, host.Y + vect.Y * dist);
                        host.UpdateCount++;
                    }

                    break;
            }

            state = s;
        }

        private enum F
        {
            DontKnowWhere,
            Acquired,
        }

        private class FollowState
        {
            public int RemainingTime;
            public F State;
        }
    }
}
