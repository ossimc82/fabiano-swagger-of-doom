#region

using System.Xml.Linq;
using wServer.networking.svrPackets;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.entities
{
    public class Wall : StaticObject
    {
        public Wall(RealmManager manager, ushort objType, XElement node)
            : base(manager, objType, GetHP(node), true, false, true)
        {
        }


        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            if (!Vulnerable || !(projectile.ProjectileOwner is Player)) return true;
            var prevHp = HP;
            var dmg = (int) StatsManager.GetDefenseDamage(this, projectile.Damage, ObjectDesc.Defense);
            HP -= dmg;
            Owner.BroadcastPacket(new DamagePacket
            {
                TargetId = Id,
                Effects = 0,
                Damage = (ushort) dmg,
                Killed = !CheckHP(),
                BulletId = projectile.ProjectileId,
                ObjectId = projectile.ProjectileOwner.Self.Id
            }, HP < 0 && !IsOneHit(dmg, prevHp) ? null : projectile.ProjectileOwner as Player);
            return true;
        }
    }
}