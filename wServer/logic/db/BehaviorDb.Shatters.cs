using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using wServer.realm;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Shatters = () => Behav()
            .Init("shtrs Stone Paladin",
                new State(
                    new State("Idle",
                        new Prioritize(
                            new Wander(0.8)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Reproduce(densityMax: 4),
                        new PlayerWithinTransition(8, "Attacking")
                        ),
                    new State("Attacking",
                        new State("Bullet",
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 90, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 100, coolDownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 110, coolDownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 120, coolDownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 130, coolDownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 140, coolDownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 150, coolDownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 160, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 170, coolDownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 180, coolDownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 180, coolDownOffset: 2000, shootAngle: 45),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 180, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 170, coolDownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 160, coolDownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 150, coolDownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 140, coolDownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 130, coolDownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 120, coolDownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 110, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 100, coolDownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 90, coolDownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, fixedAngle: 90, coolDownOffset: 2000, shootAngle: 22.5),
                            new TimedTransition(2000, "Wait")
                            ),
                        new State("Wait",
                            new Follow(0.4, range: 2),
                            new Flash(0xff00ff00, 0.1, 20),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(2000, "Bullet")
                            ),
                        new NoPlayerWithinTransition(13, "Idle")
                        )
                    )
            )
            .Init("shtrs Wooden Gate",
                new State(
                    new State("Idle",
                        new EntityNotExistsTransition("shtrs Abandoned Switch 1", 10, "Despawn")
                        ),
                    new State("Despawn",
                        new Decay(0)
                        ))
            )
            .Init("shtrs Stone Knight",
            new State(
                new State("Follow",
                        new Follow(0.6, 10, 5),
                        new PlayerWithinTransition(5, "Charge")
                    ),
                    new State("Charge",
                        new TimedTransition(2000, "Follow"),
                        new Charge(4, 5),
                        new Shoot(5, 6, projectileIndex:0, coolDown:3000)
                        )
                    )
            )
            .Init("shtrs Ice Mage",
                new State("Main",
                    new Follow(0.5, range: 1),
                    new Shoot(10, 5, 10, projectileIndex: 0, coolDown: 1500)
                    ))
            .Init("shtrs Ice Adept",
            new State(
                new State("Main",
                    new TimedTransition(7000, "Throw"),
                    new Follow(0.8, range: 1),
                    new Shoot(10, 1, projectileIndex: 0, coolDown: 100, predictive: 1),
                    new Shoot(10, 3, projectileIndex: 1, shootAngle:10, coolDown: 4000, predictive: 1)
                ),
                new State("Throw",
                    new TossObject("shtrs Ice Portal", 5, coolDown: 8000, coolDownOffset: 7000, randomToss: true),
                    new TimedTransition(1000, "Main")
                )
                  ))

            .Init("shtrs MagiGenerators",
            new State(
                new State("Main",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(15, 10, coolDown:1000),
                    new Shoot(15, 1, projectileIndex:1, coolDown:2500)
                ),
                new State("Hide",
                    new SetAltTexture(1),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable)
                    ),
                new State("Despawn",
                    new Decay()
                    )
                  ))
            .Init("shtrs Ice Portal",
                new State(
                    new State("Idle",
                        new TimedTransition(2500, "Spin")
                    ),
                    new State("Spin",
                        new TimedTransition(2000, "Pause"),
                        new State("Quadforce1",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 0, coolDown: 300),
                            new TimedTransition(150, "Quadforce2")
                        ),
                        new State("Quadforce2",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 15, coolDown: 300),
                            new TimedTransition(150, "Quadforce3")
                        ),
                        new State("Quadforce3",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 30, coolDown: 300),
                            new TimedTransition(150, "Quadforce4")
                        ),
                        new State("Quadforce4",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 45, coolDown: 300),
                            new TimedTransition(150, "Quadforce5")
                        ),
                        new State("Quadforce5",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 60, coolDown: 300),
                            new TimedTransition(150, "Quadforce6")
                        ),
                        new State("Quadforce6",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 75, coolDown: 300),
                            new TimedTransition(150, "Quadforce7")
                        ),
                        new State("Quadforce7",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 90, coolDown: 300),
                            new TimedTransition(150, "Quadforce8")
                        ),
                        new State("Quadforce8",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 105, coolDown: 300),
                            new TimedTransition(150, "Quadforce1")
                        )
                    ),
                    new State("Pause",
                       new TimedTransition(5000, "Spin")
                    )
                )
            )
            .Init("shtrs Fire Portal",
                new State(
                    new State("Idle",
                        new TimedTransition(2500, "Spin")
                    ),
                    new State("Spin",
                        new TimedTransition(2000, "Pause"),
                        new State("Quadforce1",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 0, coolDown: 300),
                            new TimedTransition(150, "Quadforce2")
                        ),
                        new State("Quadforce2",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 15, coolDown: 300),
                            new TimedTransition(150, "Quadforce3")
                        ),
                        new State("Quadforce3",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 30, coolDown: 300),
                            new TimedTransition(150, "Quadforce4")
                        ),
                        new State("Quadforce4",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 45, coolDown: 300),
                            new TimedTransition(150, "Quadforce5")
                        ),
                        new State("Quadforce5",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 60, coolDown: 300),
                            new TimedTransition(150, "Quadforce6")
                        ),
                        new State("Quadforce6",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 75, coolDown: 300),
                            new TimedTransition(150, "Quadforce7")
                        ),
                        new State("Quadforce7",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 90, coolDown: 300),
                            new TimedTransition(150, "Quadforce8")
                        ),
                        new State("Quadforce8",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 105, coolDown: 300),
                            new TimedTransition(150, "Quadforce1")
                        )
                    ),
                    new State("Pause",
                       new TimedTransition(5000, "Spin")
                    )
                )
            )
            .Init("shtrs Bridge Sentinel",
                new State(
                    new SetLootState("obelisk"),
                    new CopyLootState("shtrs encounterchestspawner", 20),
                    new HpLessTransition(0.1, "Death"),
                    new CopyDamageOnDeath("shtrs Encounter Chest"),
                    new State("Idle",
                        new PlayerWithinTransition(3, "Close Bridge")
                    ),
                    //not correct
                    new State("Close Bridge",
                        new CallWorldMethod("Shatters", "CloseBridge1", null),
                        new TimedTransition(35000, "Start")
                    ),
                    //new State("Start the Start",
                    //    new Order(10, "shtrs Bridge Obelisk A", "Talk"),
                    //    new Order(10, "shtrs Bridge Obelisk B", "Talk"),
                    //    new Order(10, "shtrs Bridge Obelisk C", "Talk"),
                    //    new Order(10, "shtrs Bridge Obelisk D", "Talk")
                    //),
                    new State("Start",
                        new Shoot(15, 10, 15, 5, 90, coolDown: 1000),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("shtrs Bridge Titanum", 10, "Wake")
                        ),
                        new State("Wake",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("Who has woken me...? Leave this place."),
                        new Timed(2100, new Shoot(15, 15, 12, projectileIndex: 0, fixedAngle: 90, coolDown: 700, coolDownOffset: 3000)),
                        new TimedTransition(8000, "Swirl Shot")
                        ),
                        new State("Swirl Shot",
                            new Taunt("Go."),
                            new TimedTransition(10000, "Blobomb"),
                            new State("Swirl1",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 12, coolDown: 200),
                            new TimedTransition(50, "Swirl2")
                            ),
                            new State("Swirl2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 24, coolDown: 200),
                            new TimedTransition(50, "Swirl3")
                            ),
                            new State("Swirl3",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 36, coolDown: 200),
                            new TimedTransition(50, "Swirl4")
                            ),
                            new State("Swirl4",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 48, coolDown: 200),
                            new TimedTransition(50, "Swirl5")
                            ),
                            new State("Swirl5",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 60, coolDown: 200),
                            new TimedTransition(50, "Swirl6")
                            ),
                            new State("Swirl6",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 72, coolDown: 200),
                            new TimedTransition(50, "Swirl7")
                            ),
                            new State("Swirl7",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 84, coolDown: 200),
                            new TimedTransition(50, "Swirl8")
                            ),
                            new State("Swirl8",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 96, coolDown: 200),
                            new TimedTransition(50, "Swirl9")
                            ),
                            new State("Swirl9",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 108, coolDown: 200),
                            new TimedTransition(50, "Swirl10")
                            ),
                            new State("Swirl10",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 120, coolDown: 200),
                            new TimedTransition(50, "Swirl11")
                            ),
                            new State("Swirl11",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 132, coolDown: 200),
                            new TimedTransition(50, "Swirl12")
                            ),
                            new State("Swirl12",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 144, coolDown: 200),
                            new TimedTransition(50, "Swirl13")
                            ),
                            new State("Swirl13",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 156, coolDown: 200),

                            new TimedTransition(50, "Swirl14")
                            ),
                            new State("Swirl14",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 168, coolDown: 200),
                            new TimedTransition(50, "Swirl15")
                            ),
                            new State("Swirl15",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 168, coolDown: 200),
                            new TimedTransition(50, "Swirl16")
                            ),
                            new State("Swirl16",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 156, coolDown: 200),

                            new TimedTransition(50, "Swirl17")
                            ),
                            new State("Swirl17",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 144, coolDown: 200),
                            new TimedTransition(50, "Swirl18")
                            ),
                            new State("Swirl18",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 132, coolDown: 200),
                            new TimedTransition(50, "Swirl19")
                            ),
                            new State("Swirl19",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 120, coolDown: 200),
                            new TimedTransition(50, "Swirl20")
                            ),
                            new State("Swirl20",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 108, coolDown: 200),
                            new TimedTransition(50, "Swirl21")
                            ),
                            new State("Swirl21",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 96, coolDown: 200),
                            new TimedTransition(50, "Swirl22")
                            ),
                            new State("Swirl22",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 84, coolDown: 200),
                            new TimedTransition(50, "Swirl23")
                            ),
                            new State("Swirl23",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 72, coolDown: 200),
                            new TimedTransition(50, "Swirl24")
                            ),
                            new State("Swirl24",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 60, coolDown: 200),
                            new TimedTransition(50, "Swirl25")
                            ),
                            new State("Swirl25",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 48, coolDown: 200),
                            new TimedTransition(50, "Swirl26")
                            ),
                            new State("Swirl26",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 36, coolDown: 200),
                            new TimedTransition(50, "Swirl27")
                            ),
                            new State("Swirl27",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 24, coolDown: 200),
                            new TimedTransition(50, "Swirl28")
                            ),
                            new State("Swirl28",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 12, coolDown: 200),
                            new TimedTransition(50, "Swirl1")
                            )
                            ),
                            new State("Blobomb",
                            new Taunt("You live still? DO NOT TEMPT FATE!"),
                            new Taunt("CONSUME!"),
                            new Order(20, "shtrs blobomb maker", "Spawn"),
                            new EntityNotExistsTransition("shtrs Blobomb", 30, "SwirlAndShoot")
                                ),
                                new State("SwirlAndShoot",
                                    new TimedTransition(10000, "Blobomb"),
                                    new Taunt("FOOLS! YOU DO NOT UNDERSTAND!"),
                                    new ChangeSize(20, 130),
                            new Shoot(15, 15, 11, projectileIndex: 0, fixedAngle: 90, coolDown: 700, coolDownOffset: 700),
                                    new State("Swirl1_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 12, coolDown: 200),
                            new TimedTransition(50, "Swirl2_2")
                            ),
                            new State("Swirl2_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 24, coolDown: 200),
                            new TimedTransition(50, "Swirl3_2")
                            ),
                            new State("Swirl3_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 36, coolDown: 200),
                            new TimedTransition(50, "Swirl4_2")
                            ),
                            new State("Swirl4_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 48, coolDown: 200),
                            new TimedTransition(50, "Swirl5_2")
                            ),
                            new State("Swirl5_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 60, coolDown: 200),
                            new TimedTransition(50, "Swirl6_2")
                            ),
                            new State("Swirl6_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 72, coolDown: 200),
                            new TimedTransition(50, "Swirl7_2")
                            ),
                            new State("Swirl7_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 84, coolDown: 200),
                            new TimedTransition(50, "Swirl8_2")
                            ),
                            new State("Swirl8_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 96, coolDown: 200),
                            new TimedTransition(50, "Swirl9_2")
                            ),
                            new State("Swirl9_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 108, coolDown: 200),
                            new TimedTransition(50, "Swirl10_2")
                            ),
                            new State("Swirl10_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 120, coolDown: 200),
                            new TimedTransition(50, "Swirl11_2")
                            ),
                            new State("Swirl11_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 132, coolDown: 200),
                            new TimedTransition(50, "Swirl12_2")
                            ),
                            new State("Swirl12_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 144, coolDown: 200),
                            new TimedTransition(50, "Swirl13_2")
                            ),
                            new State("Swirl13_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 156, coolDown: 200),
                            new TimedTransition(50, "Swirl14_2")
                            ),
                            new State("Swirl14_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 168, coolDown: 200),
                            new TimedTransition(50, "Swirl15_2")
                            ),
                            new State("Swirl15_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 168, coolDown: 200),
                            new TimedTransition(50, "Swirl16_2")
                            ),
                            new State("Swirl16_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 156, coolDown: 200),
                            new TimedTransition(50, "Swirl17_2")
                            ),
                            new State("Swirl17_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 144, coolDown: 200),
                            new TimedTransition(50, "Swirl18_2")
                            ),
                            new State("Swirl18_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 132, coolDown: 200),
                            new TimedTransition(50, "Swirl19_2")
                            ),
                            new State("Swirl19_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 120, coolDown: 200),
                            new TimedTransition(50, "Swirl20_2")
                            ),
                            new State("Swirl20_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 108, coolDown: 200),
                            new TimedTransition(50, "Swirl21_2")
                            ),
                            new State("Swirl21_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 96, coolDown: 200),
                            new TimedTransition(50, "Swirl22_2")
                            ),
                            new State("Swirl22_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 84, coolDown: 200),
                            new TimedTransition(50, "Swirl23_2")
                            ),
                            new State("Swirl23_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 72, coolDown: 200),
                            new TimedTransition(50, "Swirl24_2")
                            ),
                            new State("Swirl24_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 60, coolDown: 200),
                            new TimedTransition(50, "Swirl25_2")
                            ),
                            new State("Swirl25_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 48, coolDown: 200),
                            new TimedTransition(50, "Swirl26_2")
                            ),
                            new State("Swirl26_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 36, coolDown: 200),
                            new TimedTransition(50, "Swirl27_2")
                            ),
                            new State("Swirl27_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 24, coolDown: 200),
                            new TimedTransition(50, "Swirl28_2")
                            ),
                            new State("Swirl28_2",
                            new Shoot(15, 1, projectileIndex: 0, fixedAngle: 12, coolDown: 200),
                            new TimedTransition(50, "Swirl1_2")
                            )
                                    ),
                                    new State("Death",
                                        new CallWorldMethod("Shatters", "OpenBridge1Behind"),
                                        new ChangeSize(20, 130),
                                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                        new Taunt("I tried to protect you...I have failed. You release a great evil upon this realm..."),
                                        new Shoot(15, 12, projectileIndex: 0, coolDown: 100000, coolDownOffset: 3000),
                                        new CopyDamageOnDeath("shtrs Encounter Chest"),
                                        new Order(10, "shtrs encounterchestspawner", "Spawn"),
                                        new Suicide()
                                        )
                        )
            )

            .Init("shtrs Twilight Archmage",
                new State(
                    new SetLootState("archmage"),
                    new CopyLootState("shtrs encounterchestspawner", 20),
                    new HpLessTransition(.1, "Death"),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition2("shtrs Glassier Archmage", "shtrs Archmage of Flame", 15, "Wake")
                    ),
                    new State("Wake",
                        new State("Comment1",
                            new SetAltTexture(1),
                            new Taunt("Ha...ha........hahahahahaha! You will make a fine sacrifice!"),
                            new TimedTransition(3000, "Comment2")
                        ),
                        new SetAltTexture(1),
                        new State("Comment2",
                            new Taunt("You will find that it was...unwise...to wake me."),
                            new TimedTransition(1000, "Comment3")
                        ),
                        new State("Comment3",
                            new SetAltTexture(1),
                            new Taunt("Let us see what can conjure up!"),
                            new TimedTransition(1000, "Comment4")
                        ),
                        new State("Comment4",
                            new SetAltTexture(1),
                            new Taunt("I will freeze the life from you!"),
                            new TimedTransition(1000, "Blue1")
                        )
                    ),
                    new State("Blue1",
                        new SetAltTexture(2),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TossObject("shtrs Ice Portal", 4, 90, 100000000),
                        new Spawn("shtrs Ice Shield", 1, 1, 1000000000),
                        new TimedTransition(2000, "checkSphere")
                    ),
                    new State("checkSphere",
                        new EntityNotExistsTransition("shtrs Ice Shield", 15, "Spawn Birds")
                    ),
                    new State("Spawn Birds",
                        new Taunt("You leave me no choice...Inferno! Blizzard!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new InvisiToss("shtrs Inferno", 3, 0, 1000000000, 7000),
                        new InvisiToss("shtrs Blizzard", 3, 180, 1000000000, 7000),
                        new Order(15, "shtrs MagiGenerators", "Hide"),
                        new TimedTransition(8000, "wait")
                    ),
                    new State("wait",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition2("shtrs Inferno", "shtrs Blizzard", 15, "Change")
                    ),
                    new State("Change",
                        new SetAltTexture(2),
                        new ChangeSize(100, 200),
                        new Taunt("Your souls feed my King."),
                        new TimedTransition(3000, "Active 1")
                    ),
                    new State("Active 1",
                        new Taunt("Darkness give me strength!"),
                        new MoveTo(6, 0),
                        new Order(1, "shtrs MagiGenerators", "Despawn"),
                        new TimedTransition(4000, "Active2")
                    ),
                    new State("Active2",
                        new MoveTo(0, 4, 1.5),
                        new Order(1, "shtrs MagiGenerators", "Despawn"),
                        new Taunt("THE POWER...IT CONSUMES ME!"),
                        new Shoot(15, 20, projectileIndex:2, coolDown:100000000, coolDownOffset:5000),
                        new Shoot(15, 20, projectileIndex: 3, coolDown: 100000000, coolDownOffset: 5000),
                        new Shoot(15, 20, projectileIndex: 4, coolDown: 100000000, coolDownOffset: 5100),
                        new Shoot(15, 20, projectileIndex: 2, coolDown: 100000000, coolDownOffset: 5200),
                        new Shoot(15, 20, projectileIndex: 5, coolDown: 100000000, coolDownOffset: 5350),
                        new Shoot(15, 20, projectileIndex: 6, coolDown: 100000000, coolDownOffset: 5400),
                        new TimedTransition(6000, "Active3")
                    ),
                    new State("Active3",
                        new MoveTo(8, 0, 1.5),
                        new Order(1, "shtrs MagiGenerators", "Despawn"),
                        new Taunt("THE POWER...IT CONSUMES ME!"),
                        new Shoot(15, 20, projectileIndex: 2, coolDown: 100000000, coolDownOffset: 5000),
                        new Shoot(15, 20, projectileIndex: 3, coolDown: 100000000, coolDownOffset: 5000),
                        new Shoot(15, 20, projectileIndex: 4, coolDown: 100000000, coolDownOffset: 5100),
                        new Shoot(15, 20, projectileIndex: 2, coolDown: 100000000, coolDownOffset: 5200),
                        new Shoot(15, 20, projectileIndex: 5, coolDown: 100000000, coolDownOffset: 5350),
                        new Shoot(15, 20, projectileIndex: 6, coolDown: 100000000, coolDownOffset: 5400)
                    ),
                    new State("Death",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("I...will........retuuurr...n...n....."),
                        new Shoot(15, 12, projectileIndex:5, coolDown:1000000, coolDownOffset:30000),
                        new CopyDamageOnDeath("shtrs Encounter Chest"),
                        new Order(10, "shtrs encounterchestspawner", "Spawn"),
                        new Suicide()
                    )
                )
            )
            .Init("shtrs The Forgotten King",
                new State(
                    new SetLootState("forgottenKing"),
                    new CopyLootState("shtrs encounterchestspawner", 20),

                    new State("Idle",
                        new HpLessTransition(0.1, "Death")
                    ),

                    new State("Death",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new CopyDamageOnDeath("shtrs Encounter Chest"),
                        new Order(10, "shtrs encounterchestspawner", "Spawn"),
                        new Suicide()
                    )
                )
            )
            .Init("shtrs blobomb maker",
                new State(
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                    new State("Spawn",
                        new Spawn("shtrs Blobomb", coolDown: 1000),
                        new TimedTransition(6000, "Idle")
                     ),
                    new State("blobombs avatar",
                        new Spawn("shtrs Blobomb", maxChildren: 1, coolDown: 2000)
                        ),
                    new State("AVATAR HELP!",
                        new Spawn("shtrs Blobomb", maxChildren: 1, coolDown: 2000),
                        new TimedTransition(2000, "Idle")
                    )
                )
            )
            .Init("shtrs encounterchestspawner",
                new State(
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                    new State("Spawn",
                        new Spawn("shtrs Encounter Chest", 1, 1),
                        new CopyLootState("shtrs Encounter Chest", 10),
                        new TimedTransition(5000, "Idle")
                    )
                )
            )

            .Init("shtrs Encounter Chest",
                new State(
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "Bracer")
                    ),
                    new State("Bracer")
                ),
                new Threshold(0.1,
                    new TierLoot(11, ItemType.Weapon, 0.06),
                    new TierLoot(12, ItemType.Weapon, 0.05),
                    new TierLoot(6, ItemType.Ability, 0.05),
                    new TierLoot(12, ItemType.Armor, 0.06),
                    new TierLoot(13, ItemType.Armor, 0.05),
                    new TierLoot(6, ItemType.Ring, 0.06)
                ),
                new LootState("obelisk",
                    new Threshold(0.32,
                        new ItemLoot("Potion of Attack", 1),
                        new ItemLoot("Potion of Defense", 0.5)
                    ),
                    new Threshold(0.1,
                        new ItemLoot("Bracer of the Guardian", 0.005)
                    )
                ),
                new LootState("archmage",
                    new Threshold(0.32,
                        new ItemLoot("Potion of Mana", 1)
                    ),
                    new Threshold(0.1,
                        new ItemLoot("The Twilight Gemstone", 0.005)
                    )
                ),
                new LootState("forgottenKing",
                    new Threshold(0.32,
                        new ItemLoot("Potion of Life", 1)
                    ),
                    new Threshold(0.1,
                        new ItemLoot("The Forgotten Crown", 0.005)
                    )
                )
            )
            
            .Init("shtrs Inferno",
                new State(
                    new Follow(1, range: 1, coolDown: 1000),
                    new Orbit(1, 4, 15, "shtrs Blizzard")
                )
            )

            .Init("shtrs Blizzard",
                new State(
                    new Follow(1, range: 1, coolDown: 1000),
                    new Orbit(1, 4, 15, "shtrs Inferno")
                )
            );
    }
}
