#region

using System;
using System.Collections.Generic;
using db.data;

#endregion

namespace wServer.realm.setpieces
{
    internal class Grove : ISetPiece
    {
        private static readonly string Floor = "Light Grass";
        private static readonly string Tree = "Cherry Tree";

        private readonly Random rand = new Random();

        public int Size
        {
            get { return 25; }
        }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            int radius = rand.Next(Size - 5, Size + 1)/2;
            List<IntPoint> border = new List<IntPoint>();

            int[,] t = new int[Size, Size];
            for (int y = 0; y < Size; y++)
                for (int x = 0; x < Size; x++)
                {
                    double dx = x - (Size/2.0);
                    double dy = y - (Size/2.0);
                    double r = Math.Sqrt(dx*dx + dy*dy);
                    if (r <= radius)
                    {
                        t[x, y] = 1;
                        if (radius - r < 1.5)
                            border.Add(new IntPoint(x, y));
                    }
                }

            HashSet<IntPoint> trees = new HashSet<IntPoint>();
            while (trees.Count < border.Count*0.5)
                trees.Add(border[rand.Next(0, border.Count)]);

            foreach (IntPoint i in trees)
                t[i.X, i.Y] = 2;

            XmlData dat = world.Manager.GameData;
            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++)
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
                        tile.TileId = dat.IdToTileType[Floor];
                        tile.ObjType = dat.IdToObjectType[Tree];
                        tile.Name = "size:" + (rand.Next()%2 == 0 ? 120 : 140);
                        if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                }

            Entity ent = Entity.Resolve(world.Manager, "Ent Ancient");
            ent.Size = 140;
            ent.Move(pos.X + Size/2 + 1, pos.Y + Size/2 + 1);
            world.EnterWorld(ent);
        }
    }
}