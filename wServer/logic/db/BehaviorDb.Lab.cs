using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Lab = () => Behav()  // credits to sebastianfra12 and ossimc82 :3
        .Init("Dr Terrible",
            new State(
                new State("idle",
                    new PlayerWithinTransition(12, "GP"),
                    new HpLessTransition(.2, "rage")
                    ),
                new State("rage",
                    new Order(100, "Monster Cage", "no spawn"),
                    new Order(100, "Dr Terrible Bubble", "nothing change"),
                    new Order(100, "Red Gas Spawner UL", "OFF"),
                    new Order(100, "Red Gas Spawner UR", "OFF"),
                    new Order(100, "Red Gas Spawner LL", "OFF"),
                    new Order(100, "Red Gas Spawner LR", "OFF"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Green Potion", coolDown: 1500, coolDownOffset: 0),
                    new TimedTransition(12000, "rage TA")
                    ),
                new State("rage TA",
                    new Order(100, "Monster Cage", "no spawn"),
                    new Order(100, "Dr Terrible Bubble", "nothing change"),
                    new Order(100, "Red Gas Spawner UL", "OFF"),
                    new Order(100, "Red Gas Spawner UR", "OFF"),
                    new Order(100, "Red Gas Spawner LL", "OFF"),
                    new Order(100, "Red Gas Spawner LR", "OFF"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Turret Attack", coolDown: 1500, coolDownOffset: 0),
                    new TimedTransition(10000, "rage")
                    ),
                new State("GP",
                    new Order(100, "Monster Cage", "no spawn"),
                    new Order(100, "Dr Terrible Bubble", "nothing change"),
                    new Order(100, "Red Gas Spawner UL", "OFF"),
                    new Order(100, "Red Gas Spawner UR", "ON"),
                    new Order(100, "Red Gas Spawner LL", "ON"),
                    new Order(100, "Red Gas Spawner LR", "ON"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new Taunt(0.5, "For Science"),
                    new TossObject("Green Potion", coolDown: 2000, coolDownOffset: 0),
                    new TimedTransition(12000, "TA")
                    ),
                new State("TA",
                    new Order(100, "Monster Cage", "no spawn"),
                    new Order(100, "Dr Terrible Bubble", "nothing change"),
                    new Order(100, "Red Gas Spawner UL", "OFF"),
                    new Order(100, "Red Gas Spawner UR", "ON"),
                    new Order(100, "Red Gas Spawner LL", "ON"),
                    new Order(100, "Red Gas Spawner LR", "ON"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Turret Attack", coolDown: 2000, coolDownOffset: 0),
                    new TimedTransition(10000, "hide")
                    ),
                new State("hide",
                    new Order(100, "Monster Cage", "spawn"),
                    new Order(100, "Dr Terrible Bubble", "Bubble time"),
                    new Order(100, "Red Gas Spawner UL", "OFF"),
                    new Order(100, "Red Gas Spawner UR", "OFF"),
                    new Order(100, "Red Gas Spawner LL", "OFF"),
                    new Order(100, "Red Gas Spawner LR", "OFF"),
                    new ReturnToSpawn(speed: 1),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),

                    new TimedTransition(15000, "nohide")
                    ),
                new State("nohide",
                    new Order(100, "Monster Cage", "no spawn"),
                    new Order(100, "Dr Terrible Bubble", "nothing change"),
                    new Order(100, "Red Gas Spawner UL", "ON"),
                    new Order(100, "Red Gas Spawner UR", "OFF"),
                    new Order(100, "Red Gas Spawner LL", "ON"),
                    new Order(100, "Red Gas Spawner LR", "ON"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Green Potion", coolDown: 2000, coolDownOffset: 0),
                    new TimedTransition(12000, "TA2")
                    ),
                new State("TA2",
                    new Order(100, "Monster Cage", "no spawn"),
                    new Order(100, "Dr Terrible Bubble", "nothing change"),
                    new Order(100, "Red Gas Spawner UL", "ON"),
                    new Order(100, "Red Gas Spawner UR", "OFF"),
                    new Order(100, "Red Gas Spawner LL", "ON"),
                    new Order(100, "Red Gas Spawner LR", "ON"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Green Potion", coolDown: 2000, coolDownOffset: 0),
                    new TimedTransition(10000, "hide2")
                    ),
                new State("hide2",
                    new Order(100, "Monster Cage", "spawn"),
                    new Order(100, "Dr Terrible Bubble", "Bubble time"),
                    new Order(100, "Red Gas Spawner UL", "OFF"),
                    new Order(100, "Red Gas Spawner UR", "OFF"),
                    new Order(100, "Red Gas Spawner LL", "OFF"),
                    new Order(100, "Red Gas Spawner LR", "OFF"),
                    new ReturnToSpawn(speed: 1),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),
                    new TimedTransition(15000, "nohide2")
                    ),
                new State("nohide2",
                    new Order(100, "Monster Cage", "no spawn"),
                    new Order(100, "Dr Terrible Bubble", "nothing change"),
                    new Order(100, "Red Gas Spawner UL", "ON"),
                    new Order(100, "Red Gas Spawner UR", "ON"),
                    new Order(100, "Red Gas Spawner LL", "OFF"),
                    new Order(100, "Red Gas Spawner LR", "ON"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Green Potion", coolDown: 2000, coolDownOffset: 0),
                    new TimedTransition(12000, "TA3")
                    ),
                new State("TA3",
                    new Order(100, "Monster Cage", "no spawn"),
                    new Order(100, "Dr Terrible Bubble", "nothing change"),
                    new Order(100, "Red Gas Spawner UL", "ON"),
                    new Order(100, "Red Gas Spawner UR", "ON"),
                    new Order(100, "Red Gas Spawner LL", "OFF"),
                    new Order(100, "Red Gas Spawner LR", "ON"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Green Potion", coolDown: 2000, coolDownOffset: 0),
                    new TimedTransition(10000, "hide3")
                    ),
                new State("hide3",
                    new Order(100, "Monster Cage", "spawn"),
                    new Order(100, "Dr Terrible Bubble", "Bubble time"),
                    new Order(100, "Red Gas Spawner UL", "OFF"),
                    new Order(100, "Red Gas Spawner UR", "OFF"),
                    new Order(100, "Red Gas Spawner LL", "OFF"),
                    new Order(100, "Red Gas Spawner LR", "OFF"),
                    new ReturnToSpawn(speed: 1),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),
                    new TimedTransition(15000, "nohide3")
                    ),
                new State("nohide3",
                    new Order(100, "Monster Cage", "no spawn"),
                    new Order(100, "Dr Terrible Bubble", "nothing change"),
                    new Order(100, "Red Gas Spawner UL", "ON"),
                    new Order(100, "Red Gas Spawner UR", "ON"),
                    new Order(100, "Red Gas Spawner LL", "ON"),
                    new Order(100, "Red Gas Spawner LR", "OFF"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Green Potion", coolDown: 2000, coolDownOffset: 0),
                    new TimedTransition(12000, "TA4")
                    ),
                new State("TA4",
                    new Order(100, "Monster Cage", "no spawn"),
                    new Order(100, "Dr Terrible Bubble", "nothing change"),
                    new Order(100, "Red Gas Spawner UL", "ON"),
                    new Order(100, "Red Gas Spawner UR", "ON"),
                    new Order(100, "Red Gas Spawner LL", "ON"),
                    new Order(100, "Red Gas Spawner LR", "OFF"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Green Potion", coolDown: 2000, coolDownOffset: 0),
                    new TimedTransition(10000, "hide4")
                    ),
                new State("hide4",
                    new Order(100, "Monster Cage", "spawn"),
                    new Order(100, "Dr Terrible Bubble", "Bubble time"),
                    new Order(100, "Red Gas Spawner UL", "OFF"),
                    new Order(100, "Red Gas Spawner UR", "OFF"),
                    new Order(100, "Red Gas Spawner LL", "OFF"),
                    new Order(100, "Red Gas Spawner LR", "OFF"),
                    new ReturnToSpawn(speed: 1),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),
                    new TimedTransition(15000, "idle")
                    )
                ),
                    new Threshold(0.32,
                    new ItemLoot("Potion of Wisdom", 1)
                ),
                new Threshold(0.1,
                    new ItemLoot("Scepter of Fulmination", 0.005),
                    new ItemLoot("Robe of the Mad Scientist", 0.005),
                    new ItemLoot("Experimental Ring", 0.01),
                    new ItemLoot("Wine Cellar Incantation", 0.005),
                    new ItemLoot("Golden Nut", 0.01),
                    new ItemLoot("Golden Bolt", 0.01),
                    new ItemLoot("The Magician Tarot Card", 0.01),
                    new ItemLoot("Robobuddy Egg", 0.05),

                    new TierLoot(8, ItemType.Weapon, 0.2),
                    new TierLoot(9, ItemType.Weapon, 0.15),
                    new TierLoot(10, ItemType.Weapon, 0.1),
                    new TierLoot(11, ItemType.Weapon, 0.05),

                    new TierLoot(8, ItemType.Armor, 0.2),
                    new TierLoot(9, ItemType.Armor, 0.15),
                    new TierLoot(10, ItemType.Armor, 0.1),
                    new TierLoot(11, ItemType.Armor, 0.05),

                    new TierLoot(4, ItemType.Ability, 0.15),
                    new TierLoot(5, ItemType.Ability, 0.1)
                    )
            )
        .Init("Dr Terrible Mini Bot",
            new State(
                 new Wander(0.5),
                 new Shoot(10, 2, 20, angleOffset: 0 / 2, projectileIndex: 0, coolDown: 1000)
                 )
            )
        .Init("Dr Terrible Rampage Cyborg",
            new State(
                new State("idle",
                    new PlayerWithinTransition(10, "normal"),
                new State("normal",
                    new Wander(0.5),
                    new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 0, predictive: 1,
                    coolDown: 800, coolDownOffset: 0),
                    new HpLessTransition(.2, "blink"),
                    new TimedTransition(10000, "rage blink")
                    ),
                new State("rage blink",
                    new Wander(0.5),
                    new Flash(0xf0e68c, flashRepeats: 5, flashPeriod: 0.1),
                    new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 1, predictive: 1,
                    coolDown: 800, coolDownOffset: 0),
                    new HpLessTransition(.2, "blink"),
                    new TimedTransition(3000, "rage")
                    ),
                new State("rage",
                     new Wander(0.5),
                    new Flash(0xf0e68c, flashRepeats: 5, flashPeriod: 0.1),
                    new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 1, predictive: 1,
                    coolDown: 800, coolDownOffset: 0),
                    new HpLessTransition(.2, "blink")
                    ),
                new State("blink",
                    new Wander(0.5),
                    new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                    new Flash(0xfFF0000, flashRepeats: 10000, flashPeriod: 0.1),
                    new TimedTransition(2000, "explode")
                    ),
                new State("explode",
                    new Flash(0xfFF0000, 1, 9000001),
                    new Shoot(10, count: 8, projectileIndex: 2, fixedAngle: fixedAngle_RingAttack2),
                    new Suicide()
                )
                    )
            )
            )
        .Init("Dr Terrible Escaped Experiment",
              new State(
                  new Wander(0.5),
                  new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 1, predictive: 1,
                  coolDown: 800, coolDownOffset: 0)
                  )
               )
            .Init("Mini Bot",
            new State(
                 new Wander(0.5),
                 new Shoot(10, 2, 20, angleOffset: 0 / 2, projectileIndex: 0, coolDown: 1000)
                 )
            )
         .Init("Rampage Cyborg",
            new State(
                new State("idle",
                    new PlayerWithinTransition(10, "normal"),
                new State("normal",
                    new Wander(0.5),
                    new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 0, predictive: 1,
                    coolDown: 800, coolDownOffset: 0),
                    new HpLessTransition(.2, "blink"),
                    new TimedTransition(10000, "rage blink")
                    ),
                new State("rage blink",
                    new Wander(0.5),
                    new Flash(0xf0e68c, flashRepeats: 5, flashPeriod: 0.1),
                    new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 1, predictive: 1,
                    coolDown: 800, coolDownOffset: 0),
                    new HpLessTransition(.2, "blink"),
                    new TimedTransition(3000, "rage")
                    ),
                new State("rage",
                    new Wander(0.5),
                    new Flash(0xf0e68c, flashRepeats: 5, flashPeriod: 0.1),
                    new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 1, predictive: 1,
                    coolDown: 800, coolDownOffset: 0),
                    new HpLessTransition(.2, "blink")
                    ),
                new State("blink",
                    new Wander(0.5),
                    new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                    new Flash(0xfFF0000, flashRepeats: 10000, flashPeriod: 0.1),
                    new TimedTransition(2000, "explode")
                    ),
                new State("explode",
                    new Flash(0xfFF0000, 1, 9000001),
                    new Shoot(10, count: 8, projectileIndex: 2, fixedAngle: fixedAngle_RingAttack2),
                    new Suicide()
                )
                    )
            )
                )
        .Init("Escaped Experiment",
              new State(
                  new Wander(0.5),
                  new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 0, predictive: 1,
                  coolDown: 800, coolDownOffset: 0)
                  )
            )
        .Init("West Automated Defense Turret",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Shoot(32, fixedAngle: 0, coolDown: new Cooldown(3000, 1000))
                    )
            )
        .Init("East Automated Defense Turret",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Shoot(32, fixedAngle: 180, coolDown: new Cooldown(3000, 1000))
                    )
            )
        .Init("Crusher Abomination",
            new State(
                new State("1 step",
                    new Wander(0.5),
                    new Shoot(10, 3, 20, angleOffset: 0 / 3, projectileIndex: 0, coolDown: 1000),
                    new HpLessTransition(.75, "2 step")
                    ),
                new State("2 step",
                    new Wander(0.5),
                    new ChangeSize(11, 150),
                    new Shoot(10, 2, 20, angleOffset: 0 / 3, projectileIndex: 1, coolDown: 1000),
                    new HpLessTransition(.5, "3 step")
                    ),
                new State("3 step",
                     new Wander(0.5),
                    new ChangeSize(11, 175),
                    new Shoot(10, 2, 20, angleOffset: 0 / 3, projectileIndex: 2, coolDown: 1000),
                    new HpLessTransition(.25, "4 step")
                    ),
                new State("4 step",
                    new Wander(0.5),
                    new ChangeSize(11, 200),
                    new Shoot(10, 2, 20, angleOffset: 0 / 3, projectileIndex: 3, coolDown: 1000)
                    )
                )
            )
        .Init("Enforcer Bot 3000",
            new State(
                new Wander(0.5),
                new Shoot(10, 3, 20, angleOffset: 0 / 3, projectileIndex: 0, coolDown: 1000),
                new Shoot(10, 4, 20, angleOffset: 0 / 4, projectileIndex: 1, coolDown: 1000),
                new TransformOnDeath("Mini Bot", 0, 3)

                )
            )
        .Init("Green Potion",
            new State(
                new State("Idle",
                    new TimedTransition(2000, "explode")
                    ),
                new State("explode",
                      new Shoot(10, count: 6, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2),
                      new Suicide()
                    )
                )
            )
        .Init("Red Gas Spawner UL",
            new State(
                new EntityNotExistsTransition("Dr Terrible", 50, "OFF"),
                new State("OFF",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                new State("ON",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Shoot(10, count: 20, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2)
                )
                )
        )
        .Init("Red Gas Spawner UR",
           new State(
               new EntityNotExistsTransition("Dr Terrible", 50, "OFF"),
                new State("OFF",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                new State("ON",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Shoot(10, count: 20, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2)
                )
                )
        )
        .Init("Red Gas Spawner LL",
            new State(
                new EntityNotExistsTransition("Dr Terrible", 50, "OFF"),
                new State("OFF",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                new State("ON",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Shoot(10, count: 20, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2)
                )
                )
        )
        .Init("Red Gas Spawner LR",
            new State(
                new EntityNotExistsTransition("Dr Terrible", 50, "OFF"),
                new State("OFF",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                new State("ON",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Shoot(10, count: 20, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2)
                )
                )
        )
        .Init("Turret Attack",
            new State(
                new Shoot(10, 2, 20, angleOffset: 0 / 2, projectileIndex: 0, coolDown: 1000)
            )
        )
        .Init("Mad Lab Open Wall",//need fix
            new State(
                new TransformOnDeath("Invisible"),
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("destroy all tesla coils",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new EntityNotExistsTransition("Tesla Coil", 1000, "Suicide :D")
                    ),
                new State("Suicide :D",
                    new Suicide()
                )
                )
        )
        .Init("Mad Scientist Summoner",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new RealmPortalDrop(),
                new State("idle",
                     new EntitiesNotExistsTransition(300, "Death", "Dr Terrible")
                    ),
                new State("Death",
                    new Suicide()
                    )
                )
            )
        .Init("Dr Terrible Bubble",
            new State(
                new State("nothing change",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    //new SetAltTexture(0)
                    ),
                new State("Bubble time",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    //new SetAltTexture(1),
                    new TimedTransition(1000, "Bubble time2")
                    ),
                new State("Bubble time2",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    //new SetAltTexture(2),
                    new TimedTransition(1000, "Bubble time")
                )
            )
            )
        .Init("Mad Gas Controller", //don't need xD
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible, true)
               )
           )
        .Init("Monster Cage",
            new State(
                new State("no spawn",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                //new SetAltTexture(0)
                ),
                new State("spawn",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    // new SetAltTexture(2),
                    new Spawn("Dr Terrible Rampage Cyborg", maxChildren: 1, initialSpawn: 0.5),
                    new Spawn("Dr Terrible Mini Bot", maxChildren: 1, initialSpawn: 0.5),
                    new Spawn("Dr Terrible Escaped Experiment", maxChildren: 1, initialSpawn: 0.5)
                    )
                )
            );

    }
}
