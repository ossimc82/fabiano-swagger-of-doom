#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace terrain
{
    internal class Biome
    {
        private readonly PolygonMap map;
        private readonly Random rand;
        private HashSet<MapPolygon> beaches;


        private double[] elevationThreshold;
        private double[] moistureThreshold;

        public Biome(int seed, PolygonMap map)
        {
            rand = new Random(seed);
            this.map = map;
        }

        public void ComputeBiomes(TerrainTile[,] buff)
        {
            Dictionary<MapNode, double> nodeMoist = ComputeMoisture();
            Dictionary<MapPolygon, double> polyMoist = RedistributeMoisture(nodeMoist);

            double[] elevs = map.Polygons
                .SelectMany(_ => _.Nodes)
                .Select(_ => _.DistanceToCoast.Value)
                .OrderBy(_ => _)
                .Distinct().ToArray();
            elevationThreshold = new[]
            {
                0/5.0,
                1/5.0,
                3/5.0,
                4/5.0
            };
            beaches = new HashSet<MapPolygon>(map.Polygons.Where(b =>
                !b.IsWater &&
                b.Neighbour.Any(w1 => (w1.IsCoast && w1.IsOcean) || w1.Neighbour.Any(w2 => w2.IsCoast && w2.IsOcean))));

            double[] moists = nodeMoist
                .Select(_ => _.Value)
                .OrderBy(_ => _)
                .Distinct().ToArray();
            moistureThreshold = new[]
            {
                1/7.0,
                2/7.0,
                3/7.0,
                4/7.0,
                5/7.0,
                6/7.0
            };

            AddNoiseAndBiome(buff, polyMoist);
            BlurElevation(buff, 5);
            Randomize(buff);
            ComputeSpawnTerrains(buff);
        }

        private Dictionary<MapNode, double> ComputeMoisture()
        {
            Dictionary<MapNode, double> moisture = new Dictionary<MapNode, double>();
            int totalCount = map.Polygons
                .SelectMany(_ => _.Nodes)
                .Distinct().Count();
            MapNode[] waterNodes = map.Polygons
                .Where(_ => !_.IsOcean) //no ocean
                .Where(_ => !_.Neighbour.Any(__ => __.IsOcean)) //no beaches
                .SelectMany(_ => _.Nodes)
                .Where(_ => _.RiverValue != null && !_.IsOcean)
                .Distinct().ToArray();

            Queue<MapNode> q = new Queue<MapNode>();
            int max = waterNodes.Max(n => n.RiverValue.Value);
            foreach (MapNode i in waterNodes)
            {
                q.Enqueue(i);
                moisture[i] = max - i.RiverValue.Value;
            }

            do
            {
                MapNode node = q.Dequeue();
                double dist = moisture[node] + 1;
                foreach (MapEdge i in node.Edges)
                {
                    MapNode target = i.To;
                    double targetDist;
                    if (!moisture.TryGetValue(target, out targetDist))
                        targetDist = int.MaxValue;
                    if (targetDist > dist)
                    {
                        moisture[target] = dist;
                        q.Enqueue(target);
                    }
                }
            } while (q.Count > 0);
            return moisture;
        }

        private Dictionary<MapPolygon, double> RedistributeMoisture(Dictionary<MapNode, double> nodes)
        {
            List<double> sorted = new List<double>(nodes.Values.Distinct());
            sorted.Sort();
            Dictionary<double, double> dict = new Dictionary<double, double>();
            for (int i = 0; i < sorted.Count; i++)
            {
                double y = (double) (sorted.Count - i)/sorted.Count;
                double x = (Math.Sqrt(1.0) - Math.Sqrt(1.0*(1 - y)));
                //double x = y * 0.8;
                dict[sorted[i]] = (x > 1 ? 1 : x);
            }
            foreach (MapNode i in nodes.Keys.ToArray())
            {
                nodes[i] = dict[nodes[i]]*(1 - i.DistanceToCoast.Value*0);
            }

            Dictionary<MapPolygon, double> ret = new Dictionary<MapPolygon, double>();
            foreach (MapPolygon i in map.Polygons)
            {
                ret[i] = i.Nodes.Average(_ => nodes[_]);
            }
            return ret;
        }

        private ushort GetBiomeGround(string biome)
        {
            switch (biome)
            {
                case "road":
                    return TileTypes.Road;
                case "river":
                    return TileTypes.Water;

                case "beach":
                    return TileTypes.Beach;

                case "snowy":
                    return TileTypes.SnowRock;
                case "mountain":
                    return TileTypes.Rock;

                case "taiga":
                    return TileTypes.BrightGrass;
                case "shrub":
                    return TileTypes.LightGrass;

                case "rainforest":
                    return TileTypes.BlueGrass;
                case "forest":
                    return TileTypes.DarkGrass;
                case "grassland":
                    return TileTypes.Grass;

                case "dryland":
                    return TileTypes.YellowGrass;
                case "desert":
                    return TileTypes.Sand;
            }
            return 0;
        }

        private TerrainType GetBiomeTerrain(TerrainTile tile)
        {
            if (tile.PolygonId == -1 ||
                tile.TileId == TileTypes.Road ||
                tile.TileId == TileTypes.Water) return TerrainType.None;
            MapPolygon poly = map.Polygons[tile.PolygonId];

            if (!poly.IsWater && beaches.Contains(poly))
                return TerrainType.ShoreSand;
            if (poly.IsWater)
                return TerrainType.None;
            if (tile.Elevation >= elevationThreshold[3])
                return TerrainType.Mountains;
            if (tile.Elevation > elevationThreshold[2])
            {
                if (tile.Moisture > moistureThreshold[4])
                    return TerrainType.HighPlains;
                if (tile.Moisture > moistureThreshold[2])
                    return TerrainType.HighForest;
                return TerrainType.HighSand;
            }
            if (tile.Elevation > elevationThreshold[1])
            {
                if (tile.Moisture > moistureThreshold[4])
                    return TerrainType.MidForest;
                if (tile.Moisture > moistureThreshold[2])
                    return TerrainType.MidPlains;
                return TerrainType.MidSand;
            }
            if (poly.Neighbour.Any(_ => beaches.Contains(_)))
            {
                if (tile.Moisture > moistureThreshold[2])
                    return TerrainType.ShorePlains;
            }

            if (tile.Moisture > moistureThreshold[3])
                return TerrainType.LowForest;
            if (tile.Moisture > moistureThreshold[2])
                return TerrainType.LowPlains;
            return TerrainType.LowSand;
            //return TerrainType.None;
        }

        private string GetBiome(TerrainTile tile)
        {
            if (tile.PolygonId == -1) return "unknown";
            if (tile.TileId == TileTypes.Road) return "road";
            if (tile.TileId == TileTypes.Water) return "river";
            MapPolygon poly = map.Polygons[tile.PolygonId];

            if (tile.TileId == 0xb4) return "towel";

            if (beaches.Contains(poly))
                return "beach";
            if (poly.IsWater)
            {
                if (poly.IsCoast) return "coast";
                if (poly.IsOcean) return "ocean";
                return "water";
            }
            if (tile.Elevation >= elevationThreshold[3])
            {
                if (tile.Moisture > moistureThreshold[4])
                    return "snowy";
                return "mountain";
            }
            if (tile.Elevation > elevationThreshold[2])
            {
                if (tile.Moisture > moistureThreshold[4])
                    return "dryland";
                if (tile.Moisture > moistureThreshold[2])
                    return "taiga";
                return "desert";
            }
            if (tile.Elevation > elevationThreshold[1])
            {
                if (tile.Moisture > moistureThreshold[4])
                    return "forest";
                if (tile.Moisture > moistureThreshold[2])
                    return "shrub";
                return "desert";
            }
            if (tile.Moisture > moistureThreshold[4])
                return "rainforest";
            if (tile.Moisture > moistureThreshold[3])
                return "forest";
            if (tile.Moisture > moistureThreshold[2])
                return "grassland";
            return "desert";
            //return "unknown";
        }

        private void AddNoiseAndBiome(TerrainTile[,] buff, Dictionary<MapPolygon, double> moist)
        {
            int w = buff.GetLength(0);
            int h = buff.GetLength(1);
            Noise elevationNoise = new Noise(rand.Next());
            Noise moistureNoise = new Noise(rand.Next());
            //var elevationNoise = PerlinNoise.GetPerlinNoise(rand.Next(), 256, 256, 2);
            //var moistureNoise = PerlinNoise.GetPerlinNoise(rand.Next(), 256, 256, 2);
            for (int y = 0; y < w; y++)
                for (int x = 0; x < h; x++)
                {
                    TerrainTile tile = buff[x, y];
                    if (tile.PolygonId != -1)
                    {
                        MapPolygon poly = map.Polygons[tile.PolygonId];

                        tile.Elevation = Math.Min(1, (float) (poly.DistanceToCoast + poly.DistanceToCoast*
                                                              elevationNoise.GetNoise(x*128.0/w, y*128.0/h, 0.3)*0.01f)*
                                                     2);
                        if (tile.Elevation > 1) tile.Elevation = 1;
                        else if (tile.Elevation < 0) tile.Elevation = 0;
                        tile.Elevation = (float) Math.Pow(tile.Elevation, 1.5);

                        tile.Moisture = (float) (moist[poly] + moist[poly]*
                                                 moistureNoise.GetNoise(x*128.0/w, y*128.0/h, 0.3)*0.01f);
                        if (tile.Moisture > 1) tile.Moisture = 1;
                        else if (tile.Moisture < 0) tile.Moisture = 0;
                    }

                    tile.Biome = GetBiome(tile);
                    ushort biomeGround = GetBiomeGround(tile.Biome);
                    if (biomeGround != 0)
                        tile.TileId = biomeGround;

                    buff[x, y] = tile;
                }
        }

        private void Randomize(TerrainTile[,] buff)
        {
            int w = buff.GetLength(0);
            int h = buff.GetLength(1);
            TerrainTile[,] tmp = (TerrainTile[,]) buff.Clone();
            for (int y = 10; y < h - 10; y++)
                for (int x = 10; x < w - 10; x++)
                {
                    TerrainTile tile = buff[x, y];

                    if (tile.TileId == TileTypes.Water && tile.Elevation >= elevationThreshold[3])
                        tile.TileId = TileTypes.SnowRock;
                    else if (tile.TileId != TileTypes.Water && tile.TileId != TileTypes.Road &&
                             tile.TileId != TileTypes.Beach && tile.TileId != TileTypes.MovingWater &&
                             tile.TileId != TileTypes.DeepWater)
                    {
                        ushort id = tmp[x + rand.Next(-3, 4), y + rand.Next(-3, 4)].TileId;
                        while (id == TileTypes.Water || id == TileTypes.Road ||
                               id == TileTypes.Beach || id == TileTypes.MovingWater ||
                               id == TileTypes.DeepWater)
                            id = tmp[x + rand.Next(-3, 4), y + rand.Next(-3, 4)].TileId;
                        tile.TileId = id;
                    }

                    //if (tile.TileId == TileTypes.Beach)
                    //    tile.Region = TileRegion.Spawn;

                    string biome = tile.Biome;
                    if (tile.TileId == TileTypes.Beach) biome = "beach";
                    else if (tile.TileId == TileTypes.MovingWater) biome = "coast";

                    string biomeObj = Decoration.GetDecor(biome, rand);
                    if (biomeObj != null)
                    {
                        tile.TileObj = biomeObj;
                        int? size = Decoration.GetSize(biomeObj, rand);
                        if (size != null)
                            tile.Name = "size:" + size;
                    }

                    float elevation = 0;
                    int c = 0;
                    for (int dy = -1; dy <= 1; dy++)
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            if (x + dx < 0 || x + dx >= w || y + dy < 0 || y + dy >= h) continue;
                            elevation += tmp[x + dx, y + dy].Elevation;
                            c++;
                        }
                    tile.Elevation = elevation/c;

                    buff[x, y] = tile;
                }
        }

        private void ComputeSpawnTerrains(TerrainTile[,] buff)
        {
            int w = buff.GetLength(0);
            int h = buff.GetLength(1);
            for (int y = 0; y < w; y++)
                for (int x = 0; x < h; x++)
                {
                    TerrainTile tile = buff[x, y];
                    tile.Terrain = GetBiomeTerrain(tile);

                    buff[x, y] = tile;
                }
        }


        //https://code.google.com/p/imagelibrary/source/browse/trunk/Filters/GaussianBlurFilter.cs
        //Blur the elevation

        private static void BlurElevation(TerrainTile[,] tiles, double radius)
        {
            int w = tiles.GetLength(0);
            int h = tiles.GetLength(1);

            int shift, source;
            int blurDiam = (int) Math.Pow(radius, 2);
            int gaussWidth = (blurDiam*2) + 1;

            double[] kernel = CreateKernel(gaussWidth, blurDiam);

            // Calculate the sum of the Gaussian kernel      
            double gaussSum = 0;
            for (int n = 0; n < gaussWidth; n++)
            {
                gaussSum += kernel[n];
            }

            // Scale the Gaussian kernel
            for (int n = 0; n < gaussWidth; n++)
            {
                kernel[n] = kernel[n]/gaussSum;
            }
            //premul = kernel[k] / gaussSum;


            // Create an X & Y pass buffer  
            float[,] gaussPassX = new float[w, h];

            // Do Horizontal Pass  
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    // Iterate through kernel  
                    for (int k = 0; k < gaussWidth; k++)
                    {
                        // Get pixel-shift (pixel dist between dest and source)  
                        shift = k - blurDiam;

                        // Basic edge clamp  
                        source = x + shift;
                        if (source <= 0 || source >= w)
                        {
                            source = x;
                        }

                        // Combine source and destination pixels with Gaussian Weight  
                        gaussPassX[x, y] = (float) (gaussPassX[x, y] + tiles[source, y].Elevation*kernel[k]);
                    }
                }
            }

            // Do Vertical Pass  
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    tiles[x, y].Elevation = 0;
                    // Iterate through kernel  
                    for (int k = 0; k < gaussWidth; k++)
                    {
                        // Get pixel-shift (pixel dist between dest and source)   
                        shift = k - blurDiam;

                        // Basic edge clamp  
                        source = y + shift;
                        if (source <= 0 || source >= h)
                        {
                            source = y;
                        }

                        // Combine source and destination pixels with Gaussian Weight  
                        tiles[x, y].Elevation = (float) (tiles[x, y].Elevation + (gaussPassX[x, source])*kernel[k]);
                    }
                }
            }
        }

        private static double[] CreateKernel(int gaussianWidth, int blurDiam)
        {
            double[] kernel = new double[gaussianWidth];

            // Set the maximum value of the Gaussian curve  
            const double sd = 255;

            // Set the width of the Gaussian curve  
            double range = gaussianWidth;

            // Set the average value of the Gaussian curve   
            double mean = (range/sd);

            // Set first half of Gaussian curve in kernel  
            for (int pos = 0, len = blurDiam + 1; pos < len; pos++)
            {
                // Distribute Gaussian curve across kernel[array]   
                kernel[gaussianWidth - 1 - pos] =
                    kernel[pos] = Math.Sqrt(Math.Sin((((pos + 1)*(Math.PI/2)) - mean)/range))*sd;
            }

            return kernel;
        }
    }
}