#region

using wServer.logic.behaviors;
using wServer.logic.transitions;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Misc = () => Behav()
            .Init("White Fountain",
                new State(
                    new NexusHealHp(5, 100, 1000)
                    )
            )
            .Init("Winter Fountain Frozen", //Frozen <3
                                            //Kabam let it go :DDD
                new State(
                    new NexusHealHp(5, 100, 1000)
                    )
            )
            .Init("Sheep",
                new State(
                    new PlayerWithinTransition(15, "player_nearby"),
                    new State("player_nearby",
                        new Prioritize(
                            new StayCloseToSpawn(0.1, 2),
                            new Wander(0.1)
                            ),
                        new Taunt(0.001, 1000, "baa", "baa baa")
                        )
                    )
            );
    }
}