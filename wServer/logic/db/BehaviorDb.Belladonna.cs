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
        private const float fixedAngle_RingAttack2 = 22.5f;

        private _ Belladonna = () => Behav()
            .Init("vlntns Botany Bella",
                new State(
                    new TransformOnDeath("vlntns Loot Balloon Bella", returnToSpawn: true),
                    new CopyDamageOnDeath("vlntns Loot Balloon Bella"),
                    //new Taunt(new string[3]
                    //{
                    //    "Start",
                    //    "Ring_Attack",
                    //    "Ring_Attack2"
                    //},
                    //text: "You are nothing more than nutiment for my roots.", cooldown: 15000),

                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(3, "Start")
                    ),
                    new State("Start",
                        new Taunt("You are nothing more than nutiment for my roots."),
                        new Shoot(10, count: 4, shootAngle: (float)(180 / 4), predictive: 0.7, coolDown: 1000),
                        new Shoot(10, count: 2, shootAngle: (float)30, angleOffset: (float)45, projectileIndex: 3, coolDown: 1000),
                        new Shoot(10, count: 2, shootAngle: (float)30, angleOffset: (float)-45, projectileIndex: 3, coolDown: 1000),
                        new HpLessTransition(0.9, "Ring_Attack")
                    ),
                    new State("Ring_Attack",
                        new Prioritize(
                            new StayCloseToSpawn(0.5, 10),
                            new Wander(0.5)
                        ),
                        new Shoot(10, count: 12, projectileIndex: 1, coolDown: 5000),
                        new Shoot(10, count: 12, projectileIndex: 1, coolDown: 3000),
                        new HpLessTransition(0.7, "Ring_Attack2")
                    ),
                    new State("Ring_Attack2",
                        new State("Yo_man_I_show_you_what_I_can_do",
                            new Timed(600, new Shoot(10, count: 12)),
                            new TimedTransition(800, "Ring")
                        ),
                        new State("Ring",
                            new Shoot(0, count: 6, projectileIndex: 2, fixedAngle: 0),
                            new TimedTransition(250, "Ring1")
                        ),
                        new State("Ring1",
                            new Shoot(0, count: 6, projectileIndex: 2, fixedAngle: fixedAngle_RingAttack2),
                            new TimedTransition(250, "Ring2")
                        ),
                        new State("Ring2",
                            new Shoot(0, count: 6, projectileIndex: 2, fixedAngle: fixedAngle_RingAttack2 * 2),
                            new TimedTransition(250, "Ring3")
                        ),
                        new State("Ring3",
                            new Shoot(10, count: 6, projectileIndex: 2, fixedAngle: fixedAngle_RingAttack2 * 3),
                            new TimedTransition(250, "Ring4")
                        ),
                        new State("Ring4",
                            new Shoot(10, count: 6, projectileIndex: 2, fixedAngle: fixedAngle_RingAttack2 * 4),
                            new TimedTransition(250, "Ring5")
                        ),
                        new State("Ring5",
                            new Shoot(10, count: 6, projectileIndex: 2, fixedAngle: fixedAngle_RingAttack2 * 5),
                            new TimedTransition(250, "-Ring")
                        ),
                        new State("-Ring",
                            new Shoot(10, count: 6, projectileIndex: 2, fixedAngle: fixedAngle_RingAttack2 * 6),
                            new TimedTransition(250, "-Ring1")
                        ),
                        new State("-Ring1",
                            new Shoot(10, count: 6, projectileIndex: 2, fixedAngle: fixedAngle_RingAttack2 * 5),
                            new TimedTransition(250, "-Ring2")
                        ),
                        new State("-Ring2",
                            new Shoot(10, count: 6, projectileIndex: 2, fixedAngle: fixedAngle_RingAttack2 * 4),
                            new TimedTransition(250, "-Ring3")
                        ),
                        new State("-Ring3",
                            new Shoot(10, count: 6, projectileIndex: 2, fixedAngle: fixedAngle_RingAttack2 * 3),
                            new TimedTransition(250, "-Ring4")
                        ),
                        new State("-Ring4",
                            new Shoot(10, count: 6, projectileIndex: 2, fixedAngle: fixedAngle_RingAttack2 * 2),
                            new TimedTransition(250, "-Ring5")
                        ),
                        new State("-Ring5",
                            new Shoot(10, count: 6, projectileIndex: 2, fixedAngle: fixedAngle_RingAttack2),
                            new TimedTransition(250, "-Ring6")
                        ),
                        new State("-Ring6",
                            new Shoot(10, count: 6, projectileIndex: 2, fixedAngle: 0),
                            new Timed(200, new Shoot(10, count: 2, shootAngle: (float)30, defaultAngle: (float)45, projectileIndex: 3)),
                            new Timed(200, new Shoot(10, count: 2, shootAngle: (float)30, defaultAngle: (float)135, projectileIndex: 3)),
                            new TimedTransition(200, "StartTheFun")
                        )
                    ),
                    new State("StartTheFun",
                        new Taunt(true, 0, "You will be as food for my children!"),
                        new Flash(0xf0e68c, flashPeriod: 0.5, flashRepeats: 10),
                        new ReturnToSpawn(speed: 1),
                        new TimedTransition(5000, "RAGE_MODE")
                    ),
                    new State("RAGE_MODE",
                        new State("Everything_is_cool",
                            new State("spawn_minions",
                                new HpLessTransition(0.2, "OMG_HALP_ME_BUDS!!!!!!!!!!!!!!1111111oneoneoneeleven"),
                                new InvisiToss("vlntns Red", range: 9.9, angle: 15),
                                new InvisiToss("vlntns White", range: 6.6, angle: 30),
                                new InvisiToss("vlntns Yellow", range: 3.3, angle: 45),
                                new InvisiToss("vlntns White", range: 9.9, angle: 60),
                                new InvisiToss("vlntns Red", range: 6.6, angle: 75),
                                new InvisiToss("vlntns White", range: 3.3, angle: 90),
                                new InvisiToss("vlntns Yellow", range: 9.9, angle: 105),
                                new InvisiToss("vlntns White", range: 6.6, angle: 120),

                                new InvisiToss("vlntns Red", range: 3.3, angle: 135),
                                new InvisiToss("vlntns White", range: 9.9, angle: 150),
                                new InvisiToss("vlntns Yellow", range: 6.6, angle: 165),
                                new InvisiToss("vlntns White", range: 3.3, angle: 180),
                                new InvisiToss("vlntns Red", range: 9.9, angle: 195),
                                new InvisiToss("vlntns White", range: 6.6, angle: 210),
                                new InvisiToss("vlntns Yellow", range: 3.3, angle: 225),
                                new InvisiToss("vlntns White", range: 9.9, angle: 240),

                                new InvisiToss("vlntns Red", range: 6.6, angle: 255),
                                new InvisiToss("vlntns White", range: 3.3, angle: 270),
                                new InvisiToss("vlntns Yellow", range: 9.9, angle: 285),
                                new InvisiToss("vlntns White", range: 6.6, angle: 300),
                                new InvisiToss("vlntns Red", range: 3.3, angle: 315),
                                new InvisiToss("vlntns White", range: 9.9, angle: 330),
                                new InvisiToss("vlntns Yellow", range: 6.6, angle: 345),
                                new InvisiToss("vlntns White", range: 3.3, angle: 360),

                                new Spawn("vlntns Bella Buds", maxChildren: 5, initialSpawn: 1),
                                new TimedTransition(0, "shoot1")
                            ),
                            new State("shoot1",
                                new HpLessTransition(0.2, "OMG_HALP_ME_BUDS!!!!!!!!!!!!!!1111111oneoneoneeleven"),
                                new Shoot(0, projectileIndex: 1, fixedAngle: 45),
                                new Shoot(0, projectileIndex: 1, fixedAngle: 135),
                                new Shoot(0, projectileIndex: 1, fixedAngle: 225),
                                new Shoot(0, projectileIndex: 1, fixedAngle: 315),
                                new TimedTransition(800, "shoot2")
                            ),
                            new State("shoot2",
                                new HpLessTransition(0.2, "OMG_HALP_ME_BUDS!!!!!!!!!!!!!!1111111oneoneoneeleven"),
                                new Shoot(0, 2, shootAngle: 10, projectileIndex: 1, fixedAngle: 45),
                                new Shoot(0, 2, shootAngle: 10, projectileIndex: 1, fixedAngle: 135),
                                new Shoot(0, 2, shootAngle: 10, projectileIndex: 1, fixedAngle: 225),
                                new Shoot(0, 2, shootAngle: 10, projectileIndex: 1, fixedAngle: 315),
                                new TimedTransition(800, "shoot3")
                            ),
                            new State("shoot3",
                                new HpLessTransition(0.2, "OMG_HALP_ME_BUDS!!!!!!!!!!!!!!1111111oneoneoneeleven"),
                                new Shoot(0, 2, shootAngle: 15, projectileIndex: 1, fixedAngle: 45),
                                new Shoot(0, 2, shootAngle: 15, projectileIndex: 1, fixedAngle: 135),
                                new Shoot(0, 2, shootAngle: 15, projectileIndex: 1, fixedAngle: 225),
                                new Shoot(0, 2, shootAngle: 15, projectileIndex: 1, fixedAngle: 315),
                                new TimedTransition(800, "shoot1")
                            )
                        )
                    ),
                    new State("OMG_HALP_ME_BUDS!!!!!!!!!!!!!!1111111oneoneoneeleven",
                        new Order(100, "vlntns Bella Buds", "CUTIES_PROTECT_ME"),
                        new Taunt(1, true, "My children...Aid me..."),
                        new Shoot(10, count: 6, projectileIndex: 3),
                        new Shoot(10, count: 3, shootAngle: 15, predictive: 0.7, coolDown: 1000),
                        new State("shoot1",
                            new Shoot(0, projectileIndex: 3, fixedAngle: 45),
                            new Shoot(0, projectileIndex: 3, fixedAngle: 135),
                            new Shoot(0, projectileIndex: 3, fixedAngle: 225),
                            new Shoot(0, projectileIndex: 3, fixedAngle: 315),
                            new TimedTransition(1000, "shoot2")
                        ),
                        new State("shoot2",
                            new Shoot(0, 2, shootAngle: 15, projectileIndex: 3, fixedAngle: 45),
                            new Shoot(0, 2, shootAngle: 15, projectileIndex: 3, fixedAngle: 135),
                            new Shoot(0, 2, shootAngle: 15, projectileIndex: 3, fixedAngle: 225),
                            new Shoot(0, 2, shootAngle: 15, projectileIndex: 3, fixedAngle: 315),
                            new TimedTransition(1000, "shoot1")
                        )
                    )
                )
            )
            .Init("vlntns Bella Buds",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new EntityNotExistsTransition("vlntns Botany Bella", 100, "Despawn"),
                    new State("I_WILL_HALP_YOU_BUD_<3",
                        new State("IMMA_MOVE_NAOW",
                            new Follow(1, range: 1),
                            new PlayerWithinTransition(1, "IMMA_DONT_MOVE_NAOW"),
                            new State("shoot1",
                                new Shoot(10, fixedAngle: 145),
                                new TimedTransition(333, "shoot2")
                            ),
                            new State("shoot2",
                                new Shoot(10, fixedAngle: 265),
                                new TimedTransition(333, "shoot3")
                            ),
                            new State("shoot3",
                                new Shoot(10, fixedAngle: 385),
                                new TimedTransition(333, "shoot1")
                            )
                        ),
                        new State("IMMA_DONT_MOVE_NAOW",
                            new TimedTransition(2000, "IMMA_MOVE_NAOW"),
                            new State("shoot1",
                                new Shoot(10, fixedAngle: 145),
                                new TimedTransition(333, "shoot2")
                            ),
                            new State("shoot2",
                                new Shoot(10, fixedAngle: 265),
                                new TimedTransition(333, "shoot3")
                            ),
                            new State("shoot3",
                                new Shoot(10, fixedAngle: 385),
                                new TimedTransition(333, "shoot1")
                            )
                        )
                    ),
                    new State("CUTIES_PROTECT_ME",
                        new Orbit(0.7, 1, target: "vlntns Botany Bella", radiusVariance: 1),
                        new Shoot(10, coolDown: 2000)
                    ),
                    new State("Despawn",
                        new Timed(1000, new Suicide())
                    )
                )
            )
            .Init("vlntns Red",
                new State(
                    new EntityNotExistsTransition("vlntns Botany Bella", 50, "Despawn"),
                    new State("SleepForASec",
                        new EntityNotExistsTransition("vlntns White", 50, "Imma_Full_of_POWAAA"),
                        new TimedTransition(1000, "Imma_Full_of_POWAAA")
                    ),
                    new State("Imma_Full_of_POWAAA",
                        new Shoot(20, 6, coolDown: 1000),
                        new TransformOnDeath("vlntns White"),
                        new State(
                            new State("tex1",
                                new SetAltTexture(1),
                                new TimedTransition(1000, "tex2")
                            ),
                            new State("tex2",
                                new SetAltTexture(2),
                                new TimedTransition(1000, "tex2")
                            ),
                            new State("tex3",
                                new SetAltTexture(3)
                            )
                        )
                    ),
                    new State("Despawn",
                        new Timed(1000, new Suicide())
                    )
                )
            )
            .Init("vlntns White",
                new State(
                    new EntityNotExistsTransition("vlntns Botany Bella", 50, "Despawn"),
                    new State("SleepForASec",
                        new EntityNotExistsTransition("vlntns Yellow", 50, "Imma_Full_of_POWAAA"),
                        new TimedTransition(1000, "Imma_Full_of_POWAAA")
                    ),
                    new State("Imma_Full_of_POWAAA",
                        new Shoot(20, 6, coolDown: 1000),
                        new TransformOnDeath("vlntns Yellow"),
                        new State(
                            new State("tex1",
                                new SetAltTexture(1),
                                new TimedTransition(1000, "tex2")
                            ),
                            new State("tex2",
                                new SetAltTexture(2),
                                new TimedTransition(1000, "tex2")
                            ),
                            new State("tex3",
                                new SetAltTexture(3)
                            )
                        )
                    ),
                    new State("Despawn",
                        new Timed(1000, new Suicide())
                    )
                )
            )
            .Init("vlntns Yellow",
                new State(
                    new EntityNotExistsTransition("vlntns Botany Bella", 50, "Despawn"),
                    new State("SleepForASec",
                        new EntityNotExistsTransition("vlntns Red", 50, "Imma_Full_of_POWAAA"),
                        new TimedTransition(1000, "Imma_Full_of_POWAAA")
                    ),
                    new State("Imma_Full_of_POWAAA",
                        new Shoot(20, 6, coolDown: 1000),
                        new TransformOnDeath("vlntns Red"),
                        new State(
                            new State("tex1",
                                new SetAltTexture(1),
                                new TimedTransition(1000, "tex2")
                            ),
                            new State("tex2",
                                new SetAltTexture(2),
                                new TimedTransition(1000, "tex2")
                            ),
                            new State("tex3",
                                new SetAltTexture(3)
                            )
                        )
                    ),
                    new State("Despawn",
                        new Timed(1000, new Suicide())
                    )
                )
            )
            .Init("vlntns Loot Balloon Bella",
                new State(
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new Threshold(0.1,
                    new ItemLoot("Diamond Bladed Katana", 0.1),
                    new ItemLoot("Cupid's Bow", 0.1),
                    new ItemLoot("Staff of Adoration", 0.1),
                    new ItemLoot("Wand of Budding Romance", 0.1),
                    new ItemLoot("Heartfind Dagger", 0.1),
                    new ItemLoot("Vinesword", 0.1),
                    new ItemLoot("Bashing Bride Skin", 0.02),
                    new ItemLoot("Eligible Bachelor Skin", 0.02)
                )
            );
    }
}
