#region

using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;

#endregion

namespace terrain
{
    internal class WorldMapExporter
    {
        public static void Export(TerrainTile[,] tiles, string path)
        {
            File.WriteAllBytes(path, Export(tiles));
        }
        public static byte[] Export(TerrainTile[,] tiles)
        {
            List<TerrainTile> dict = new List<TerrainTile>();

            int w = tiles.GetLength(0);
            int h = tiles.GetLength(1);
            byte[] dat = new byte[w * h * 3];
            int idx = 0;
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    TerrainTile tile = tiles[x, y];
                    short i = (short)dict.IndexOf(tile);
                    if (i == -1)
                    {
                        i = (short)dict.Count;
                        dict.Add(tile);
                    }
                    dat[idx] = (byte)(i & 0xff);
                    dat[idx + 1] = (byte)(i >> 8);
                    dat[idx + 2] = (byte)(tile.Elevation * 255);
                    idx += 3;
                }

            MemoryStream ms = new MemoryStream();
            using (BinaryWriter wtr = new BinaryWriter(ms))
            {
                wtr.Write((short)dict.Count);
                foreach (var i in dict)
                {
                    wtr.Write(i.TileId);
                    wtr.Write(i.TileObj ?? "");
                    wtr.Write(i.Name ?? "");
                    wtr.Write((byte)i.Terrain);
                    wtr.Write((byte)i.Region);
                }
                wtr.Write(w);
                wtr.Write(h);
                wtr.Write(dat);
            }
            byte[] buff = ZlibStream.CompressBuffer(ms.ToArray());
            byte[] ret = new byte[buff.Length + 1];
            Buffer.BlockCopy(buff, 0, ret, 1, buff.Length);
            ret[0] = 2;
            return ret;
        }
    }
}