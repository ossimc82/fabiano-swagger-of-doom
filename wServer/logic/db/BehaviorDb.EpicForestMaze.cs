using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ EForestMaze = () => Behav()
        .Init("Murderous Megamoth",
             new State(
                 new RealmPortalDrop(),
                 new State("idle",
                     new Wander(0.2),
                     new Follow(5.0, 10, coolDown: 0),
                     new Spawn("Mini Larva", coolDown: 500, maxChildren: 10, initialSpawn: 4),
                     new Reproduce("Mini Larva", coolDown: 500, densityMax: 20, densityRadius: 4),
                     new Shoot(25, projectileIndex: 0, count: 2, shootAngle: 10, coolDown: 500, coolDownOffset: 500),
                     new Shoot(25, projectileIndex: 1, count: 1, shootAngle: 0, coolDown: 1, coolDownOffset: 1)
                     )
                 ),
                 new MostDamagers(1,
                    new ItemLoot("Potion of Vitality", 0.9)
                ),
                new Threshold(0.025,
                    new TierLoot(8, ItemType.Armor, 0.035),
                    new TierLoot(9, ItemType.Armor, 0.03),
                    new TierLoot(10, ItemType.Armor, 0.025),
                    new TierLoot(11, ItemType.Armor, 0.02),
                    new TierLoot(12, ItemType.Armor, 0.015),
                    new TierLoot(13, ItemType.Armor, 0.01),
                    new TierLoot(4, ItemType.Ability, 0.03),
                    new TierLoot(8, ItemType.Weapon, 0.01),
                    new TierLoot(9, ItemType.Weapon, 0.01),
                    new TierLoot(12, ItemType.Weapon, 0.01),
                    new ItemLoot("Wine Cellar Incantation", 0.01),
                    new ItemLoot("Leaf Bow", 0.005)
                ),
                new Threshold(0.2,
                    new EggLoot(EggRarity.Common, 0.1),
                    new EggLoot(EggRarity.Uncommon, 0.05),
                    new EggLoot(EggRarity.Rare, 0.01),
                    new EggLoot(EggRarity.Legendary, 0.002)
                )
            )
        .Init("Mini Larva",
            new State(
                new State("idle",
                    new Wander(0.1),
                    new Protect(1, "Murderous Megamoth", 100, 5, 5),
                    new Shoot(10, count: 4, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2)
                    )
                )
            )
        .Init("Epic Mama Megamoth",
            new State(
                new HpLessTransition(.3, "change"),
                new TransformOnDeath("Murderous Megamoth"),
                new State("idle",
                    new Wander(0.2),
                    new Follow(4.0, 10, coolDown: 0),
                    new Spawn("Woodland Mini Megamoth", coolDown: 500, initialSpawn: 5),
                    new Reproduce("Woodland Mini Megamoth", coolDown: 500, densityMax: 12, densityRadius: 5),
                    new Shoot(25, projectileIndex: 0, count: 3, shootAngle: 10, coolDown: 1, coolDownOffset: 1)
                    ),
                new State("change",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xfFF0000, 1, 900001),
                    new TimedTransition(3000, "suicide")
                    ),
                new State("suicide",
                    new Suicide()
                    )

                )
            )
        .Init("Woodland Mini Megamoth",
            new State(
                new EntityNotExistsTransition("Epic Mama Megamoth", 20, "suicide"),
                new State("idle",
                    new Wander(0.1),
                    new Shoot(25, projectileIndex: 0, count: 1, shootAngle: 0, coolDown: 0, coolDownOffset: 0),
                    new Protect(1, "Epic Mama Megamoth", 20, 5, 1)
                    ),
                new State("suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Epic Larva",
            new State(
                new Prioritize(
                    new Follow(1.5, 8, 1),
                    new Wander(0.25)
                ),
                new HpLessTransition(.3, "change"),
                new HpLessTransition(.75, "shoot4"),
                new TransformOnDeath("Epic Mama Megamoth"),
                new PlayerWithinTransition(10, "shoot1"),
                new State("shoot1",
                    new Shoot(0, 3, shootAngle: 15, projectileIndex: 0, fixedAngle: 45, coolDownOffset: 1250),
                    new Shoot(0, 3, shootAngle: 15, projectileIndex: 0, fixedAngle: 135, coolDownOffset: 1250),
                    new Shoot(0, 3, shootAngle: 15, projectileIndex: 0, fixedAngle: 225, coolDownOffset: 1250),
                    new Shoot(0, 3, shootAngle: 15, projectileIndex: 0, fixedAngle: 315, coolDownOffset: 1250),
                    new Shoot(10, count: 8, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2, coolDownOffset: 1500),
                    new Shoot(10, count: 4, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2, coolDownOffset: 2000)
                    ),
                new State("shoot4",
                    //toss Larva Puke
                    new Shoot(0, 3, shootAngle: 15, projectileIndex: 0, fixedAngle: 45, coolDownOffset: 1250),
                    new Shoot(0, 3, shootAngle: 15, projectileIndex: 0, fixedAngle: 135, coolDownOffset: 1250),
                    new Shoot(0, 3, shootAngle: 15, projectileIndex: 0, fixedAngle: 225, coolDownOffset: 1250),
                    new Shoot(0, 3, shootAngle: 15, projectileIndex: 0, fixedAngle: 315, coolDownOffset: 1250),
                    new Shoot(10, count: 8, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2, coolDownOffset: 1500),
                    new Shoot(10, count: 4, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2, coolDownOffset: 2000)
                    ),
                new State("change",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xfFF0000, 1, 900001),
                    new TimedTransition(3000, "suicide")
                    ),
                new State("suicide",
                    new Suicide()
                    )

                )
            )
        .Init("Woodland Ultimate Squirrel",
            new State(
                new Prioritize(
                    new Follow(0.4, 6, 1, -1, 0),
                    new Wander(0.3)
                    ),
                new Shoot(radius: 7, count: 1, projectileIndex: 0, shootAngle: 20, coolDown: 2000)
                ),
                new Threshold(0.025,
                    new ItemLoot("Speed Sprout", 0.05)
                )
            )
        .Init("Woodland Goblin Mage",
            new State(
                new Prioritize(
                    new Wander(0.4),
                    new Shoot(radius: 10, count: 2, projectileIndex: 0, predictive: 1, coolDown: 500, shootAngle: 2)
                    )
                ),
                new Threshold(0.025,
                    new ItemLoot("Speed Sprout", 0.05)
                )
            )
        .Init("Woodland Goblin",
            new State(
                new Prioritize(
                    new Wander(0.4),
                    new Follow(0.7, 10, 3, -1, 0),
                    new Shoot(radius: 4, count: 1, projectileIndex: 0, coolDown: 500)
                    )
                ),
                new Threshold(0.025,
                    new ItemLoot("Speed Sprout", 0.05)
                )
            )
        .Init("Wooland Armor Squirrel",
            new State(
                new Prioritize(
                    new Follow(0.6, 6, 1, -1, 0),
                    new Wander(0.7),
                    new Shoot(radius: 7, count: 3, projectileIndex: 0, predictive: 1, coolDown: 1000, shootAngle: 15)
                    )
                ),
                new Threshold(0.025,
                    new ItemLoot("Speed Sprout", 0.05)
                )
            )
        .Init("Woodland Silence Turret",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("idle",
                    new Shoot(10, count: 8, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2)
                    )
                )
            )
        .Init("Woodland Weakness Turret",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("idle",
                    new Shoot(10, count: 8, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2)
                    )
                )
            )
        .Init("Woodland Paralyze Turret",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("idle",
                    new Shoot(10, count: 8, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2)
                    )
                )
            );
                 }
}
