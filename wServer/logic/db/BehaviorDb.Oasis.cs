#region

using wServer.logic.behaviors;
using wServer.logic.loot;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Oasis = () => Behav()
            .Init("Oasis Giant",
                new State(
                    new Shoot(10, 4, 7, predictive: 1),
                    new Prioritize(
                        new StayCloseToSpawn(0.3, 2),
                        new Wander(0.4)
                        ),
                    new SpawnGroup("Oasis", 20, coolDown: 5000),
                    new Taunt(0.7, 10000,
                        "Come closer, {PLAYER}! Yes, closer!",
                        "I rule this place, {PLAYER}!",
                        "Surrender to my aquatic army, {PLAYER}!",
                        "You must be thirsty, {PLAYER}. Enter my waters!",
                        "Minions! We shall have {PLAYER} for dinner!"
                        )
                    )
            )
            .Init("Oasis Ruler",
                new State(
                    new Prioritize(
                        new Protect(0.5, "Oasis Giant", 15, 10, 3),
                        new Follow(1, range: 9),
                        new Wander(0.5)
                        ),
                    new Shoot(10)
                    ),
                new ItemLoot("Magic Potion", 0.05)
            )
            .Init("Oasis Soldier",
                new State(
                    new Prioritize(
                        new Protect(0.5, "Oasis Giant", 15, 11, 3),
                        new Follow(1, range: 7),
                        new Wander(0.5)
                        ),
                    new Shoot(10, predictive: 0.5)
                    ),
                new ItemLoot("Health Potion", 0.05)
            )
            .Init("Oasis Creature",
                new State(
                    new Prioritize(
                        new Protect(0.5, "Oasis Giant", 15, 12, 3),
                        new Follow(1, range: 5),
                        new Wander(0.5)
                        ),
                    new Shoot(10, coolDown: 400)
                    ),
                new ItemLoot("Health Potion", 0.05)
            )
            .Init("Oasis Monster",
                new State(
                    new Prioritize(
                        new Protect(0.5, "Oasis Giant", 15, 13, 3),
                        new Follow(1, range: 3),
                        new Wander(0.5)
                        ),
                    new Shoot(10, predictive: 0.5)
                    ),
                new ItemLoot("Magic Potion", 0.05)
            )
            ;
    }
}