using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ CandyLand = () => Behav()
        .Init("Candy Gnome",
            new State(
                new DropPortalOnDeath("Candyland Portal", percent: 50, PortalDespawnTimeSec: 120),
                new Prioritize(
                    new StayBack(1.5, 55),
                    new Wander(1.4)
                    )
                ),
            new Threshold(0.18,
                new ItemLoot("Red Gumball", 0.5),
                new ItemLoot("Purple Gumball", 0.5),
                new ItemLoot("Blue Gumball", 0.5),
                new ItemLoot("Green Gumball", 0.5),
                new ItemLoot("Yellow Gumball", 0.5)
                )
            )
            .Init("Desire Troll",
                new State(
                    new Wander(0.5),
                    new Grenade(6, 200, range: 8, coolDown: 3000),
                    new Shoot(15, 3, 5, angleOffset: 30 / 3, projectileIndex: 0, coolDown: 2100),
                    new Shoot(15, 5, 10, angleOffset: 60 / 3, projectileIndex: 2, coolDown: 1950),
                    new Shoot(15, 1, 0, angleOffset: 30 / 3, projectileIndex: 1, coolDown: 1950)
                ),
                new TierLoot(6, ItemType.Weapon, 0.04),
                new TierLoot(7, ItemType.Weapon, 0.02),
                new TierLoot(8, ItemType.Weapon, 0.01),
                new TierLoot(7, ItemType.Armor, 0.04),
                new TierLoot(8, ItemType.Armor, 0.02),
                new TierLoot(9, ItemType.Armor, 0.01),
                new TierLoot(3, ItemType.Ring, 0.015),
                new TierLoot(4, ItemType.Ring, 0.005),
                new Threshold(0.18,
                    new ItemLoot("Potion of Defense", 0.02),
                    new ItemLoot("Potion of Attack", 0.02),
                    new ItemLoot("Yellow Gumball", 0.3),
                    new ItemLoot("Green Gumball", 0.3),
                    new ItemLoot("Blue Gumball", 0.3),
                    new ItemLoot("Red Gumball", 0.3),
                    new ItemLoot("Blue Gumball", 0.3),
                    new ItemLoot("Fairy Plate", 0.009),
                    new ItemLoot("Pixie-Enchanted Sword", 0.009),
                    new ItemLoot("Seal of the Enchanted Forest", 0.009),
                    new ItemLoot("Candy-Coated Armor", 0.01),
                    new ItemLoot("Wine Cellar Incantation", 0.01),
                    new ItemLoot("Ring of Pure Wishes", 0.009),
                    new ItemLoot("Potion of Attack", 0.005),
                    new ItemLoot("Potion of Wisdom", 0.005)
                )
            )
            .Init("Gigacorn",
                new State(
                    new Wander(0.5),
                    new Charge(2.0, 10f, 4000),
                    new Shoot(20, 1, 40, angleOffset: 60 / 3, projectileIndex: 0, coolDown: 2100),
                    new Shoot(20, 1, 40, angleOffset: 60 / 3, projectileIndex: 0, coolDown: 2200),
                    new Shoot(20, 1, 40, angleOffset: 60 / 3, projectileIndex: 0, coolDown: 2300),
                    new Shoot(20, 1, 40, angleOffset: 60 / 3, projectileIndex: 0, coolDown: 2400),
                    new Shoot(20, 3, 15, angleOffset: 40 / 3, projectileIndex: 1, coolDown: 4000),
                    new Shoot(20, 3, 15, angleOffset: 20 / 3, projectileIndex: 1, coolDown: 2000)
                ),
                new TierLoot(6, ItemType.Weapon, 0.04),
                new TierLoot(7, ItemType.Weapon, 0.02),
                new TierLoot(8, ItemType.Weapon, 0.01),
                new TierLoot(7, ItemType.Armor, 0.04),
                new TierLoot(8, ItemType.Armor, 0.02),
                new TierLoot(9, ItemType.Armor, 0.01),
                new TierLoot(3, ItemType.Ring, 0.015),
                new TierLoot(4, ItemType.Ring, 0.005),
                new Threshold(0.18,
                    new ItemLoot("Potion of Defense", 0.02),
                    new ItemLoot("Potion of Attack", 0.02),
                    new ItemLoot("Yellow Gumball", 0.3),
                    new ItemLoot("Green Gumball", 0.3),
                    new ItemLoot("Blue Gumball", 0.3),
                    new ItemLoot("Red Gumball", 0.3),
                    new ItemLoot("Blue Gumball", 0.3),
                    new ItemLoot("Fairy Plate", 0.009),
                    new ItemLoot("Pixie-Enchanted Sword", 0.009),
                    new ItemLoot("Seal of the Enchanted Forest", 0.009),
                    new ItemLoot("Candy-Coated Armor", 0.01),
                    new ItemLoot("Wine Cellar Incantation", 0.01),
                    new ItemLoot("Ring of Pure Wishes", 0.009),
                    new ItemLoot("Potion of Defense", 0.005)
                )
            )
            .Init("Spoiled Creampuff",
                new State(
                    new Shoot(20, 2, 40, angleOffset: 60 / 3, projectileIndex: 0, coolDown: 1500),
                    new Shoot(20, 4, 15, angleOffset: 40 / 3, projectileIndex: 1, coolDown: 1000),
                    new Spawn("Big Creampuff", maxChildren: 2, initialSpawn: 2, coolDown: 5000)
                ),
                new TierLoot(6, ItemType.Weapon, 0.04),
                new TierLoot(7, ItemType.Weapon, 0.02),
                new TierLoot(8, ItemType.Weapon, 0.01),
                new TierLoot(7, ItemType.Armor, 0.04),
                new TierLoot(8, ItemType.Armor, 0.02),
                new TierLoot(9, ItemType.Armor, 0.01),
                new TierLoot(3, ItemType.Ring, 0.015),
                new TierLoot(4, ItemType.Ring, 0.005),
                new Threshold(0.18,
                    new ItemLoot("Potion of Defense", 0.02),
                    new ItemLoot("Potion of Attack", 0.02),
                    new ItemLoot("Yellow Gumball", 0.3),
                    new ItemLoot("Green Gumball", 0.3),
                    new ItemLoot("Blue Gumball", 0.3),
                    new ItemLoot("Red Gumball", 0.3),
                    new ItemLoot("Blue Gumball", 0.3),
                    new ItemLoot("Fairy Plate", 0.009),
                    new ItemLoot("Pixie-Enchanted Sword", 0.009),
                    new ItemLoot("Seal of the Enchanted Forest", 0.009),
                    new ItemLoot("Candy-Coated Armor", 0.01),
                    new ItemLoot("Ring of Pure Wishes", 0.009),
                    new ItemLoot("Potion of Defense", 0.005)
                )
            )
            .Init("Big Creampuff",
                new State(
                    new Wander(0.5),
                    new Shoot(20, 1, 0, angleOffset: 40 / 3, projectileIndex: 0, coolDown: 1000),
                    new Spawn("Small Creampuff", maxChildren: 2, initialSpawn: 0.5, coolDown: 5000)
                )
            )
            .Init("Small Creampuff",
                new State(
                    new Wander(.5),
                    new Shoot(20, 3, 30, angleOffset: 40 / 3, projectileIndex: 1, coolDown: 1400)
                )
            );
    }
}
