using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.logic.loot;
using wServer.logic.behaviors;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Pentaract = () => Behav()
            .Init("Pentaract",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Entry",
                        new PentaractStar(250),
                        new EntitiesNotExistsTransition(50, "Suicide", "Pentaract Tower"),
                        new State("EntryTimer",
                            new TimedTransition(15000, "RespawnTowers")
                        ),
                        new State("RespawnTowers",
                            new Order(50, "Pentaract Tower Corpse", "Respawn"),
                            new TimedTransition(0, "EntryTimer")
                        )
                    ),
                    new State("Suicide",
                        new Suicide()
                    )
                )
            )
            .Init("Pentaract Tower",
                new State(
                    new Spawn("Pentaract Eye", 5, 1),
                    new Grenade(4, 100, 8, coolDown: 2000),
                    new TransformOnDeath("Pentaract Tower Corpse"),
                    new CopyDamageOnDeath("Pentaract Tower Corpse", 2)
                )
            )
            .Init("Pentaract Eye",
                new State(
                    new Swirl(5, 20, targeted: false),
                    new Shoot(10, coolDown: 200)
                )
            )
            .Init("Pentaract Tower Corpse",
                new State(
                    new State("Entry",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new EntitiesNotExistsTransition(50, "Suicide", "Pentaract")
                    ),
                    new State("Respawn",
                        new Transform("Pentaract Tower")
                    ),
                    new State("Suicide",
                        new Suicide()
                    )
                ),
                new Threshold(0.3,
                    new ItemLoot("Potion of Defense", 1)
                ),
                new Threshold(0.2,
                    new ItemLoot("Potion of Speed", 0.5),
                    new ItemLoot("Potion of Wisdom", 0.5)
                ),
                new Threshold(0.1,
                    new ItemLoot("Seal of Blasphemous Prayer", 0.001),

                    new TierLoot(11, ItemType.Weapon, 0.005),
                    new TierLoot(11, ItemType.Armor, 0.005),
                    new TierLoot(5, ItemType.Ring, 0.005),

                    new TierLoot(10, ItemType.Weapon, 0.01),
                    new TierLoot(10, ItemType.Armor, 0.01),

                    new TierLoot(9, ItemType.Weapon, 0.02),
                    new TierLoot(5, ItemType.Ability, 0.02),
                    new TierLoot(9, ItemType.Armor, 0.02),

                    new ItemLoot("Potion of Attack", 0.1),
                    new ItemLoot("Potion of Vitality", 0.1),
                    new ItemLoot("Potion of Dexterity", 0.1),

                    new TierLoot(4, ItemType.Ability, 0.05),
                    new TierLoot(8, ItemType.Armor, 0.05),

                    new TierLoot(8, ItemType.Weapon, 0.1),
                    new TierLoot(7, ItemType.Armor, 0.1),
                    new TierLoot(3, ItemType.Ring, 0.1)
                )
            );
    }
}
