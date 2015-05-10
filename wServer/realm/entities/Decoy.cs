#region

//using wServer.logic;
using System;
using System.Collections.Generic;
using Mono.Game;
using wServer.networking.svrPackets;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.entities
{
    internal class Decoy : StaticObject, IPlayer
    {
        private static readonly Random rand = new Random();

        private readonly int duration;
        private readonly Player player;
        private readonly float speed;
        private Vector2 direction;
        private bool exploded;

        public Decoy(RealmManager manager, Player player, int duration, float tps)
            : base(manager, 0x0715, duration, true, true, true)
        {
            this.player = player;
            this.duration = duration;
            speed = tps;

            Position? history = player.TryGetHistory(100);
            if (history == null)
                direction = GetRandDirection();
            else
            {
                direction = new Vector2(player.X - history.Value.X, player.Y - history.Value.Y);
                if (direction.LengthSquared() == 0)
                    direction = GetRandDirection();
                else
                    direction.Normalize();
            }
        }

        public void Damage(int dmg, Entity chr) { }

        public bool IsVisibleToEnemy() { return true; }

        private Vector2 GetRandDirection()
        {
            double angle = rand.NextDouble()*2*Math.PI;
            return new Vector2(
                (float) Math.Cos(angle),
                (float) Math.Sin(angle)
                );
        }

        public static Decoy DecoyRandom(RealmManager manager, Player player, int duration, float tps)
        {
            Decoy d = new Decoy(manager, player, duration, tps);
            d.direction = d.GetRandDirection();
            return d;
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.Texture1] = player.Texture1;
            stats[StatsType.Texture2] = player.Texture2;
            base.ExportStats(stats);
        }

        public override void Tick(RealmTime time)
        {
            if (HP > duration / 2)
            {
                this.ValidateAndMove(
                    X + direction.X * speed * time.thisTickTimes / 1000,
                    Y + direction.Y * speed * time.thisTickTimes / 1000
                );
            }
            if (HP < 250 && !exploded)
            {
                exploded = true;
                Owner.BroadcastPacket(new ShowEffectPacket()
                {
                    EffectType = EffectType.AreaBlast,
                    Color = new ARGB(0xffff0000),
                    TargetId = Id,
                    PosA = new Position() { X = 1 }
                }, null);
            }
            base.Tick(time);
        }
    }
}