#region

using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

#endregion

namespace wServer.realm.setpieces
{
    internal class SetPieces
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (SetPieces));

        private static readonly List<Tuple<ISetPiece, int, int, WmapTerrain[]>> setPieces = new List
            <Tuple<ISetPiece, int, int, WmapTerrain[]>>
        {
            SetPiece(new Building(), 80, 100, WmapTerrain.LowForest, WmapTerrain.LowPlains, WmapTerrain.MidForest),
            SetPiece(new Graveyard(), 5, 10, WmapTerrain.LowSand, WmapTerrain.LowPlains),
            SetPiece(new Grove(), 17, 25, WmapTerrain.MidForest, WmapTerrain.MidPlains),
            SetPiece(new LichyTemple(), 4, 7, WmapTerrain.MidForest, WmapTerrain.MidPlains),
            SetPiece(new Castle(), 4, 7, WmapTerrain.HighForest, WmapTerrain.HighPlains),
            SetPiece(new Tower(), 8, 15, WmapTerrain.HighForest, WmapTerrain.HighPlains),
            SetPiece(new TempleA(), 10, 20, WmapTerrain.MidForest, WmapTerrain.MidPlains),
            SetPiece(new TempleB(), 10, 20, WmapTerrain.MidForest, WmapTerrain.MidPlains),
            SetPiece(new Oasis(), 0, 5, WmapTerrain.LowSand, WmapTerrain.MidSand),
            SetPiece(new Pyre(), 0, 5, WmapTerrain.MidSand, WmapTerrain.HighSand),
            SetPiece(new LavaFissure(), 3, 5, WmapTerrain.Mountains)
        };

        private static Tuple<ISetPiece, int, int, WmapTerrain[]> SetPiece(ISetPiece piece, int min, int max,
            params WmapTerrain[] terrains)
        {
            return Tuple.Create(piece, min, max, terrains);
        }

        public static int[,] rotateCW(int[,] mat)
        {
            int M = mat.GetLength(0);
            int N = mat.GetLength(1);
            int[,] ret = new int[N, M];
            for (int r = 0; r < M; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    ret[c, M - 1 - r] = mat[r, c];
                }
            }
            return ret;
        }

        public static int[,] reflectVert(int[,] mat)
        {
            int M = mat.GetLength(0);
            int N = mat.GetLength(1);
            int[,] ret = new int[M, N];
            for (int x = 0; x < M; x++)
                for (int y = 0; y < N; y++)
                    ret[x, N - y - 1] = mat[x, y];
            return ret;
        }

        public static int[,] reflectHori(int[,] mat)
        {
            int M = mat.GetLength(0);
            int N = mat.GetLength(1);
            int[,] ret = new int[M, N];
            for (int x = 0; x < M; x++)
                for (int y = 0; y < N; y++)
                    ret[M - x - 1, y] = mat[x, y];
            return ret;
        }

        //private static int DistSqr(IntPoint a, IntPoint b)
        //{
        //    return (a.X - b.X)*(a.X - b.X) + (a.Y - b.Y)*(a.Y - b.Y);
        //}

        public static void ApplySetPieces(World world)
        {
            log.InfoFormat("Applying set pieces to world {0}({1}).", world.Id, world.Name);

            Wmap map = world.Map;
            int w = map.Width, h = map.Height;

            Random rand = new Random();
            HashSet<Rect> rects = new HashSet<Rect>();
            foreach (Tuple<ISetPiece, int, int, WmapTerrain[]> dat in setPieces)
            {
                int size = dat.Item1.Size;
                int count = rand.Next(dat.Item2, dat.Item3);
                for (int i = 0; i < count; i++)
                {
                    IntPoint pt = new IntPoint();
                    Rect rect;

                    int max = 50;
                    do
                    {
                        pt.X = rand.Next(0, w);
                        pt.Y = rand.Next(0, h);
                        rect = new Rect {x = pt.X, y = pt.Y, w = size, h = size};
                        max--;
                    } while ((Array.IndexOf(dat.Item4, map[pt.X, pt.Y].Terrain) == -1 ||
                              rects.Any(_ => Rect.Intersects(rect, _))) &&
                             max > 0);
                    if (max <= 0) continue;
                    dat.Item1.RenderSetPiece(world, pt);
                    rects.Add(rect);
                }
            }

            log.Info("Set pieces applied.");
        }

        private struct Rect
        {
            public int h;
            public int w;
            public int x;
            public int y;

            public static bool Intersects(Rect r1, Rect r2)
            {
                return !(r2.x > r1.x + r1.w ||
                         r2.x + r2.w < r1.x ||
                         r2.y > r1.y + r1.h ||
                         r2.y + r2.h < r1.y);
            }
        }
    }
}