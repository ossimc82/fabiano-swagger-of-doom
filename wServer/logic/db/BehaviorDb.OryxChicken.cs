using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.logic.transitions;
using wServer.logic.behaviors;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ OryxChicken = () => Behav()
            .Init("Test Egg",
                new State(
                    new State("Idle",
                        new HpLessTransition(630000, "1")
                    ),
                    new State("1",
                        new HpLessTransition(610000, "2"),
                        new SetAltTexture(1)
                    ),
                    new State("2",
                        new HpLessTransition(590000, "3"),
                        new SetAltTexture(2)
                    ),
                    new State("3",
                        new HpLessTransition(570000, "4"),
                        new SetAltTexture(3)
                    ),
                    new State("4",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new SetAltTexture(4),
                        new TimedTransition(250, "Break")
                    ),
                    new State("Break",
                        new Transform("TestChicken 2")
                    )
                )
            )
            .Init("TestChicken 2",
                new State(
                    new HpLessTransition(1000, "Death"),
                    new Prioritize(
                        new StayCloseToSpawn(0.6, 5),
                        new Wander(0.6)
                    ),
                    new State("Idle",
                        new ChangeSize(20, 100),
                        new TimedTransition(600, "Start")
                    ),
                    new State("Start",
                        new Taunt("CLUCK!"),
                        new State("Shoot",
                            new EntityNotExistsTransition("TestChicken Small", 10, "Spawn Minions"),
                            new Shoot(10, 2)
                        ),
                        new State("Spawn Minions",
                            new Spawn("TestChicken Small", 5, 1),
                            new TimedTransition(0, "Shoot")
                        )
                    ),
                    new State("Death",
                        new Shoot(100, 10, projectileIndex: 6),
                        new Suicide()
                    )
                )
            )
            .Init("TestChicken Small",
                new State(
                    new Prioritize(
                        new Protect(0.6, "TestChicken 2"),
                        new Wander(0.6)
                    ),
                    new State("Default",
                        new Shoot(10, 8, projectileIndex: 2, coolDown: 1000)
                    ),
                    new State("Despawn",
                        new Suicide()
                    )
                )
            );
    }
}
