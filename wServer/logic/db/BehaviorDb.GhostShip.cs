using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.entities.player;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ GhostShip = () => Behav()
        //made by omni the greatest rapper ever
                    .Init("Vengeful Spirit",
                new State(
                    new State("Start",
                        new ChangeSize(50, 120),
                        new Prioritize(
                            new Follow(0.48, 8, 1),
                            new Wander(0.45)
                            ),
                        new Shoot(8.4, count: 3, projectileIndex: 0, shootAngle: 16, coolDown: 1000),
                        new TimedTransition(1000, "Vengeful")
                        ),
                    new State("Vengeful",
                        new Prioritize(
                            new Follow(1, 8, 1),
                            new Wander(0.45)
                            ),
                        new Shoot(8.4, count: 3, projectileIndex: 0, shootAngle: 16, coolDown: 1645),
                        new TimedTransition(3000, "Vengeful2")
                        ),
                        new State("Vengeful2",
                        new ReturnToSpawn(once: false, speed: 0.6),
                        new Shoot(8.4, count: 3, projectileIndex: 0, shootAngle: 16, coolDown: 1500),
                        new TimedTransition(1500, "Vengeful")
                        )))
                   .Init("Water Mine",
                    new State(
                       new State("Seek",
                        new Prioritize(
                            new Follow(0.45, 8, 1),
                            new Wander(0.55)
                            ),
                        new TimedTransition(3750, "Boom")
                        ),
                        new State("Boom",
                        new Shoot(8.4, count: 10, projectileIndex: 0, coolDown: 1000),
                        new Suicide()
                 )))
                 .Init("Beach Spectre",
                    new State(
                       new State("Fight",
                           new Wander(0.03),
                       new ChangeSize(10, 120),
                       new Shoot(8.4, count: 3, projectileIndex: 0, shootAngle: 14, coolDown: 1750)
                 )))

                 .Init("Beach Spectre Spawner",
                    new State(
                       new ConditionalEffect(ConditionEffectIndex.Invincible),
                       new State("Spawn",
                       new Reproduce("Beach Spectre", densityMax: 1, densityRadius: 3, spawnRadius: 1, coolDown: 1250)
                 )))
                  .Init("Tempest Cloud",
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Start1",
                       new ChangeSize(70, 130),
                       new TimedTransition(3000, "Start2")
                 ),
                    new State("Start2",
                       new SetAltTexture(1),
                       new TimedTransition(1, "Start3")
                 ),
                    new State("Start3",
                       new SetAltTexture(2),
                       new TimedTransition(1, "Start4")
                 ),
                     new State("Start4",
                       new SetAltTexture(3),
                       new TimedTransition(1, "Start5")
                 ),
                     new State("Start5",
                       new SetAltTexture(4),
                       new TimedTransition(1, "Start6")
                 ),
                     new State("Start6",
                       new SetAltTexture(5),
                       new TimedTransition(1, "Start7")
                 ),
                     new State("Start7",
                       new SetAltTexture(6),
                       new TimedTransition(1, "Start8")
                 ),
                     new State("Start8",
                       new SetAltTexture(7),
                       new TimedTransition(1, "Start9")
                 ),
                     new State("Start9",
                       new SetAltTexture(8),
                       new TimedTransition(1, "Final")
                 ),
                     new State("Final",
                       new SetAltTexture(9),
                       new TimedTransition(1, "CircleAndStorm")
                 ),
                     new State("CircleAndStorm",
                       new Orbit(0.25, 9, 20, "Ghost Ship Anchor", speedVariance: 0.1),
                       new Shoot(8.4, count: 7, projectileIndex: 0, coolDown: 1000)
                 )))
                .Init("Ghost Ship Anchor",
                    new State(
                       new State("idle",                        
                       new ConditionalEffect(ConditionEffectIndex.Invincible)
                 ),
                    new State("tempestcloud",
                        new InvisiToss("Tempest Cloud", 9, 0, coolDown: 9999999),
                        new InvisiToss("Tempest Cloud", 9, 45, coolDown: 9999999),
                        new InvisiToss("Tempest Cloud", 9, 90, coolDown: 9999999),
                        new InvisiToss("Tempest Cloud", 9, 135, coolDown: 9999999),
                        new InvisiToss("Tempest Cloud", 9, 180, coolDown: 9999999),
                        new InvisiToss("Tempest Cloud", 9, 225, coolDown: 9999999),
                        new InvisiToss("Tempest Cloud", 9, 270, coolDown: 9999999),
                        new InvisiToss("Tempest Cloud", 9, 315, coolDown: 9999999),
                        new InvisiToss("Tempest Cloud", 9, 350, coolDown: 9999999),
                        new InvisiToss("Tempest Cloud", 9, 250, coolDown: 9999999),
                        new InvisiToss("Tempest Cloud", 9, 110, coolDown: 9999999),
                        new InvisiToss("Tempest Cloud", 9, 200, coolDown: 9999999),
                        new InvisiToss("Tempest Cloud", 9, 10, coolDown: 9999999),
                        new InvisiToss("Tempest Cloud", 9, 290, coolDown: 9999999),

                        //Spectre Spawner
                        new InvisiToss("Beach Spectre Spawner", 17, 0, coolDown: 9999999),
                        new InvisiToss("Beach Spectre Spawner", 17, 45, coolDown: 9999999),
                        new InvisiToss("Beach Spectre Spawner", 17, 90, coolDown: 9999999),
                        new InvisiToss("Beach Spectre Spawner", 17, 135, coolDown: 9999999),
                        new InvisiToss("Beach Spectre Spawner", 17, 180, coolDown: 9999999),
                        new InvisiToss("Beach Spectre Spawner", 17, 225, coolDown: 9999999),
                        new InvisiToss("Beach Spectre Spawner", 17, 270, coolDown: 9999999),
                        new InvisiToss("Beach Spectre Spawner", 17, 315, coolDown: 9999999),
                        new InvisiToss("Beach Spectre Spawner", 17, 250, coolDown: 9999999),
                        new InvisiToss("Beach Spectre Spawner", 17, 110, coolDown: 9999999),
                        new InvisiToss("Beach Spectre Spawner", 17, 200, coolDown: 9999999),
                       new ConditionalEffect(ConditionEffectIndex.Invincible)
                 )

                        ))
                    .Init("Ghost Ship",
                new State(
                    new DropPortalOnDeath("Davy Jones' Locker Portal", 21),
                    new OnDeathBehavior(
                        new RemoveEntity(100, "Tempest Cloud")
                        ),
                    new OnDeathBehavior(
                        new RemoveEntity(100, "Beach Spectre")
                        ),
                     new OnDeathBehavior(
                        new RemoveEntity(100, "Beach Spectre Spawner")
                        ),
                    new State("idle",
                        new SetAltTexture(1),
                        new Wander(0.1),
                        new DamageTakenTransition(2000, "pause")
                        ),
                    new State("pause",
                         new SetAltTexture(2),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(2000, "start")
                        ),
                      new State("start",
                           new SetAltTexture(0),
                      new Reproduce("Vengeful Spirit", densityMax: 2, spawnRadius: 1, coolDown: 1000),
                      new TimedTransition(15000, "midfight"),
                       new State("2",
                        new SetAltTexture(0),
                        new Prioritize(
                             new Wander(0.45),
                             new StayBack(0.3, 5)
                            ),
                        new Shoot(8.4, count: 1, projectileIndex: 0,  coolDown: 450),
                        new Shoot(8.4, count: 3, projectileIndex: 0, shootAngle: 20, coolDown: 1750),
                        new TimedTransition(3250, "1")
                        ),
                     new State("1",
                        new TossObject("Water Mine", 5, coolDown: 1500),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(once: false, speed: 0.4),
                        new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 450),
                        new Shoot(8.4, count: 3, projectileIndex: 0, shootAngle: 20, coolDown: 1750),
                        new TimedTransition(1500, "2")
                         )
                        ),


                       new State("midfight",
                     new Order(100, "Ghost Ship Anchor", "tempestcloud"),
                      new Reproduce("Vengeful Spirit", densityMax: 1, spawnRadius: 1, coolDown: 1000),
                      new TossObject("Water Mine", 5, coolDown: 2250),                      
                      new TimedTransition(10000, "countdown"),
                       new State("2",
                        new SetAltTexture(0),
                        new ReturnToSpawn(once: false, speed: 0.4),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(10, count: 4, projectileIndex: 0, coolDownOffset: 1100, angleOffset: 270, coolDown: 1250),
                        new Shoot(10, count: 4, projectileIndex: 0, coolDownOffset: 1100, angleOffset: 90, coolDown: 1250),
                        new Shoot(8.4, count: 1, projectileIndex: 1, coolDown: 1250),
                        new TimedTransition(3000, "1")
                        ),
                     new State("1",
                        new Prioritize(
                             new Follow(0.45, 8, 1),
                             new Wander(0.3)
                            ),
                        new Taunt(1.00, "Fire at will!"),
                        new Shoot(8.4, count: 2, shootAngle: 25, projectileIndex: 1, coolDown: 3850),
                        new Shoot(8.4, count: 6, projectileIndex: 0, shootAngle: 10, coolDown: 2750),
                        new TimedTransition(4000, "2")
                         )
                        ),
                    new State("countdown",
                        new Wander(0.1),
                        new Timed(1000,
                            new Taunt(1.00, "Ready..")
                            ),
                         new Timed(2000,
                            new Taunt(1.00, "Aim..")
                            ),
                        new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 450),
                        new Shoot(8.4, count: 3, projectileIndex: 0, shootAngle: 20, coolDown: 750),
                        new TimedTransition(2000, "fire")
                        ),
                    new State("fire",
                       new Prioritize(
                             new Follow(0.36, 8, 1),
                             new Wander(0.12)
                            ),
                         new Shoot(10, count: 4, projectileIndex: 1, coolDownOffset: 1100, angleOffset: 270, coolDown: 1250),
                        new Shoot(10, count: 4, projectileIndex: 1, coolDownOffset: 1100, angleOffset: 90, coolDown: 1250),
                        new Shoot(8.4, count: 10, projectileIndex: 0, coolDown: 3400),
                        new TimedTransition(3400, "midfight")
                        )

               ),
                new MostDamagers(3,
                    new ItemLoot("Potion of Wisdom", 1.0)
                ),
                new ItemLoot("Ghost Pirate Rum", 1.0),
                new Threshold(0.025,
                    new TierLoot(9, ItemType.Weapon, 0.1),
                    new TierLoot(4, ItemType.Ability, 0.1),
                    new TierLoot(5, ItemType.Ability, 0.05),
                    new TierLoot(9, ItemType.Armor, 0.1),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(10, ItemType.Armor, 0.05),
                    new TierLoot(11, ItemType.Armor, 0.04),
                    new TierLoot(10, ItemType.Weapon, 0.05),
                    new TierLoot(11, ItemType.Weapon, 0.04),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new TierLoot(5, ItemType.Ring, 0.02),
                    new EggLoot(EggRarity.Common, 0.05),
                    new EggLoot(EggRarity.Uncommon, 0.025),
                    new EggLoot(EggRarity.Rare, 0.02),
                    new EggLoot(EggRarity.Legendary, 0.005)
                )
            )



        ;
    }
}