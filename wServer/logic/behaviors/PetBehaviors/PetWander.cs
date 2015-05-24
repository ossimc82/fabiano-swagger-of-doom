using Mono.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.worlds;

namespace wServer.logic.behaviors.PetBehaviors
{
    internal class PetWander : CycleBehavior
    {
        //State storage: wander state
        private readonly float speed;
        private Cooldown coolDown;

        public PetWander(double speed, Cooldown coolDown)
        {
            this.speed = (float)speed;
            this.coolDown = coolDown;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if (host is Pet) if ((host as Pet).PlayerOwner != null) return;

            WmapTile tile = host.Owner.Map[(int)host.X, (int)host.Y].Clone();
            if (tile.Region == TileRegion.None && host.Owner is PetYard)
            {
                Position pos = (host as Pet).SpawnPoint;
                host.Move(pos.X, pos.Y);
                return;
            }

            if (host.GetNearestEntity(1, null) == null)
            {
                WanderStorage storage;
                if (state == null) storage = new WanderStorage();
                else storage = (WanderStorage)state;

                Status = CycleStatus.NotStarted;

                if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed)) return;

                Status = CycleStatus.InProgress;
                if (storage.RemainingDistance <= 0)
                {
                    storage.Direction = new Vector2(Random.Next(-2, 2), Random.Next(-2, 2));
                    storage.Direction.Normalize();
                    storage.RemainingDistance = coolDown.Next(Random) / 1000f;
                    Status = CycleStatus.Completed;
                }

                float dist = host.GetSpeed(speed) * (time.thisTickTimes / 1000f);
                host.ValidateAndMove(host.X + storage.Direction.X * dist, host.Y + storage.Direction.Y * dist);
                host.UpdateCount++;

                storage.RemainingDistance -= dist;

                state = storage;
            }
        }

        private class WanderStorage
        {
            public Vector2 Direction;
            public float RemainingDistance;
        }
    }
}
