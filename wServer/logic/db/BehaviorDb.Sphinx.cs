using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ GrandSphinx = () => Behav()
            .Init("Grand Sphinx",
                    new State(
                        new HpLessOrder(50, 0.15, "Horrid Reaper", "Go away"),
                        new DropPortalOnDeath("Tomb of the Ancients Portal", 35),
                        new Spawn("Horrid Reaper", maxChildren: 8, initialSpawn: 1),
                        new State("BlindAttack",
                            new Wander(0.0005),
                            new StayCloseToSpawn(0.5, 8),
                            new Taunt("You hide like cowards... but you can't hide from this!"),
                            new State("1",
                                new Shoot(10, projectileIndex: 1, count: 10, shootAngle: 10, fixedAngle: 0),
                                new Shoot(10, projectileIndex: 1, count: 10, shootAngle: 10, fixedAngle: 180),
                                new TimedTransition(1500, "2")
                            ),
                            new State("2",
                                new Shoot(10, projectileIndex: 1, count: 10, shootAngle: 10, fixedAngle: 270),
                                new Shoot(10, projectileIndex: 1, count: 10, shootAngle: 10, fixedAngle: 90),
                                new TimedTransition(1500, "1")
                            ),
                            new TimedTransition(10000, "Ilde")
                        ),
                        new State("Ilde",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xff00ff00, 0.01, 50),
                            new TimedTransition(2000, "ArmorBreakAttack")
                        ),
                        new State("ArmorBreakAttack",
                            new Wander(0.0005),
                            new StayCloseToSpawn(0.5, 8),
                            new Shoot(0, projectileIndex: 2, count: 8, shootAngle: 5, coolDown: 300),
                            new TimedTransition(10000, "Ilde2")
                        ),
                        new State("Ilde2",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xff00ff00, 0.01, 50),
                            new TimedTransition(2000, "WeakenAttack")
                        ),
                        new State("WeakenAttack",
                            new Wander(0.0005),
                            new StayCloseToSpawn(0.5, 8),
                            new Shoot(10, projectileIndex: 3, count: 3, shootAngle: 120, coolDown: 900),
                            new Shoot(10, projectileIndex: 2, count: 3, shootAngle: 15, fixedAngle: 40, coolDown: 1600, coolDownOffset: 0),
                            new Shoot(10, projectileIndex: 2, count: 3, shootAngle: 15, fixedAngle: 220, coolDown: 1600, coolDownOffset: 0),
                            new Shoot(10, projectileIndex: 2, count: 3, shootAngle: 15, fixedAngle: 130, coolDown: 1600, coolDownOffset: 800),
                            new Shoot(10, projectileIndex: 2, count: 3, shootAngle: 15, fixedAngle: 310, coolDown: 1600, coolDownOffset: 800),
                            new TimedTransition(10000, "Ilde3")
                        ),
                        new State("Ilde3",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xff00ff00, 0.01, 50),
                            new TimedTransition(2000, "BlindAttack")
                        )
                    ),
                    new MostDamagers(3,
                        new ItemLoot("Potion of Vitality", 1),
                        new ItemLoot("Potion of Wisdom", 1)
                    ),
                    new Threshold(0.05,
                        new ItemLoot("Helm of the Juggernaut", 0.005)
                    ),
                    new Threshold(0.1,
                        new OnlyOne(
                            LootTemplates.DefaultEggLoot(EggRarity.Legendary)
                        )
                    )
                )
                .Init("Horrid Reaper",
                    new State(
                        new Shoot(radius: 25, shootAngle: 10 * (float)Math.PI / 180, count: 1, projectileIndex: 0, coolDown: 1000),
                        new State("Idle",
                            new StayCloseToSpawn(0.5, 15),
                            new Wander(0.5),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true)
                        ),
                        new State("Go away",
                            new TimedTransition(5000, "I AM OUT"),
                            new Taunt("OOaoaoAaAoaAAOOAoaaoooaa!!!")
                        ),
                        new State("I AM OUT",
                            new Decay(0)
                        )
                    )
                );
            //
            //        new QueuedBehavior(
            //            SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
            //            Timed.Instance(2500, False.Instance(Flashing.Instance(200, 0xff00ff00))),
            //            UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),

            //            Cooldown.Instance(600, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 2, projectileIndex: 2)),
            //            Cooldown.Instance(600, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 2, projectileIndex: 2)),
            //            Cooldown.Instance(300, RingAttack.Instance(9, projectileIndex: 2)),

            //            Cooldown.Instance(600, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 2, projectileIndex: 2)),
            //            Cooldown.Instance(600, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 2, projectileIndex: 2)),
            //            Cooldown.Instance(500, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 8, projectileIndex: 2)),
            //            Cooldown.Instance(500, RingAttack.Instance(9, projectileIndex: 2)),

            //            Cooldown.Instance(600, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 2, projectileIndex: 2)),
            //            Cooldown.Instance(600, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 2, projectileIndex: 2)),
            //            Cooldown.Instance(500, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 8, projectileIndex: 2)),
            //            Cooldown.Instance(500, RingAttack.Instance(9, projectileIndex: 2)),

            //            Cooldown.Instance(600, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 2, projectileIndex: 2)),
            //            Cooldown.Instance(600, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 2, projectileIndex: 2)),
            //            Cooldown.Instance(500, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 8, projectileIndex: 2)),
            //            Cooldown.Instance(500, RingAttack.Instance(9, projectileIndex: 2)),

            //            SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
            //            Timed.Instance(2500, False.Instance(Flashing.Instance(200, 0xff00ff00))),
            //            UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),

            //            Timed.Instance(7000, False.Instance(new QueuedBehavior(
            //                Cooldown.Instance(300, MultiAttack.Instance(15, 30 * (float)Math.PI / 180, 3, projectileIndex: 0)),
            //                Cooldown.Instance(300, RingAttack.Instance(3, projectileIndex: 0))
            //            ))),

            //            SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
            //            new SimpleTaunt("You hide like cowards... but you can't hide from this!"),
            //            Timed.Instance(2500, False.Instance(Flashing.Instance(200, 0xff00ff00))),
            //            UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),

            //            AngleMultiAttack.Instance((0 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            AngleMultiAttack.Instance((0 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            CooldownExact.Instance(1000),

            //            AngleMultiAttack.Instance((1 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            AngleMultiAttack.Instance((1 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            CooldownExact.Instance(1000),

            //            AngleMultiAttack.Instance((2 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            AngleMultiAttack.Instance((2 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            CooldownExact.Instance(1000),

            //            AngleMultiAttack.Instance((3 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            AngleMultiAttack.Instance((3 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            CooldownExact.Instance(1000),

            //            AngleMultiAttack.Instance((4 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            AngleMultiAttack.Instance((4 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            CooldownExact.Instance(1000),

            //            AngleMultiAttack.Instance((0 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            AngleMultiAttack.Instance((0 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            CooldownExact.Instance(1000),

            //            AngleMultiAttack.Instance((1 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            AngleMultiAttack.Instance((1 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            CooldownExact.Instance(1000),

            //            AngleMultiAttack.Instance((2 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            AngleMultiAttack.Instance((2 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            CooldownExact.Instance(1000),

            //            AngleMultiAttack.Instance((3 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            AngleMultiAttack.Instance((3 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            CooldownExact.Instance(1000),

            //            AngleMultiAttack.Instance((4 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            AngleMultiAttack.Instance((4 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            CooldownExact.Instance(1000),

            //            AngleMultiAttack.Instance((0 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            AngleMultiAttack.Instance((0 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
            //            CooldownExact.Instance(1000)
            //        ),
            //        new RunBehaviors(
            //            Once.Instance(SpawnMinionImmediate.Instance(0x0d55, 4, 4, 4)),
            //            HpLesserPercent.Instance(0.15f,
            //                OrderAllEntity.Instance(30, 0x0d55,
            //                    new SetKey(-1, 1)
            //                )
            //            ),
            //            SmoothWandering.Instance(3, 2)
            //        ),
            //        loot: new LootBehavior(LootDef.Empty,
            //            Tuple.Create(800, new LootDef(0, 1, 0, 8,
            //                Tuple.Create(0.001, (ILoot)new ItemLoot("Helm of the Juggernaut")),
            //                Tuple.Create(0.05, (ILoot)new StatPotionLoot(StatPotion.Vit)),
            //                Tuple.Create(0.05, (ILoot)new StatPotionLoot(StatPotion.Wis))
            //            ))
            //        )
            //    ))
            //.Init(0x0d55, Behaves("Horrid Reaper",
            //        new QueuedBehavior(
            //            True.Instance(Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable))),
            //            Timed.Instance(3000, False.Instance(new RunBehaviors(
            //                Cooldown.Instance(500,
            //                    SimpleAttack.Instance(10, projectileIndex: 1)
            //                ),
            //                Chasing.Instance(4, 15, 0, null)
            //            ))),
            //            Timed.Instance(3000, False.Instance(new RunBehaviors(
            //                Cooldown.Instance(200,
            //                    SimpleAttack.Instance(10, projectileIndex: 0)
            //                ),
            //                Chasing.Instance(4, 15, 0, null)
            //            ))),
            //            Timed.Instance(3000, False.Instance(
            //                IfNot.Instance(
            //                    Chasing.Instance(20, 40, 30, 0x0d54),
            //                    SimpleWandering.Instance(20)
            //                )
            //            )),
            //            Timed.Instance(8000, False.Instance(
            //                Cooldown.Instance(600,
            //                    RingAttack.Instance(6, offset: 30 * (float)Math.PI / 180, projectileIndex: 0)
            //                )
            //            ))
            //        ),
            //        IfExist.Instance(-1, new QueuedBehavior(
            //            new SimpleTaunt("OOaoaoAaAoaAAOOAoaaoooaa!!!"),
            //            Timed.Instance(500, False.Instance(Flashing.Instance(100, 0xff00ff00))),
            //            Despawn.Instance,
            //            False.Instance(NullBehavior.Instance)
            //        ))
            //    ));
    }
}
