#region

using System;
using System.Collections.Generic;
using System.Linq;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.GeometriesGraph;
using NetTopologySuite.Operation.Overlay;
using NetTopologySuite.Triangulate;

#endregion

namespace terrain
{
    internal struct MapEdge
    {
        public MapNode From { get; set; }
        public MapNode To { get; set; }
    }

    internal class MapNode
    {
        public double X { get; set; }
        public double Y { get; set; }
        public MapEdge[] Edges { get; set; }
        public bool IsWater { get; set; }
        public bool IsOcean { get; set; }
        public int? RiverValue { get; set; }
        public double? DistanceToCoast { get; set; }
    }

    internal class MapPolygon
    {
        public int Id { get; set; }
        public MapPolygon[] Neighbour { get; set; }
        public MapNode[] Nodes { get; set; }
        public double? DistanceToCoast { get; set; }
        public double CentroidX { get; set; }
        public double CentroidY { get; set; }
        public bool IsWater { get; set; }
        public bool IsCoast { get; set; }
        public bool IsOcean { get; set; }
        public Polygon Polygon { get; set; }
    }

    internal class PolygonMap
    {
        private readonly HashSet<MapNode> oceans = new HashSet<MapNode>();
        private readonly int seed;
        private readonly HashSet<MapNode> waters = new HashSet<MapNode>();

        public PolygonMap(int seed)
        {
            this.seed = seed;
        }

        public IGeometryCollection VoronoiDiagram { get; private set; }
        public MapPolygon[] Polygons { get; private set; }

        public IEnumerable<MapNode> Oceans
        {
            get { return oceans; }
        }

        private static IGeometryCollection ClipGeometryCollection(IGeometryCollection geom, Envelope clipEnv)
        {
            IGeometry clipPoly = geom.Factory.ToGeometry(clipEnv);
            List<IGeometry> clipped = new List<IGeometry>();
            for (int i = 0; i < geom.NumGeometries; i++)
            {
                IGeometry g = geom.GetGeometryN(i);
                IGeometry result = null;
                // don't clip unless necessary
                if (clipEnv.Contains(g.EnvelopeInternal))
                    result = g;
                else if (clipEnv.Intersects(g.EnvelopeInternal))
                {
                    result = clipPoly.Intersection(g);
                    // keep vertex key info
                    result.UserData = g.UserData;
                }

                if (result != null && !result.IsEmpty)
                {
                    clipped.Add(result);
                }
            }
            return geom.Factory.CreateGeometryCollection(GeometryFactory.ToGeometryArray(clipped));
        }

        private void DetermineLandmass() // Perlin
        {
            waters.Clear();
            Noise noise = new Noise(seed);
            foreach (MapPolygon i in Polygons)
            {
                int total = 0;
                int water = 0;
                foreach (MapNode j in i.Nodes)
                {
                    //var n = noise[(int)((j.X + 1) / 2 * 255), (int)((j.Y + 1) / 2 * 255)];
                    float n = noise.GetNoise((j.X + 1)*2, (j.Y + 1)*2, 0);
                    double d = j.X*j.X + j.Y*j.Y;
                    if (n < d*0.7 ||
                        (Math.Abs(j.X) > 0.9 || Math.Abs(j.Y) > 0.9))
                    {
                        j.IsWater = true;
                        waters.Add(j);
                        water++;
                    }
                    total++;
                }
            }
        }

        //void DetermineLandmass()    // Sine
        //{
        //    var rand = new Random(seed);
        //    int bumps = rand.Next(1, 7);
        //    double startAngle = rand.NextDouble() * 2 * Math.PI;
        //    double dipAngle = rand.NextDouble() * 2 * Math.PI;
        //    double dipWidth = rand.NextDouble() / 2 + 0.2;

        //    waters.Clear();
        //    foreach (var i in Polygons)
        //    {
        //        int total = 0;
        //        int water = 0;
        //        foreach (var j in i.Nodes)
        //        {
        //            var x = j.X;
        //            var y = j.Y;
        //            var angle = Math.Atan2(y, x);
        //            var length = 0.5 * (Math.Max(Math.Abs(x), Math.Abs(y)) + Math.Sqrt(x * x + y * y));

        //            var r1 = 0.5 + 0.40 * Math.Sin(startAngle + bumps * angle + Math.Cos((bumps + 3) * angle));
        //            var r2 = 0.7 - 0.20 * Math.Sin(startAngle + bumps * angle - Math.Sin((bumps + 2) * angle));
        //            if (Math.Abs(angle - dipAngle) < dipWidth ||
        //                Math.Abs(angle - dipAngle + 2 * Math.PI) < dipWidth ||
        //                Math.Abs(angle - dipAngle - 2 * Math.PI) < dipWidth)
        //                r1 = r2 = 0.2;

        //            if (!(length < r1 || (length > r1 * 0.5 && length < r2)))
        //            {
        //                j.IsWater = true;
        //                waters.Add(j);
        //                water++;
        //            }
        //            total++;
        //        }
        //    }
        //}

