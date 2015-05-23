using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.entities.player;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ LordOfTheLostLands = () => Behav()
            .Init("Lord of the Lost Lands",
                new State(
                    new DropPortalOnDeath("Ice Cave Portal", 10),
                    new HpLessTransition(0.15, "IMDONELIKESOOOODONE!"),
                    new State("timetogeticey",
                        new PlayerWithinTransition(8, "startupandfireup")
                        ),
                    new State("startupandfireup",
                        new SetAltTexture(0),
                        new Wander(0.3),
                        new Shoot(10, count: 7, shootAngle: 7, coolDownOffset: 1100, angleOffset: 270, coolDown: 2250),
                        new Shoot(10, count: 7, shootAngle: 7, coolDownOffset: 1100, angleOffset: 90, coolDown: 2250),

                        new Shoot(10, count: 7, shootAngle: 7, coolDown: 2250),
                        new Shoot(10, count: 7, shootAngle: 7, angleOffset: 180, coolDown: 2250),

                        new TimedTransition(8500, "GatherUp")
                        ),
                    new State("GatherUp",
                        new SetAltTexture(3),
                        new Taunt("GATHERING POWER!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(8.4, count: 6, shootAngle: 60, projectileIndex: 1, coolDown: 4550),
                        new Shoot(8.4, count: 6, shootAngle: 60, predictive: 2, projectileIndex: 1, coolDown: 2700),
                        new TimedTransition(5750, "protect")
                        ),
                    new State("protect",
                        //Minions spawn
                        new TossObject("Guardian of the Lost Lands", 5, 0, coolDown: 9999999, randomToss: false),
                        new TossObject("Guardian of the Lost Lands", 5, 90, coolDown: 9999999, randomToss: false),
                        new TossObject("Guardian of the Lost Lands", 5, 180, coolDown: 9999999, randomToss: false),
                        new TossObject("Guardian of the Lost Lands", 5, 270, coolDown: 9999999, randomToss: false),
                        new TimedTransition(1000, "crystals")
                        ),
                    new State("crystals",
                        new SetAltTexture(1),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TossObject("Protection Crystal", 4, 0, coolDown: 9999999, randomToss: false),
                        new TossObject("Protection Crystal", 4, 45, coolDown: 9999999, randomToss: false),
                        new TossObject("Protection Crystal", 4, 90, coolDown: 9999999, randomToss: false),
                        new TossObject("Protection Crystal", 4, 135, coolDown: 9999999, randomToss: false),
                        new TossObject("Protection Crystal", 4, 180, coolDown: 9999999, randomToss: false),
                        new TossObject("Protection Crystal", 4, 225, coolDown: 9999999, randomToss: false),
                        new TossObject("Protection Crystal", 4, 270, coolDown: 9999999, randomToss: false),
                        new TossObject("Protection Crystal", 4, 315, coolDown: 9999999, randomToss: false),
                        new TimedTransition(2100, "checkforcrystals")
                        ),
                    new State("checkforcrystals",
                        new SetAltTexture(1),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(9999, "startupandfireup", "Protection Crystal")
                        ),
                    new State("IMDONELIKESOOOODONE!",
                        new Taunt("NOOOOOOOOOOOOOOO!"),
                        new SetAltTexture(3),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Flash(0xFF0000, 0.2, 3),
                        new TimedTransition(5250, "dead")
                        ),
                    new State("dead",
                        new Shoot(8.4, count: 6, shootAngle: 60, projectileIndex: 1),
                        new Suicide()
                        )
                    ),
                new MostDamagers(3,
                    LootTemplates.StatIncreasePotionsLoot()
                ),
                new Threshold(0.05,
                    new ItemLoot("Shield of Ogmur", 0.005),
                    new TierLoot(8, ItemType.Weapon, 0.2),
                    new TierLoot(9, ItemType.Weapon, 0.175),
                    new TierLoot(10, ItemType.Weapon, 0.125),
                    new TierLoot(11, ItemType.Weapon, 0.05),
                    new TierLoot(8, ItemType.Armor, 0.2),
                    new TierLoot(9, ItemType.Armor, 0.175),
                    new TierLoot(10, ItemType.Armor, 0.15),
                    new TierLoot(11, ItemType.Armor, 0.1),
                    new TierLoot(12, ItemType.Armor, 0.05),
                    new TierLoot(4, ItemType.Ability, 0.15),
                    new TierLoot(5, ItemType.Ability, 0.1),
                    new TierLoot(5, ItemType.Ring, 0.05)
                )
            )
            .Init("Protection Crystal",
                new State(
                    new State("PROTECT!",
                        new Orbit(0.3, 3, 20, "Lord of the Lost Lands"),
                        new Shoot(8.4, count: 3, projectileIndex: 0, shootAngle: 10, coolDown: 100)
                        )
                    )
            )
            .Init("Guardian of the Lost Lands",
                new State(
                    new State("Tough",
                        new Follow(0.35, 8, 1),
                        new Spawn("Knight of the Lost Lands", initialSpawn: 1, maxChildren: 1),
                        new Shoot(8.4, count: 6, shootAngle: 60, projectileIndex: 1, coolDown: 2000),
                        new Shoot(8.4, count: 6, projectileIndex: 0, coolDown: 1300),
                        new HpLessTransition(0.35, "Scrub")
                        ),
                    new State("Scrub",
                        new StayBack(0.75, 5),
                        new Shoot(8.4, count: 6, shootAngle: 60, projectileIndex: 1, coolDown: 2000),
                        new Shoot(8.4, count: 5, projectileIndex: 0, coolDown: 1300)
                        )
                ),
                new ItemLoot("Health Potion", 0.07),
                new ItemLoot("Magic Potion", 0.07)
            )
            .Init("Knight of the Lost Lands",
                new State(
                    new State("Fighting",
                        new Follow(0.4, 8, 1),
                        new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 900)
                        )
                    ),
                new ItemLoot("Health Potion", 0.04),
                new ItemLoot("Magic Potion", 0.04)
            );
    }
}
