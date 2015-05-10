#region

using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ EntAncient = () => Behav()
            .Init("Ent Ancient",
                new State(
                    new State("Idle",
                        new StayCloseToSpawn(0.1, 6),
                        new Wander(0.1),
                        new HpLessTransition(0.99999, "EvaluationStart1")
                        ),
                    new State("EvaluationStart1",
                        new Taunt("Uhh. So... sleepy..."),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Prioritize(
                            new StayCloseToSpawn(0.2, 3),
                            new Wander(0.2)
                            ),
                        new TimedTransition(2500, "EvaluationStart2")
                        ),
                    new State("EvaluationStart2",
                        new Flash(0x0000ff, 0.1, 60),
                        new Shoot(10, 2, 180, coolDown: 800),
                        new Prioritize(
                            new StayCloseToSpawn(0.3, range: 5),
                            new Wander(0.3)
                            ),
                        new HpLessTransition(0.87, "EvaluationEnd"),
                        new TimedTransition(6000, "EvaluationEnd")
                        ),
                    new State("EvaluationEnd",
                        new HpLessTransition(0.875, "HugeMob"),
                        new HpLessTransition(0.952, "Mob"),
                        new HpLessTransition(0.985, "SmallGroup"),
                        new HpLessTransition(0.99999, "Solo")
                        ),
                    new State("HugeMob",
                        new Taunt("You are many, yet the sum of your years is nothing."),
                        new Spawn("Greater Nature Sprite", 6, 0, 400),
                        new TossObject("Ent", 3, 0, 100000),
                        new TossObject("Ent", 3, 180, 100000),
                        new TossObject("Ent", 5, 10, 100000),
                        new TossObject("Ent", 5, 190, 100000),
                        new TossObject("Ent", 5, 70, 100000),
                        new TossObject("Ent", 7, 20, 100000),
                        new TossObject("Ent", 7, 200, 100000),
                        new TossObject("Ent", 7, 80, 100000),
                        new TossObject("Ent", 10, 30, 100000),
                        new TossObject("Ent", 10, 210, 100000),
                        new TossObject("Ent", 10, 90, 100000),
                        new TimedTransition(5000, "Wait")
                        ),
                    new State("Mob",
                        new Taunt("Little flies, little flies... we will swat you."),
                        new Spawn("Greater Nature Sprite", 3, 0, 1000),
                        new TossObject("Ent", 3, 0, 100000),
                        new TossObject("Ent", 4, 180, 100000),
                        new TossObject("Ent", 5, 10, 100000),
                        new TossObject("Ent", 6, 190, 100000),
                        new TossObject("Ent", 7, 20, 100000),
                        new TimedTransition(5000, "Wait")
                        ),
                    new State("SmallGroup",
                        new Taunt("It will be trivial to dispose of you."),
                        new Spawn("Greater Nature Sprite", 1, 1, 100000),
                        new TossObject("Ent", 3, 0, 100000),
                        new TossObject("Ent", 4.5, 180, 100000),
                        new TimedTransition(3000, "Wait")
                        ),
                    new State("Solo",
                        new Taunt("Mmm? Did you say something, mortal?"),
                        new TimedTransition(3000, "Wait")
                        ),
                    new State("Wait",
                        new Transform("Actual Ent Ancient")
                        )
                    )
            )
            .Init("Actual Ent Ancient",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(0.2, 6),
                        new Wander(0.2)
                        ),
                    new Spawn("Ent Sapling", 3, 0, 3000),
                    new State("Start",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(11, 160),
                        new Shoot(10, projectileIndex: 0, count: 1),
                        new TimedTransition(1600, "Growing1"),
                        new HpLessTransition(0.9, "Growing1")
                        ),
                    new State("Growing1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(11, 180),
                        new Shoot(10, projectileIndex: 1, count: 3, shootAngle: 120),
                        new TimedTransition(1600, "Growing2"),
                        new HpLessTransition(0.8, "Growing2")
                        ),
                    new State("Growing2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(11, 200),
                        new Taunt(0.35, "Little mortals, your years are my minutes."),
                        new Shoot(10, projectileIndex: 2, count: 1),
                        new TimedTransition(1600, "Growing3"),
                        new HpLessTransition(0.7, "Growing3")
                        ),
                    new State("Growing3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(11, 220),
                        new Shoot(10, projectileIndex: 3, count: 1),
                        new TimedTransition(1600, "Growing4"),
                        new HpLessTransition(0.6, "Growing4")
                        ),
                    new State("Growing4",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(11, 240),
                        new Taunt(0.35, "No axe can fell me!"),
                        new Shoot(10, projectileIndex: 4, count: 3, shootAngle: 120),
                        new TimedTransition(1600, "Growing5"),
                        new HpLessTransition(0.5, "Growing5")
                        ),
                    new State("Growing5",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(11, 260),
                        new Shoot(10, projectileIndex: 5, count: 1),
                        new TimedTransition(1600, "Growing6"),
                        new HpLessTransition(0.45, "Growing6")
                        ),
                    new State("Growing6",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(11, 280),
                        new Taunt(0.35, "Yes, YES..."),
                        new Shoot(10, projectileIndex: 6, count: 1),
                        new TimedTransition(1600, "Growing7"),
                        new HpLessTransition(0.4, "Growing7")
                        ),
                    new State("Growing7",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(11, 300),
                        new Shoot(10, projectileIndex: 7, count: 3, shootAngle: 120),
                        new TimedTransition(1600, "Growing8"),
                        new HpLessTransition(0.36, "Growing8")
                        ),
                    new State("Growing8",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(11, 320),
                        new Taunt(0.35, "I am the FOREST!!"),
                        new Shoot(10, projectileIndex: 8, count: 1),
                        new TimedTransition(1600, "Growing9"),
                        new HpLessTransition(0.32, "Growing9")
                        ),
                    new State("Growing9",
                        new ChangeSize(11, 340),
                        new Taunt(1.0, "YOU WILL DIE!!!"),
                        new Shoot(10, projectileIndex: 9, count: 1),
                        new State("convert_sprites",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Order(50, "Greater Nature Sprite", "Transform"),
                            new TimedTransition(2000, "shielded")
                            ),
                        new State("received_armor",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new ConditionalEffect(ConditionEffectIndex.Armored, true),
                            new TimedTransition(1000, "shielded")
                            ),
                        new State("shielded",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(4000, "unshielded")
                            ),
                        new State("unshielded",
                            new Shoot(10, projectileIndex: 3, count: 3, shootAngle: 120, coolDown: 700),
                            new TimedTransition(4000, "shielded")
                            )
                        )
                    ),
                new TierLoot(2, ItemType.Ring, 0.15),
                new TierLoot(3, ItemType.Ring, 0.04),
                new TierLoot(6, ItemType.Weapon, 0.3),
                new TierLoot(7, ItemType.Weapon, 0.1),
                new TierLoot(6, ItemType.Armor, 0.3),
                new TierLoot(7, ItemType.Armor, 0.1),
                new TierLoot(1, ItemType.Ability, 0.95),
                new TierLoot(2, ItemType.Ability, 0.25),
                new TierLoot(3, ItemType.Ability, 0.05),
                new ItemLoot("Health Potion", 0.7),
                new ItemLoot("Magic Potion", 0.7)
            )
            .Init("Ent",
                new State(
                    new Prioritize(
                        new Protect(0.25, "Ent Ancient", 12, 7, 7),
                        new Follow(0.25, range: 1, acquireRange: 9),
                        new Shoot(10, 5, 72, fixedAngle: 30, coolDown: 1600, coolDownOffset: 800)
                        ),
                    new Shoot(10, predictive: 0.4, coolDown: 600),
                    new Decay(90000)
                    ),
                new ItemLoot("Tincture of Dexterity", 0.02)
            )
            .Init("Ent Sapling",
                new State(
                    new Prioritize(
                        new Protect(0.55, "Ent Ancient", 10, 4, 4),
                        new Wander(0.55)
                        ),
                    new Shoot(10, coolDown: 1000)
                    )
            )
            .Init("Greater Nature Sprite",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(10, 4, 10),
                    new Prioritize(
                        new StayCloseToSpawn(1.5, 11),
                        new Orbit(1.5, 4, 7),
                        new Follow(200, 7, 2),
                        new Follow(0.3, 7, 0.2)
                        ),
                    new State("Idle"),
                    new State("Transform",
                        new Transform("Actual Greater Nature Sprite")
                        ),
                    new Decay(90000)
                    )
            )
            .Init("Actual Greater Nature Sprite",
                new State(
                    new Flash(0xff484848, 0.6, 1000),
                    new Spawn("Ent", 2, 0, 3000),
                    new Heal(15, "Heros", 200),
                    new State("armor_ent_ancient",
                        new Order(30, "Actual Ent Ancient", "received_armor"),
                        new TimedTransition(1000, "last_fight")
                        ),
                    new State("last_fight",
                        new Shoot(10, 4, 10),
                        new Prioritize(
                            new StayCloseToSpawn(1.5, 11),
                            new Orbit(1.5, 4, 7),
                            new Follow(200, 7, 2),
                            new Follow(0.3, 7, 0.2)
                            )
                        ),
                    new Decay(60000)
                    ),
                new ItemLoot("Magic Potion", 0.25),
                new ItemLoot("Tincture of Life", 0.06),
                new ItemLoot("Green Drake Egg", 0.08),
                new ItemLoot("Quiver of Thunder", 0.002),
                new TierLoot(8, ItemType.Armor, 0.3)
            )
            ;
    }
}