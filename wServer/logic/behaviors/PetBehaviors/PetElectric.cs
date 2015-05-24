using System;
using System.Linq;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors.PetBehaviors
{
    internal class PetElectric : Behavior
    {
        //State storage: cooldown

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = 0;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if (state == null) return;
            int cool = (int) state;

            if (cool <= 0)
            {
                
                PetLevel level = null;
                if (host is Pet)
                {
                    Pet p = host as Pet;
                    level = p.GetPetLevelFromAbility(Ability.Electric, true);
                }
                else return;

                if (level == null) return;

                double dist = getDist(host as Pet, level);

                Enemy[] targets = host.GetNearestEntities(dist).OfType<Enemy>().ToArray();
                foreach (Enemy e in targets)
                {
                    if (e.HasConditionEffect(ConditionEffectIndex.Invulnerable) || e.HasConditionEffect(ConditionEffectIndex.Invincible) || e.HasConditionEffect(ConditionEffectIndex.Stasis)) continue;
                    if (Random.Next(0, 100) > level.Level) break;

                    if (e.ObjectDesc == null | !e.ObjectDesc.Enemy) continue;

                    if (e.HasConditionEffect(ConditionEffectIndex.Invincible)) continue;

                    e.ApplyConditionEffect(new ConditionEffect
                    {
                        DurationMS = level.Level * 40,
                        Effect = ConditionEffectIndex.Paralyzed
                    });

                    e.Owner.BroadcastPacket(new ShowEffectPacket
                    {
                        EffectType = EffectType.ElectricFlashing,
                        PosA = new Position { X = level.Level * 40},
                        TargetId = e.Id
                    }, null);

                    host.Owner.BroadcastPacket(new ShowEffectPacket
                    {
                        PosA = new Position { X = host.X, Y = host.Y },
                        EffectType = EffectType.ElectricBolts,
                        TargetId = host.Id,
                    }, null);

                    e.Damage(null, time, level.Level, true, new ConditionEffect
                    {
                        DurationMS = level.Level * 40,
                        Effect = ConditionEffectIndex.Paralyzed
                    });
                }

                cool = getCooldown(host as Pet, level) / host.Manager.TPS;
            }
            else
                cool -= time.thisTickTimes;

            state = cool;
        }

        private int getCooldown(Pet host, PetLevel type)
        {
            if (type.Level <= 30)
            {
                double cool = 2500;
                for (int i = 0; i < type.Level; i++)
                    cool -= 16.6666666666666;
                return (int)cool;
            }
            else if (type.Level > 89)
            {
                double cool = 500;
                for (int i = 0; i < type.Level - 90; i++)
                    cool -= 40;
                return (int)cool;
            }
            else
            {
                double cool = 2000;
                for (int i = 0; i < type.Level - 30; i++)
                    cool -= 25;
                return (int)cool;
            }
        }

        private double getDist(Pet host, PetLevel type)
        {
            return 2;
        }
    }
}
