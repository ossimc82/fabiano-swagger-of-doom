#region

using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Tutorial = () => Behav()
            .Init("West Tutorial Gun",
                new State(
                    new Shoot(32, fixedAngle: 180, coolDown: new Cooldown(3000, 1000))
                    )
            )
            .Init("North Tutorial Gun",
                new State(
                    new Shoot(32, fixedAngle: 270, coolDown: new Cooldown(3000, 1000))
                    )
            )
            .Init("East Tutorial Gun",
                new State(
                    new Shoot(32, fixedAngle: 0, coolDown: new Cooldown(3000, 1000))
                    )
            )
            .Init("South Tutorial Gun",
                new State(
                    new Shoot(32, fixedAngle: 90, coolDown: new Cooldown(3000, 1000))
                    )
            )
            .Init("Evil Chicken",
                new State(
                    new Wander(0.3)
                    )
            )
            .Init("Evil Chicken Minion",
                new State(
                    new Wander(0.3),
                    new Protect(0.3, "Evil Chicken God")
                    )
            )
            .Init("Evil Chicken God",
                new State(
                    new Prioritize(
                        new Follow(0.4, range: 5),
                        new Wander(0.3)
                        ),
                    new Reproduce("Evil Chicken Minion", densityMax: 12)
                    )
            )
            .Init("Evil Hen",
                new State(
                    new Wander(0.3)
                    ),
                new ItemLoot("Minor Health Potion", 1)
            )
            .Init("Kitchen Guard",
                new State(
                    new Prioritize(
                        new Follow(0.6, range: 6),
                        new Wander(0.4)
                        ),
                    new Shoot(7)
                    )
            )
            .Init("Butcher",
                new State(
                    new Prioritize(
                        new Follow(0.8, range: 1),
                        new Wander(0.4)
                        ),
                    new Shoot(3)
                    ),
                new ItemLoot("Minor Health Potion", 0.1),
                new ItemLoot("Minor Magic Potion", 0.1)
            )
            .Init("Bonegrind the Butcher",
                new State(
                    new State("Begin",
                        new Wander(0.6),
                        new PlayerWithinTransition(10, "AttackX")
                        ),
                    new State("AttackX",
                        new Taunt("Ah, fresh meat for the minions!"),
                        new Shoot(6, coolDown: 1400),
                        new Prioritize(
                            new Follow(0.6, 9, 3),
                            new Wander(0.6)
                            ),
                        new TimedTransition(4500, "AttackY"),
                        new HpLessTransition(0.3, "Flee")
                        ),
                    new State("AttackY",
                        new Prioritize(
                            new Follow(0.6, 9, 3),
                            new Wander(0.6)
                            ),
                        new Sequence(
                            new Shoot(7, 4, fixedAngle: 25),
                            new Shoot(7, 4, fixedAngle: 50),
                            new Shoot(7, 4, fixedAngle: 75),
                            new Shoot(7, 4, fixedAngle: 100),
                            new Shoot(7, 4, fixedAngle: 125)
                            ),
                        new TimedTransition(5200, "AttackX"),
                        new HpLessTransition(0.3, "Flee")
                        ),
                    new State("Flee",
                        new Taunt("The meat ain't supposed to bite back! Waaaaa!!"),
                        new Flash(0xff000000, 10, 100),
                        new Prioritize(
                            new StayBack(0.5, 6),
                            new Wander(0.5)
                            )
                        )
                    ),
                new ItemLoot("Minor Health Potion", 1),
                new ItemLoot("Minor Magic Potion", 1)
            )
            ;
    }
}