#region

using System;
using System.Linq;
using db.data;
using wServer.logic.loot;
using wServer.realm.entities;

#endregion

namespace wServer.realm.setpieces
{
    internal class Castle : ISetPiece
    {
        private static readonly string Floor = "Rock";
        private static readonly string Bridge = "Bridge";
        private static readonly string WaterA = "Shallow Water";
        private static readonly string WaterB = "Dark Water";
        private static readonly string WallA = "Grey Wall";
        private static readonly string WallB = "Destructible Grey Wall";

        private static readonly Loot chest = new Loot(
            new TierLoot(6, ItemType.Weapon, 0.3),
            new TierLoot(7, ItemType.Weapon, 0.2),
            new TierLoot(8, ItemType.Weapon, 0.1),
            new TierLoot(5, ItemType.Armor, 0.3),
            new TierLoot(6, ItemType.Armor, 0.2),
            new TierLoot(7, ItemType.Armor, 0.1),
            new TierLoot(2, ItemType.Ability, 0.3),
            new TierLoot(3, ItemType.Ability, 0.2),
            new TierLoot(4, ItemType.Ability, 0.1),
            new TierLoot(2, ItemType.Ring, 0.25),
            new TierLoot(3, ItemType.Ring, 0.15),
            new TierLoot(1, ItemType.Potion, 0.5)
            );

        private readonly Random rand = new Random();

        public int Size
        {
            get { return 40; }
        }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            int[,] t = new int[31, 40];

            for (int x = 0; x < 13; x++) //Moats
                for (int y = 0; y < 13; y++)
                {
                    if ((x == 0 && (y < 3 || y > 9)) ||
                        (y == 0 && (x < 3 || x > 9)) ||
                        (x == 12 && (y < 3 || y > 9)) ||
                        (y == 12 && (x < 3 || x > 9)))
                        continue;
                    t[x + 0, y + 0] = t[x + 18, y + 0] = 2;
                    t[x + 0, y + 27] = t[x + 18, y + 27] = 2;
                }
            for (int x = 3; x < 28; x++)
                for (int y = 3; y < 37; y++)
                {
                    if (x < 6 || x > 24 || y < 6 || y > 33)
                        t[x, y] = 2;
                }

            for (int x = 7; x < 24; x++) //Floor
                for (int y = 7; y < 33; y++)
                    t[x, y] = rand.Next()%3 == 0 ? 0 : 1;

            for (int x = 0; x < 7; x++) //Perimeter
                for (int y = 0; y < 7; y++)
                {
                    if ((x == 0 && y != 3) ||
                        (y == 0 && x != 3) ||
                        (x == 6 && y != 3) ||
                        (y == 6 && x != 3))
                        continue;
                    t[x + 3, y + 3] = t[x + 21, y + 3] = 4;
                    t[x + 3, y + 30] = t[x + 21, y + 30] = 4;
                }
            for (int x = 6; x < 25; x++)
                t[x, 6] = t[x, 33] = 4;
            for (int y = 6; y < 34; y++)
                t[6, y] = t[24, y] = 4;

            for (int x = 13; x < 18; x++) //Bridge
                for (int y = 3; y < 7; y++)
                    t[x, y] = 6;

            for (int x = 0; x < 31; x++) //Corruption
                for (int y = 0; y < 40; y++)
                {
                    if (t[x, y] == 1 || t[x, y] == 0) continue;
                    double p = rand.NextDouble();
                    if (t[x, y] == 6)
                    {
                        if (p < 0.4)
                            t[x, y] = 0;
                        continue;
                    }

                    if (p < 0.1)
                        t[x, y] = 1;
                    else if (p < 0.4)
                        t[x, y]++;
                }

            //Boss & Chest
            t[15, 27] = 7;
            t[15, 20] = 8;

            int r = rand.Next(0, 4);
            for (int i = 0; i < r; i++) //Rotation
                t = SetPieces.rotateCW(t);
            int w = t.GetLength(0), h = t.GetLength(1);

            XmlData dat = world.Manager.GameData;
            for (int x = 0; x < w; x++) //Rendering
                for (int y = 0; y < h; y++)
                {
                    if (t[x, y] == 1)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = dat.IdToTileType[Floor];
                        tile.ObjType = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }

                    else if (t[x, y] == 2)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = dat.IdToTileType[WaterA];
                        tile.ObjType = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 3)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = dat.IdToTileType[WaterB];
                        tile.ObjType = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }

                    else if (t[x, y] == 4)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = dat.IdToTileType[Floor];
                        tile.ObjType = dat.IdToObjectType[WallA];
                        if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 5)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = dat.IdToTileType[Floor];
                        world.Map[x + pos.X, y + pos.Y] = tile;
                        Entity wall = Entity.Resolve(world.Manager, dat.IdToObjectType[WallB]);
                        wall.Move(x + pos.X + 0.5f, y + pos.Y + 0.5f);
                        world.EnterWorld(wall);
                    }

                    else if (t[x, y] == 6)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = dat.IdToTileType[Bridge];
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 7)
                    {
                        Container container = new Container(world.Manager, 0x0501, null, false);
                        Item[] items = chest.GetLoots(world.Manager, 5, 8).ToArray();
                        for (int i = 0; i < items.Length; i++)
                            container.Inventory[i] = items[i];
                        container.Move(pos.X + x + 0.5f, pos.Y + y + 0.5f);
                        world.EnterWorld(container);
                    }
                    else if (t[x, y] == 8)
                    {
                        Entity cyclops = Entity.Resolve(world.Manager, "Cyclops God");
                        cyclops.Move(pos.X + x, pos.Y + y);
                        world.EnterWorld(cyclops);
                    }
                }
        }
    }
}