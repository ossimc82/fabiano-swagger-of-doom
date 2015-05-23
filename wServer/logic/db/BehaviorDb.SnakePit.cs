using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ SnakePit = () => Behav()
            .Init("Stheno the Snake Queen",
                new State(
                    new RealmPortalDrop(),
                    new State("Idle",
                        new PlayerWithinTransition(20, "Silver Blasts")
                    ),
                    new State("Silver Blasts",
                        new State("Silver Blasts 1",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(10, count: 2, shootAngle: 10, angleOffset: 45, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 135, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 225, projectileIndex: 0),
                            new Shoot(10, count: 2, shootAngle: 10, angleOffset: 315, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blasts 2")
                        ),
                        new State("Silver Blasts 2",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(10, count: 2, shootAngle: 10, angleOffset: 45, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 135, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 225, projectileIndex: 0),
                            new Shoot(10, count: 2, shootAngle: 10, angleOffset: 315, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blasts 3")
                        ),
                        new State("Silver Blasts 3",
                            new Shoot(10, count: 2, shootAngle: 10, angleOffset: 45, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 135, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 225, projectileIndex: 0),
                            new Shoot(10, count: 2, shootAngle: 10, angleOffset: 315, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blasts 4")
                        ),
                        new State("Silver Blasts 4",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(10, count: 2, shootAngle: 10, angleOffset: 45, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 135, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 225, projectileIndex: 0),
                            new Shoot(10, count: 2, shootAngle: 10, angleOffset: 315, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blasts 5")
                        ),
                        new State("Silver Blasts 5",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(10, count: 2, shootAngle: 10, angleOffset: 45, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 135, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 225, projectileIndex: 0),
                            new Shoot(10, count: 2, shootAngle: 10, angleOffset: 315, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blasts 6")
                        ),
                        new State("Silver Blasts 6",
                            new Shoot(10, count: 2, shootAngle: 10, angleOffset: 45, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 135, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 225, projectileIndex: 0),
                            new Shoot(10, count: 2, shootAngle: 10, angleOffset: 315, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blasts 7")
                        ),
                        new State("Silver Blasts 7",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(10, count: 2, shootAngle: 10, angleOffset: 45, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 135, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 225, projectileIndex: 0),
                            new Shoot(10, count: 2, shootAngle: 10, angleOffset: 315, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blasts 8")
                        ),
                        new State("Silver Blasts 8",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(10, count: 2, shootAngle: 10, angleOffset: 45, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 135, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 225, projectileIndex: 0),
                            new Shoot(10, count: 2, shootAngle: 10, angleOffset: 315, projectileIndex: 0),
                            new TimedTransition(1000, "Spawn Stheno Swarm")
                        )
                    ),
                    new State("Spawn Stheno Swarm",
                        new Prioritize(
                            new StayCloseToSpawn(0.4, 2),
                            new Wander(0.4)
                        ),
                        new Reproduce("Stheno Swarm", 2.5, 8, coolDown: 750),
                        new State("Silver Blast 1",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(10, count: 1, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blast 2")
                        ),
                        new State("Silver Blast 2",
                            new Shoot(10, count: 1, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blast 3")
                        ),
                        new State("Silver Blast 3",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(10, count: 1, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blast 4")
                        ),
                        new State("Silver Blast 4",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(10, count: 1, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blast 5")
                        ),
                        new State("Silver Blast 5",
                            new Shoot(10, count: 1, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blast 6")
                        ),
                        new State("Silver Blast 6",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(10, count: 1, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blast 7")
                        ),
                        new State("Silver Blast 7",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(10, count: 1, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blast 8")
                        ),
                        new State("Silver Blast 8",
                            new Shoot(10, count: 1, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blast 9")
                        ),
                        new State("Silver Blast 9",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(10, count: 1, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blast 10")
                        ),
                        new State("Silver Blast 10",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(10, count: 1, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blast 11")
                        ),
                        new State("Silver Blast 11",
                            new Shoot(10, count: 1, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blast 12")
                        ),
                        new State("Silver Blast 12",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(10, count: 1, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blast 13")
                        ),
                        new State("Silver Blast 13",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(10, count: 1, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blast 14")
                        ),
                        new State("Silver Blast 14",
                            new Shoot(10, count: 1, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blast 15")
                        ),
                        new State("Silver Blast 15",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(10, count: 1, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(1000, "Silver Blast 16")
                        ),
                        new State("Silver Blast 16",
                            new Shoot(10, count: 1, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(1000, "Leave me")
                        ),
                        new State("Leave me",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Order(100, "Stheno Swarm", "Despawn"),
                            new TimedTransition(1000, "Blind Ring Attack + ThrowAttack")
                        )
                    ),
                    new State("Blind Ring Attack + ThrowAttack",
                        new ReturnToSpawn(speed: 0.3),
                        new State("Blind Ring Attack + ThrowAttack 1",
                            new Shoot(10, count: 6, projectileIndex: 1),
                            new Grenade(2.5, 100, 10),
                            new TimedTransition(500, "Blind Ring Attack + ThrowAttack 2")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 2",
                            new Shoot(10, count: 6, projectileIndex: 1),
                            new Grenade(2.5, 100, 10),
                            new TimedTransition(500, "Blind Ring Attack + ThrowAttack 3")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 3",
                            new Shoot(10, count: 6, projectileIndex: 1),
                            new Grenade(2.5, 100, 10),
                            new TimedTransition(500, "Blind Ring Attack + ThrowAttack 4")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 4",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(10, count: 6, projectileIndex: 1),
                            new Grenade(2.5, 100, 10),
                            new TimedTransition(500, "Blind Ring Attack + ThrowAttack 5")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 5",
                            new Shoot(10, count: 6, projectileIndex: 1),
                            new Grenade(2.5, 100, 10),
                            new TimedTransition(500, "Blind Ring Attack + ThrowAttack 6")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 6",
                            new Shoot(10, count: 6, projectileIndex: 1),
                            new Grenade(2.5, 100, 10),
                            new TimedTransition(500, "Blind Ring Attack + ThrowAttack 7")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 7",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(10, count: 6, projectileIndex: 1),
                            new Grenade(2.5, 100, 10),
                            new TimedTransition(500, "Blind Ring Attack + ThrowAttack 8")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 8",
                            new Shoot(10, count: 6, projectileIndex: 1),
                            new Grenade(2.5, 100, 10),
                            new TimedTransition(500, "Blind Ring Attack + ThrowAttack 9")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 9",
                            new Shoot(10, count: 6, projectileIndex: 1),
                            new Grenade(2.5, 100, 10),
                            new TimedTransition(500, "Blind Ring Attack + ThrowAttack 10")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 10",
                            new Shoot(10, count: 6, projectileIndex: 1),
                            new Grenade(2.5, 100, 10),
                            new TimedTransition(500, "Blind Ring Attack + ThrowAttack 11")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 11",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(10, count: 6, projectileIndex: 1),
                            new Grenade(2.5, 100, 10),
                            new TimedTransition(500, "Blind Ring Attack + ThrowAttack 12")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 12",
                            new Shoot(10, count: 6, projectileIndex: 1),
                            new Grenade(2.5, 100, 10),
                            new TimedTransition(500, "Silver Blasts")
                        )
                    )
                ),
                new MostDamagers(3,
                    new ItemLoot("Potion of Speed", 1)
                ),
                new Threshold(0.1,
                    new ItemLoot("Wand of the Bulwark", 0.005),
                    new ItemLoot("Snake Skin Armor", 0.1),
                    new ItemLoot("Snake Skin Shield", 0.1),
                    new ItemLoot("Snake Eye Ring", 0.1),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new TierLoot(9, ItemType.Weapon, 0.2),
                    new TierLoot(10, ItemType.Weapon, 0.1),
                    new TierLoot(8, ItemType.Armor, 0.3),
                    new TierLoot(9, ItemType.Armor, 0.2),
                    new TierLoot(10, ItemType.Armor, 0.1)
                ),
                new Threshold(0.2,
                    new OnlyOne(
                        LootTemplates.DefaultEggLoot(EggRarity.Legendary)
                    )
                )
            )
            .Init("Stheno Swarm",
                new State(
                    new State("Protect",
                        new Prioritize(
                            new Protect(0.3, "Stheno the Snake Queen"),
                            new Wander(0.3)
                        ),
                        new Shoot(10, coolDown: new Cooldown(750, 250))
                    ),
                    new State("Despawn",
                        new Suicide()
                    )
                )
            )
            .Init("Stheno Pet",
                new State(
                    new State("Protect",
                        new Shoot(25, coolDown: 1000),
                        new State("Protect",
                            new EntityNotExistsTransition("Stheno the Snake Queen", 100, "Wander"),
                            new Orbit(7.5, 10, acquireRange: 50, target: "Stheno the Snake Queen")
                        ),
                        new State("Wander",
                            new Prioritize(
                                new Wander(1)
                            )
                        )
                    )
                )
            )
            .Init("Pit Snake",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(1),
                        new Wander(1)
                    ),
                    new Shoot(20, coolDown: 1000)
                )
            )
            .Init("Pit Viper",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(1),
                        new Wander(1)
                    ),
                    new Shoot(20, coolDown: 1000)
                )
            )
            .Init("Yellow Python",
                new State(
                    new Prioritize(
                        new Follow(1, 10, 1),
                        new StayCloseToSpawn(1),
                        new Wander(1)
                    ),
                    new Shoot(20, coolDown: 1000)
                ),
                new ItemLoot("Snake Oil", 0.1),
                new ItemLoot("Ring of Speed", 0.1),
                new ItemLoot("Ring of Vitality", 0.1)
            )
            .Init("Brown Python",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(1),
                        new Wander(1)
                    ),
                    new Shoot(20, coolDown: 1000)
                ),
                new ItemLoot("Snake Oil", 0.1),
                new ItemLoot("Leather Armor", 0.1),
                new ItemLoot("Ring of Wisdom", 0.1)
            )
            .Init("Fire Python",
                new State(
                    new Prioritize(
                        new Follow(1, 10, 1, coolDown: 2000),
                        new Wander(1)
                    ),
                    new Shoot(15, count: 3, shootAngle: 5, coolDown: 1000)
                ),
                new ItemLoot("Snake Oil", 0.1),
                new ItemLoot("Fire Bow", 0.1),
                new ItemLoot("Fire Nova Spell", 0.1)
            )
            .Init("Greater Pit Snake",
                new State(
                    new Prioritize(
                        new Follow(1, 10, 5),
                        new Wander(1)
                    ),
                    new Shoot(15, count: 3, shootAngle: 5, coolDown: 1000)
                ),
                new ItemLoot("Snake Oil", 0.1),
                new ItemLoot("Glass Sword", 0.1),
                new ItemLoot("Avenger Staff", 0.1),
                new ItemLoot("Wand of Dark Magic", 0.1)
            )
            .Init("Greater Pit Viper",
                new State(
                    new Prioritize(
                        new Follow(1, 10, 5),
                        new Wander(1)
                    ),
                    new Shoot(15, coolDown: 300)
                ),
                new ItemLoot("Snake Oil", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Ring of Greater Attack", 0.1),
                    new ItemLoot("Ring of Greater Health", 0.1)
                )
            )
            .Init("Snakepit Guard",
                new State(
                    new ChangeSize(100, 100),
                    new Shoot(25, count: 3, shootAngle: 25, projectileIndex: 0, coolDown: new Cooldown(1000, 200)),
                    new Shoot(10, count: 6, projectileIndex: 1, coolDown: 1000),
                    new State("Phase 1",
                        new Prioritize(
                            new StayCloseToSpawn(0.2, 4),
                            new Wander(0.2)
                        ),
                        new HpLessTransition(0.6, "Phase 2")
                    ),
                    new State("Phase 2",
                        new Prioritize(
                            new Follow(0.2, acquireRange: 10, range: 3),
                            new Wander(0.2)
                        ),
                        new Shoot(15, count: 3, projectileIndex: 2, coolDown: 2000)
                    )
                ),
                new Threshold(0.32,
                    new ItemLoot("Potion of Speed", 1)
                ),
                new Threshold(0.1,
                    new ItemLoot("Wand of the Bulwark", 0.005),
                    new ItemLoot("Snake Skin Armor", 0.1),
                    new ItemLoot("Snake Skin Shield", 0.1),
                    new ItemLoot("Snake Eye Ring", 0.1),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new TierLoot(9, ItemType.Weapon, 0.2),
                    new TierLoot(10, ItemType.Weapon, 0.1),
                    new TierLoot(8, ItemType.Armor, 0.3),
                    new TierLoot(9, ItemType.Armor, 0.2),
                    new TierLoot(10, ItemType.Armor, 0.1)
                ),
                new Threshold(0.2,
                    new EggLoot(EggRarity.Common, 0.1),
                    new EggLoot(EggRarity.Uncommon, 0.05),
                    new EggLoot(EggRarity.Rare, 0.01),
                    new EggLoot(EggRarity.Legendary, 0.002)
                )
            )
            .Init("Snakepit Dart Thrower",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Idle"),
                    new State("Protect the Guard",
                        new EntityNotExistsTransition("Snakepit Guard", 40, "Idle"),
                        new SnakePitTowerShoot()
                    )
                )
            )
            .Init("Snakepit Button",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Idle",
                        new PlayerWithinTransition(0.5, "Order")
                    ),
                    new State("Order",
                        new Order(15, "Snakepit Guard Spawner", "Spawn the Guard"),
                        new SetAltTexture(1),
                        new TimedTransition(0, "I am out")
                    ),
                    new State("I am out")
                )
            )
            .Init("Snakepit Guard Spawner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Idle"),
                    new State("Spawn the Guard",
                        new Order(15, "Snakepit Dart Thrower", "Protect the Guard"),
                        new Spawn("Snakepit Guard", maxChildren: 1, initialSpawn: 1),
                        new TimedTransition(0, "Idle")
                    )
                )
            )
            .Init("Snake Grate",
                new State(
                    new State("Idle",
                        new EntityNotExistsTransition("Pit Snake", 5, "Spawn Pit Snake"),
                        new EntityNotExistsTransition("Pit Viper", 5, "Spawn Pit Viper")
                    ),
                    new State("Spawn Pit Snake",
                        new Spawn("Pit Snake", 1, 1),
                        new TimedTransition(2000, "Idle")
                    ),
                    new State("Spawn Pit Viper",
                        new Spawn("Pit Viper", 1, 1),
                        new TimedTransition(2000, "Idle")
                    )
                )
            );
    }
}
