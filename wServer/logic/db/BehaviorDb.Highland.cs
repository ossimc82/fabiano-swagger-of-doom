#region

using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Highland = () => Behav()
            .Init("Minotaur",
                new State(
                    new DropPortalOnDeath("Snake Pit Portal", 20, PortalDespawnTimeSec: 100),
                    new State("idle",
                        new StayAbove(0.6, 160),
                        new PlayerWithinTransition(10, "charge")
                        ),
                    new State("charge",
                        new Prioritize(
                            new StayAbove(0.6, 160),
                            new Follow(6, 11, 1.6)
                            ),
                        new TimedTransition(200, "spam_blades")
                        ),
                    new State("spam_blades",
                        new Shoot(8, projectileIndex: 0, count: 1, coolDown: 100000, coolDownOffset: 1000),
                        new Shoot(8, projectileIndex: 0, count: 2, shootAngle: 16, coolDown: 100000,
                            coolDownOffset: 1200),
                        new Shoot(8, projectileIndex: 0, count: 3, predictive: 0.2, coolDown: 100000,
                            coolDownOffset: 1600),
                        new Shoot(8, projectileIndex: 0, count: 1, shootAngle: 24, coolDown: 100000,
                            coolDownOffset: 2200),
                        new Shoot(8, projectileIndex: 0, count: 2, predictive: 0.2, coolDown: 100000,
                            coolDownOffset: 2800),
                        new Shoot(8, projectileIndex: 0, count: 3, shootAngle: 16, coolDown: 100000,
                            coolDownOffset: 3200),
                        new Prioritize(
                            new StayAbove(0.6, 160),
                            new Wander(0.6)
                            ),
                        new TimedTransition(4400, "blade_ring")
                        ),
                    new State("blade_ring",
                        new Shoot(7, fixedAngle: 0, count: 12, shootAngle: 30, coolDown: 800, projectileIndex: 1,
                            coolDownOffset: 600),
                        new Shoot(7, fixedAngle: 15, count: 6, shootAngle: 60, coolDown: 800, projectileIndex: 2,
                            coolDownOffset: 1000),
                        new Prioritize(
                            new StayAbove(0.6, 160),
                            new Follow(0.6, 10, 1),
                            new Wander(0.6)
                            ),
                        new TimedTransition(3500, "pause")
                        ),
                    new State("pause",
                        new Prioritize(
                            new StayAbove(0.6, 160),
                            new Wander(0.6)
                            ),
                        new TimedTransition(1000, "idle")
                        )
                    ),
                new TierLoot(5, ItemType.Weapon, 0.16),
                new TierLoot(6, ItemType.Weapon, 0.08),
                new TierLoot(7, ItemType.Weapon, 0.04),
                new TierLoot(5, ItemType.Armor, 0.16),
                new TierLoot(6, ItemType.Armor, 0.08),
                new TierLoot(7, ItemType.Armor, 0.04),
                new TierLoot(3, ItemType.Ring, 0.05),
                new TierLoot(3, ItemType.Ability, 0.2),
                new ItemLoot("Purple Drake Egg", 0.005)
            )
            .Init("Ogre King",
                new State(
                    new DropPortalOnDeath("Snake Pit Portal", 20, PortalDespawnTimeSec: 100),
                    new Spawn("Ogre Warrior", 4, coolDown: 12000),
                    new Spawn("Ogre Mage", 2, coolDown: 16000),
                    new Spawn("Ogre Wizard", 2, coolDown: 20000),
                    new State("idle",
                        new Prioritize(
                            new StayAbove(0.3, 160),
                            new Wander(0.3)
                            ),
                        new PlayerWithinTransition(10, "grenade_blade_combo")
                        ),
                    new State("grenade_blade_combo",
                        new State("grenade1",
                            new Grenade(3, 60, coolDown: 100000),
                            new Prioritize(
                                new StayAbove(0.3, 160),
                                new Wander(0.3)
                                ),
                            new TimedTransition(2000, "grenade2")
                            ),
                        new State("grenade2",
                            new Grenade(3, 60, coolDown: 100000),
                            new Prioritize(
                                new StayAbove(0.5, 160),
                                new Wander(0.5)
                                ),
                            new TimedTransition(3000, "slow_follow")
                            ),
                        new State("slow_follow",
                            new Shoot(13, coolDown: 1000),
                            new Prioritize(
                                new StayAbove(0.4, 160),
                                new Follow(0.4, 9, 3.5, 4),
                                new Wander(0.4)
                                ),
                            new TimedTransition(4000, "grenade1")
                            ),
                        new HpLessTransition(0.45, "furious")
                        ),
                    new State("furious",
                        new Grenade(2.4, 55, 9, coolDown: 1500),
                        new Prioritize(
                            new StayAbove(0.6, 160),
                            new Wander(0.6)
                            ),
                        new TimedTransition(12000, "idle")
                        )
                    ),
                new TierLoot(4, ItemType.Weapon, 0.2),
                new TierLoot(5, ItemType.Weapon, 0.02),
                new TierLoot(4, ItemType.Armor, 0.2),
                new TierLoot(5, ItemType.Armor, 0.12),
                new TierLoot(6, ItemType.Armor, 0.02),
                new TierLoot(2, ItemType.Ring, 0.1),
                new TierLoot(2, ItemType.Ability, 0.18)
            )
            .Init("Ogre Warrior",
                new State(
                    new Shoot(3, predictive: 0.5),
                    new Prioritize(
                        new StayAbove(1.2, 160),
                        new Protect(1.2, "Ogre King", 15, 10, 5),
                        new Follow(1.4, 10.5, 1.6, 2600, 2200),
                        new Orbit(0.6, 6),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Ogre Mage",
                new State(
                    new Shoot(10, predictive: 0.3),
                    new Prioritize(
                        new StayAbove(1.2, 160),
                        new Protect(1.2, "Ogre King", 30, 10, reprotectRange: 1),
                        new Orbit(0.5, 6),
                        new Wander(0.4)
                        )
                    )
            )
            .Init("Ogre Wizard",
                new State(
                    new Shoot(10, coolDown: 300),
                    new Prioritize(
                        new StayAbove(1.2, 160),
                        new Protect(1.2, "Ogre King", 30, 10, reprotectRange: 1),
                        new Orbit(0.5, 6),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Lizard God",
                new State(
                    new DropPortalOnDeath("Snake Pit Portal", 20, PortalDespawnTimeSec: 100),
                    new Spawn("Night Elf Archer", 4),
                    new Spawn("Night Elf Warrior", 3),
                    new Spawn("Night Elf Mage", 2),
                    new Spawn("Night Elf Veteran", 2),
                    new Spawn("Night Elf King", 1),
                    new Prioritize(
                        new StayAbove(0.3, 160),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new State("idle",
                        new PlayerWithinTransition(10.2, "normal_attack")
                        ),
                    new State("normal_attack",
                        new Shoot(10, 3, 3, predictive: 0.5),
                        new TimedTransition(4000, "if_cloaked")
                        ),
                    new State("if_cloaked",
                        new Shoot(10, 8, 45, fixedAngle: 20, coolDown: 1600, coolDownOffset: 400),
                        new Shoot(10, 8, 45, fixedAngle: 42, coolDown: 1600, coolDownOffset: 1200),
                        new PlayerWithinTransition(10, "normal_attack")
                        )
                    ),
                new TierLoot(5, ItemType.Weapon, 0.16),
                new TierLoot(6, ItemType.Weapon, 0.08),
                new TierLoot(7, ItemType.Weapon, 0.04),
                new TierLoot(5, ItemType.Armor, 0.16),
                new TierLoot(6, ItemType.Armor, 0.08),
                new TierLoot(7, ItemType.Armor, 0.04),
                new TierLoot(3, ItemType.Ring, 0.05),
                new TierLoot(3, ItemType.Ability, 0.15),
                new ItemLoot("Purple Drake Egg", 0.005)
            )
            .Init("Night Elf Archer",
                new State(
                    new Shoot(10, predictive: 1),
                    new Prioritize(
                        new StayAbove(0.4, 160),
                        new Follow(1.5, range: 7),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Night Elf Warrior",
                new State(
                    new Shoot(3, predictive: 1),
                    new Prioritize(
                        new StayAbove(0.4, 160),
                        new Follow(1.5, range: 1),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Night Elf Mage",
                new State(
                    new Shoot(10, predictive: 1),
                    new Prioritize(
                        new StayAbove(0.4, 160),
                        new Follow(1.5, range: 7),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Night Elf Veteran",
                new State(
                    new Shoot(10, predictive: 1),
                    new Prioritize(
                        new StayAbove(0.4, 160),
                        new Follow(1.5, range: 7),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Night Elf King",
                new State(
                    new Shoot(10, predictive: 1),
                    new Prioritize(
                        new StayAbove(0.4, 160),
                        new Follow(1.5, range: 7),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Undead Dwarf God",
                new State(
                    new Spawn("Undead Dwarf Warrior", 3),
                    new Spawn("Undead Dwarf Axebearer", 3),
                    new Spawn("Undead Dwarf Mage", 3),
                    new Spawn("Undead Dwarf King", 2),
                    new Spawn("Soulless Dwarf", 1),
                    new Prioritize(
                        new StayAbove(0.3, 160),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(10, projectileIndex: 0, count: 3, shootAngle: 15),
                    new Shoot(10, projectileIndex: 1, predictive: 0.5, coolDown: 1200)
                    ),
                new TierLoot(5, ItemType.Weapon, 0.16),
                new TierLoot(6, ItemType.Weapon, 0.08),
                new TierLoot(7, ItemType.Weapon, 0.04),
                new TierLoot(5, ItemType.Armor, 0.16),
                new TierLoot(6, ItemType.Armor, 0.08),
                new TierLoot(7, ItemType.Armor, 0.04),
                new TierLoot(3, ItemType.Ring, 0.05),
                new TierLoot(3, ItemType.Ability, 0.2),
                new ItemLoot("Purple Drake Egg", 0.005)
            )
            .Init("Undead Dwarf Warrior",
                new State(
                    new DropPortalOnDeath("Spider Den Portal", 20, PortalDespawnTimeSec: 100),
                    new Shoot(3),
                    new Prioritize(
                        new StayAbove(1, 160),
                        new Follow(1, range: 1),
                        new Wander(0.4)
                        )
                    )
            )
            .Init("Undead Dwarf Axebearer",
                new State(
                    new Shoot(3),
                    new Prioritize(
                        new StayAbove(1, 160),
                        new Follow(1, range: 1),
                        new Wander(0.4)
                        )
                    )
            )
            .Init("Undead Dwarf Mage",
                new State(
                    new State("circle_player",
                        new Shoot(8, predictive: 0.3, coolDown: 1000, coolDownOffset: 500),
                        new Prioritize(
                            new StayAbove(0.7, 160),
                            new Protect(0.7, "Undead Dwarf King", 11, 10, 3),
                            new Orbit(0.7, 3.5, 11),
                            new Wander(0.7)
                            ),
                        new TimedTransition(3500, "circle_king")
                        ),
                    new State("circle_king",
                        new Shoot(8, 5, 72, defaultAngle: 20, predictive: 0.3, coolDown: 1600, coolDownOffset: 500),
                        new Shoot(8, 5, 72, defaultAngle: 33, predictive: 0.3, coolDown: 1600, coolDownOffset: 1300),
                        new Prioritize(
                            new StayAbove(0.7, 160),
                            new Orbit(1.2, 2.5, target: "Undead Dwarf King", acquireRange: 12, radiusVariance: 0.1,
                                speedVariance: 0.1),
                            new Wander(0.7)
                            ),
                        new TimedTransition(3500, "circle_player")
                        )
                    ),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Undead Dwarf King",
                new State(
                    new Shoot(3),
                    new Prioritize(
                        new StayAbove(1, 160),
                        new Follow(0.8, range: 1.4),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Soulless Dwarf",
                new State(
                    new Shoot(10),
                    new State("idle",
                        new PlayerWithinTransition(10.5, "run1")
                        ),
                    new State("run1",
                        new Prioritize(
                            new StayAbove(0.4, 160),
                            new Protect(1.1, "Undead Dwarf God", 16, 10, reprotectRange: 1),
                            new Wander(0.4)
                            ),
                        new TimedTransition(2000, "run2")
                        ),
                    new State("run2",
                        new Prioritize(
                            new StayAbove(0.4, 160),
                            new StayBack(0.8, 4),
                            new Wander(0.4)
                            ),
                        new TimedTransition(1400, "run3")
                        ),
                    new State("run3",
                        new Prioritize(
                            new StayAbove(0.4, 160),
                            new Protect(1, "Undead Dwarf King", 16, 2, 2),
                            new Protect(1, "Undead Dwarf Axebearer", 16, 2, 2),
                            new Protect(1, "Undead Dwarf Warrior", 16, 2, 2),
                            new Wander(0.4)
                            ),
                        new TimedTransition(2000, "idle")
                        )
                    ),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Flayer God",
                new State(
                    new DropPortalOnDeath("Snake Pit Portal", 20, PortalDespawnTimeSec: 100),
                    new Spawn("Flayer", 2),
                    new Spawn("Flayer Veteran", 3),
                    new Reproduce("Flayer God", densityMax: 2),
                    new Prioritize(
                        new StayAbove(0.4, 155),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(10, projectileIndex: 0, predictive: 0.5, coolDown: 400),
                    new Shoot(10, projectileIndex: 1, predictive: 1)
                    ),
                new TierLoot(5, ItemType.Weapon, 0.16),
                new TierLoot(6, ItemType.Weapon, 0.08),
                new TierLoot(7, ItemType.Weapon, 0.04),
                new TierLoot(5, ItemType.Armor, 0.16),
                new TierLoot(6, ItemType.Armor, 0.08),
                new TierLoot(7, ItemType.Armor, 0.04),
                new TierLoot(3, ItemType.Ring, 0.05),
                new TierLoot(3, ItemType.Ability, 0.15),
                new ItemLoot("Purple Drake Egg", 0.005)
            )
            .Init("Flayer",
                new State(
                    new Shoot(10, predictive: 0.5),
                    new Prioritize(
                        new StayAbove(1, 155),
                        new Follow(1.2, range: 7),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Flayer Veteran",
                new State(
                    new Shoot(10, predictive: 0.5),
                    new Prioritize(
                        new StayAbove(1, 155),
                        new Follow(1.2, range: 7),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Flamer King",
                new State(
                    new Spawn("Flamer", 5, coolDown: 10000),
                    new State("Attacking",
                        new State("Charge",
                            new Follow(0.7, range: 0.1),
                            new PlayerWithinTransition(2, "Bullet")
                            ),
                        new State("Bullet",
                            new Flash(0xffffaa00, 0.2, 20),
                            new ChangeSize(20, 140),
                            new Shoot(8, coolDown: 200),
                            new TimedTransition(4000, "Wait")
                            ),
                        new State("Wait",
                            new ChangeSize(-20, 80),
                            new TimedTransition(500, "Charge")
                            ),
                        new HpLessTransition(0.2, "FlashBeforeExplode")
                        ),
                    new State("FlashBeforeExplode",
                        new Flash(0xffff0000, 1, 1),
                        new TimedTransition(300, "Explode")
                        ),
                    new State("Explode",
                        new Shoot(0, 10, 36, fixedAngle: 0),
                        new Decay(0)
                        )
                    ),
                new ItemLoot("Health Potion", 0.01),
                new ItemLoot("Magic Potion", 0.01),
                new TierLoot(2, ItemType.Ring, 0.04)
            )
            .Init("Flamer",
                new State(
                    new State("Attacking",
                        new State("Charge",
                            new Prioritize(
                                new Protect(0.7, "Flamer King"),
                                new Follow(0.7, range: 0.1)
                                ),
                            new PlayerWithinTransition(2, "Bullet")
                            ),
                        new State("Bullet",
                            new Flash(0xffffaa00, 0.2, 20),
                            new ChangeSize(20, 130),
                            new Shoot(8, coolDown: 200),
                            new TimedTransition(4000, "Wait")
                            ),
                        new State("Wait",
                            new ChangeSize(-20, 70),
                            new TimedTransition(600, "Charge")
                            ),
                        new HpLessTransition(0.2, "FlashBeforeExplode")
                        ),
                    new State("FlashBeforeExplode",
                        new Flash(0xffff0000, 1, 1),
                        new TimedTransition(300, "Explode")
                        ),
                    new State("Explode",
                        new Shoot(0, 10, 36, fixedAngle: 0),
                        new Decay(0)
                        )
                    ),
                new ItemLoot("Magic Potion", 0.2),
                new TierLoot(5, ItemType.Weapon, 0.04)
            )
            .Init("Dragon Egg",
                new State(
                    new TransformOnDeath("White Dragon Whelp", probability: 0.3),
                    new TransformOnDeath("Juvenile White Dragon", probability: 0.2),
                    new TransformOnDeath("Adult White Dragon", probability: 0.1)
                    )
            )
            .Init("White Dragon Whelp",
                new State(
                    new Shoot(10, 2, 20, predictive: 0.3, coolDown: 750),
                    new Prioritize(
                        new StayAbove(1, 150),
                        new Follow(2, range: 2.5, acquireRange: 10.5, duration: 2200, coolDown: 3200),
                        new Wander(0.9)
                        )
                    )
            )
            .Init("Juvenile White Dragon",
                new State(
                    new Shoot(10, 2, 20, predictive: 0.3, coolDown: 750),
                    new Prioritize(
                        new StayAbove(9, 150),
                        new Follow(1.8, range: 2.2, acquireRange: 10.5, duration: 3000, coolDown: 3000),
                        new Wander(0.75)
                        )
                    ),
                new TierLoot(7, ItemType.Weapon, 0.01),
                new TierLoot(7, ItemType.Armor, 0.02),
                new TierLoot(6, ItemType.Armor, 0.07)
            )
            .Init("Adult White Dragon",
                new State(
                    new Shoot(10, 3, 15, predictive: 0.3, coolDown: 750),
                    new Prioritize(
                        new StayAbove(9, 150),
                        new Follow(1.4, range: 1.8, acquireRange: 10.5, duration: 4000, coolDown: 2000),
                        new Wander(0.75)
                        )
                    ),
                new ItemLoot("Health Potion", 0.03),
                new ItemLoot("Magic Potion", 0.03),
                new TierLoot(7, ItemType.Armor, 0.05),
                new ItemLoot("Seal of the Divine", 0.015),
                new ItemLoot("White Drake Egg", 0.004)
            )
            .Init("Shield Orc Shield",
                new State(
                    new Prioritize(
                        new Orbit(1, 3, target: "Shield Orc Flooder"),
                        new Wander(0.1)
                        ),
                    new State("Attacking",
                        new State("Attack",
                            new Flash(0xff000000, 10, 100),
                            new Shoot(10, coolDown: 500),
                            new HpLessTransition(0.5, "Heal"),
                            new EntityNotExistsTransition("Shield Orc Key", 7, "Idling")
                            ),
                        new State("Heal",
                            new Heal(7, "Shield Orcs", 500),
                            new TimedTransition(500, "Attack"),
                            new EntityNotExistsTransition("Shield Orc Key", 7, "Idling")
                            )
                        ),
                    new State("Flash",
                        new Flash(0xff0000, 1, 1),
                        new TimedTransition(300, "Idling")
                        ),
                    new State("Idling")
                    ),
                new ItemLoot("Health Potion", 0.04),
                new ItemLoot("Magic Potion", 0.01),
                new TierLoot(2, ItemType.Ring, 0.01)
            )
            .Init("Shield Orc Flooder",
                new State(
                    new Prioritize(
                        new Wander(0.1)
                        ),
                    new State("Attacking",
                        new State("Attack",
                            new Flash(0xff000000, 10, 100),
                            new Shoot(10, coolDown: 500),
                            new HpLessTransition(0.5, "Heal"),
                            new EntityNotExistsTransition("Shield Orc Key", 7, "Idling")
                            ),
                        new State("Heal",
                            new Heal(7, "Shield Orcs", 500),
                            new TimedTransition(500, "Attack"),
                            new EntityNotExistsTransition("Shield Orc Key", 7, "Idling")
                            )
                        ),
                    new State("Flash",
                        new Flash(0xff0000, 1, 1),
                        new TimedTransition(300, "Idling")
                        ),
                    new State("Idling")
                    ),
                new ItemLoot("Health Potion", 0.04),
                new ItemLoot("Magic Potion", 0.01),
                new TierLoot(4, ItemType.Ability, 0.01)
            )
            .Init("Shield Orc Key",
                new State(
                    new Spawn("Shield Orc Flooder", 1, 1, 10000),
                    new Spawn("Shield Orc Shield", 1, 1, 10000),
                    new Spawn("Shield Orc Shield", 1, 1, 10000),
                    new State("Start",
                        new TimedTransition(500, "Attacking")
                        ),
                    new State("Attacking",
                        new Orbit(1, 3, target: "Shield Orc Flooder"),
                        new Order(7, "Shield Orc Flooder", "Attacking"),
                        new Order(7, "Shield Orc Shield", "Attacking"),
                        new HpLessTransition(0.5, "FlashBeforeExplode")
                        ),
                    new State("FlashBeforeExplode",
                        new Order(7, "Shield Orc Flooder", "Flash"),
                        new Order(7, "Shield Orc Shield", "Flash"),
                        new Flash(0xff0000, 1, 1),
                        new TimedTransition(300, "Explode")
                        ),
                    new State("Explode",
                        new Shoot(0, 10, 36, fixedAngle: 0),
                        new Decay(0)
                        )
                    ),
                new ItemLoot("Health Potion", 0.04),
                new ItemLoot("Magic Potion", 0.01),
                new TierLoot(4, ItemType.Armor, 0.01)
            )
            .Init("Left Horizontal Trap",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("weak_effect",
                        new Shoot(1, fixedAngle: 0, projectileIndex: 0, coolDown: 200),
                        new TimedTransition(2000, "blind_effect")
                        ),
                    new State("blind_effect",
                        new Shoot(1, fixedAngle: 0, projectileIndex: 1, coolDown: 200),
                        new TimedTransition(2000, "pierce_effect")
                        ),
                    new State("pierce_effect",
                        new Shoot(1, fixedAngle: 0, projectileIndex: 2, coolDown: 200),
                        new TimedTransition(2000, "weak_effect")
                        ),
                    new Decay(6000)
                    )
            )
            .Init("Top Vertical Trap",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("weak_effect",
                        new Shoot(1, fixedAngle: 90, projectileIndex: 0, coolDown: 200),
                        new TimedTransition(2000, "blind_effect")
                        ),
                    new State("blind_effect",
                        new Shoot(1, fixedAngle: 90, projectileIndex: 1, coolDown: 200),
                        new TimedTransition(2000, "pierce_effect")
                        ),
                    new State("pierce_effect",
                        new Shoot(1, fixedAngle: 90, projectileIndex: 2, coolDown: 200),
                        new TimedTransition(2000, "weak_effect")
                        ),
                    new Decay(6000)
                    )
            )
            .Init("45-225 Diagonal Trap",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("weak_effect",
                        new Shoot(1, fixedAngle: 45, projectileIndex: 0, coolDown: 200),
                        new TimedTransition(2000, "blind_effect")
                        ),
                    new State("blind_effect",
                        new Shoot(1, fixedAngle: 45, projectileIndex: 1, coolDown: 200),
                        new TimedTransition(2000, "pierce_effect")
                        ),
                    new State("pierce_effect",
                        new Shoot(1, fixedAngle: 45, projectileIndex: 2, coolDown: 200),
                        new TimedTransition(2000, "weak_effect")
                        ),
                    new Decay(6000)
                    )
            )
            .Init("135-315 Diagonal Trap",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("weak_effect",
                        new Shoot(1, fixedAngle: 135, projectileIndex: 0, coolDown: 200),
                        new TimedTransition(2000, "blind_effect")
                        ),
                    new State("blind_effect",
                        new Shoot(1, fixedAngle: 135, projectileIndex: 1, coolDown: 200),
                        new TimedTransition(2000, "pierce_effect")
                        ),
                    new State("pierce_effect",
                        new Shoot(1, fixedAngle: 135, projectileIndex: 2, coolDown: 200),
                        new TimedTransition(2000, "weak_effect")
                        ),
                    new Decay(6000)
                    )
            )
            .Init("Urgle",
                new State(
                    new DropPortalOnDeath("Spider Den Portal", 20, PortalDespawnTimeSec: 100),
                    new Prioritize(
                        new StayCloseToSpawn(0.8, 3),
                        new Wander(0.5)
                        ),
                    new Shoot(8, predictive: 0.3),
                    new State("idle",
                        new PlayerWithinTransition(10.5, "toss_horizontal_traps")
                        ),
                    new State("toss_horizontal_traps",
                        new TossObject("Left Horizontal Trap", 9, 230, 100000),
                        new TossObject("Left Horizontal Trap", 10, 180, 100000),
                        new TossObject("Left Horizontal Trap", 9, 140, 100000),
                        new TimedTransition(1000, "toss_vertical_traps")
                        ),
                    new State("toss_vertical_traps",
                        new TossObject("Top Vertical Trap", 8, 200, 100000),
                        new TossObject("Top Vertical Trap", 10, 240, 100000),
                        new TossObject("Top Vertical Trap", 10, 280, 100000),
                        new TossObject("Top Vertical Trap", 8, 320, 100000),
                        new TimedTransition(1000, "toss_diagonal_traps")
                        ),
                    new State("toss_diagonal_traps",
                        new TossObject("45-225 Diagonal Trap", 2, 45, 100000),
                        new TossObject("45-225 Diagonal Trap", 7, 45, 100000),
                        new TossObject("45-225 Diagonal Trap", 11, 225, 100000),
                        new TossObject("45-225 Diagonal Trap", 6, 225, 100000),
                        new TossObject("135-315 Diagonal Trap", 2, 135, 100000),
                        new TossObject("135-315 Diagonal Trap", 7, 135, 100000),
                        new TossObject("135-315 Diagonal Trap", 11, 315, 100000),
                        new TossObject("135-315 Diagonal Trap", 6, 315, 100000),
                        new TimedTransition(1000, "wait")
                        ),
                    new State("wait",
                        new TimedTransition(2400, "idle")
                        )
                    ),
                new TierLoot(5, ItemType.Weapon, 0.16),
                new TierLoot(6, ItemType.Weapon, 0.08),
                new TierLoot(7, ItemType.Weapon, 0.04),
                new TierLoot(5, ItemType.Armor, 0.16),
                new TierLoot(6, ItemType.Armor, 0.08),
                new TierLoot(7, ItemType.Armor, 0.04),
                new TierLoot(3, ItemType.Ring, 0.05),
                new TierLoot(3, ItemType.Ability, 0.1)
            )
            ;
    }
}
