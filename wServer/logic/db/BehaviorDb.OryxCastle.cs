using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ OryxCastle = () => Behav()
            .Init("Oryx Stone Guardian Right",
                new State(
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new PlayerWithinTransition(2, "Order")
                    ),
                    new State("Order",
                        new Order(10, "Oryx Stone Guardian Left", "Start"),
                        new TimedTransition(0, "Start")
                    ),
                    new State("Start",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xC0C0C0, 0.5, 3),
                        new TimedTransition(1500, "Lets go")
                    ),
                    new State("Together is better",
                        new EntityNotExistsTransition("Oryx Stone Guardian Left", 100, "Forever Alone"),
                        new State("Lets go",
                            new TimedTransition(10000, "Circle"),
                            new State("Imma Follow",
                                new Follow(1, 2, 0.3),
                                new Shoot(5, 5, shootAngle: 5, coolDown: 1000),
                                new TimedTransition(5000, "Imma chill")
                            ),
                            new State("Imma chill",
                                new Prioritize(
                                    new StayCloseToSpawn(0.5, 3),
                                    new Wander(0.5)
                                ),
                                new Shoot(0, 10, projectileIndex: 2, fixedAngle: 0, coolDown: 1000),
                                new TimedTransition(5000, "Imma Follow")
                            )
                        ),
                        new State("Circle",
                            new State("Prepare",
                                new MoveTo(127.5f, 39.5f, once: true, isMapPosition: true),
                                new EntityExistsTransition("Oryx Stone Guardian Left", 1, "Prepare2")
                            ),
                            new State("Prepare2",
                                new MoveTo(130.5f, 39.5f, once: true, isMapPosition: true),
                                new TimedTransition(1000, "PrepareEnd")
                            ),
                            new State("PrepareEnd",
                                new Orbit(1, 5, target: "Oryx Guardian TaskMaster"),
                                new State("cpe_1",
                                    new Shoot(0, 2, fixedAngle: 0, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_2")
                                ),
                                new State("cpe_2",
                                    new Shoot(0, 2, fixedAngle: 36, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_3")
                                ),
                                new State("cpe_3",
                                    new Shoot(0, 2, fixedAngle: 72, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_4")
                                ),
                                new State("cpe_4",
                                    new Shoot(0, 2, fixedAngle: 108, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_5")
                                ),
                                new State("cpe_5",
                                    new Shoot(0, 2, fixedAngle: 144, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_6")
                                ),
                                new State("cpe_6",
                                    new Shoot(0, 2, fixedAngle: 180, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_7")
                                ),
                                new State("cpe_7",
                                    new Shoot(0, 2, fixedAngle: 216, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_8")
                                ),
                                new State("cpe_8",
                                    new Shoot(0, 2, fixedAngle: 252, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_9")
                                ),
                                new State("cpe_9",
                                    new Shoot(0, 2, fixedAngle: 288, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_10")
                                ),
                                new State("cpe_10",
                                    new Shoot(0, 2, fixedAngle: 324, projectileIndex: 1),
                                    new TimedTransition(200, "checkEntities")
                                ),
                                new State("checkEntities",
                                    new PlayerWithinTransition(3, "cpe_Imma Follow"),
                                    new NoPlayerWithinTransition(3, "cpe_Imma chill")
                                ),
                                new State("cpe_x",
                                    new TimedTransition(5000, "Move Sideways"),
                                    new State("cpe_Imma Follow",
                                        new Follow(1, 3, 0.3),
                                        new Shoot(5, 5, coolDown: 1000),
                                        new TimedTransition(2500, "cpe_Imma chill")
                                    ),
                                    new State("cpe_Imma chill",
                                        new Prioritize(
                                            new StayCloseToSpawn(0.5, 3),
                                            new Wander(0.5)
                                        ),
                                        new Shoot(0, 10, projectileIndex: 2, fixedAngle: 0, coolDown: 1000),
                                        new TimedTransition(2500, "cpe_Imma Follow")
                                    )
                                )
                            )
                        ),
                        new State("Move Sideways",
                            new State("msw_prepare",
                                new MoveTo(141.5f, 39.5f, once: true, isMapPosition: true),
                                new TimedTransition(1500, "msw_shoot")
                            ),
                            new State("msw_shoot",
                                new Shoot(0, 2, fixedAngle: 90, coolDownOffset: 0),
                                new Shoot(0, 2, fixedAngle: 85.5, coolDownOffset: 100),
                                new Shoot(0, 2, fixedAngle: 81, coolDownOffset: 200),
                                new Shoot(0, 2, fixedAngle: 76.5, coolDownOffset: 300),
                                new Shoot(0, 2, fixedAngle: 72, coolDownOffset: 400),
                                new Shoot(0, 2, fixedAngle: 67.5, coolDownOffset: 500),
                                new Shoot(0, 2, fixedAngle: 63, coolDownOffset: 600),
                                new Shoot(0, 2, fixedAngle: 58.5, coolDownOffset: 700),
                                new Shoot(0, 2, fixedAngle: 54, coolDownOffset: 800),
                                new Shoot(0, 2, fixedAngle: 49.5, coolDownOffset: 900),
                                new Shoot(0, 2, fixedAngle: 45, coolDownOffset: 1000),
                                new Shoot(0, 2, fixedAngle: 40.5, coolDownOffset: 1100),
                                new Shoot(0, 2, fixedAngle: 36, coolDownOffset: 1200),
                                new Shoot(0, 2, fixedAngle: 31.5, coolDownOffset: 1300),
                                new Shoot(0, 2, fixedAngle: 27, coolDownOffset: 1400),
                                new Shoot(0, 2, fixedAngle: 22.5, coolDownOffset: 1500),
                                new Shoot(0, 2, fixedAngle: 18, coolDownOffset: 1600),
                                new Shoot(0, 2, fixedAngle: 13.5, coolDownOffset: 1700),
                                new Shoot(0, 2, fixedAngle: 9, coolDownOffset: 1800),
                                new Shoot(0, 2, fixedAngle: 4.5, coolDownOffset: 1900)
                            )
                        )
                    ),
                    new State("Forever Alone")
                )
            )
            .Init("Oryx Stone Guardian Left",
                new State(
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new PlayerWithinTransition(2, "Order")
                    ),
                    new State("Order",
                        new Order(10, "Oryx Stone Guardian Right", "Start"),
                        new TimedTransition(0, "Start")
                    ),
                    new State("Start",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xC0C0C0, 0.5, 3),
                        new TimedTransition(1500, "Together is better")
                    ),
                    new State("Together is better",
                        new EntityNotExistsTransition("Oryx Stone Guardian Right", 100, "Forever Alone"),
                        new State("Lets go",
                            new TimedTransition(10000, "Circle"),
                            new State("Imma Follow",
                                new Follow(1, 2, 0.3),
                                new Shoot(5, 5, shootAngle: 5, coolDown: 1000),
                                new TimedTransition(5000, "Imma chill")
                            ),
                            new State("Imma chill",
                                new Prioritize(
                                    new StayCloseToSpawn(0.5, 3),
                                    new Wander(0.5)
                                ),
                                new Shoot(0, 10, projectileIndex: 2, fixedAngle: 0, coolDown: 1000),
                                new TimedTransition(5000, "Imma Follow")
                            )
                        ),
                        new State("Circle",
                            new State("Prepare",
                                new MoveTo(127.5f, 39.5f, once: true, isMapPosition: true),
                                new EntityExistsTransition("Oryx Stone Guardian Right", 1, "Prepare2")
                            ),
                            new State("Prepare2",
                                new MoveTo(124.5f, 39.5f, once: true, isMapPosition: true),
                                new TimedTransition(1000, "PrepareEnd")
                            ),
                            new State("PrepareEnd",
                                new Orbit(1, 5, target: "Oryx Guardian TaskMaster"),
                                new State("cpe_1",
                                    new Shoot(0, 2, fixedAngle: 0, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_2")
                                ),
                                new State("cpe_2",
                                    new Shoot(0, 2, fixedAngle: 36, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_3")
                                ),
                                new State("cpe_3",
                                    new Shoot(0, 2, fixedAngle: 72, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_4")
                                ),
                                new State("cpe_4",
                                    new Shoot(0, 2, fixedAngle: 108, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_5")
                                ),
                                new State("cpe_5",
                                    new Shoot(0, 2, fixedAngle: 144, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_6")
                                ),
                                new State("cpe_6",
                                    new Shoot(0, 2, fixedAngle: 180, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_7")
                                ),
                                new State("cpe_7",
                                    new Shoot(0, 2, fixedAngle: 216, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_8")
                                ),
                                new State("cpe_8",
                                    new Shoot(0, 2, fixedAngle: 252, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_9")
                                ),
                                new State("cpe_9",
                                    new Shoot(0, 2, fixedAngle: 288, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_10")
                                ),
                                new State("cpe_10",
                                    new Shoot(0, 2, fixedAngle: 324, projectileIndex: 1),
                                    new TimedTransition(200, "checkEntities")
                                ),
                                new State("checkEntities",
                                    new PlayerWithinTransition(3, "cpe_Imma Follow"),
                                    new NoPlayerWithinTransition(3, "cpe_Imma chill")
                                ),
                                new State("cpe_x",
                                    new TimedTransition(5000, "Move Sideways"),
                                    new State("cpe_Imma Follow",
                                        new Follow(1, 3, 0.3),
                                        new Shoot(5, 5, coolDown: 1000),
                                        new TimedTransition(2500, "cpe_Imma chill")
                                    ),
                                    new State("cpe_Imma chill",
                                        new Prioritize(
                                            new StayCloseToSpawn(0.5, 3),
                                            new Wander(0.5)
                                        ),
                                        new Shoot(0, 10, projectileIndex: 2, fixedAngle: 0, coolDown: 1000),
                                        new TimedTransition(2500, "cpe_Imma Follow")
                                    )
                                )
                            )
                        ),
                        new State("Move Sideways",
                            new State("msw_prepare",
                                new MoveTo(113.5f, 39.5f, once: true, isMapPosition: true),
                                new TimedTransition(1500, "msw_shoot")
                            ),
                            new State("msw_shoot",
                                new Shoot(0, 2, fixedAngle: 90, coolDownOffset: 0),
                                new Shoot(0, 2, fixedAngle: 85.5, coolDownOffset: 100),
                                new Shoot(0, 2, fixedAngle: 81, coolDownOffset: 200),
                                new Shoot(0, 2, fixedAngle: 76.5, coolDownOffset: 300),
                                new Shoot(0, 2, fixedAngle: 72, coolDownOffset: 400),
                                new Shoot(0, 2, fixedAngle: 67.5, coolDownOffset: 500),
                                new Shoot(0, 2, fixedAngle: 63, coolDownOffset: 600),
                                new Shoot(0, 2, fixedAngle: 58.5, coolDownOffset: 700),
                                new Shoot(0, 2, fixedAngle: 54, coolDownOffset: 800),
                                new Shoot(0, 2, fixedAngle: 49.5, coolDownOffset: 900),
                                new Shoot(0, 2, fixedAngle: 45, coolDownOffset: 1000),
                                new Shoot(0, 2, fixedAngle: 40.5, coolDownOffset: 1100),
                                new Shoot(0, 2, fixedAngle: 36, coolDownOffset: 1200),
                                new Shoot(0, 2, fixedAngle: 31.5, coolDownOffset: 1300),
                                new Shoot(0, 2, fixedAngle: 27, coolDownOffset: 1400),
                                new Shoot(0, 2, fixedAngle: 22.5, coolDownOffset: 1500),
                                new Shoot(0, 2, fixedAngle: 18, coolDownOffset: 1600),
                                new Shoot(0, 2, fixedAngle: 13.5, coolDownOffset: 1700),
                                new Shoot(0, 2, fixedAngle: 9, coolDownOffset: 1800),
                                new Shoot(0, 2, fixedAngle: 4.5, coolDownOffset: 1900)
                            )
                        )
                    ),
                    new State("Forever Alone")
                )
            )
            .Init("Oryx Guardian TaskMaster",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Idle",
                        new EntitiesNotExistsTransition(100, "Death", "Oryx Stone Guardian Right", "Oryx Stone Guardian Left")
                    ),
                    new State("Death",
                        new Spawn("Oryx's Chamber Portal", 1, 1),
                        new Suicide()
                    )
                )
            )
            .Init("Oryx's Living Floor Fire Down",
                new State(
                    new State("Idle",
                        new PlayerWithinTransition(20, "Toss")
                    ),
                    new State("Toss",
                        new TossObject("Quiet Bomb", 10, coolDown: 1000),
                        new NoPlayerWithinTransition(21, "Idle"),
                        new PlayerWithinTransition(5, "Shoot and Toss")
                    ),
                    new State("Shoot and Toss",
                        new NoPlayerWithinTransition(21, "Idle"),
                        new NoPlayerWithinTransition(6, "Toss"),
                        new Shoot(0, 18, fixedAngle: 0, coolDown: new Cooldown(750, 250)),
                        new TossObject("Quiet Bomb", 10, coolDown: 1000)
                    )
                )
            )
            .Init("Quiet Bomb",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Idle",
                        new State("Tex1",
                            new TimedTransition(250, "Tex2")
                        ),
                        new State("Tex2",
                            new SetAltTexture(1),
                            new TimedTransition(250, "Tex3")
                        ),
                        new State("Tex3",
                            new SetAltTexture(0),
                            new TimedTransition(250, "Tex4")
                        ),
                        new State("Tex4",
                            new SetAltTexture(1),
                            new TimedTransition(250, "Explode")
                        )
                    ),
                    new State("Explode",
                        new SetAltTexture(0),
                        new Shoot(0, 18, fixedAngle: 0),
                        new Suicide()
                    )
                )
            );
    }
}
