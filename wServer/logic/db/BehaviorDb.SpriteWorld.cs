using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ SpriteWorld = () => Behav()
        .Init("Limon the Sprite God",
             new State(
                 new State("idle",
                     new Flash(0x66FF00, 0.6, 9),
                     new Wander(0.07),
                     new PlayerWithinTransition(12, "nothing")
                 ),
                 new State("nothing",
                     new Wander(0.07),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new TimedTransition(2000, "e.e")
                     ),
                 new State("e.e",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Prioritize(
                         new StayCloseToSpawn(2, 14),
                         new StayAbove(2, 55),
                         new Wander(2)
                     ),
                     new Shoot(10, 2, 20, angleOffset: 0 / 2, projectileIndex: 0, coolDown: 300),
                     new Shoot(10, 1, 0, defaultAngle: 180, angleOffset: 180, projectileIndex: 0, predictive: 1,
                     coolDown: 300, coolDownOffset: 0),
                     new TimedTransition(3000, ":c")
                     ),
                 new State(":c",
                     new Prioritize(
                         new StayCloseToSpawn(2, 14),
                         new StayAbove(2, 55),
                         new Wander(2)
                     ),
                     new Shoot(10, 2, 20, angleOffset: 0 / 2, projectileIndex: 0, coolDown: 300),
                     new Shoot(10, 1, 0, defaultAngle: 180, angleOffset: 180, projectileIndex: 0, predictive: 1,
                     coolDown: 300, coolDownOffset: 0),
                     new TimedTransition(7000, ":P")
                     ),
                 new State(":P",
                     new ReturnToSpawn(speed: 1.4),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new TimedTransition(1000, "rage")
                     ),
                 new State("rage",
                     new TossObject("Limon Element 1", 6, 45, 100000),
                     new TossObject("Limon Element 2", 6, 135, 100000),
                     new TossObject("Limon Element 3", 6, 225, 100000),
                     new TossObject("Limon Element 4", 6, 315, 100000),
                     new TossObject("Limon Element 1", 10, 45, 100000),
                     new TossObject("Limon Element 2", 10, 135, 100000),
                     new TossObject("Limon Element 3", 10, 225, 100000),
                     new TossObject("Limon Element 4", 10, 315, 100000),
                     new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new Shoot(10, 1, 0, defaultAngle: 120, angleOffset: 120, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new Shoot(10, 1, 0, defaultAngle: 240, angleOffset: 240, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new TimedTransition(2000, "rage2")
                     ),
                 new State("rage2",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new Shoot(10, 1, 0, defaultAngle: 120, angleOffset: 120, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new Shoot(10, 1, 0, defaultAngle: 240, angleOffset: 240, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new TimedTransition(3000, "rage3")
                     ),
                 new State("rage3",
                      new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new Shoot(10, 1, 0, defaultAngle: 120, angleOffset: 120, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new Shoot(10, 1, 0, defaultAngle: 240, angleOffset: 240, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new TimedTransition(3000, "rage4")
                     ),
                 new State("rage4",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Order(100, "Limon Element 1", "X Shape"),
                     new Order(100, "Limon Element 2", "X Shape"),
                     new Order(100, "Limon Element 3", "X Shape"),
                     new Order(100, "Limon Element 4", "X Shape"),
                     new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new Shoot(10, 1, 0, defaultAngle: 120, angleOffset: 120, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new Shoot(10, 1, 0, defaultAngle: 240, angleOffset: 240, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new TimedTransition(3000, "rage5")
                     ),
                 new State("rage5",
                     new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new Shoot(10, 1, 0, defaultAngle: 120, angleOffset: 120, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new Shoot(10, 1, 0, defaultAngle: 240, angleOffset: 240, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new TimedTransition(3000, "rage6")
                     ),
                 new State("rage6",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new Shoot(10, 1, 0, defaultAngle: 120, angleOffset: 120, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new Shoot(10, 1, 0, defaultAngle: 240, angleOffset: 240, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new TimedTransition(3000, "rage7")
                     ),
                 new State("rage7",
                     new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new Shoot(10, 1, 0, defaultAngle: 120, angleOffset: 120, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new Shoot(10, 1, 0, defaultAngle: 240, angleOffset: 240, projectileIndex: 0, predictive: 1,
                     coolDown: 800, coolDownOffset: 0),
                     new TimedTransition(3000, "brown shield")
                     ),
                 new State("brown shield",
                     new Wander(0.07),
                     new ConditionalEffect(ConditionEffectIndex.Armored),
                     new Spawn("Native Magic Sprite", maxChildren: 2, initialSpawn: 0.5),
                     new Spawn("Native Ice Sprite", maxChildren: 2, initialSpawn: 0.5),
                     new Order(100, "Limon Element 1", "suicide"),
                     new Order(100, "Limon Element 2", "suicide"),
                     new Order(100, "Limon Element 3", "suicide"),
                     new Order(100, "Limon Element 4", "suicide"),
                     new Shoot(10, 2, 20, angleOffset: 0 / 2, projectileIndex: 0, coolDown: 300),
                     new Shoot(10, 1, 0, defaultAngle: 180, angleOffset: 0, projectileIndex: 0, predictive: 1,
                     coolDown: 300, coolDownOffset: 0),
                     new TimedTransition(2000, "nothing")
                     )
                 ),
                    new Threshold(0.32,
                    new ItemLoot("Potion of Dexterity", 1),
                    new ItemLoot("Potion of Defense", 0.05)
                ),
                new Threshold(0.1,
                    new ItemLoot("Staff of Extreme Prejudice", 0.005),
                    new ItemLoot("Cloak of the Planewalker", 0.005),
                    new ItemLoot("Wine Cellar Incantation", 0.005),
                    new ItemLoot("Sprite Wand", 0.01),

                    new TierLoot(3, ItemType.Ring, 0.2),

                    new TierLoot(6, ItemType.Armor, 0.2),

                    new TierLoot(3, ItemType.Ability, 0.2),
                    new TierLoot(4, ItemType.Ability, 0.15),
                    new TierLoot(5, ItemType.Ability, 0.1),
                 new Threshold(0.2,
                    new EggLoot(EggRarity.Common, 0.1),
                    new EggLoot(EggRarity.Uncommon, 0.05),
                    new EggLoot(EggRarity.Rare, 0.01),
                    new EggLoot(EggRarity.Legendary, 0.002)
                     )
            )
            )
        .Init("Limon Element 1",
             new State(
                 new EntityNotExistsTransition("Limon the Sprite God", 1000, "suicide"),
                 new State("Shoot",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 270, fixedAngle: 270, coolDown: 200),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 180, fixedAngle: 180, coolDown: 200)
                     ),
                 new State("X Shape",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 270, fixedAngle: 270, coolDown: 200),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 180, fixedAngle: 180, coolDown: 200),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 225, fixedAngle: 225, coolDown: 200)
                     ),
                 new State("suicide",
                     new Suicide()
               )
                 )
            )
       .Init("Limon Element 2",
             new State(
                 new EntityNotExistsTransition("Limon the Sprite God", 1000, "suicide"),
                 new State("Shoot",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 360, fixedAngle: 360, coolDown: 200),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 270, fixedAngle: 270, coolDown: 200)
                     ),
                 new State("X Shape",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 270, fixedAngle: 270, coolDown: 200),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 360, fixedAngle: 360, coolDown: 200),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 315, fixedAngle: 315, coolDown: 200)
                     ),
                 new State("suicide",
                     new Suicide()
               )
                 )
            )
        .Init("Limon Element 3",
             new State(
                 new EntityNotExistsTransition("Limon the Sprite God", 1000, "suicide"),
                 new State("Shoot",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 90, fixedAngle: 90, coolDown: 200),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 360, fixedAngle: 360, coolDown: 200)
                     ),
                 new State("X Shape",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 90, fixedAngle: 90, coolDown: 200),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 360, fixedAngle: 360, coolDown: 200),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 45, fixedAngle: 45, coolDown: 200)
                     ),
                 new State("suicide",
                     new Suicide()
               )
                 )
            )
                .Init("Limon Element 4",
             new State(
                 new EntityNotExistsTransition("Limon the Sprite God", 1000, "suicide"),
                 new State("Shoot",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 90, fixedAngle: 90, coolDown: 200),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 180, fixedAngle: 180, coolDown: 200)
                     ),
                 new State("X Shape",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 90, fixedAngle: 90, coolDown: 200),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 180, fixedAngle: 180, coolDown: 200),
                     new Shoot(0, projectileIndex: 0, count: 1, shootAngle: 135, fixedAngle: 135, coolDown: 200)
                     ),
                 new State("suicide",
                     new Suicide()
               )
                 )
            )
         .Init("Native Fire Sprite",
                new State(
                    new Reproduce(densityMax: 2),
                    new Shoot(10, 2, 7, coolDown: 300),
                    new Prioritize(
                        new StayAbove(1.4, 55),
                        new Wander(1.4)
                        )
                    ),
                new TierLoot(5, ItemType.Weapon, 0.02),
                new ItemLoot("Magic Potion", 0.05)
            )
            .Init("Native Ice Sprite",
                new State(
                    new Reproduce(densityMax: 2),
                    new Shoot(10, 3, 7),
                    new Prioritize(
                        new StayAbove(1.4, 60),
                        new Wander(1.4)
                        )
                    ),
                new TierLoot(2, ItemType.Ability, 0.04),
                new ItemLoot("Magic Potion", 0.05)
            )
            .Init("Native Magic Sprite",
                new State(
                    new Reproduce(densityMax: 2),
                    new Shoot(10, 4, 7),
                    new Prioritize(
                        new StayAbove(1.4, 60),
                        new Wander(1.4)
                        )
                    ),
                new TierLoot(6, ItemType.Armor, 0.01),
                new ItemLoot("Magic Potion", 0.05)
        )
        .Init("Native Nature Sprite",
             new State(
                    new Shoot(10, 5, 20, angleOffset: 0 / 5, projectileIndex: 0, coolDown: 1000),
                    new Prioritize(
                        new StayAbove(1.4, 60),
                        new Wander(1.6)
                        )
                    ),
                new ItemLoot("Sprite Wand", 0.05),
                new ItemLoot("Magic Potion", 0.05),
                new ItemLoot("Ring of Greater Magic", 0.05)
            )
        .Init("Native Darkness Sprite", 
             new State(
                    new Shoot(10, 5, 20, angleOffset: 0 / 5, projectileIndex: 0, coolDown: 1000),
                    new Prioritize(
                        new StayAbove(1.4, 60),
                        new Wander(1.6)
                        )
                    ),
                new ItemLoot("Health Potion", 0.05),
                new ItemLoot("Ring of Dexterity", 0.05)
        )
        .Init("Native Sprite God",
                new State(
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Wander(0.4)
                        ),
                    new Shoot(12, projectileIndex: 0, count: 4, shootAngle: 10),
                    new Shoot(10, projectileIndex: 1, predictive: 1)
                    ),
                new TierLoot(6, ItemType.Weapon, 0.04),
                new TierLoot(7, ItemType.Weapon, 0.02),
                new TierLoot(8, ItemType.Weapon, 0.01),
                new TierLoot(7, ItemType.Armor, 0.04),
                new TierLoot(8, ItemType.Armor, 0.02),
                new TierLoot(9, ItemType.Armor, 0.01),
                new TierLoot(4, ItemType.Ring, 0.02),
                new TierLoot(4, ItemType.Ability, 0.02),
                new Threshold(0.18,
                    new ItemLoot("Potion of Attack", 0.015),
                    new EggLoot(EggRarity.Common, 0.1),
                    new EggLoot(EggRarity.Uncommon, 0.05)
                    )
            );


    }
}
