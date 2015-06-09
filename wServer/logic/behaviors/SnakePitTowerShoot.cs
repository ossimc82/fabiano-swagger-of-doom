using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    public class SnakePitTowerShoot : Shoot
    {
        public SnakePitTowerShoot()
            : base(radius: 25, count: 1, projectileIndex: 0, coolDown: 2000)
        {
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            base.OnStateEntry(host, time, ref state);
            WmapTile tile = host.Owner.Map[(int)host.X + 1, (int)host.Y].Clone();
            if (tile.ObjType != 0)
                base.fixedAngle = 140;
            else
                base.fixedAngle = 0;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if (state == null) return;
            int cool = (int)state;
            Status = CycleStatus.NotStarted;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffectIndex.Stunned)) return;

                Entity player = host.GetNearestEntity(radius, null);

                WmapTile tile = host.Owner.Map[(int)host.X + 1, (int)host.Y].Clone();

                if (tile.ObjType != 0)
                    base.fixedAngle = 180 * Math.PI / 180;
                else
                    base.fixedAngle = 0 * Math.PI / 180;


                if (player != null || defaultAngle != null || fixedAngle != null)
                {
                    ProjectileDesc desc = host.ObjectDesc.Projectiles[projectileIndex];

                    double a = fixedAngle ??
                               (player == null ? defaultAngle.Value : Math.Atan2(player.Y - host.Y, player.X - host.X));
                    a += angleOffset;

                    int dmg;
                    if (host is Character)
                        dmg = (host as Character).Random.Next(desc.MinDamage, desc.MaxDamage);
                    else
                        dmg = Random.Next(desc.MinDamage, desc.MaxDamage);

                    double startAngle = a - shootAngle * (count - 1) / 2;
                    byte prjId = 0;
                    Position prjPos = new Position { X = host.X, Y = host.Y };
                    for (int i = 0; i < count; i++)
                    {
                        Projectile prj = host.CreateProjectile(
                            desc, host.ObjectType, dmg, time.tickTimes,
                            prjPos, (float)(startAngle + shootAngle * i));
                        host.Owner.EnterWorld(prj);
                        if (i == 0)
                            prjId = prj.ProjectileId;
                    }

                    host.Owner.BroadcastPacket(new ShootPacket
                    {
                        BulletId = prjId,
                        OwnerId = host.Id,
                        Position = prjPos,
                        Angle = (float)startAngle,
                        Damage = (short)dmg,
                        BulletType = (byte)(desc.BulletType),
                        AngleInc = (float)shootAngle,
                        NumShots = (byte)count,
                    }, null);
                }
                cool = coolDown.Next(Random);
                Status = CycleStatus.Completed;
            }
            else
            {
                cool -= time.thisTickTimes;
                Status = CycleStatus.InProgress;
            }

            state = cool;
        }
    }
}
