#region

using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Tomb = () => Behav()
            .Init("Tomb Defender",
                new State(
                    new State("idle",
                        new Taunt("THIS WILL NOW BE YOUR TOMB!"),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new HpLessTransition(.99, "weakning")
                        ),
                    new State("weakning",
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Taunt("Impudence! I am an immortal, I needn't take you seriously."),
                        new Shoot(50, 24, projectileIndex: 3, coolDown: 6000, coolDownOffset: 2000),
                        new HpLessTransition(.97, "active")
                        ),
                    new State("active",
                        new Orbit(.7, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Shoot(50, 8, projectileIndex: 2, coolDown: 1000, coolDownOffset: 500),
                        new Shoot(50, 4, projectileIndex: 1, coolDown: 3000, coolDownOffset: 500),
                        new Shoot(50, 6, projectileIndex: 0, coolDown: 3100, coolDownOffset: 500),
                        new HpLessTransition(.9, "boomerang")
                        ),
                    new State("boomerang",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Shoot(50, 8, projectileIndex: 2, coolDown: 1000, coolDownOffset: 500),
                        new Shoot(50, 8, 10, 1, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 1, 10, 0, coolDown: 4750, coolDownOffset: 500),
                        new HpLessTransition(.66, "double shot")
                        ),
                    new State("double shot",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(.7, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Shoot(50, 8, projectileIndex: 2, coolDown: 1000, coolDownOffset: 500),
                        new Shoot(50, 8, 10, 1, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 2, 10, 0, coolDown: 4750, coolDownOffset: 500),
                        new HpLessTransition(.5, "artifacts")
                        ),
                    new State("triple shot",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Shoot(50, 8, projectileIndex: 2, coolDown: 1000, coolDownOffset: 500),
                        new Shoot(50, 8, 10, 1, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 3, 10, 0, coolDown: 4750, coolDownOffset: 500),
                        new HpLessTransition(.1, "rage")
                        ),
                    new State("artifacts",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Taunt("My artifacts shall prove my wall of defense is impenetrable!"),
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Shoot(50, 8, projectileIndex: 2, coolDown: 1000, coolDownOffset: 500),
                        new Shoot(50, 8, 10, 1, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 2, 10, 0, coolDown: 4750, coolDownOffset: 500),
                        new Spawn("Pyramid Artifact 1", 3, 3, 2000),
                        new Reproduce("Pyramid Artifact 1", 10, 3, 1500),
                        new Spawn("Pyramid Artifact 2", 3, 0, 3500000),
                        new Reproduce("Pyramid Artifact 2", 10, 3, 1500),
                        new Spawn("Pyramid Artifact 3", 3, 0, 3500000),
                        new Reproduce("Pyramid Artifact 3", 10, 3, 1500),
                        new HpLessTransition(.33, "triple shot")
                        ),
                    new State("rage",
                        new Taunt("The end of your path is here!"),
                        new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                        new Flash(0xfFF0000, 1, 9000001),
                        new Shoot(50, 8, 10, 1, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 4, 10, 4, coolDown: 300),
                        new Shoot(50, 3, 10, 0, coolDown: 4750, coolDownOffset: 500)
                        )
                    ),
                    new Threshold(0.32,
                        new ItemLoot("Potion of Life", 1)
                    ),
                    new Threshold(0.1,
                        new ItemLoot("Ring of the Pyramid", 0.005),
                        new ItemLoot("Tome of Holy Protection", 0.005),
                        new ItemLoot("Wine Cellar Incantation", 0.005)
                    ),
                    new Threshold(0.2,
                        new EggLoot(EggRarity.Common, 0.1),
                        new EggLoot(EggRarity.Uncommon, 0.05),
                        new EggLoot(EggRarity.Rare, 0.01),
                        new EggLoot(EggRarity.Legendary, 0.002)
                    )
            )
            .Init("Tomb Support",
                new State(
                    new State("idle",
                        new Taunt("ENOUGH OF YOUR VANDALISM!"),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new HpLessTransition(.99, "weakning")
                        ),
                    new State("weakning",
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Taunt("Geb, eradicate these cretins from our tomb!"),
                        new Shoot(50, 24, projectileIndex: 3, coolDown: 6000, coolDownOffset: 2000),
                        new HpLessTransition(.97, "active")
                        ),
                    new State("active",
                        new Orbit(.7, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Shoot(50, 1, projectileIndex: 6, coolDown: 8000, shootAngle: 10, coolDownOffset: 500),
                        new Shoot(50, 1, projectileIndex: 5, coolDown: 5000, coolDownOffset: 3000),
                        new Shoot(50, 1, projectileIndex: 5, coolDown: 6000, coolDownOffset: 4000),
                        new HpLessTransition(.9, "boomerang")
                        ),
                    new State("boomerang",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Shoot(50, 4, projectileIndex: 2, coolDown: 3000, coolDownOffset: 1000),
                        new Shoot(50, 5, projectileIndex: 3, coolDown: 4000, coolDownOffset: 3000),
                        new Shoot(50, 5, projectileIndex: 4, coolDown: 5000, coolDownOffset: 5000) //,
            //new HpLessTransition(.66, "double shot")
                        ),
                    new State("rage",
                        new Taunt("This cannot be! You shall not succeed!")
                        )
                    ),
                    new Threshold(0.32,
                        new ItemLoot("Potion of Life", 1)
                    ),
                    new Threshold(0.1,
                        new ItemLoot("Ring of the Sphinx", 0.005),
                        new ItemLoot("Wine Cellar Incantation", 0.005)
                    ),
                    new Threshold(0.2,
                        new EggLoot(EggRarity.Common, 0.1),
                        new EggLoot(EggRarity.Uncommon, 0.05),
                        new EggLoot(EggRarity.Rare, 0.01),
                        new EggLoot(EggRarity.Legendary, 0.002)
                    )
            )

            .Init("Tomb Attacker",
                new State(
                    new State("idle",
                        new Taunt("ENOUGH OF YOUR VANDALISM!"),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new HpLessTransition(.99, "weakning")
                    ),
                    new State("weakning",
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Taunt("Geb, eradicate these cretins from our tomb!"),
                        new Shoot(50, 24, projectileIndex: 3, coolDown: 6000, coolDownOffset: 2000)
            //new HpLessTransition(.97, "active")
                    ),
                    new State("rage",
                        new Taunt("This cannot be! You shall not succeed!")
                    )
                ),
                new Threshold(0.32,
                        new ItemLoot("Potion of Life", 1)
                ),
                new Threshold(0.1,
                    new ItemLoot("Ring of the Nile", 0.002),
                    new ItemLoot("Wine Cellar Incantation", 0.002)
                ),
                new Threshold(0.2,
                    new EggLoot(EggRarity.Common, 0.1),
                    new EggLoot(EggRarity.Uncommon, 0.05),
                    new EggLoot(EggRarity.Rare, 0.01),
                    new EggLoot(EggRarity.Legendary, 0.002)
                )
            )

            //Minions
            .Init("Pyramid Artifact 1",
                new State(
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Defender", radiusVariance: 0.5),
                        new Follow(0.85, range: 1, duration: 5000, coolDown: 0)
                        ),
                    new Shoot(3, coolDown: 2500)
                    ))
            .Init("Pyramid Artifact 2",
                new State(
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Attacker", radiusVariance: 0.5),
                        new Follow(0.85, range: 1, duration: 5000, coolDown: 0)
                        ),
                    new Shoot(3, coolDown: 2500)
                    ))
            .Init("Pyramid Artifact 3",
                new State(
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Support", radiusVariance: 0.5),
                        new Follow(0.85, range: 1, duration: 5000, coolDown: 0)
                        ),
                    new Shoot(3, coolDown: 2500)
                    ))
            .Init("Tomb Defender Statue",
                new State(
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "checkActive"),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "checkInactive")
                        ),
                    new State("checkActive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("checkInactive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("ItsGoTime",
                        new Transform("Tomb Defender")
                        )
                    ))
            .Init("Tomb Support Statue",
                new State(
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "checkActive"),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "checkInactive")
                        ),
                    new State("checkActive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("checkInactive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("ItsGoTime",
                        new Transform("Tomb Support")
                        )
                    ))
            .Init("Tomb Attacker Statue",
                new State(
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "checkActive"),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "checkInactive")
                        ),
                    new State("checkActive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("checkInactive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("ItsGoTime",
                        new Transform("Tomb Attacker")
                        )
                    ))
            .Init("Inactive Sarcophagus",
                new State(
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Beam Priestess", 14, "checkPriest"),
                        new EntityNotExistsTransition("Beam Priest", 1000, "checkPriestess")
                        ),
                    new State("checkPriest",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Beam Priest", 1000, "activate")
                        ),
                    new State("checkPriestess",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Beam Priestess", 1000, "activate")
                        ),
                    new State("activate",
                        new Transform("Active Sarcophagus")
                        )
                    ))
            .Init("Scarab",
                new State(
                    new NoPlayerWithinTransition(7, "Idle"),
                    new PlayerWithinTransition(7, "Chase"),
                    new State("Idle",
                        new Wander(.1)
                    ),
                    new State("Chase",
                        new Follow(1.5, 7, 0),
                        new Shoot(3, projectileIndex: 1, coolDown: 500)
                    )
                )
                )
            .Init("Active Sarcophagus",
                new State(
                    new State(
                        new HpLessTransition(60, "stun")
                        ),
                    new State("stun",
                        new Shoot(50, 8, 10, 0, coolDown: 9999999, coolDownOffset: 500),
                        new Shoot(50, 8, 10, 0, coolDown: 9999999, coolDownOffset: 1000),
                        new Shoot(50, 8, 10, 0, coolDown: 9999999, coolDownOffset: 1500),
                        new TimedTransition(1500, "idle")
                        ),
                    new State("idle",
                        new ChangeSize(100, 100)
                        )
                    ),
                    new ItemLoot("Magic Potion", 0.002),
                    new ItemLoot("Health Potion", 0.15),
                    new Threshold(0.32,
                        new ItemLoot("Tincture of Mana", 0.15),
                        new ItemLoot("Tincture of Dexterity", 0.15),
                        new ItemLoot("Tincture of Life", 0.15)
                    ),
                    new Threshold(0.2,
                        new EggLoot(EggRarity.Common, 0.1),
                        new EggLoot(EggRarity.Uncommon, 0.05),
                        new EggLoot(EggRarity.Rare, 0.01),
                        new EggLoot(EggRarity.Legendary, 0.002)
                    )
            )
            .Init("Tomb Boss Anchor",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new RealmPortalDrop(),
                    new State("Idle",
                        new EntitiesNotExistsTransition(300, "Death", "Tomb Support", "Tomb Attacker", "Tomb Defender",
                            "Active Sarcophagus", "Tomb Defender Statue", "Tomb Support Statue", "Tomb Attacker Statue")
                    ),
                    new State("Death",
                        new RemoveEntity(10, "Tomb Portal of Cowardice"),
                        new Suicide()
                    )
                )
            );
    }
}