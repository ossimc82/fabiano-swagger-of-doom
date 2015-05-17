#region

using System;
using System.Collections.Generic;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.entities
{
    public interface IProjectileOwner
    {
        Projectile[] Projectiles { get; }
        Entity Self { get; }
    }

    public class Projectile : Entity
    {
        private readonly HashSet<Entity> hitted = new HashSet<Entity>();
        private CollisionMap<Entity> collisionMap;

        public Projectile(RealmManager manager, ProjectileDesc desc)
            : base(manager, manager.GameData.IdToObjectType[desc.ObjectId])
        {
            Descriptor = desc;
        }

        public IProjectileOwner ProjectileOwner { get; set; }
        public new byte ProjectileId { get; set; }
        public short Container { get; set; }
        public int Damage { get; set; }

        public long BeginTime { get; set; }
        public Position BeginPos { get; set; }
        public float Angle { get; set; }

        public ProjectileDesc Descriptor { get; set; }

        public void Destroy(bool immediate)
        {
            if (!immediate)
                Manager.Logic.AddPendingAction(_ => Destroy(true), PendingPriority.Destruction);
            if (Owner != null)
            {
                if (ProjectileOwner is Player)
                    (ProjectileOwner as Player).FameCounter.RemoveProjectile(this);
                //ProjectileOwner.Projectiles[ProjectileId] = null;
                Owner.LeaveWorld(this);
            }
        }

        public Position GetPosition(long elapsedTicks)
        {
            double x = BeginPos.X;
            double y = BeginPos.Y;

            double dist = (elapsedTicks / 1000.0) * (Descriptor.Speed / 10.0);
            double period = ProjectileId % 2 == 0 ? 0 : Math.PI;
            if (Descriptor.Wavy)
            {
                double theta = Angle + (Math.PI * 64) * Math.Sin(period + 6 * Math.PI * (elapsedTicks / 1000));
                x += dist * Math.Cos(theta);
                y += dist * Math.Sin(theta);
            }
            else if (Descriptor.Parametric)
            {
                double theta = (double)elapsedTicks / Descriptor.LifetimeMS * 2 * Math.PI;
                double a = Math.Sin(theta) * (ProjectileId % 2 != 0 ? 1 : -1);
                double b = Math.Sin(theta * 2) * (ProjectileId % 4 < 2 ? 1 : -1);
                double c = Math.Sin(Angle);
                double d = Math.Cos(Angle);
                x += (a * d - b * c) * Descriptor.Magnitude;
                y += (a * c + b * d) * Descriptor.Magnitude;
            }
            else
            {
                if (Descriptor.Boomerang)
                {
                    double d = (Descriptor.LifetimeMS / 1000.0) * (Descriptor.Speed / 10.0) / 2;
                    if (dist > d)
                        dist = d - (dist - d);
                }
                x += dist * Math.Cos(Angle);
                y += dist * Math.Sin(Angle);
                if (Descriptor.Amplitude != 0)
                {
                    double d = Descriptor.Amplitude *
                               Math.Sin(period +
                                        (double)elapsedTicks / Descriptor.LifetimeMS * Descriptor.Frequency * 2 * Math.PI);
                    x += d * Math.Cos(Angle + Math.PI / 2);
                    y += d * Math.Sin(Angle + Math.PI / 2);
                }
            }
            return new Position { X = (float)x, Y = (float)y };
        }

        public override void Tick(RealmTime time)
        {
            if (collisionMap == null)
                collisionMap = ProjectileOwner is Player
                    ? Owner.EnemiesCollision
                    : Owner.PlayersCollision;

            long elapsedTicks = time.tickTimes - BeginTime;
            if (elapsedTicks > Descriptor.LifetimeMS)
            {
                Destroy(true);
                return;
            }
            long counter = time.thisTickTimes;
            while (counter > Manager.Logic.MsPT && TickCore(elapsedTicks - counter, time))
                counter -= Manager.Logic.MsPT;
            if (Owner != null)
                TickCore(elapsedTicks, time);

            base.Tick(time);
        }

        private bool TickCore(long elapsedTicks, RealmTime time)
        {
            Position pos = GetPosition(elapsedTicks);
            Move(pos.X, pos.Y);

            if (pos.X < 0 || pos.X > Owner.Map.Width)
            {
                Destroy(true);
                return false;
            }
            if (pos.Y < 0 || pos.Y > Owner.Map.Height)
            {
                Destroy(true);
                return false;
            }
            if (Owner.Map[(int)pos.X, (int)pos.Y].TileId == 0xff)
            {
                Destroy(true);
                return false;
            }
            bool penetrateObsta = Descriptor.PassesCover;
            bool penetrateEnemy = Descriptor.MultiHit;

            ushort objId = Owner.Map[(int)pos.X, (int)pos.Y].ObjType;
            if (objId != 0 &&
                Manager.GameData.ObjectDescs[objId].OccupySquare &&
                !penetrateObsta)
            {
                Destroy(true);
                return false;
            }
            return true;
        }

        public void ForceHit(Entity entity, RealmTime time)
        {
            bool penetrateObsta = Descriptor.PassesCover;
            bool penetrateEnemy = Descriptor.MultiHit;
            Move(entity.X, entity.Y);
            if (entity.HitByProjectile(this, time))
            {
                if ((entity is Enemy && penetrateEnemy) ||
                    (entity is StaticObject && (entity as StaticObject).Static && !(entity is Wall) && penetrateObsta))
                    hitted.Add(entity);
                else
                    Destroy(true);
                ProjectileOwner.Self.ProjectileHit(this, entity);
            }
        }
    }
}