        private void FindOceans()
        {
            oceans.Clear();
            foreach (MapNode i in waters)
            {
                if (i.X == -1 || i.X == 1 ||
                    i.Y == -1 || i.Y == 1 ||
                    i.IsOcean ||
                    i.Edges.Select(_ => _.To).All(_ => _.IsWater))
                {
                    i.IsOcean = true;
                    continue;
                }

                bool isOcean = false;
                Queue<MapNode> q = new Queue<MapNode>();
                q.Enqueue(i);
                HashSet<MapNode> v = new HashSet<MapNode>();
                do
                {
                    MapNode current = q.Dequeue();
                    foreach (MapEdge j in current.Edges)
                    {
                        if (v.Contains(j.To) || !j.To.IsWater) continue;
                        if (j.To.X == -1 || j.To.X == 1 ||
                            j.To.Y == -1 || j.To.Y == 1)
                        {
                            isOcean = true;
                            q.Clear();
                            break;
                        }
                        q.Enqueue(j.To);
                        v.Add(j.To);
                    }
                } while (q.Count > 0);
                if (isOcean)
                {
                    q.Clear();
                    v.Clear();
                    q.Enqueue(i);

                    do
                    {
                        MapNode current = q.Dequeue();
                        oceans.Add(current);
                        current.IsOcean = true;
                        foreach (MapEdge j in current.Edges)
                        {
                            if (v.Contains(j.To) || !j.To.IsWater) continue;
                            q.Enqueue(j.To);
                            v.Add(j.To);
                        }
                    } while (q.Count > 0);
                }
            }
        }

        private void FindLakesAndCoasts()
        {
            HashSet<MapPolygon> lake = new HashSet<MapPolygon>(Polygons.Where(_ => _.IsWater));
            HashSet<MapPolygon> coast = new HashSet<MapPolygon>();
            MapPolygon start = Polygons.First(_ => _.Nodes.Any(__ => __.X == -1 && __.Y == -1));
            lake.Remove(start);
            Queue<MapPolygon> q = new Queue<MapPolygon>();
            q.Enqueue(start);
            do
            {
                MapPolygon poly = q.Dequeue();
                foreach (MapPolygon i in poly.Neighbour)
                    if (i.IsWater && lake.Contains(i))
                    {
                        if (i.Neighbour.Any(_ => !_.IsWater))
                            coast.Add(i);
                        lake.Remove(i);
                        q.Enqueue(i);
                    }
            } while (q.Count > 0);

            foreach (MapPolygon i in lake)
            {
                foreach (MapNode j in i.Nodes) j.RiverValue = 1;
                i.IsOcean = false;
                if (i.Neighbour.Any(_ => !_.IsWater))
                    coast.Add(i);
            }

            foreach (MapPolygon i in coast)
                i.IsCoast = true;
            foreach (MapPolygon i in coast)
                foreach (MapPolygon j in i.Neighbour.Where(n => n.IsWater))
                    j.IsCoast = true;
        }

        private void ComputeDistances()
        {
            Queue<MapNode> queue = new Queue<MapNode>();
            HashSet<MapNode> visited = new HashSet<MapNode>();
            foreach (MapNode i in oceans)
            {
                i.DistanceToCoast = 0;
                queue.Enqueue(i);
                visited.Add(i);
            }

            do
            {
                MapNode n = queue.Dequeue();
                double dist = n.DistanceToCoast.Value;
                visited.Add(n);
                foreach (MapEdge i in n.Edges)
                {
                    double d = i.To.DistanceToCoast ?? double.MaxValue;
                    if (d > dist + 1)
                        d = dist + 1;
                    i.To.DistanceToCoast = d;

                    if (!visited.Contains(i.To))
                        queue.Enqueue(i.To);
                }
            } while (queue.Count > 0);
        }

        private void RedistributeElevation(IEnumerable<MapNode> nodes)
        {
            List<double> sorted = new List<double>(nodes.Select(_ => _.DistanceToCoast.Value).Distinct());
            sorted.Sort();
            Dictionary<double, double> dict = new Dictionary<double, double>();
            for (int i = 0; i < sorted.Count; i++)
            {
                double y = (double) i/(sorted.Count - 1);
                double x = (Math.Sqrt(1.0) - Math.Sqrt(1.0*(1 - y)));
                dict[sorted[i]] = x > 1 ? 1 : x;
            }
            foreach (MapNode i in nodes)
                i.DistanceToCoast = dict[i.DistanceToCoast.Value];
            foreach (MapPolygon i in Polygons)
            {
                i.DistanceToCoast = i.Nodes.Average(_ => _.DistanceToCoast);

                int c = 0, wc = 0;
                bool hasOcean = false;
                foreach (MapNode j in i.Nodes)
                {
                    if (j.IsWater) wc++;
                    if (j.IsOcean) hasOcean = true;
                    c++;
                }
                if (c == wc || hasOcean)
                {
                    foreach (MapNode j in i.Nodes)
                        j.IsWater = true;
                    i.IsWater = true;
                    i.IsOcean = hasOcean;
                }
                else
                {
                    foreach (MapNode j in i.Nodes)
                        j.IsWater = false;
                }
            }
        }

