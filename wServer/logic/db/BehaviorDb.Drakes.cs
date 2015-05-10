using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.logic.behaviors.Drakes;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Drakes = () => Behav()
            .Init("White Drake",
                new State(
                    new DrakeFollow(),
                    new WhiteDrakeAttack()
                )
            )
            .Init("Blue Drake",
                new State(
                    new DrakeFollow(),
                    new BlueDrakeAttack()
                )
            )
            .Init("Purple Drake",
                new State(
                    new DrakeFollow(),
                    new PurpleDrakeAttack()
                )
            )
            .Init("Orange Drake",
                new State(
                    new DrakeFollow(),
                    new OrangeDrakeAttack()
                )
            )
            .Init("Yellow Drake",
                new State(
                    new DrakeFollow(),
                    new YellowDrakeAttack()
                )
            )
            .Init("Green Drake",
                new State(
                    new DrakeFollow(),
                    new GreenDrakeAttack()
                )
            );
    }
}
