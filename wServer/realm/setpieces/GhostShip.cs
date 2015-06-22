using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.logic.loot;
using wServer.realm.entities;
using db.data;

namespace wServer.realm.setpieces
{
    class GhostShip : ISetPiece
    {
        public int Size
        {
            get { return 50; }
        }

        private static readonly string Tree = "Tree Jungle";
        private static readonly string Water = "GhostWater";
        private static readonly string Sand = "Ghost Water Beach";
        
        Random rand = new Random();
        public void RenderSetPiece(World world, IntPoint pos)
        {
            XmlData dat = world.Manager.GameData;
            int DarkGrassradiu = 17;
            int sandRadius = 17;
            int waterRadius = 14;

            List<IntPoint> border = new List<IntPoint>();

            int[,] o = new int[Size, Size];
            int[,] t = new int[Size, Size];

            for (int y = 0; y < Size; y++)      //Outer
                for (int x = 0; x < Size; x++)
                {
                    double dx = x - (Size / 2.0);
                    double dy = y - (Size / 2.0);
                    double r = Math.Sqrt(dx * dx + dy * dy);
                    if (r <= sandRadius)
                        t[x, y] = 2;
                }
            for (int y = 0; y < Size; y++)      //Water
                for (int x = 0; x < Size; x++)
                {
                    double dx = x - (Size / 2.0);
                    double dy = y - (Size / 2.0);
                    double r = Math.Sqrt(dx * dx + dy * dy);
                    if (r <= waterRadius)
                    {
                        t[x, y] = 3;
                    }
                }

            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++)
                {
                    if (((x > 5 && x < DarkGrassradiu) || (x < Size - 5 && x > Size - DarkGrassradiu) ||
                         (y > 5 && y < DarkGrassradiu) || (y < Size - 5 && y > Size - DarkGrassradiu)) &&
                        o[x, y] == 0 && t[x, y] == 1)
                    {
                        t[x, y] = 4;
                    }
                }
            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++)
                {
                    if (t[x, y] == 1)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.ObjId = dat.IdToObjectType[Tree];
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 2)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = dat.IdToTileType[Sand];
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 3)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = dat.IdToTileType[Water];
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 4)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.ObjId = dat.IdToObjectType[Tree];
                        if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                }

            Entity gship = Entity.Resolve(world.Manager, "Ghost Ship");
            gship.Move(pos.X + Size / 2f, pos.Y + Size / 2f);
            world.EnterWorld(gship);

            Entity gshipanchor = Entity.Resolve(world.Manager, "Ghost Ship Anchor");
            gshipanchor.Move(pos.X + Size / 2f, pos.Y + Size / 2f);
            world.EnterWorld(gshipanchor);

        }


    }
}