#region

using System;
using System.Collections.Generic;
using System.Linq;
using GeoAPI.Geometries;

#endregion

namespace terrain
{
    internal class MapFeatures
    {
        private readonly PolygonMap map;
        private readonly Random rand;

        public MapFeatures(PolygonMap map, int seed)
        {
            this.map = map;
            rand = new Random(seed);
        }

        private static MapEdge SelectDownhill(MapNode node)
        {
            double? dist = node.DistanceToCoast;
            MapEdge ret = node.Edges.First();
            foreach (MapEdge e in node.Edges)
                if (e.To.DistanceToCoast < dist)
                    ret = e;
            return ret;
        }

        public IEnumerable<MapNode[]> GenerateRivers()
        {
            int c = 0;
            MapNode[] eligibleRivers = map.Polygons
                .SelectMany(_ => _.Nodes)
                .Where(_ => _.DistanceToCoast > 0.25 && _.DistanceToCoast < 0.8)
                .ToArray();
            List<MapNode[]> ret = new List<MapNode[]>();
            do
            {
                MapNode node = eligibleRivers[rand.Next(0, eligibleRivers.Length)];
                Stack<MapEdge> edges = new Stack<MapEdge>();
                HashSet<MapNode> visited = new HashSet<MapNode>();
                edges.Push(SelectDownhill(node));

                List<MapNode> nodes = new List<MapNode>();
                nodes.Add(node);
                while (edges.Count > 0)
                {
                    MapEdge ed = edges.Pop();
                    nodes.Add(ed.To);

                    MapEdge edge = SelectDownhill(ed.To);
                    if (!edge.To.IsOcean && !visited.Contains(edge.To))
                    {
                        visited.Add(edge.To);
                        edges.Push(edge);
                    }
                }
                ret.Add(nodes.ToArray());
                c++;
            } while (c < 10);
            return ret;
        }

        public IEnumerable<Coordinate[]> GenerateRoads()
        {
            double[] heights = map.Polygons
                .Select(_ => _.DistanceToCoast.Value)
                .Distinct()
                .OrderBy(_ => _).ToArray();
            double[] roadHeights =
            {
                heights[heights.Length*1/10],
                heights[heights.Length*3/10],
                heights[heights.Length*5/10],
                1
            };
            Dictionary<MapPolygon, int> centerContour = new Dictionary<MapPolygon, int>();
            Queue<MapPolygon> queue = new Queue<MapPolygon>();
            foreach (MapPolygon i in map.Polygons)
                if (i.IsOcean)
                    queue.Enqueue(i);
            do
            {
                MapPolygon n = queue.Dequeue();
                foreach (MapPolygon i in n.Neighbour)
                {
                    int newLevel;
                    if (!centerContour.TryGetValue(n, out newLevel))
                        newLevel = 0;

                    while (i.DistanceToCoast > roadHeights[newLevel])
                        newLevel++;

                    int iLevel;
                    if (!centerContour.TryGetValue(i, out iLevel))
                        iLevel = int.MaxValue;

                    if (newLevel < iLevel)
                    {
                        centerContour[i] = newLevel;
                        queue.Enqueue(i);
                    }
                }
            } while (queue.Count > 0);

            Dictionary<MapNode, int> cornerContour = new Dictionary<MapNode, int>();
            foreach (MapPolygon i in map.Polygons)
                foreach (MapNode j in i.Nodes)
                {
                    int curr;
                    if (!cornerContour.TryGetValue(j, out curr)) curr = int.MaxValue;
                    int poly;
                    if (!centerContour.TryGetValue(i, out poly)) poly = int.MaxValue;
                    cornerContour[j] = Math.Min(curr, poly);
                }

            List<Coordinate>[] points = new List<Coordinate>[roadHeights.Length - 1];
            for (int i = 0; i < points.Length; i++) points[i] = new List<Coordinate>();
            foreach (MapEdge i in map.Polygons.SelectMany(_ => _.Nodes).SelectMany(_ => _.Edges).Distinct())
            {
                if (cornerContour[i.From] < cornerContour[i.To])
                {
                    points[cornerContour[i.From]].Add(new Coordinate(
                        (i.From.X + i.To.X)/2,
                        (i.From.Y + i.To.Y)/2));
                }
            }
            foreach (List<Coordinate> i in points)
            {
                List<Coordinate> pts = new List<Coordinate>();
                List<Coordinate[]> paths = new List<Coordinate[]>();
                for (int j = 0; j < i.Count; j++)
                {
                    double minDist = double.MaxValue;
                    for (int k = j + 1; k < i.Count; k++)
                    {
                        double dx = i[j].X - i[k].X;
                        double dy = i[j].Y - i[k].Y;
                        double d = dx*dx + dy*dy;
                        if (d < minDist)
                        {
                            minDist = d;
                            Coordinate tmp = i[j + 1];
                            i[j + 1] = i[k];
                            i[k] = tmp;
                        }
                    }
                    if (minDist > 0.1 && minDist != double.MaxValue)
                    {
                        if (pts.Count > 0)
                            pts.Add(pts[0]);
                        paths.Add(pts.ToArray());
                        pts.Clear();
                    }
                    else pts.Add(i[j]);
                }
                if (pts.Count > 0)
                    pts.Add(pts[0]);
                paths.Add(pts.ToArray());
                pts.Clear();

                foreach (Coordinate[] j in paths)
                {
                    if (j.Length < 4) continue;
                    yield return j;
                }
            }
        }
    }
}