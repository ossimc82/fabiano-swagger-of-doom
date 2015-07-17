#region

using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Lowland = () => Behav()
            .Init("Hobbit Mage",
                new State(
                    new DropPortalOnDeath("Forest Maze Portal", 20, PortalDespawnTimeSec: 100),
                    new State("idle",
                        new PlayerWithinTransition(12, "ring1")
                        ),
                    new State("ring1",
                        new Shoot(1, fixedAngle: 0, count: 15, shootAngle: 24, coolDown: 1200, projectileIndex: 0),
                        new TimedTransition(400, "ring2")
                        ),
                    new State("ring2",
                        new Shoot(1, fixedAngle: 8, count: 15, shootAngle: 24, coolDown: 1200, projectileIndex: 1),
                        new TimedTransition(400, "ring3")
                        ),
                    new State("ring3",
                        new Shoot(1, fixedAngle: 16, count: 15, shootAngle: 24, coolDown: 1200, projectileIndex: 2),
                        new TimedTransition(400, "idle")
                        ),
                    new Prioritize(
                        new StayAbove(0.4, 9),
                        new Follow(0.75, range: 6),
                        new Wander(0.4)
                        ),
                    new Spawn("Hobbit Archer", 4, coolDown: 12000),
                    new Spawn("Hobbit Rogue", 3, coolDown: 6000)
                    ),
                new TierLoot(2, ItemType.Weapon, 0.3),
                new TierLoot(2, ItemType.Armor, 0.3),
                new TierLoot(1, ItemType.Ring, 0.11),
                new TierLoot(1, ItemType.Ability, 0.39),
                new ItemLoot("Health Potion", 0.02),
                new ItemLoot("Magic Potion", 0.02)
            )
            .Init("Hobbit Archer",
                new State(
                    new Shoot(10),
                    new State("run1",
                        new Prioritize(
                            new Protect(1.1, "Hobbit Mage", 12, 10, reprotectRange: 1),
                            new Wander(0.4)
                            ),
                        new TimedTransition(400, "run2")
                        ),
                    new State("run2",
                        new Prioritize(
                            new StayBack(0.8, 4),
                            new Wander(0.4)
                            ),
                        new TimedTransition(600, "run3")
                        ),
                    new State("run3",
                        new Prioritize(
                            new Protect(1, "Hobbit Archer", 16, 2, 2),
                            new Wander(0.4)
                            ),
                        new TimedTransition(400, "run1")
                        )
                    ),
                new ItemLoot("Health Potion", 0.04)
            )
            .Init("Hobbit Rogue",
                new State(
                    new Shoot(3),
                    new Prioritize(
                        new Protect(1.2, "Hobbit Mage", 15, 9, 2.5),
                        new Follow(0.85, range: 1),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Health Potion", 0.04)
            )
            .Init("Undead Hobbit Mage",
                new State(
                    new Shoot(10, projectileIndex: 3),
                    new State("idle",
                        new PlayerWithinTransition(12, "ring1")
                        ),
                    new State("ring1",
                        new Shoot(1, fixedAngle: 0, count: 15, shootAngle: 24, coolDown: 1200, projectileIndex: 0),
                        new TimedTransition(400, "ring2")
                        ),
                    new State("ring2",
                        new Shoot(1, fixedAngle: 8, count: 15, shootAngle: 24, coolDown: 1200, projectileIndex: 1),
                        new TimedTransition(400, "ring3")
                        ),
                    new State("ring3",
                        new Shoot(1, fixedAngle: 16, count: 15, shootAngle: 24, coolDown: 1200, projectileIndex: 2),
                        new TimedTransition(400, "idle")
                        ),
                    new Prioritize(
                        new StayAbove(0.4, 20),
                        new Follow(0.75, range: 6),
                        new Wander(0.4)
                        ),
                    new Spawn("Undead Hobbit Archer", 4, coolDown: 12000),
                    new Spawn("Undead Hobbit Rogue", 3, coolDown: 6000)
                    ),
                new TierLoot(3, ItemType.Weapon, 0.3),
                new TierLoot(3, ItemType.Armor, 0.3),
                new TierLoot(1, ItemType.Ring, 0.12),
                new TierLoot(1, ItemType.Ability, 0.39),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Undead Hobbit Archer",
                new State(
                    new Shoot(10),
                    new State("run1",
                        new Prioritize(
                            new Protect(1.1, "Undead Hobbit Mage", 12, 10, reprotectRange: 1),
                            new Wander(0.4)
                            ),
                        new TimedTransition(400, "run2")
                        ),
                    new State("run2",
                        new Prioritize(
                            new StayBack(0.8, 4),
                            new Wander(0.4)
                            ),
                        new TimedTransition(600, "run3")
                        ),
                    new State("run3",
                        new Prioritize(
                            new Protect(1, "Undead Hobbit Archer", 16, 2, 2),
                            new Wander(0.4)
                            ),
                        new TimedTransition(400, "run1")
                        )
                    ),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Undead Hobbit Rogue",
                new State(
                    new Shoot(3),
                    new Prioritize(
                        new Protect(1.2, "Undead Hobbit Mage", 15, 9, 2.5),
                        new Follow(0.85, range: 1),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Health Potion", 0.04)
            )
            .Init("Sumo Master",
                new State(
                    new State("sleeping1",
                        new SetAltTexture(0),
                        new TimedTransition(1000, "sleeping2"),
                        new HpLessTransition(0.99, "hurt")
                        ),
                    new State("sleeping2",
                        new SetAltTexture(3),
                        new TimedTransition(1000, "sleeping1"),
                        new HpLessTransition(0.99, "hurt")
                        ),
                    new State("hurt",
                        new SetAltTexture(2),
                        new Spawn("Lil Sumo", coolDown: 200),
                        new TimedTransition(1000, "awake")
                        ),
                    new State("awake",
                        new SetAltTexture(1),
                        new Shoot(3, coolDown: 250),
                        new Prioritize(
                            new Follow(0.05, range: 1),
                            new Wander(0.05)
                            ),
                        new HpLessTransition(0.5, "rage")
                        ),
                    new State("rage",
                        new SetAltTexture(4),
                        new Taunt("Engaging Super-Mode!!!"),
                        new Prioritize(
                            new Follow(0.6, range: 1),
                            new Wander(0.6)
                            ),
                        new State("shoot",
                            new Shoot(8, projectileIndex: 1, coolDown: 150),
                            new TimedTransition(700, "rest")
                            ),
                        new State("rest",
                            new TimedTransition(400, "shoot")
                            )
                        )
                    ),
                new ItemLoot("Health Potion", 0.05),
                new ItemLoot("Magic Potion", 0.05)
            )
            .Init("Lil Sumo",
                new State(
                    new Shoot(8),
                    new Prioritize(
                        new Orbit(0.4, 2, target: "Sumo Master"),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Health Potion", 0.02),
                new ItemLoot("Magic Potion", 0.02)
            )
            .Init("Elf Wizard",
                new State(
                    new DropPortalOnDeath("Forest Maze Portal", 20, PortalDespawnTimeSec: 100),
                    new State("idle",
                        new Wander(0.4),
                        new PlayerWithinTransition(11, "move1")
                        ),
                    new State("move1",
                        new Shoot(10, 3, 14, predictive: 0.3),
                        new Prioritize(
                            new StayAbove(0.4, 14),
                            new BackAndForth(0.8)
                            ),
                        new TimedTransition(2000, "move2")
                        ),
                    new State("move2",
                        new Shoot(10, 3, 10, predictive: 0.5),
                        new Prioritize(
                            new StayAbove(0.4, 14),
                            new Follow(0.6, 10.5, 3),
                            new Wander(0.4)
                            ),
                        new TimedTransition(2000, "move3")
                        ),
                    new State("move3",
                        new Prioritize(
                            new StayAbove(0.4, 14),
                            new StayBack(0.6, 5),
                            new Wander(0.4)
                            ),
                        new TimedTransition(2000, "idle")
                        ),
                    new Spawn("Elf Archer", 2, coolDown: 15000),
                    new Spawn("Elf Swordsman", 4, coolDown: 7000),
                    new Spawn("Elf Mage", 1, coolDown: 8000)
                    ),
                new TierLoot(2, ItemType.Weapon, 0.36),
                new TierLoot(2, ItemType.Armor, 0.36),
                new TierLoot(1, ItemType.Ring, 0.11),
                new TierLoot(1, ItemType.Ability, 0.39),
                new ItemLoot("Health Potion", 0.02),
                new ItemLoot("Magic Potion", 0.02)
            )
            .Init("Elf Archer",
                new State(
                    new Shoot(10, predictive: 1),
                    new Prioritize(
                        new Orbit(0.5, 3, speedVariance: 0.1, radiusVariance: 0.5),
                        new Protect(1.2, "Elf Wizard", 30, 10, reprotectRange: 1),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Health Potion", 0.04)
            )
            .Init("Elf Swordsman",
                new State(
                    new Shoot(10, predictive: 1),
                    new Prioritize(
                        new Protect(1.2, "Elf Wizard", 15, 10, 5),
                        new Buzz(1, 1, 2000),
                        new Orbit(0.6, 3, speedVariance: 0.1, radiusVariance: 0.5),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Health Potion", 0.04)
            )
            .Init("Elf Mage",
                new State(
                    new Shoot(8, coolDown: 300),
                    new Prioritize(
                        new Orbit(0.5, 3),
                        new Protect(1.2, "Elf Wizard", 30, 10, reprotectRange: 1),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Goblin Rogue",
                new State(
                    new State("protect",
                        new Protect(0.8, "Goblin Mage", 12, 1.5, 1.5),
                        new TimedTransition(1200, "scatter", true)
                        ),
                    new State("scatter",
                        new Orbit(0.8, 7, target: "Goblin Mage", radiusVariance: 1),
                        new TimedTransition(2400, "protect")
                        ),
                    new Shoot(3),
                    new State("help",
                        new Protect(0.8, "Goblin Mage", 12, 6, 3),
                        new Follow(0.8, 10.5, 1.5),
                        new EntityNotExistsTransition("Goblin Mage", 15, "protect")
                        )
                    ),
                new ItemLoot("Health Potion", 0.04)
            )
            .Init("Goblin Warrior",
                new State(
                    new DropPortalOnDeath("Pirate Cave Portal", 20, PortalDespawnTimeSec: 100),
                    new State("protect",
                        new Protect(0.8, "Goblin Mage", 12, 1.5, 1.5),
                        new TimedTransition(1200, "scatter", true)
                        ),
                    new State("scatter",
                        new Orbit(0.8, 7, target: "Goblin Mage", radiusVariance: 1),
                        new TimedTransition(2400, "protect")
                        ),
                    new Shoot(3),
                    new State("help",
                        new Protect(0.8, "Goblin Mage", 12, 6, 3),
                        new Follow(0.8, 10.5, 1.5),
                        new EntityNotExistsTransition("Goblin Mage", 15, "protect")
                        )
                    ),
                new ItemLoot("Health Potion", 0.04)
            )
            .Init("Goblin Mage",
                new State(
                    new DropPortalOnDeath("Forest Maze Portal", 20, PortalDespawnTimeSec: 100),
                    new State("unharmed",
                        new Shoot(8, projectileIndex: 0, predictive: 0.35, coolDown: 1000),
                        new Shoot(8, projectileIndex: 1, predictive: 0.35, coolDown: 1300),
                        new Prioritize(
                            new StayAbove(0.4, 16),
                            new Follow(0.5, 10.5, 4),
                            new Wander(0.4)
                            ),
                        new HpLessTransition(0.65, "activate_horde")
                        ),
                    new State("activate_horde",
                        new Shoot(8, projectileIndex: 0, predictive: 0.25, coolDown: 1000),
                        new Shoot(8, projectileIndex: 1, predictive: 0.25, coolDown: 1000),
                        new Flash(0xff484848, 0.6, 5000),
                        new Order(12, "Goblin Rogue", "help"),
                        new Order(12, "Goblin Warrior", "help"),
                        new Prioritize(
                            new StayAbove(0.4, 16),
                            new StayBack(0.5, 6)
                            )
                        ),
                    new Spawn("Goblin Rogue", 7, coolDown: 12000),
                    new Spawn("Goblin Warrior", 7, coolDown: 12000)
                    ),
                new TierLoot(3, ItemType.Weapon, 0.3),
                new TierLoot(3, ItemType.Armor, 0.3),
                new TierLoot(1, ItemType.Ring, 0.09),
                new TierLoot(1, ItemType.Ability, 0.38),
                new ItemLoot("Health Potion", 0.02),
                new ItemLoot("Magic Potion", 0.02)
            )
            .Init("Easily Enraged Bunny",
                new State(
                    new Prioritize(
                        new StayAbove(0.4, 15),
                        new Follow(0.7, 9.5, 1)
                        ),
                    new TransformOnDeath("Enraged Bunny")
                    )
            )
            .Init("Enraged Bunny",
                new State(
                    new DropPortalOnDeath("Forest Maze Portal", 20, PortalDespawnTimeSec: 100),
                    new Shoot(9, predictive: 0.5, coolDown: 400),
                    new State("red",
                        new Flash(0xff0000, 1.5, 1),
                        new TimedTransition(1600, "yellow")
                        ),
                    new State("yellow",
                        new Flash(0xffff33, 1.5, 1),
                        new TimedTransition(1600, "orange")
                        ),
                    new State("orange",
                        new Flash(0xff9900, 1.5, 1),
                        new TimedTransition(1600, "red")
                        ),
                    new Prioritize(
                        new StayAbove(0.4, 15),
                        new Follow(0.85, 9, 2.5),
                        new Wander(0.85)
                        )
                    ),
                new ItemLoot("Health Potion", 0.01),
                new ItemLoot("Magic Potion", 0.02)
            )
            .Init("Forest Nymph",
                new State(
                    new DropPortalOnDeath("Pirate Cave Portal", 20, PortalDespawnTimeSec: 100),
                    new State("circle",
                        new Shoot(4, projectileIndex: 0, count: 1, predictive: 0.1, coolDown: 900),
                        new Prioritize(
                            new StayAbove(0.4, 25),
                            new Follow(0.9, 11, 3.5, 1000, 5000),
                            new Orbit(1.3, 3.5, 12),
                            new Wander(0.7)
                            ),
                        new TimedTransition(4000, "dart_away")
                        ),
                    new State("dart_away",
                        new Shoot(9, projectileIndex: 1, count: 6, fixedAngle: 20, shootAngle: 60, coolDown: 1400),
                        new Wander(0.4),
                        new TimedTransition(3600, "circle")
                        )
                    ),
                new ItemLoot("Health Potion", 0.03),
                new ItemLoot("Magic Potion", 0.02)
            )
            .Init("Sandsman King",
                new State(
                    new Shoot(10, coolDown: 10000),
                    new Prioritize(
                        new StayAbove(0.4, 15),
                        new Follow(0.6, range: 4),
                        new Wander(0.4)
                        ),
                    new Spawn("Sandsman Archer", 2, coolDown: 10000),
                    new Spawn("Sandsman Sorcerer", 3, coolDown: 8000)
                    ),
                new TierLoot(3, ItemType.Weapon, 0.3),
                new TierLoot(3, ItemType.Armor, 0.3),
                new TierLoot(1, ItemType.Ring, 0.11),
                new TierLoot(1, ItemType.Ability, 0.39),
                new ItemLoot("Health Potion", 0.04)
            )
            .Init("Sandsman Sorcerer",
                new State(
                    new Shoot(10, projectileIndex: 0, coolDown: 5000),
                    new Shoot(5, projectileIndex: 1, coolDown: 400),
                    new Prioritize(
                        new Protect(1.2, "Sandsman King", 15, 6, 5),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Sandsman Archer",
                new State(
                    new Shoot(10, predictive: 0.5),
                    new Prioritize(
                        new Orbit(0.8, 3.25, 15, "Sandsman King", radiusVariance: 0.5),
                        new Wander(0.4)
                        )
                    ),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Giant Crab",
                new State(
                    new DropPortalOnDeath("Pirate Cave Portal", 20, PortalDespawnTimeSec: 100),
                    new State("idle",
                        new Prioritize(
                            new StayAbove(0.6, 13),
                            new Wander(0.6)
                            ),
                        new PlayerWithinTransition(11, "scuttle")
                        ),
                    new State("scuttle",
                        new Shoot(9, projectileIndex: 0, coolDown: 1000),
                        new Shoot(9, projectileIndex: 1, coolDown: 1000),
                        new Shoot(9, projectileIndex: 2, coolDown: 1000),
                        new Shoot(9, projectileIndex: 3, coolDown: 1000),
                        new State("move",
                            new Prioritize(
                                new Follow(1, 10.6, 2),
                                new StayAbove(1, 25),
                                new Wander(0.6)
                                ),
                            new TimedTransition(400, "pause")
                            ),
                        new State("pause",
                            new TimedTransition(200, "move")
                            ),
                        new TimedTransition(4700, "tri-spit")
                        ),
                    new State("tri-spit",
                        new Shoot(9, projectileIndex: 4, predictive: 0.5, coolDownOffset: 1200, coolDown: 90000),
                        new Shoot(9, projectileIndex: 4, predictive: 0.5, coolDownOffset: 1800, coolDown: 90000),
                        new Shoot(9, projectileIndex: 4, predictive: 0.5, coolDownOffset: 2400, coolDown: 90000),
                        new State("move",
                            new Prioritize(
                                new Follow(1, 10.6, 2),
                                new StayAbove(1, 25),
                                new Wander(0.6)
                                ),
                            new TimedTransition(400, "pause")
                            ),
                        new State("pause",
                            new TimedTransition(200, "move")
                            ),
                        new TimedTransition(3200, "idle")
                        )
                    ),
                new TierLoot(2, ItemType.Weapon, 0.14),
                new TierLoot(2, ItemType.Armor, 0.19),
                new TierLoot(1, ItemType.Ring, 0.05),
                new TierLoot(1, ItemType.Ability, 0.28),
                new ItemLoot("Health Potion", 0.02),
                new ItemLoot("Magic Potion", 0.02)
            )
            .Init("Sand Devil",
                new State(
                    new DropPortalOnDeath("Pirate Cave Portal", 20, PortalDespawnTimeSec: 100),
                    new State("wander",
                        new Shoot(8, predictive: 0.3, coolDown: 700),
                        new Prioritize(
                            new StayAbove(0.7, 10),
                            new Follow(0.7, 10, 2.2),
                            new Wander(0.7)
                            ),
                        new TimedTransition(3000, "circle")
                        ),
                    new State("circle",
                        new Shoot(8, predictive: 0.3, coolDownOffset: 1000, coolDown: 1000),
                        new Orbit(0.7, 2, 9),
                        new TimedTransition(3100, "wander")
                        )
                    )
            );
    }
}
