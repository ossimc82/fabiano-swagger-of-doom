#region

using System.Collections.Generic;

#endregion

namespace wServer.realm.terrain
{
    internal class JsonMapExporter
    {
        //public string Export(TerrainTile[,] tiles)
        //{
        //    var w = tiles.GetLength(0);
        //    var h = tiles.GetLength(1);
        //    var dat = new byte[w*h*2];
        //    var i = 0;
        //    var idxs = new Dictionary<TerrainTile, ushort>(new TileComparer());
        //    var dict = new List<loc>();
        //    for (var y = 0; y < h; y++)
        //        for (var x = 0; x < w; x++)
        //        {
        //            var tile = tiles[x, y];
        //            ushort idx;
        //            if (!idxs.TryGetValue(tile, out idx))
        //            {
        //                idxs.Add(tile, idx = (ushort) dict.Count);
        //                dict.Add(new loc
        //                {
        //                    ground = XmlDatas.TypeToId[tile.TileId],
        //                    objs = tile.TileObj == null
        //                        ? null
        //                        : new[]
        //                        {
        //                            new obj
        //                            {
        //                                id = tile.TileObj,
        //                                name = tile.Name == null ? null : tile.Name
        //                            }
        //                        },
        //                    regions = null
        //                });
        //            }
        //            dat[i + 1] = (byte) (idx & 0xff);
        //            dat[i] = (byte) (idx >> 8);
        //            i += 2;
        //        }
        //    var ret = new json_dat
        //    {
        //        data = ZlibStream.CompressBuffer(dat),
        //        width = w,
        //        height = h,
        //        dict = dict.ToArray()
        //    };
        //    return JsonConvert.SerializeObject(ret);
        //}

        //private struct TileComparer : IEqualityComparer<TerrainTile>
        //{
        //    public bool Equals(TerrainTile x, TerrainTile y)
        //    {
        //        return x.TileId == y.TileId && x.TileObj == y.TileObj;
        //    }

        //    public int GetHashCode(TerrainTile obj)
        //    {
        //        return obj.TileId*13 +
        //               (obj.TileObj == null ? 0 : obj.TileObj.GetHashCode()*obj.Name.GetHashCode()*29);
        //    }
        //}
    }
}