        public void Generate(int pointCount)
        {
            //Generate random points
            HashSet<Coordinate> hashSet = new HashSet<Coordinate>();
            {
                Random rand = new Random(seed);
                while (hashSet.Count < pointCount)
                {
                    double x = rand.NextDouble()*2 - 1;
                    double y = rand.NextDouble()*2 - 1;
                    if (x < -0.99 || y < -0.99 || x > 0.99 || y > 0.99) continue;
                    hashSet.Add(new Coordinate(x, y));
                }
            }
            //Optimize points
            {
                Coordinate[] points = hashSet.ToArray();
                for (int i = 0; i < 2; i++)
                {
                    VoronoiDiagramBuilder builder = new VoronoiDiagramBuilder();
                    builder.SetSites(points);
                    VoronoiDiagram = builder.GetDiagram(new GeometryFactory());
                    for (int j = 0; j < points.Length; j++)
                    {
                        Polygon poly = VoronoiDiagram[j] as Polygon;
                        points[j] = new Coordinate(poly.Centroid.X, poly.Centroid.Y);
                    }
                }
            }
            //Build graph
            PlanarGraph graph;
            {
                VoronoiDiagram = ClipGeometryCollection(VoronoiDiagram, new Envelope(-1, 1, -1, 1));
                graph = new PlanarGraph(new OverlayNodeFactory());
                List<Edge> edges = new List<Edge>();
                for (int i = 0; i < VoronoiDiagram.Count; i++)
                {
                    Polygon poly = VoronoiDiagram[i] as Polygon;
                    Coordinate[] coords = poly.Coordinates;
                    for (int j = 1; j < coords.Length; j++)
                    {
                        edges.Add(new Edge(new[] {coords[j - 1], coords[j]}, new Label(Location.Boundary)));
                    }
                }
                graph.AddEdges(edges);
            }
            //Convert graph
            Dictionary<Node, MapNode> nodeDict;
            {
                Dictionary<MapPolygon, HashSet<MapPolygon>> polys = new Dictionary<MapPolygon, HashSet<MapPolygon>>();
                nodeDict = new Dictionary<Node, MapNode>();
                Dictionary<MapNode, Tuple<HashSet<MapPolygon>, HashSet<MapEdge>>> dats =
                    new Dictionary<MapNode, Tuple<HashSet<MapPolygon>, HashSet<MapEdge>>>();
                for (int i = 0; i < VoronoiDiagram.Count; i++)
                {
                    List<MapNode> nodes = new List<MapNode>();
                    MapPolygon poly = new MapPolygon
                    {
                        CentroidX = VoronoiDiagram[i].Centroid.X,
                        CentroidY = VoronoiDiagram[i].Centroid.Y,
                        Polygon = VoronoiDiagram[i] as Polygon
                    };
                    foreach (Coordinate j in VoronoiDiagram[i].Coordinates.Skip(1))
                    {
                        Node n = graph.Find(j);
                        MapNode mapNode;
                        if (!nodeDict.TryGetValue(n, out mapNode))
                        {
                            mapNode = new MapNode {X = j.X, Y = j.Y};
                            dats[mapNode] =
                                new Tuple<HashSet<MapPolygon>, HashSet<MapEdge>>(new HashSet<MapPolygon> {poly},
                                    new HashSet<MapEdge>());
                        }
                        else
                            dats[mapNode].Item1.Add(poly);
                        nodes.Add(nodeDict[n] = mapNode);
                    }
                    poly.Nodes = nodes.ToArray();
                    polys.Add(poly, new HashSet<MapPolygon>());
                }
                foreach (KeyValuePair<Node, MapNode> i in nodeDict)
                {
                    foreach (MapPolygon j in dats[i.Value].Item1)
                        foreach (MapPolygon k in dats[i.Value].Item1)
                            if (j != k)
                            {
                                polys[j].Add(k);
                                polys[k].Add(j);
                            }
                    foreach (EdgeEnd j in i.Key.Edges)
                    {
                        MapNode from = nodeDict[graph.Find(j.Coordinate)];
                        MapNode to = nodeDict[graph.Find(j.DirectedCoordinate)];
                        dats[from].Item2.Add(new MapEdge {From = from, To = to});
                    }
                }
                int ftrh = dats.Count(_ => _.Value.Item2.Count == 0);
                foreach (KeyValuePair<MapNode, Tuple<HashSet<MapPolygon>, HashSet<MapEdge>>> i in dats)
                    i.Key.Edges = i.Value.Item2.ToArray();
                KeyValuePair<MapPolygon, HashSet<MapPolygon>>[] x = polys.ToArray();
                for (int i = 0; i < x.Length; i++)
                {
                    x[i].Key.Neighbour = x[i].Value.ToArray();
                    x[i].Key.Id = i;
                }
                Polygons = x.Select(_ => _.Key).ToArray();
            }
            //Generate map
            DetermineLandmass();
            FindOceans();
            ComputeDistances();
            RedistributeElevation(nodeDict.Values);
            FindLakesAndCoasts();
        }
    }
}