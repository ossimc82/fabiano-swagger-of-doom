#region

using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Cyclops = () => Behav()
            .Init("Cyclops God",
                new State(
                    new DropPortalOnDeath("Spider Den Portal", 20, PortalDespawnTimeSec: 100),
                    new State("idle",
                        new PlayerWithinTransition(11, "blade_attack"),
                        new HpLessTransition(0.8, "blade_attack")
                        ),
                    new State("blade_attack",
                        new Prioritize(
                            new Follow(0.4, range: 7),
                            new Wander(0.4)
                            ),
                        new Shoot(10, projectileIndex: 4, count: 1, shootAngle: 15, predictive: 0.5, coolDown: 100000),
                        new Shoot(10, projectileIndex: 4, count: 2, shootAngle: 10, predictive: 0.5, coolDown: 100000,
                            coolDownOffset: 700),
                        new Shoot(10, projectileIndex: 4, count: 3, shootAngle: 8.5, predictive: 0.5, coolDown: 100000,
                            coolDownOffset: 1400),
                        new Shoot(10, projectileIndex: 4, count: 4, shootAngle: 7, predictive: 0.5, coolDown: 100000,
                            coolDownOffset: 2100),
                        new TimedTransition(4000, "if_cloaked1")
                        ),
                    new State("if_cloaked1",
                        new Shoot(10, projectileIndex: 4, count: 15, shootAngle: 24, fixedAngle: 8, coolDown: 1500,
                            coolDownOffset: 400),
                        new TimedTransition(10000, "wave_attack"),
                        new PlayerWithinTransition(10.5, "wave_attack")
                        ),
                    new State("wave_attack",
                        new Prioritize(
                            new Follow(0.6, range: 5),
                            new Wander(0.6)
                            ),
                        new Shoot(9, projectileIndex: 0, coolDown: 700, coolDownOffset: 700),
                        new Shoot(9, projectileIndex: 1, coolDown: 700, coolDownOffset: 700),
                        new Shoot(9, projectileIndex: 2, coolDown: 700, coolDownOffset: 700),
                        new Shoot(9, projectileIndex: 3, coolDown: 700, coolDownOffset: 700),
                        new TimedTransition(3800, "if_cloaked2")
                        ),
                    new State("if_cloaked2",
                        new Shoot(10, projectileIndex: 4, count: 15, shootAngle: 24, fixedAngle: 8, coolDown: 1500,
                            coolDownOffset: 400),
                        new TimedTransition(10000, "idle"),
                        new PlayerWithinTransition(10.5, "idle")
                        ),
                    new Taunt(0.7, 10000, "I will floss with your tendons!",
                        "I smell the blood of an Englishman!",
                        "I will suck the marrow from your bones!",
                        "You will be my food, {PLAYER}!",
                        "Blargh!!",
                        "Leave my castle!",
                        "More wine!"
                        ),
                    new StayCloseToSpawn(1.2, 5),
                    new Spawn("Cyclops", 5, coolDown: 10000),
                    new Spawn("Cyclops Warrior", 5, coolDown: 10000),
                    new Spawn("Cyclops Noble", 5, coolDown: 10000),
                    new Spawn("Cyclops Prince", 5, coolDown: 10000),
                    new Spawn("Cyclops King", 5, coolDown: 10000)
                    )
            )
            .Init("Cyclops",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(1.2, 5),
                        new Follow(1.2, range: 1),
                        new Wander(0.4)
                        ),
                    new Shoot(3)
                    ),
                new ItemLoot("Golden Sword", 0.02),
                new ItemLoot("Studded Leather Armor", 0.02),
                new ItemLoot("Health Potion", 0.05)
            )
            .Init("Cyclops Warrior",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(1.2, 5),
                        new Follow(1.2, range: 1),
                        new Wander(0.4)
                        ),
                    new Shoot(3)
                    ),
                new ItemLoot("Golden Sword", 0.03),
                new ItemLoot("Golden Shield", 0.02),
                new ItemLoot("Health Potion", 0.05)
            )
            .Init("Cyclops Noble",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(1.2, 5),
                        new Follow(1.2, range: 1),
                        new Wander(0.4)
                        ),
                    new Shoot(3)
                    ),
                new ItemLoot("Golden Dagger", 0.02),
                new ItemLoot("Studded Leather Armor", 0.02),
                new ItemLoot("Health Potion", 0.05)
            )
            .Init("Cyclops Prince",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(1.2, 5),
                        new Follow(1.2, range: 1),
                        new Wander(0.4)
                        ),
                    new Shoot(3)
                    ),
                new ItemLoot("Mithril Dagger", 0.02),
                new ItemLoot("Plate Mail", 0.02),
                new ItemLoot("Seal of the Divine", 0.01),
                new ItemLoot("Health Potion", 0.05)
            )
            .Init("Cyclops King",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(1.2, 5),
                        new Follow(1.2, range: 1),
                        new Wander(0.4)
                        ),
                    new Shoot(3)
                    ),
                new ItemLoot("Golden Sword", 0.02),
                new ItemLoot("Mithril Armor", 0.02),
                new ItemLoot("Health Potion", 0.05)
            )
            ;
    }
}
