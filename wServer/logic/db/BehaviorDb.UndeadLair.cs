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
        private _ UndeadLair = () => Behav()
            .Init("Septavius the Ghost God",
                new State(
                    new State("Idle",
                        new PlayerWithinTransition(15, "Cycle")
                    ),
                    new State("Cycle",
                        new Shoot(15, projectileIndex: 3, coolDown: 1000),
                        new State("Cycle Begin",
                            new State("Cycle 1",
                                new Shoot(15, count: 3, fixedAngle: 0),
                                new TimedTransition(150, "Cycle 2")
                            ),
                            new State("Cycle 2",
                                new Shoot(15, count: 3, fixedAngle: 18),
                                new TimedTransition(150, "Cycle 3")
                            ),
                            new State("Cycle 3",
                                new Shoot(15, count: 3, fixedAngle: 36),
                                new TimedTransition(150, "Cycle 4")
                            ),
                            new State("Cycle 4",
                                new Shoot(15, count: 3, fixedAngle: 54),
                                new TimedTransition(150, "Cycle 5")
                            ),
                            new State("Cycle 5",
                                new Shoot(15, count: 3, fixedAngle: 72),
                                new TimedTransition(150, "Cycle 6")
                            ),
                            new State("Cycle 6",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                                new Shoot(15, count: 3, fixedAngle: 90),
                                new TimedTransition(150, "Cycle 7")
                            ),
                            new State("Cycle 7",
                                new Shoot(15, count: 3, fixedAngle: 108),
                                new TimedTransition(150, "Cycle 8")
                            ),
                            new State("Cycle 8",
                                new Shoot(15, count: 3, fixedAngle: 126),
                                new TimedTransition(150, "Cycle 9")
                            ),
                            new State("Cycle 9",
                                new Shoot(15, count: 3, fixedAngle: 144),
                                new TimedTransition(150, "Cycle 10")
                            ),
                            new State("Cycle 10",
                                new Shoot(15, count: 3, fixedAngle: 162),
                                new TimedTransition(150, "Cycle 11")
                            ),
                            new State("Cycle 11",
                                new Shoot(15, count: 3, fixedAngle: 180),
                                new TimedTransition(150, "Cycle 12")
                            ),
                            new State("Cycle 12",
                                new Shoot(15, count: 3, fixedAngle: 198),
                                new TimedTransition(150, "Cycle 13")
                            ),
                            new State("Cycle 13",
                                new Shoot(15, count: 3, fixedAngle: 216),
                                new TimedTransition(150, "Cycle 14")
                            ),
                            new State("Cycle 14",
                                new Shoot(15, count: 3, fixedAngle: 234),
                                new TimedTransition(150, "Cycle 15")
                            ),
                            new State("Cycle 15",
                                new Shoot(15, count: 3, fixedAngle: 252),
                                new TimedTransition(150, "Cycle 16")
                            ),
                            new State("Cycle 16",
                                new Shoot(15, count: 3, fixedAngle: 270),
                                new TimedTransition(150, "Cycle 17")
                            ),
                            new State("Cycle 17",
                                new Shoot(15, count: 3, fixedAngle: 288),
                                new TimedTransition(150, "Cycle 18")
                            ),
                            new State("Cycle 18",
                                new Shoot(15, count: 3, fixedAngle: 306),
                                new TimedTransition(150, "Cycle 19")
                            ),
                            new State("Cycle 19",
                                new Shoot(15, count: 3, fixedAngle: 324),
                                new TimedTransition(150, "Cycle 20")
                            ),
                            new State("Cycle 20",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new Shoot(15, count: 3, fixedAngle: 342),
                                new TimedTransition(250, "Cycle 21")
                            ),
                            new State("Cycle 21",
                                new Shoot(15, count: 3, fixedAngle: 360),
                                new TimedTransition(100, "Cycle_2")
                            )
                        ),
                        new State("Cycle_2",
                            new State("Cycle_2 1",
                                new Shoot(15, count: 3, fixedAngle: 0),
                                new TimedTransition(150, "Cycle_2 2")
                            ),
                            new State("Cycle_2 2",
                                new Shoot(15, count: 3, fixedAngle: 18),
                                new TimedTransition(150, "Cycle_2 3")
                            ),
                            new State("Cycle_2 3",
                                new Shoot(15, count: 3, fixedAngle: 36),
                                new TimedTransition(150, "Cycle_2 4")
                            ),
                            new State("Cycle_2 4",
                                new Shoot(15, count: 3, fixedAngle: 54),
                                new TimedTransition(150, "Cycle_2 5")
                            ),
                            new State("Cycle_2 5",
                                new Shoot(15, count: 3, fixedAngle: 72),
                                new TimedTransition(150, "Cycle_2 6")
                            ),
                            new State("Cycle_2 6",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                                new Shoot(15, count: 3, fixedAngle: 90),
                                new TimedTransition(150, "Cycle_2 7")
                            ),
                            new State("Cycle_2 7",
                                new Shoot(15, count: 3, fixedAngle: 108),
                                new TimedTransition(150, "Cycle_2 8")
                            ),
                            new State("Cycle_2 8",
                                new Shoot(15, count: 3, fixedAngle: 126),
                                new TimedTransition(150, "Cycle_2 9")
                            ),
                            new State("Cycle_2 9",
                                new Shoot(15, count: 3, fixedAngle: 144),
                                new TimedTransition(150, "Cycle_2 10")
                            ),
                            new State("Cycle_2 10",
                                new Shoot(15, count: 3, fixedAngle: 162),
                                new TimedTransition(150, "Cycle_2 11")
                            ),
                            new State("Cycle_2 11",
                                new Shoot(15, count: 3, fixedAngle: 180),
                                new TimedTransition(150, "Cycle_2 12")
                            ),
                            new State("Cycle_2 12",
                                new Shoot(15, count: 3, fixedAngle: 198),
                                new TimedTransition(150, "Cycle_2 13")
                            ),
                            new State("Cycle_2 13",
                                new Shoot(15, count: 3, fixedAngle: 216),
                                new TimedTransition(150, "Cycle_2 14")
                            ),
                            new State("Cycle_2 14",
                                new Shoot(15, count: 3, fixedAngle: 234),
                                new TimedTransition(150, "Cycle_2 15")
                            ),
                            new State("Cycle_2 15",
                                new Shoot(15, count: 3, fixedAngle: 252),
                                new TimedTransition(150, "Cycle_2 16")
                            ),
                            new State("Cycle_2 16",
                                new Shoot(15, count: 3, fixedAngle: 270),
                                new TimedTransition(150, "Cycle_2 17")
                            ),
                            new State("Cycle_2 17",
                                new Shoot(15, count: 3, fixedAngle: 288),
                                new TimedTransition(150, "Cycle_2 18")
                            ),
                            new State("Cycle_2 18",
                                new Shoot(15, count: 3, fixedAngle: 306),
                                new TimedTransition(150, "Cycle_2 19")
                            ),
                            new State("Cycle_2 19",
                                new Shoot(15, count: 3, fixedAngle: 324),
                                new TimedTransition(150, "Cycle_2 20")
                            ),
                            new State("Cycle_2 20",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new Shoot(15, count: 3, fixedAngle: 342),
                                new TimedTransition(250, "Cycle_2 21")
                            ),
                            new State("Cycle_2 21",
                                new Shoot(15, count: 3, fixedAngle: 360),
                                new TimedTransition(100, "Ring Attack + Flashing")
                            )
                        )
                    ),
                    new State("Ring Attack + Flashing",
                        new HpLessTransition(0.1, "Spawn Minions"),
                        new State("Flash 1",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0x0000FF0C, 0.5, 4),
                            new TimedTransition(2000, "Ring Attack")
                        ),
                        new State("Ring Attack",
                            new Shoot(12, count: 10, fixedAngle: 12, projectileIndex: 3, coolDown: 2500),
                            new State("Ring Attack Idle",
                                new TimedTransition(2500, "SetEffect")
                            ),
                            new State("SetEffect",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                                new TimedTransition(7500, "Flash 2")
                            )
                        ),
                        new State("Flash 2",
                            new Flash(0x0000FF0C, 0.2, 8),
                            new TimedTransition(1600, "Confuse + Quiet")
                        )
                    ),
                    new State("Confuse + Quiet",
                        new HpLessTransition(0.1, "Spawn Minions"),
                        new State("Shoot",
                            new Shoot(15, count: 3, shootAngle: 15, projectileIndex: 2, coolDown: new Cooldown(750, 250)),
                            new Shoot(25, count: 12, fixedAngle: 0, projectileIndex: 1, coolDown: 500),
                            new State("Confuse + Quiet Shoot Idle",
                                new TimedTransition(5000, "Start UnsetConditionEffect")
                            ),
                            new State("Start UnsetConditionEffect",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new TimedTransition(0, "Unset")
                            ),
                            new State("Unset",
                                new TimedTransition(5000, "Stop Shooting")
                            )
                        ),
                        new State("Stop Shooting",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Flash(0x0000FF0C, 0.5, 4),
                            new TimedTransition(3000, "Spawn Minions"))
                    ),
                    new State("Spawn Minions",
                        new State("Spawn the Fegits",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Spawn("Ghost Warrior of Septavius", 6, 0.8),
                            new Spawn("Ghost Mage of Septavius", 6, 0.8),
                            new Spawn("Ghost Rogue of Septavius", 6, 0.8),
                            new TimedTransition(0, "Lets Shoot Fegits")
                        ),
                        new State("Lets Shoot Fegits",
                            new Shoot(15, count: 3, shootAngle: 15, projectileIndex: 4, coolDown: new Cooldown(750, 250)),
                            new Spawn("Ghost Warrior of Septavius", 3, 0.7, coolDown: 1000),
                            new Spawn("Ghost Mage of Septavius", 3, 0.7, coolDown: 1000),
                            new Spawn("Ghost Rogue of Septavius", 3, 0.7, coolDown: 1000),
                            new State("Cycle_Spawn",
                                new State("Cycle_Spawn_2 Begin",
                                    new State("Cycle_Spawn 1",
                                        new Shoot(15, count: 3, fixedAngle: 0),
                                        new TimedTransition(150, "Cycle_Spawn 2")
                                    ),
                                    new State("Cycle_Spawn 2",
                                        new Shoot(15, count: 3, fixedAngle: 18),
                                        new TimedTransition(150, "Cycle_Spawn 3")
                                    ),
                                    new State("Cycle_Spawn 3",
                                        new Shoot(15, count: 3, fixedAngle: 36),
                                        new TimedTransition(150, "Cycle_Spawn 4")
                                    ),
                                    new State("Cycle_Spawn 4",
                                        new Shoot(15, count: 3, fixedAngle: 54),
                                        new TimedTransition(150, "Cycle_Spawn 5")
                                    ),
                                    new State("Cycle_Spawn 5",
                                        new Shoot(15, count: 3, fixedAngle: 72),
                                        new TimedTransition(150, "Cycle_Spawn 6")
                                    ),
                                    new State("Cycle_Spawn 6",
                                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                                        new Shoot(15, count: 3, fixedAngle: 90),
                                        new TimedTransition(150, "Cycle_Spawn 7")
                                    ),
                                    new State("Cycle_Spawn 7",
                                        new Shoot(15, count: 3, fixedAngle: 108),
                                        new TimedTransition(150, "Cycle_Spawn 8")
                                    ),
                                    new State("Cycle_Spawn 8",
                                        new Shoot(15, count: 3, fixedAngle: 126),
                                        new TimedTransition(150, "Cycle_Spawn 9")
                                    ),
                                    new State("Cycle_Spawn 9",
                                        new Shoot(15, count: 3, fixedAngle: 144),
                                        new TimedTransition(150, "Cycle_Spawn 10")
                                    ),
                                    new State("Cycle_Spawn 10",
                                        new Shoot(15, count: 3, fixedAngle: 162),
                                        new TimedTransition(150, "Cycle_Spawn 11")
                                    ),
                                    new State("Cycle_Spawn 11",
                                        new Shoot(15, count: 3, fixedAngle: 180),
                                        new TimedTransition(150, "Cycle_Spawn 12")
                                    ),
                                    new State("Cycle_Spawn 12",
                                        new Shoot(15, count: 3, fixedAngle: 198),
                                        new TimedTransition(150, "Cycle_Spawn 13")
                                    ),
                                    new State("Cycle_Spawn 13",
                                        new Shoot(15, count: 3, fixedAngle: 216),
                                        new TimedTransition(150, "Cycle_Spawn 14")
                                    ),
                                    new State("Cycle_Spawn 14",
                                        new Shoot(15, count: 3, fixedAngle: 234),
                                        new TimedTransition(150, "Cycle_Spawn 15")
                                    ),
                                    new State("Cycle_Spawn 15",
                                        new Shoot(15, count: 3, fixedAngle: 252),
                                        new TimedTransition(150, "Cycle_Spawn 16")
                                    ),
                                    new State("Cycle_Spawn 16",
                                        new Shoot(15, count: 3, fixedAngle: 270),
                                        new TimedTransition(150, "Cycle_Spawn 17")
                                    ),
                                    new State("Cycle_Spawn 17",
                                        new Shoot(15, count: 3, fixedAngle: 288),
                                        new TimedTransition(150, "Cycle_Spawn 18")
                                    ),
                                    new State("Cycle_Spawn 18",
                                        new Shoot(15, count: 3, fixedAngle: 306),
                                        new TimedTransition(150, "Cycle_Spawn 19")
                                    ),
                                    new State("Cycle_Spawn 19",
                                        new Shoot(15, count: 3, fixedAngle: 324),
                                        new TimedTransition(150, "Cycle_Spawn 20")
                                    ),
                                    new State("Cycle_Spawn 20",
                                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                        new Shoot(15, count: 3, fixedAngle: 342),
                                        new TimedTransition(250, "Cycle_Spawn 21")
                                    ),
                                    new State("Cycle_Spawn 21",
                                        new Shoot(15, count: 3, fixedAngle: 360),
                                        new TimedTransition(100, "Cycle_Spawn_2")
                                    )
                                ),
                                new State("Cycle_Spawn_2",
                                    new State("Cycle_Spawn_2 1",
                                        new Shoot(15, count: 3, fixedAngle: 0),
                                        new TimedTransition(150, "Cycle_Spawn_2 2")
                                    ),
                                    new State("Cycle_Spawn_2 2",
                                        new Shoot(15, count: 3, fixedAngle: 18),
                                        new TimedTransition(150, "Cycle_Spawn_2 3")
                                    ),
                                    new State("Cycle_Spawn_2 3",
                                        new Shoot(15, count: 3, fixedAngle: 36),
                                        new TimedTransition(150, "Cycle_Spawn_2 4")
                                    ),
                                    new State("Cycle_Spawn_2 4",
                                        new Shoot(15, count: 3, fixedAngle: 54),
                                        new TimedTransition(150, "Cycle_Spawn_2 5")
                                    ),
                                    new State("Cycle_Spawn_2 5",
                                        new Shoot(15, count: 3, fixedAngle: 72),
                                        new TimedTransition(150, "Cycle_Spawn_2 6")
                                    ),
                                    new State("Cycle_Spawn_2 6",
                                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                                        new Shoot(15, count: 3, fixedAngle: 90),
                                        new TimedTransition(150, "Cycle_Spawn_2 7")
                                    ),
                                    new State("Cycle_Spawn_2 7",
                                        new Shoot(15, count: 3, fixedAngle: 108),
                                        new TimedTransition(150, "Cycle_Spawn_2 8")
                                    ),
                                    new State("Cycle_Spawn_2 8",
                                        new Shoot(15, count: 3, fixedAngle: 126),
                                        new TimedTransition(150, "Cycle_Spawn_2 9")
                                    ),
                                    new State("Cycle_Spawn_2 9",
                                        new Shoot(15, count: 3, fixedAngle: 144),
                                        new TimedTransition(150, "Cycle_Spawn_2 10")
                                    ),
                                    new State("Cycle_Spawn_2 10",
                                        new Shoot(15, count: 3, fixedAngle: 162),
                                        new TimedTransition(150, "Cycle_Spawn_2 11")
                                    ),
                                    new State("Cycle_Spawn_2 11",
                                        new Shoot(15, count: 3, fixedAngle: 180),
                                        new TimedTransition(150, "Cycle_Spawn_2 12")
                                    ),
                                    new State("Cycle_Spawn_2 12",
                                        new Shoot(15, count: 3, fixedAngle: 198),
                                        new TimedTransition(150, "Cycle_Spawn_2 13")
                                    ),
                                    new State("Cycle_Spawn_2 13",
                                        new Shoot(15, count: 3, fixedAngle: 216),
                                        new TimedTransition(150, "Cycle_Spawn_2 14")
                                    ),
                                    new State("Cycle_Spawn_2 14",
                                        new Shoot(15, count: 3, fixedAngle: 234),
                                        new TimedTransition(150, "Cycle_Spawn_2 15")
                                    ),
                                    new State("Cycle_Spawn_2 15",
                                        new Shoot(15, count: 3, fixedAngle: 252),
                                        new TimedTransition(150, "Cycle_Spawn_2 16")
                                    ),
                                    new State("Cycle_Spawn_2 16",
                                        new Shoot(15, count: 3, fixedAngle: 270),
                                        new TimedTransition(150, "Cycle_Spawn_2 17")
                                    ),
                                    new State("Cycle_Spawn_2 17",
                                        new Shoot(15, count: 3, fixedAngle: 288),
                                        new TimedTransition(150, "Cycle_Spawn_2 18")
                                    ),
                                    new State("Cycle_Spawn_2 18",
                                        new Shoot(15, count: 3, fixedAngle: 306),
                                        new TimedTransition(150, "Cycle_Spawn_2 19")
                                    ),
                                    new State("Cycle_Spawn_2 19",
                                        new Shoot(15, count: 3, fixedAngle: 324),
                                        new TimedTransition(150, "Cycle_Spawn_2 20")
                                    ),
                                    new State("Cycle_Spawn_2 20",
                                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                        new Shoot(15, count: 3, fixedAngle: 342),
                                        new TimedTransition(250, "Cycle_Spawn_2 21")
                                    ),
                                    new State("Cycle_Spawn_2 21",
                                        new Shoot(15, count: 3, fixedAngle: 360),
                                        new TimedTransition(1000, "Ring Attack + Flashing")
                                    )
                                )
                            )
                        )
                    )
                ),
                new Threshold(0.32, /* Maximum 3 wis, minimum 0 wis */
                    new ItemLoot("Potion of Wisdom", 1)
                ),
                new Threshold(0.1,
                    new ItemLoot("Bow of the Morning Star", 0.01),
                    new ItemLoot("Doom Bow", 0.005),
                    new ItemLoot("Wine Cellar Incantation", 0.005),
                    new TierLoot(3, ItemType.Ring, 0.2),
                    new TierLoot(4, ItemType.Ring, 0.1),
                    new TierLoot(7, ItemType.Weapon, 0.2),
                    new TierLoot(8, ItemType.Weapon, 0.1),
                    new TierLoot(3, ItemType.Ability, 0.2),
                    new TierLoot(4, ItemType.Ability, 0.15),
                    new TierLoot(5, ItemType.Ability, 0.1)
                ),
                new Threshold(0.2,
                    new EggLoot(EggRarity.Common, 0.1),
                    new EggLoot(EggRarity.Uncommon, 0.05),
                    new EggLoot(EggRarity.Rare, 0.01),
                    new EggLoot(EggRarity.Legendary, 0.002)
                )
            )
            .Init("Ghost Warrior of Septavius",
                new State(
                    new Shoot(10, coolDown: new Cooldown(2000, 1000)),
                    new State("Follow",
                        new Prioritize(
                            new Follow(.4, 7, 1),
                            new Protect(1, "Septavius the Ghost God", protectionRange: 1, reprotectRange: 2)
                        )
                    ),
                    new State("Wander",
                        new Wander(0.4)
                    )
                ),
                new ItemLoot("Health Potion", 0.2),
                new ItemLoot("Magic Potion", 0.2)
            )
            .Init("Ghost Mage of Septavius",
                new State(
                    new Shoot(10, coolDown: new Cooldown(2000, 1000)),
                    new State("Follow",
                        new Prioritize(
                            new Follow(.4, 7, 1),
                            new Protect(1, "Septavius the Ghost God", protectionRange: 1, reprotectRange: 2)
                        )
                    ),
                    new State("Wander",
                        new Wander(0.4)
                    )
                ),
                new ItemLoot("Health Potion", 0.2),
                new ItemLoot("Magic Potion", 0.2)
            )
            .Init("Ghost Rogue of Septavius",
                new State(
                    new Shoot(10, coolDown: new Cooldown(2000, 1000)),
                    new State("Follow",
                        new Prioritize(
                            new Follow(.4, 7, 1),
                            new Protect(1, "Septavius the Ghost God", protectionRange: 1, reprotectRange: 2)
                        )
                    ),
                    new State("Wander",
                        new Wander(0.4)
                    )
                ),
                new ItemLoot("Health Potion", 0.2),
                new ItemLoot("Magic Potion", 0.2)
            );
    }
}