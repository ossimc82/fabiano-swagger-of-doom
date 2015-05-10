#region

using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;
using Newtonsoft.Json;

#endregion

namespace wServer.realm.terrain
{
    public class Json2Wmap
    {
        public static void Convert(RealmManager manager, string from, string to)
        {
            byte[] x = Convert(manager, File.ReadAllText(from));
            File.WriteAllBytes(to, x);
        }

        public static byte[] Convert(RealmManager manager, string json)
        {
            json_dat obj = JsonConvert.DeserializeObject<json_dat>(json);
            byte[] dat = ZlibStream.UncompressBuffer(obj.data);

            Dictionary<ushort, TerrainTile> tileDict = new Dictionary<ushort, TerrainTile>();
            for (int i = 0; i < obj.dict.Length; i++)
            {
                loc o = obj.dict[i];
                tileDict[(ushort) i] = new TerrainTile
                {
                    TileId = o.ground == null ? (ushort) 0xff : manager.GameData.IdToTileType[o.ground],
                    TileObj = o.objs == null ? null : o.objs[0].id,
                    Name = o.objs == null ? "" : o.objs[0].name ?? "",
                    Terrain = TerrainType.None,
                    Region =
                        o.regions == null
                            ? TileRegion.None
                            : (TileRegion) Enum.Parse(typeof (TileRegion), o.regions[0].id.Replace(' ', '_'))
                };
            }

            TerrainTile[,] tiles = new TerrainTile[obj.width, obj.height];
            using (NReader rdr = new NReader(new MemoryStream(dat)))
                for (int y = 0; y < obj.height; y++)
                    for (int x = 0; x < obj.width; x++)
                    {
                        tiles[x, y] = tileDict[(ushort)rdr.ReadInt16()];
                    }
            return WorldMapExporter.Export(tiles);
        }

        public static byte[] ConvertMakeWalls(RealmManager manager, string json)
        {
            json_dat obj = JsonConvert.DeserializeObject<json_dat>(json);
            byte[] dat = ZlibStream.UncompressBuffer(obj.data);

            Dictionary<ushort, TerrainTile> tileDict = new Dictionary<ushort, TerrainTile>();
            for (int i = 0; i < obj.dict.Length; i++)
            {
                loc o = obj.dict[i];
                tileDict[(ushort) i] = new TerrainTile
                {
                    TileId = o.ground == null ? (ushort) 0xff : manager.GameData.IdToObjectType[o.ground],
                    TileObj = o.objs == null ? null : o.objs[0].id,
                    Name = o.objs == null ? "" : o.objs[0].name ?? "",
                    Terrain = TerrainType.None,
                    Region =
                        o.regions == null
                            ? TileRegion.None
                            : (TileRegion) Enum.Parse(typeof (TileRegion), o.regions[0].id.Replace(' ', '_'))
                };
            }

            TerrainTile[,] tiles = new TerrainTile[obj.width, obj.height];
            using (NReader rdr = new NReader(new MemoryStream(dat)))
                for (int y = 0; y < obj.height; y++)
                    for (int x = 0; x < obj.width; x++)
                    {
                        tiles[x, y] = tileDict[(ushort)rdr.ReadInt16()];
                        tiles[x, y].X = x;
                        tiles[x, y].Y = y;
                    }

            foreach (TerrainTile i in tiles)
            {
                if (i.TileId == 0xff && i.TileObj == null)
                {
                    bool createWall = false;
                    for (int ty = -1; ty <= 1; ty++)
                        for (int tx = -1; tx <= 1; tx++)
                            try
                            {
                                if (tiles[i.X + tx, i.Y + ty].TileId != 0xff)
                                    createWall = true;
                            }
                            catch
                            {
                            }
                    if (createWall)
                        tiles[i.X, i.Y].TileObj = "Grey Wall";
                }
            }

            return WorldMapExporter.Export(tiles);
        }

        // ------------ Convert to UDL format ------------- //
        public static byte[] ConvertUDL(RealmManager manager, string json)
        {
            json_dat obj = JsonConvert.DeserializeObject<json_dat>(json);
            byte[] dat = ZlibStream.UncompressBuffer(obj.data);

            Random rand = new Random();

            Dictionary<ushort, TerrainTile> tileDict = new Dictionary<ushort, TerrainTile>();
            for (int i = 0; i < obj.dict.Length; i++)
            {
                loc o = obj.dict[i];
                tileDict[(ushort) i] = new TerrainTile
                {
                    TileId = o.ground == null ? (ushort) 0xff : manager.GameData.IdToObjectType[o.ground],
                    TileObj = o.objs == null ? null : o.objs[0].id,
                    Name = o.objs == null ? "" : o.objs[0].name ?? "",
                    Terrain = TerrainType.None,
                    Region =
                        o.regions == null
                            ? TileRegion.None
                            : (TileRegion) Enum.Parse(typeof (TileRegion), o.regions[0].id.Replace(' ', '_'))
                };
            }

            TerrainTile[,] tiles = new TerrainTile[obj.width, obj.height];
            using (NReader rdr = new NReader(new MemoryStream(dat)))
                for (int y = 0; y < obj.height; y++)
                    for (int x = 0; x < obj.width; x++)
                    {
                        tiles[x, y] = tileDict[(ushort)rdr.ReadInt16()];
                        tiles[x, y].X = x;
                        tiles[x, y].Y = y;
                    }

            foreach (TerrainTile i in tiles)
            {
                if (i.TileId == 0xff && i.TileObj == null)
                {
                    bool createWall = false;
                    for (int ty = -1; ty <= 1; ty++)
                        for (int tx = -1; tx <= 1; tx++)
                            try
                            {
                                if (tiles[i.X + tx, i.Y + ty].TileId != 0xff && tiles[i.X + tx, i.Y + ty].TileId != 0xfe &&
                                    tiles[i.X + tx, i.Y + ty].TileId != 0xfd && tiles[i.X + tx, i.Y + ty].TileId != 0xe8)
                                    createWall = true;
                            }
                            catch
                            {
                            }
                    if (createWall)
                        tiles[i.X, i.Y].TileObj = rand.Next(1, 5) == 1 ? "Grey Torch Wall" : "Grey Wall";
                }
                else if (i.TileId == manager.GameData.IdToObjectType["Grey Closed"] && rand.Next(1, 4) == 1)
                {
                    tiles[i.X, i.Y].TileId = manager.GameData.IdToObjectType["Grey Quad"];
                }
            }

            return WorldMapExporter.Export(tiles);
        }

        private struct json_dat
        {
            public byte[] data { get; set; }
            public loc[] dict { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }

        private struct loc
        {
            public string ground { get; set; }
            public obj[] objs { get; set; }
            public obj[] regions { get; set; }
        }

        private struct obj
        {
            public string id { get; set; }
            public string name { get; set; }
        }
    }
}