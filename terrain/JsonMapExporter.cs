#region

using System.Collections.Generic;
using Ionic.Zlib;
using Newtonsoft.Json;

#endregion

namespace terrain
{
    internal class JsonMapExporter
    {
        public string Export(TerrainTile[,] tiles)
        {
            int w = tiles.GetLength(0);
            int h = tiles.GetLength(1);
            byte[] dat = new byte[w*h*2];
            int i = 0;
            Dictionary<TerrainTile, ushort> idxs = new Dictionary<TerrainTile, ushort>(new TileComparer());
            List<loc> dict = new List<loc>();
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    TerrainTile tile = tiles[x, y];
                    ushort idx;
                    if (!idxs.TryGetValue(tile, out idx))
                    {
                        idxs.Add(tile, idx = (ushort) dict.Count);
                        dict.Add(new loc
                        {
                            ground = TileTypes.id[tile.TileId],
                            objs = tile.TileObj == null
                                ? null
                                : new[]
                                {
                                    new obj
                                    {
                                        id = tile.TileObj,
                                        name = tile.Name == null ? null : tile.Name
                                    }
                                },
                            regions = tile.TileId == TileTypes.Beach
                                ? new[]
                                {
                                    new obj
                                    {
                                        id = "Spawn"
                                    }
                                }
                                : null
                        });
                    }
                    dat[i + 1] = (byte) (idx & 0xff);
                    dat[i] = (byte) (idx >> 8);
                    i += 2;
                }
            json_dat ret = new json_dat
            {
                data = ZlibStream.CompressBuffer(dat),
                width = w,
                height = h,
                dict = dict.ToArray()
            };
            return JsonConvert.SerializeObject(ret);
        }

        private struct TileComparer : IEqualityComparer<TerrainTile>
        {
            public bool Equals(TerrainTile x, TerrainTile y)
            {
                return x.TileId == y.TileId && x.TileObj == y.TileObj;
            }

            public int GetHashCode(TerrainTile obj)
            {
                return obj.TileId*13 +
                       (obj.TileObj == null ? 0 : obj.TileObj.GetHashCode()*obj.Name.GetHashCode()*29);
            }
        }


        private struct json_dat
        {
            public byte[] data;
            public loc[] dict;
            public int height;
            public int width;
        }

        private struct loc
        {
            public string ground;
            public obj[] objs;
            public obj[] regions;
        }

        private struct obj
        {
            public string id;
            public string name;
        }
    }
}