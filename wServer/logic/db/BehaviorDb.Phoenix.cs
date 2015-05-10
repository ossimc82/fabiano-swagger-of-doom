#region

using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Phoenix = () => Behav()
            .Init("Phoenix Lord",
                new State(
                    new Shoot(10, 4, 7, predictive: 0.5, coolDown: 600),
                    new Prioritize(
                        new StayCloseToSpawn(0.3, 2),
                        new Wander(0.4)
                        ),
                    new SpawnGroup("Pyre", 16, coolDown: 5000),
                    new Taunt(0.7, 10000,
                        "Alas, {PLAYER}, you will taste death but once!",
                        "I have met many like you, {PLAYER}, in my thrice thousand years!",
                        "Purge yourself, {PLAYER}, in the heat of my flames!",
                        "The ashes of past heroes cover my plains!",
                        "Some die and are ashes, but I am ever reborn!"
                        ),
                    new TransformOnDeath("Phoenix Egg")
                    )
            )
            .Init("Birdman Chief",
                new State(
                    new Prioritize(
                        new Protect(0.5, "Phoenix Lord", 15, 10, 3),
                        new Follow(1, range: 9),
                        new Wander(0.5)
                        ),
                    new Shoot(10)
                    ),
                new ItemLoot("Magic Potion", 0.05)
            )
            .Init("Birdman",
                new State(
                    new Prioritize(
                        new Protect(0.5, "Phoenix Lord", 15, 11, 3),
                        new Follow(1, range: 7),
                        new Wander(0.5)
                        ),
                    new Shoot(10, predictive: 0.5)
                    ),
                new ItemLoot("Health Potion", 0.05)
            )
            .Init("Phoenix Egg",
                new State(
                    new State("shielded",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(2000, "unshielded")
                        ),
                    new State("unshielded",
                        new Flash(0xff0000, 1, 5000),
                        new State("grow",
                            new ChangeSize(20, 150),
                            new TimedTransition(800, "shrink")
                            ),
                        new State("shrink",
                            new ChangeSize(-20, 130),
                            new TimedTransition(800, "grow")
                            )
                        ),
                    new TransformOnDeath("Phoenix Reborn")
                    )
            )
            .Init("Phoenix Reborn",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(0.9, 5),
                        new Wander(0.9)
                        ),
                    new SpawnGroup("Pyre", 4, coolDown: 1000),
                    new State("born_anew",
                        new Shoot(10, projectileIndex: 0, count: 12, shootAngle: 30, fixedAngle: 10, coolDown: 100000,
                            coolDownOffset: 500),
                        new Shoot(10, projectileIndex: 0, count: 12, shootAngle: 30, fixedAngle: 25, coolDown: 100000,
                            coolDownOffset: 1000),
                        new TimedTransition(1800, "xxx")
                        ),
                    new State("xxx",
                        new Shoot(10, projectileIndex: 1, count: 4, shootAngle: 7, predictive: 0.5, coolDown: 600),
                        new TimedTransition(2800, "yyy")
                        ),
                    new State("yyy",
                        new Shoot(10, projectileIndex: 0, count: 12, shootAngle: 30, fixedAngle: 10, coolDown: 100000,
                            coolDownOffset: 500),
                        new Shoot(10, projectileIndex: 0, count: 12, shootAngle: 30, fixedAngle: 25, coolDown: 100000,
                            coolDownOffset: 1000),
                        new Shoot(10, projectileIndex: 1, predictive: 0.5, coolDown: 350),
                        new TimedTransition(4500, "xxx")
                        )
                    ),
                new ItemLoot("Wine Cellar Incantation", 0.002)
            )
            ;
    }
}