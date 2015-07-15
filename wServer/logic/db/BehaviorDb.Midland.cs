#region

using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Midland = () => Behav()
            .Init("Fire Sprite",
                new State(
                    new Reproduce(densityMax: 2),
                    new Shoot(10, 2, 7, coolDown: 300),
                    new Prioritize(
                        new StayAbove(1.4, 55),
                        new Wander(1.4)
                        )
                    ),
                new TierLoot(5, ItemType.Weapon, 0.02),
                new ItemLoot("Magic Potion", 0.05)
            )
            .Init("Ice Sprite",
                new State(
                    new Reproduce(densityMax: 2),
                    new Shoot(10, 3, 7),
                    new Prioritize(
                        new StayAbove(1.4, 60),
                        new Wander(1.4)
                        )
                    ),
                new TierLoot(2, ItemType.Ability, 0.04),
                new ItemLoot("Magic Potion", 0.05)
            )
            .Init("Magic Sprite",
                new State(
                    new Reproduce(densityMax: 2),
                    new Shoot(10, 4, 7),
                    new Prioritize(
                        new StayAbove(1.4, 60),
                        new Wander(1.4)
                        )
                    ),
                new TierLoot(6, ItemType.Armor, 0.01),
                new ItemLoot("Magic Potion", 0.05)
            )
            .Init("Orc King",
                new State(
                    new Shoot(3),
                    new Spawn("Orc Queen", 2, coolDown: 60000),
                    new Prioritize(
                        new StayAbove(1.4, 60),
                        new Follow(0.6, range: 1, duration: 3000, coolDown: 3000),
                        new Wander(0.6)
                        )
                    ),
                new TierLoot(4, ItemType.Weapon, 0.18),
                new TierLoot(5, ItemType.Weapon, 0.05),
                new TierLoot(5, ItemType.Armor, 0.21),
                new TierLoot(6, ItemType.Armor, 0.035),
                new ItemLoot("Magic Potion", 0.03),
                new TierLoot(2, ItemType.Ring, 0.07),
                new TierLoot(2, ItemType.Ability, 0.17)
            )
            .Init("Orc Queen",
                new State(
                    new Spawn("Orc Mage", 2, coolDown: 8000),
                    new Spawn("Orc Warrior", 3, coolDown: 8000),
                    new Prioritize(
                        new StayAbove(1.4, 60),
                        new Protect(0.8, "Orc King", 11, 7, 5.4),
                        new Wander(0.8)
                        ),
                    new Heal(10, "OrcKings", 300)
                    ),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Orc Mage",
                new State(
                    new State("circle_player",
                        new Shoot(8, predictive: 0.3, coolDown: 1000, coolDownOffset: 500),
                        new Prioritize(
                            new StayAbove(1.4, 60),
                            new Protect(0.7, "Orc Queen", 11, 10, 3),
                            new Orbit(0.7, 3.5, 11)
                            ),
                        new TimedTransition(3500, "circle_queen")
                        ),
                    new State("circle_queen",
                        new Shoot(8, 3, predictive: 0.3, shootAngle: 120, coolDown: 1000, coolDownOffset: 500),
                        new Prioritize(
                            new StayAbove(1.4, 60),
                            new Orbit(1.2, 2.5, target: "Orc Queen", acquireRange: 12, speedVariance: 0.1,
                                radiusVariance: 0.1)
                            ),
                        new TimedTransition(3500, "circle_player")
                        )
                    ),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Orc Warrior",
                new State(
                    new Shoot(3, predictive: 1, coolDown: 500),
                    new Prioritize(
                        new StayAbove(1.4, 60),
                        new Orbit(1.35, 2.5, target: "Orc Queen", acquireRange: 12, speedVariance: 0.1,
                            radiusVariance: 0.1)
                        )
                    ),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Pink Blob",
                new State(
                    new StayAbove(0.4, 50),
                    new Shoot(6, 3, 7),
                    new Prioritize(
                        new Follow(0.8, 15, 5),
                        new Wander(0.4)
                        ),
                    new Reproduce(densityMax: 5, densityRadius: 10)
                    ),
                new ItemLoot("Health Potion", 0.03),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Gray Blob",
                new State(
                    new State("searching",
                        new StayAbove(0.2, 50),
                        new Prioritize(
                            new Charge(2),
                            new Wander(0.4)
                            ),
                        new Reproduce(densityMax: 5, densityRadius: 10),
                        new PlayerWithinTransition(2, "creeping")
                        ),
                    new State("creeping",
                        new Shoot(0, 10, 36, fixedAngle: 0),
                        new Decay(0)
                        )
                    ),
                new ItemLoot("Health Potion", 0.03),
                new ItemLoot("Magic Potion", 0.03),
                new ItemLoot("Magic Mushroom", 0.005)
            )
            .Init("Big Green Slime",
                new State(
                    new StayAbove(0.4, 50),
                    new Shoot(9),
                    new Wander(0.4),
                    new Reproduce(densityMax: 5, densityRadius: 10),
                    new TransformOnDeath("Little Green Slime"),
                    new TransformOnDeath("Little Green Slime"),
                    new TransformOnDeath("Little Green Slime"),
                    new TransformOnDeath("Little Green Slime")
                    )
            )
            .Init("Little Green Slime",
                new State(
                    new StayAbove(0.4, 50),
                    new Shoot(6),
                    new Wander(0.4),
                    new Protect(0.4, "Big Green Slime")
                    ),
                new ItemLoot("Health Potion", 0.03),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Wasp Queen",
                new State(
                    new DropPortalOnDeath("Forest Maze Portal", 20, PortalDespawnTimeSec: 100),
                    new Spawn("Worker Wasp", 5, coolDown: 3400),
                    new Spawn("Warrior Wasp", 2, coolDown: 4400),
                    new State("idle",
                        new StayAbove(0.4, 60),
                        new Wander(0.55),
                        new PlayerWithinTransition(10, "froth")
                        ),
                    new State("froth",
                        new Shoot(8, predictive: 0.1, coolDown: 1600),
                        new Prioritize(
                            new StayAbove(0.4, 60),
                            new Wander(0.55)
                            )
                        )
                    ),
                new TierLoot(5, ItemType.Weapon, 0.14),
                new TierLoot(6, ItemType.Weapon, 0.05),
                new TierLoot(5, ItemType.Armor, 0.19),
                new TierLoot(6, ItemType.Armor, 0.02),
                new TierLoot(2, ItemType.Ring, 0.07),
                new TierLoot(3, ItemType.Ring, 0.001),
                new TierLoot(2, ItemType.Ability, 0.28),
                new TierLoot(3, ItemType.Ability, 0.01),
                new ItemLoot("Health Potion", 0.15),
                new ItemLoot("Magic Potion", 0.07)
            )
            .Init("Worker Wasp",
                new State(
                    new Shoot(8, coolDown: 4000),
                    new Prioritize(
                        new Orbit(1, 2, target: "Wasp Queen", radiusVariance: 0.5),
                        new Wander(0.75)
                        )
                    )
            )
            .Init("Warrior Wasp",
                new State(
                    new Shoot(8, predictive: 200, coolDown: 1000),
                    new State("protecting",
                        new Prioritize(
                            new Orbit(1, 2, target: "Wasp Queen", radiusVariance: 0),
                            new Wander(0.75)
                            ),
                        new TimedTransition(3000, "attacking")
                        ),
                    new State("attacking",
                        new Prioritize(
                            new Follow(0.8, 9, 3.4),
                            new Orbit(1, 2, target: "Wasp Queen", radiusVariance: 0),
                            new Wander(0.75)
                            ),
                        new TimedTransition(2200, "protecting")
                        )
                    )
            )
            .Init("Shambling Sludge",
                new State(
                    new State("idle",
                        new StayAbove(0.5, 55),
                        new PlayerWithinTransition(10, "toss_sludge")
                        ),
                    new State("toss_sludge",
                        new Prioritize(
                            new StayAbove(0.5, 55),
                            new Wander(0.5)
                            ),
                        new Shoot(8, coolDown: 1200),
                        new TossObject("Sludget", 3, 20, 100000),
                        new TossObject("Sludget", 3, 92, 100000),
                        new TossObject("Sludget", 3, 164, 100000),
                        new TossObject("Sludget", 3, 236, 100000),
                        new TossObject("Sludget", 3, 308, 100000),
                        new TimedTransition(8000, "pause")
                        ),
                    new State("pause",
                        new Prioritize(
                            new StayAbove(0.5, 55),
                            new Wander(0.5)
                            ),
                        new TimedTransition(1000, "idle")
                        )
                    ),
                new TierLoot(4, ItemType.Weapon, 0.14),
                new TierLoot(5, ItemType.Weapon, 0.05),
                new TierLoot(5, ItemType.Armor, 0.20),
                new TierLoot(6, ItemType.Armor, 0.02),
                new TierLoot(2, ItemType.Ring, 0.07),
                new TierLoot(2, ItemType.Ability, 0.27),
                new ItemLoot("Health Potion", 0.15),
                new ItemLoot("Magic Potion", 0.10)
            )
            .Init("Sludget",
                new State(
                    new State("idle",
                        new Shoot(8, predictive: 0.5, coolDown: 600),
                        new Prioritize(
                            new Protect(0.5, "Shambling Sludge", 11, 7.5, 7.4),
                            new Wander(0.5)
                            ),
                        new TimedTransition(1400, "wander")
                        ),
                    new State("wander",
                        new Prioritize(
                            new Protect(0.5, "Shambling Sludge", 11, 7.5, 7.4),
                            new Wander(0.5)
                            ),
                        new TimedTransition(5400, "jump")
                        ),
                    new State("jump",
                        new Prioritize(
                            new Protect(0.5, "Shambling Sludge", 11, 7.5, 7.4),
                            new Follow(7, 6, 1),
                            new Wander(0.5)
                            ),
                        new TimedTransition(200, "attack")
                        ),
                    new State("attack",
                        new Shoot(8, predictive: 0.5, coolDown: 600, coolDownOffset: 300),
                        new Prioritize(
                            new Protect(0.5, "Shambling Sludge", 11, 7.5, 7.4),
                            new Follow(0.5, 6, 1),
                            new Wander(0.5)
                            ),
                        new TimedTransition(4000, "idle")
                        ),
                    new Decay(9000)
                    )
            )
            .Init("Swarm",
                new State(
                    new State("circle",
                        new Prioritize(
                            new StayAbove(0.4, 60),
                            new Follow(4, 11, 3.5, 1000, 5000),
                            new Orbit(1.9, 3.5, 12),
                            new Wander(0.4)
                            ),
                        new Shoot(4, predictive: 0.1, coolDown: 500),
                        new TimedTransition(3000, "dart_away")
                        ),
                    new State("dart_away",
                        new Prioritize(
                            new StayAbove(0.4, 60),
                            new StayBack(2, 5),
                            new Wander(0.4)
                            ),
                        new Shoot(8, 5, 72, fixedAngle: 20, coolDown: 100000, coolDownOffset: 800),
                        new Shoot(8, 5, 72, fixedAngle: 56, coolDown: 100000, coolDownOffset: 1400),
                        new TimedTransition(1600, "circle")
                        ),
                    new Reproduce(densityMax: 1, densityRadius: 100)
                    ),
                new TierLoot(3, ItemType.Weapon, 0.22),
                new TierLoot(4, ItemType.Weapon, 0.05),
                new TierLoot(3, ItemType.Armor, 0.22),
                new TierLoot(4, ItemType.Armor, 0.12),
                new TierLoot(5, ItemType.Armor, 0.02),
                new TierLoot(1, ItemType.Ring, 0.1),
                new TierLoot(1, ItemType.Ability, 0.21),
                new ItemLoot("Health Potion", 0.24),
                new ItemLoot("Magic Potion", 0.07)
            )
            .Init("Black Bat",
                new State(
                    new Prioritize(
                        new Charge(),
                        new Wander(0.4)
                        ),
                    new Shoot(1),
                    new Reproduce(densityMax: 5, densityRadius: 20, coolDown: 20000)
                    ),
                new ItemLoot("Health Potion", 0.01),
                new ItemLoot("Magic Potion", 0.01),
                new TierLoot(2, ItemType.Armor, 0.01)
            )
            .Init("Red Spider",
                new State(
                    new Wander(0.8),
                    new Shoot(9),
                    new Reproduce(densityMax: 3, densityRadius: 15, coolDown: 45000)
                    ),
                new ItemLoot("Health Potion", 0.03),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Dwarf Axebearer",
                new State(
                    new Shoot(3.4),
                    new State("Default",
                        new Wander(0.4)
                        ),
                    new State("Circling",
                        new Prioritize(
                            new Orbit(0.4, 2.7, 11),
                            new Protect(1.2, "Dwarf King", 15, 6, 3),
                            new Wander(0.4)
                            ),
                        new TimedTransition(3300, "Default"),
                        new EntityNotExistsTransition("Dwarf King", 8, "Default")
                        ),
                    new State("Engaging",
                        new Prioritize(
                            new Follow(1.0, 15, 1),
                            new Protect(1.2, "Dwarf King", 15, 6, 3),
                            new Wander(0.4)
                            ),
                        new TimedTransition(2500, "Circling"),
                        new EntityNotExistsTransition("Dwarf King", 8, "Default")
                        )
                    ),
                new ItemLoot("Health Potion", 0.04)
            )
            .Init("Dwarf Mage",
                new State(
                    new State("Default",
                        new Prioritize(
                            new Protect(1.2, "Dwarf King", 15, 6, 3),
                            new Wander(0.6)
                            ),
                        new State("fire1_def",
                            new Shoot(10, predictive: 0.2, coolDown: 100000),
                            new TimedTransition(1500, "fire2_def")
                            ),
                        new State("fire2_def",
                            new Shoot(5, predictive: 0.2, coolDown: 100000),
                            new TimedTransition(1500, "fire1_def")
                            )
                        ),
                    new State("Circling",
                        new Prioritize(
                            new Orbit(0.4, 2.7, 11),
                            new Protect(1.2, "Dwarf King", 15, 6, 3),
                            new Wander(0.6)
                            ),
                        new State("fire1_cir",
                            new Shoot(10, predictive: 0.2, coolDown: 100000),
                            new TimedTransition(1500, "fire2_cir")
                            ),
                        new State("fire2_cir",
                            new Shoot(5, predictive: 0.2, coolDown: 100000),
                            new TimedTransition(1500, "fire1_cir")
                            ),
                        new TimedTransition(3300, "Default"),
                        new EntityNotExistsTransition("Dwarf King", 8, "Default")
                        ),
                    new State("Engaging",
                        new Prioritize(
                            new Follow(1.0, 15, 1),
                            new Protect(1.2, "Dwarf King", 15, 6, 3),
                            new Wander(0.4)
                            ),
                        new State("fire1_eng",
                            new Shoot(10, predictive: 0.2, coolDown: 100000),
                            new TimedTransition(1500, "fire2_eng")
                            ),
                        new State("fire2_eng",
                            new Shoot(5, predictive: 0.2, coolDown: 100000),
                            new TimedTransition(1500, "fire1_eng")
                            ),
                        new TimedTransition(2500, "Circling"),
                        new EntityNotExistsTransition("Dwarf King", 8, "Default")
                        )
                    ),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Dwarf Veteran",
                new State(
                    new Shoot(4),
                    new State("Default",
                        new Prioritize(
                            new Follow(1.0, 9, 2, 3000, 1000),
                            new Wander(0.4)
                            )
                        ),
                    new State("Circling",
                        new Prioritize(
                            new Orbit(0.4, 2.7, 11),
                            new Protect(1.2, "Dwarf King", 15, 6, 3),
                            new Wander(0.4)
                            ),
                        new TimedTransition(3300, "Default"),
                        new EntityNotExistsTransition("Dwarf King", 8, "Default")
                        ),
                    new State("Engaging",
                        new Prioritize(
                            new Follow(1.0, 15, 1),
                            new Protect(1.2, "Dwarf King", 15, 6, 3),
                            new Wander(0.4)
                            ),
                        new TimedTransition(2500, "Circling"),
                        new EntityNotExistsTransition("Dwarf King", 8, "Default")
                        )
                    ),
                new ItemLoot("Health Potion", 0.04)
            )
            .Init("Dwarf King",
                new State(
                    new DropPortalOnDeath("Forest Maze Portal", 20, PortalDespawnTimeSec: 100),
                    new SpawnGroup("Dwarves", 10, coolDown: 8000),
                    new Shoot(4, coolDown: 2000),
                    new State("Circling",
                        new Prioritize(
                            new Orbit(0.4, 2.7, 11),
                            new Wander(0.4)
                            ),
                        new TimedTransition(3400, "Engaging")
                        ),
                    new State("Engaging",
                        new Taunt(0.2, "You'll taste my axe!"),
                        new Prioritize(
                            new Follow(1.0, 15, 1),
                            new Wander(0.4)
                            ),
                        new TimedTransition(2600, "Circling")
                        )
                    ),
                new TierLoot(3, ItemType.Weapon, 0.2),
                new TierLoot(4, ItemType.Weapon, 0.12),
                new TierLoot(3, ItemType.Armor, 0.2),
                new TierLoot(4, ItemType.Armor, 0.15),
                new TierLoot(5, ItemType.Armor, 0.02),
                new TierLoot(1, ItemType.Ring, 0.11),
                new TierLoot(1, ItemType.Ability, 0.38),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Werelion",
                new State(
                    new Spawn("Weretiger", 1, coolDown: 23000),
                    new Spawn("Wereleopard", 2, coolDown: 9000),
                    new Spawn("Werepanther", 3, coolDown: 15000),
                    new Shoot(4, coolDown: 2000),
                    new State("idle",
                        new Prioritize(
                            new StayAbove(0.6, 60),
                            new Wander(0.6)
                            ),
                        new PlayerWithinTransition(11, "player_nearby")
                        ),
                    new State("player_nearby",
                        new State("normal_attack",
                            new Shoot(10, 3, 15, predictive: 1, coolDown: 10000),
                            new TimedTransition(900, "if_cloaked")
                            ),
                        new State("if_cloaked",
                            new Shoot(10, 8, 45, defaultAngle: 20, coolDown: 1600, coolDownOffset: 400),
                            new Shoot(10, 8, 45, defaultAngle: 42, coolDown: 1600, coolDownOffset: 1200),
                            new PlayerWithinTransition(10, "normal_attack")
                            ),
                        new Prioritize(
                            new StayAbove(0.6, 60),
                            new Follow(0.4, 7, 3),
                            new Wander(0.6)
                            ),
                        new TimedTransition(30000, "idle")
                        )
                    ),
                new TierLoot(4, ItemType.Weapon, 0.18),
                new TierLoot(5, ItemType.Weapon, 0.05),
                new TierLoot(5, ItemType.Armor, 0.24),
                new TierLoot(6, ItemType.Armor, 0.03),
                new TierLoot(2, ItemType.Ring, 0.07),
                new TierLoot(2, ItemType.Ability, 0.2),
                new ItemLoot("Health Potion", 0.04),
                new ItemLoot("Magic Potion", 0.05)
            )
            .Init("Weretiger",
                new State(
                    new Shoot(8, predictive: 0.3, coolDown: 1000),
                    new Prioritize(
                        new StayAbove(0.6, 60),
                        new Protect(1.1, "Werelion", 12, 10, 5),
                        new Follow(0.8, range: 6.3),
                        new Wander(0.6)
                        )
                    ),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Wereleopard",
                new State(
                    new Shoot(4.5, predictive: 0.4, coolDown: 900),
                    new Prioritize(
                        new Protect(1.1, "Werelion", 12, 10, 5),
                        new Follow(1.1, range: 3),
                        new Wander(1)
                        )
                    ),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Werepanther",
                new State(
                    new State("idle",
                        new Protect(0.65, "Werelion", 11, 7.5, 7.4),
                        new PlayerWithinTransition(9.5, "wander")
                        ),
                    new State("wander",
                        new Prioritize(
                            new Protect(0.65, "Werelion", 11, 7.5, 7.4),
                            new Follow(0.65, range: 5, acquireRange: 10),
                            new Wander(0.65)
                            ),
                        new PlayerWithinTransition(4, "jump")
                        ),
                    new State("jump",
                        new Prioritize(
                            new Protect(0.65, "Werelion", 11, 7.5, 7.4),
                            new Follow(7, range: 1, acquireRange: 6),
                            new Wander(0.55)
                            ),
                        new TimedTransition(200, "attack")
                        ),
                    new State("attack",
                        new Prioritize(
                            new Protect(0.65, "Werelion", 11, 7.5, 7.4),
                            new Follow(0.5, range: 1, acquireRange: 6),
                            new Wander(0.5)
                            ),
                        new Shoot(4, predictive: 0.5, coolDown: 800, coolDownOffset: 300),
                        new TimedTransition(4000, "idle")
                        )
                    ),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Horned Drake",
                new State(
                    new Spawn("Drake Baby", 1, 1, 50000),
                    new State("idle",
                        new StayAbove(0.8, 60),
                        new PlayerWithinTransition(10, "get_player")
                        ),
                    new State("get_player",
                        new Prioritize(
                            new StayAbove(0.8, 60),
                            new Follow(0.8, range: 2.7, acquireRange: 10, duration: 5000, coolDown: 1800),
                            new Wander(0.8)
                            ),
                        new State("one_shot",
                            new Shoot(8, predictive: 0.1, coolDown: 800),
                            new TimedTransition(900, "three_shot")
                            ),
                        new State("three_shot",
                            new Shoot(8, 3, 40, predictive: 0.1, coolDown: 100000, coolDownOffset: 800),
                            new TimedTransition(2000, "one_shot")
                            )
                        ),
                    new State("protect_me",
                        new Protect(0.8, "Drake Baby", 12, 2.5, 1.5),
                        new State("one_shot",
                            new Shoot(8, predictive: 0.1, coolDown: 700),
                            new TimedTransition(800, "three_shot")
                            ),
                        new State("three_shot",
                            new Shoot(8, 3, 40, predictive: 0.1, coolDown: 100000, coolDownOffset: 700),
                            new TimedTransition(1800, "one_shot")
                            ),
                        new EntityNotExistsTransition("Drake Baby", 8, "idle")
                        )
                    ),
                new TierLoot(5, ItemType.Weapon, 0.14),
                new TierLoot(6, ItemType.Weapon, 0.05),
                new TierLoot(5, ItemType.Armor, 0.19),
                new TierLoot(6, ItemType.Armor, 0.02),
                new TierLoot(2, ItemType.Ring, 0.07),
                new TierLoot(3, ItemType.Ring, 0.001),
                new TierLoot(2, ItemType.Ability, 0.28),
                new TierLoot(3, ItemType.Ability, 0.001),
                new ItemLoot("Health Potion", 0.09),
                new ItemLoot("Magic Potion", 0.12)
            )
            .Init("Drake Baby",
                new State(
                    new DropPortalOnDeath("Forest Maze Portal", 20, PortalDespawnTimeSec: 100),
                    new State("unharmed",
                        new Shoot(8, coolDown: 1500),
                        new State("wander",
                            new Prioritize(
                                new StayAbove(0.8, 60),
                                new Wander(0.8)
                                ),
                            new TimedTransition(2000, "find_mama")
                            ),
                        new State("find_mama",
                            new Prioritize(
                                new StayAbove(0.8, 60),
                                new Protect(1.4, "Horned Drake", 15, 4, 4)
                                ),
                            new TimedTransition(2000, "wander")
                            ),
                        new HpLessTransition(0.65, "call_mama")
                        ),
                    new State("call_mama",
                        new Flash(0xff484848, 0.6, 5000),
                        new State("get_close_to_mama",
                            new Taunt("Awwwk! Awwwk!"),
                            new Protect(1.4, "Horned Drake", 15, 1, reprotectRange: 1),
                            new TimedTransition(1500, "cry_for_mama")
                            ),
                        new State("cry_for_mama",
                            new StayBack(0.65, 8),
                            new Order(8, "Horned Drake", "protect_me")
                            )
                        )
                    )
            )
            .Init("Nomadic Shaman",
                new State(
                    new Prioritize(
                        new StayAbove(0.8, 55),
                        new Wander(0.7)
                        ),
                    new State("fire1",
                        new Shoot(10, projectileIndex: 0, count: 3, shootAngle: 11, coolDown: 500, coolDownOffset: 500),
                        new TimedTransition(3100, "fire2")
                        ),
                    new State("fire2",
                        new Shoot(10, projectileIndex: 1, coolDown: 700, coolDownOffset: 700),
                        new TimedTransition(2200, "fire1")
                        )
                    ),
                new ItemLoot("Magic Potion", 0.04)
            )
            .Init("Sand Phantom",
                new State(
                    new Prioritize(
                        new StayAbove(0.85, 60),
                        new Follow(0.85, 10.5, 1),
                        new Wander(0.85)
                        ),
                    new Shoot(8, predictive: 0.4, coolDown: 400, coolDownOffset: 600),
                    new State("follow_player",
                        new PlayerWithinTransition(4.4, "sneak_away_from_player")
                        ),
                    new State("sneak_away_from_player",
                        new Transform("Sand Phantom Wisp")
                        )
                    )
            )
            .Init("Sand Phantom Wisp",
                new State(
                    new Shoot(8, predictive: 0.4, coolDown: 400, coolDownOffset: 600),
                    new State("move_away_from_player",
                        new State("keep_back",
                            new Prioritize(
                                new StayBack(0.6, 5),
                                new Wander(0.9)
                                ),
                            new TimedTransition(800, "wander")
                            ),
                        new State("wander",
                            new Wander(0.9),
                            new TimedTransition(800, "keep_back")
                            ),
                        new TimedTransition(6500, "wisp_finished")
                        ),
                    new State("wisp_finished",
                        new Transform("Sand Phantom")
                        )
                    )
            )
            .Init("Great Lizard",
                new State(
                    new State("idle",
                        new StayAbove(0.6, 60),
                        new Wander(0.6),
                        new PlayerWithinTransition(10, "charge")
                        ),
                    new State("charge",
                        new Prioritize(
                            new StayAbove(0.6, 60),
                            new Follow(6, 11, 1.5)
                            ),
                        new TimedTransition(200, "spit")
                        ),
                    new State("spit",
                        new Shoot(8, projectileIndex: 0, count: 1, coolDown: 100000, coolDownOffset: 1000),
                        new Shoot(8, projectileIndex: 0, count: 2, shootAngle: 16, coolDown: 100000,
                            coolDownOffset: 1200),
                        new Shoot(8, projectileIndex: 0, count: 1, predictive: 0.2, coolDown: 100000,
                            coolDownOffset: 1600),
                        new Shoot(8, projectileIndex: 0, count: 2, shootAngle: 24, coolDown: 100000,
                            coolDownOffset: 2200),
                        new Shoot(8, projectileIndex: 0, count: 1, predictive: 0.2, coolDown: 100000,
                            coolDownOffset: 2800),
                        new Shoot(8, projectileIndex: 0, count: 2, shootAngle: 16, coolDown: 100000,
                            coolDownOffset: 3200),
                        new Shoot(8, projectileIndex: 0, count: 1, predictive: 0.1, coolDown: 100000,
                            coolDownOffset: 3800),
                        new Prioritize(
                            new StayAbove(0.6, 60),
                            new Wander(0.6)
                            ),
                        new TimedTransition(5000, "flame_ring")
                        ),
                    new State("flame_ring",
                        new Shoot(7, projectileIndex: 1, count: 30, shootAngle: 12, coolDown: 400, coolDownOffset: 600),
                        new Prioritize(
                            new StayAbove(0.6, 60),
                            new Follow(0.6, 9, 1),
                            new Wander(0.6)
                            ),
                        new TimedTransition(3500, "pause")
                        ),
                    new State("pause",
                        new Prioritize(
                            new StayAbove(0.6, 60),
                            new Wander(0.6)
                            ),
                        new TimedTransition(1000, "idle")
                        )
                    ),
                new TierLoot(4, ItemType.Weapon, 0.14),
                new TierLoot(5, ItemType.Weapon, 0.05),
                new TierLoot(5, ItemType.Armor, 0.19),
                new TierLoot(6, ItemType.Armor, 0.02),
                new TierLoot(2, ItemType.Ring, 0.07),
                new TierLoot(2, ItemType.Ability, 0.27),
                new ItemLoot("Health Potion", 0.12),
                new ItemLoot("Magic Potion", 0.10)
            )
            .Init("Tawny Warg",
                new State(
                    new Shoot(3.4),
                    new Prioritize(
                        new Protect(1.2, "Desert Werewolf", 14, 8, 5),
                        new Follow(0.7, 9, 2),
                        new Wander(0.8)
                        )
                    ),
                new ItemLoot("Health Potion", 0.04)
            )
            .Init("Demon Warg",
                new State(
                    new Shoot(4.5),
                    new Prioritize(
                        new Protect(1.2, "Desert Werewolf", 14, 8, 5),
                        new Wander(0.8)
                        )
                    ),
                new ItemLoot("Health Potion", 0.04)
            )
            .Init("Desert Werewolf",
                new State(
                    new SpawnGroup("Wargs", 8, coolDown: 8000),
                    new State("unharmed",
                        new Shoot(8, projectileIndex: 0, predictive: 0.3, coolDown: 1000, coolDownOffset: 500),
                        new Prioritize(
                            new Follow(0.5, 10.5, 2.5),
                            new Wander(0.5)
                            ),
                        new HpLessTransition(0.75, "enraged")
                        ),
                    new State("enraged",
                        new Shoot(8, projectileIndex: 0, predictive: 0.3, coolDown: 1000, coolDownOffset: 500),
                        new Taunt(0.7, "GRRRRAAGH!"),
                        new ChangeSize(20, 170),
                        new Flash(0xffff0000, 0.4, 5000),
                        new Prioritize(
                            new Follow(0.65, 9, 2),
                            new Wander(0.65)
                            )
                        )
                    ),
                new TierLoot(3, ItemType.Weapon, 0.2),
                new TierLoot(4, ItemType.Weapon, 0.12),
                new TierLoot(3, ItemType.Armor, 0.2),
                new TierLoot(4, ItemType.Armor, 0.15),
                new TierLoot(5, ItemType.Armor, 0.02),
                new TierLoot(1, ItemType.Ring, 0.11),
                new TierLoot(1, ItemType.Ability, 0.38),
                new ItemLoot("Magic Potion", 0.03)
            )
            ;
    }
}
