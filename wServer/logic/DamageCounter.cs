#region

using System;
using System.Collections.Generic;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.entities.player;
using wServer.realm.worlds;

#endregion

namespace wServer.logic
{
    public class DamageCounter
    {
        private readonly WeakDictionary<Player, int> hitters = new WeakDictionary<Player, int>();
        private Enemy enemy;

        public DamageCounter(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public Enemy Host
        {
            get { return enemy; }
        }

        public Projectile LastProjectile { get; private set; }
        public Player LastHitter { get; private set; }

        public DamageCounter Corpse { get; set; }
        public DamageCounter Parent { get; set; }

        public void UpdateEnemy(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void HitBy(Player player, RealmTime time, Projectile projectile, int dmg)
        {
            int totalDmg;
            if (!hitters.TryGetValue(player, out totalDmg))
                totalDmg = 0;
            totalDmg += dmg;
            hitters[player] = totalDmg;

            LastProjectile = projectile;
            LastHitter = player;

            player.FameCounter.Hit(projectile, enemy);
        }

        public Tuple<Player, int>[] GetPlayerData()
        {
            if (Parent != null)
                return Parent.GetPlayerData();
            List<Tuple<Player, int>> dat = new List<Tuple<Player, int>>();
            foreach (KeyValuePair<Player, int> i in hitters)
            {
                if (i.Key.Owner == null) continue;
                dat.Add(new Tuple<Player, int>(i.Key, i.Value));
            }
            return dat.ToArray();
        }

        public void Death(RealmTime time)
        {
            if (Corpse != null)
            {
                Corpse.Parent = this;
                return;
            }

            List<Tuple<Player, int>> eligiblePlayers = new List<Tuple<Player, int>>();
            int totalDamage = 0;
            int totalPlayer = 0;
            Enemy enemy = (Parent ?? this).enemy;
            foreach (KeyValuePair<Player, int> i in (Parent ?? this).hitters)
            {
                if (i.Key.Owner == null) continue;
                totalDamage += i.Value;
                totalPlayer++;
                eligiblePlayers.Add(new Tuple<Player, int>(i.Key, i.Value));
            }
            if (totalPlayer != 0)
            {
                float totalExp = totalPlayer*((float) enemy.ObjectDesc.MaxHP/10f)*(enemy.ObjectDesc.ExpMultiplier ?? 1);
                float lowerLimit = totalExp/totalPlayer*0.1f;
                int lvUps = 0;
                foreach (Tuple<Player, int> i in eligiblePlayers)
                {
                    float playerXp = totalExp*i.Item2/totalDamage;

                    float upperLimit = i.Item1.ExperienceGoal*0.1f;
                    if (i.Item1.Quest == enemy)
                        upperLimit = i.Item1.ExperienceGoal*0.5f;

                    if (playerXp < lowerLimit) playerXp = lowerLimit;
                    if (playerXp > upperLimit) playerXp = upperLimit;

                    bool killer = (Parent ?? this).LastHitter == i.Item1;
                    if (i.Item1.EnemyKilled(
                        enemy,
                        (int) playerXp,
                        killer) && !killer)
                        lvUps++;
                }
                (Parent ?? this).LastHitter.FameCounter.LevelUpAssist(lvUps);
            }

            if (enemy.Owner is GameWorld)
                (enemy.Owner as GameWorld).EnemyKilled(enemy, (Parent ?? this).LastHitter);
        }
    }
}