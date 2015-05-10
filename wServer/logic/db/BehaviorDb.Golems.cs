#region

using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Golems = () => Behav()
            .Init("Red Satellite",
                new State(
                    new Prioritize(
                        new Orbit(1.7, 2, target: "Fire Golem", acquireRange: 15, speedVariance: 0, radiusVariance: 0),
                        new Orbit(1.7, 2, target: "Metal Golem", acquireRange: 15, speedVariance: 0, radiusVariance: 0)
                        ),
                    new Decay(16000)
                    )
            )
            .Init("Green Satellite",
                new State(
                    new Prioritize(
                        new Orbit(1.1, 2, target: "Darkness Golem", acquireRange: 15, speedVariance: 0,
                            radiusVariance: 0),
                        new Orbit(1.1, 2, target: "Earth Golem", acquireRange: 15, speedVariance: 0, radiusVariance: 0)
                        ),
                    new Decay(16000)
                    )
            )
            .Init("Blue Satellite",
                new State(
                    new Prioritize(
                        new Orbit(1.1, 2, target: "Clockwork Golem", acquireRange: 15, speedVariance: 0,
                            radiusVariance: 0),
                        new Orbit(1.1, 2, target: "Paper Golem", acquireRange: 15, speedVariance: 0, radiusVariance: 0)
                        ),
                    new Decay(16000)
                    )
            )
            .Init("Gray Satellite 1",
                new State(
                    new Shoot(6, 3, 34, predictive: 0.3, coolDown: 850),
                    new Prioritize(
                        new Orbit(2.2, 0.75, target: "Red Satellite", acquireRange: 15, speedVariance: 0,
                            radiusVariance: 0),
                        new Orbit(2.2, 0.75, target: "Blue Satellite", acquireRange: 15, speedVariance: 0,
                            radiusVariance: 0)
                        ),
                    new Decay(16000)
                    )
            )
            .Init("Gray Satellite 2",
                new State(
                    new Shoot(7, predictive: 0.3, coolDown: 600),
                    new Prioritize(
                        new Orbit(2.2, 0.75, target: "Green Satellite", acquireRange: 15, speedVariance: 0,
                            radiusVariance: 0),
                        new Orbit(2.2, 0.75, target: "Blue Satellite", acquireRange: 15, speedVariance: 0,
                            radiusVariance: 0)
                        ),
                    new Decay(16000)
                    )
            )
            .Init("Gray Satellite 3",
                new State(
                    new Shoot(7, 5, 72, coolDown: 3200, coolDownOffset: 600),
                    new Shoot(7, 4, 90, coolDown: 3200, coolDownOffset: 1400),
                    new Shoot(7, 5, 72, defaultAngle: 36, coolDown: 3200, coolDownOffset: 2200),
                    new Shoot(7, 4, 90, defaultAngle: 45, coolDown: 3200, coolDownOffset: 3000),
                    new Prioritize(
                        new Orbit(2.2, 0.75, target: "Red Satellite", acquireRange: 15, speedVariance: 0,
                            radiusVariance: 0),
                        new Orbit(2.2, 0.75, target: "Green Satellite", acquireRange: 15, speedVariance: 0,
                            radiusVariance: 0)
                        ),
                    new Decay(16000)
                    )
            )
            .Init("Earth Golem",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(11, "player_nearby")
                        ),
                    new State("player_nearby",
                        new Shoot(8, 2, 12, coolDown: 600),
                        new State("first_satellites",
                            new Spawn("Green Satellite", 1, coolDown: 200),
                            new Spawn("Gray Satellite 3", 1, coolDown: 200),
                            new TimedTransition(300, "next_satellite")
                            ),
                        new State("next_satellite",
                            new Spawn("Gray Satellite 3", 1, coolDown: 200),
                            new TimedTransition(200, "follow")
                            ),
                        new State("follow",
                            new Prioritize(
                                new StayAbove(1.4, 65),
                                new Follow(1.4, range: 3),
                                new Wander(0.8)
                                ),
                            new TimedTransition(2000, "wander1")
                            ),
                        new State("wander1",
                            new Prioritize(
                                new StayAbove(1.55, 65),
                                new Wander(0.55)
                                ),
                            new TimedTransition(4000, "circle")
                            ),
                        new State("circle",
                            new Prioritize(
                                new StayAbove(1.2, 65),
                                new Orbit(1.2, 5.4, 11)
                                ),
                            new TimedTransition(4000, "wander2")
                            ),
                        new State("wander2",
                            new Prioritize(
                                new StayAbove(0.55, 65),
                                new Wander(0.55)
                                ),
                            new TimedTransition(3000, "back_and_forth")
                            ),
                        new State("back_and_forth",
                            new Prioritize(
                                new StayAbove(0.55, 65),
                                new BackAndForth(0.8)
                                ),
                            new TimedTransition(3000, "first_satellites")
                            )
                        ),
                    new Reproduce(densityMax: 1)
                    ),
                new TierLoot(2, ItemType.Ring, 0.02),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Paper Golem",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(11, "player_nearby")
                        ),
                    new State("player_nearby",
                        new Spawn("Blue Satellite", 1, coolDown: 200),
                        new Spawn("Gray Satellite 1", 1, coolDown: 200),
                        new Shoot(10, predictive: 0.5, coolDown: 700),
                        new Prioritize(
                            new StayAbove(1.4, 65),
                            new Follow(1, range: 3, duration: 3000, coolDown: 3000),
                            new Wander(0.4)
                            ),
                        new TimedTransition(12000, "idle")
                        ),
                    new Reproduce(densityMax: 1)
                    ),
                new TierLoot(5, ItemType.Weapon, 0.02),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Fire Golem",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(11, "player_nearby")
                        ),
                    new State("player_nearby",
                        new Prioritize(
                            new StayAbove(1.4, 65),
                            new Follow(1, range: 3, duration: 3000, coolDown: 3000),
                            new Wander(0.4)
                            ),
                        new Spawn("Red Satellite", 1, coolDown: 200),
                        new Spawn("Gray Satellite 1", 1, coolDown: 200),
                        new State("slowshot",
                            new Shoot(10, projectileIndex: 0, predictive: 0.5, coolDown: 300, coolDownOffset: 600),
                            new TimedTransition(5000, "megashot")
                            ),
                        new State("megashot",
                            new Flash(0xffffffff, 0.2, 5),
                            new Shoot(10, projectileIndex: 1, predictive: 0.2, coolDown: 90, coolDownOffset: 1000),
                            new TimedTransition(1200, "slowshot")
                            )
                        ),
                    new Reproduce(densityMax: 1)
                    ),
                new TierLoot(6, ItemType.Armor, 0.015),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Darkness Golem",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(11, "player_nearby")
                        ),
                    new State("player_nearby",
                        new State("first_satellites",
                            new Spawn("Green Satellite", 1, coolDown: 200),
                            new Spawn("Gray Satellite 2", 1, coolDown: 200),
                            new TimedTransition(200, "next_satellite")
                            ),
                        new State("next_satellite",
                            new Spawn("Gray Satellite 2", 1, coolDown: 200),
                            new TimedTransition(200, "follow")
                            ),
                        new State("follow",
                            new Shoot(6, projectileIndex: 0, coolDown: 200),
                            new Prioritize(
                                new StayAbove(1.2, 65),
                                new Follow(1.2, range: 1),
                                new Wander(0.5)
                                ),
                            new TimedTransition(3000, "wander1")
                            ),
                        new State("wander1",
                            new Shoot(6, projectileIndex: 0, coolDown: 200),
                            new Prioritize(
                                new StayAbove(0.65, 65),
                                new Wander(0.65)
                                ),
                            new TimedTransition(3800, "back_up")
                            ),
                        new State("back_up",
                            new Flash(0xffffffff, 0.2, 25),
                            new Shoot(9, projectileIndex: 1, coolDown: 1400, coolDownOffset: 1000),
                            new Prioritize(
                                new StayAbove(0.4, 65),
                                new StayBack(0.4, 4),
                                new Wander(0.4)
                                ),
                            new TimedTransition(5400, "wander2")
                            ),
                        new State("wander2",
                            new Shoot(6, projectileIndex: 0, coolDown: 200),
                            new Prioritize(
                                new StayAbove(0.65, 65),
                                new Wander(0.65)
                                ),
                            new TimedTransition(3800, "first_satellites")
                            )
                        ),
                    new Reproduce(densityMax: 1)
                    ),
                new TierLoot(2, ItemType.Ring, 0.02),
                new ItemLoot("Magic Potion", 0.03)
            )
            ;
    }
}