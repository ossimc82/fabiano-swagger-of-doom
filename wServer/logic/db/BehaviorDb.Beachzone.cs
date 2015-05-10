using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Beachzone = () => Behav()
            .Init("Masked Party God",
                new State(
                    new Heal(1, "Self", 5000),
                    new State("1",
                        new Taunt(true, "Oh no, Mixcoatl is my brother, I prefer partying to fighting."),
                        new SetAltTexture(1),
                        new TimedTransition(500, "2")
                    ),
                    new State("2",
                        new Taunt(true, "Lets have a fun-time in the sun-shine!"),
                        new SetAltTexture(2),
                        new TimedTransition(500, "3")
                    ),
                    new State("3",
                        new Taunt(true, "Nothing like relaxin' on the beach."),
                        new SetAltTexture(3),
                        new TimedTransition(500, "4")
                    ),
                    new State("4",
                        new Taunt(true, "Chillin' is the name of the game!"),
                        new SetAltTexture(1),
                        new TimedTransition(500, "5")
                    ),
                    new State("5",
                        new Taunt(true, "I hope you're having a good time!"),
                        new SetAltTexture(2),
                        new TimedTransition(500, "6")
                    ),
                    new State("6",
                        new Taunt(true, "How do you like my shades?"),
                        new SetAltTexture(3),
                        new TimedTransition(500, "7")
                    ),
                    new State("7",
                        new Taunt(true, "EVERYBODY BOOGEY!"),
                        new SetAltTexture(1),
                        new TimedTransition(500, "8")
                    ),
                    new State("8",
                        new Taunt(true, "What a beautiful day!"),
                        new SetAltTexture(2),
                        new TimedTransition(500, "9")
                    ),
                    new State("9",
                        new Taunt(true, "Whoa there!"),
                        new SetAltTexture(3),
                        new TimedTransition(500, "10")
                    ),
                    new State("10",
                        new Taunt(true, "Oh SNAP!"),
                        new SetAltTexture(1),
                        new TimedTransition(500, "11")
                    ),
                    new State("11",
                        new Taunt(true, "Ho!"),
                        new SetAltTexture(2),
                        new TimedTransition(500, "end")
                    ),
                    new State("end",
                        new SetAltTexture(3),
                        new TimedTransition(500, "1")
                    )
                ),
                new Threshold(0.1,
                    new ItemLoot("Blue Paradise", 0.2),
                    new ItemLoot("Pink Passion Breeze", 0.2),
                    new ItemLoot("Bahama Sunrise", 0.2),
                    new ItemLoot("Lime Jungle Bay", 0.2)
                )
            );
    }
}
