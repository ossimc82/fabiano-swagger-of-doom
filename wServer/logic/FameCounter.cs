#region

using System.Collections.Generic;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.entities.player;

#endregion

namespace wServer.logic
{
    public class FameCounter
    {
        private readonly Player player;
        private readonly HashSet<Projectile> projs = new HashSet<Projectile>();

        private readonly FameStats stats;
        private int elapsed;

        public FameCounter(Player player)
        {
            this.player = player;
            stats = player.Client.Character.FameStats;
        }

        public Player Host
        {
            get { return player; }
        }

        public void Shoot(Projectile proj)
        {
            stats.Shots++;
            projs.Add(proj);
        }

        public void Hit(Projectile proj, Enemy enemy)
        {
            if (projs.Contains(proj))
            {
                projs.Remove(proj);
                stats.ShotsThatDamage++;
            }
        }

        public void Killed(Enemy enemy, bool killer)
        {
            if (enemy.ObjectDesc.God)
                stats.GodAssists++;
            else
                stats.MonsterAssists++;
            if (player.Quest == enemy)
                stats.QuestsCompleted++;
            if (killer)
            {
                if (enemy.ObjectDesc.God)
                    stats.GodKills++;
                else
                    stats.MonsterKills++;

                if (enemy.ObjectDesc.Cube)
                    stats.CubeKills++;
                if (enemy.ObjectDesc.Oryx)
                    stats.OryxKills++;
            }
        }

        public void LevelUpAssist(int count)
        {
            stats.LevelUpAssists += count;
        }

        public void TileSent(int num)
        {
            stats.TilesUncovered += num;
        }

        public void Teleport()
        {
            stats.Teleports++;
        }

        public void UseAbility()
        {
            stats.SpecialAbilityUses++;
        }

        public void DrinkPot()
        {
            stats.PotionsDrunk++;
        }

        public void Tick(RealmTime time)
        {
            elapsed += time.thisTickTimes;
            if (elapsed > 1000*60)
            {
                elapsed -= 1000*60;
                stats.MinutesActive++;
            }
        }

        public void RemoveProjectile(Projectile projectile)
        {
            if(projs.Contains(projectile))
                projs.Remove(projectile);
        }
    }
}