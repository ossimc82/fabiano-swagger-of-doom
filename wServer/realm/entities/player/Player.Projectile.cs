namespace wServer.realm.entities.player
{
    public partial class Player
    {
        internal Projectile PlayerShootProjectile(
            byte id, ProjectileDesc desc, ushort objType,
            int time, Position position, float angle)
        {
            ProjectileId = id;
            return CreateProjectile(desc, objType,
                (int) StatsManager.GetAttackDamage(desc.MinDamage, desc.MaxDamage),
                time, position, angle);
        }
    }
}