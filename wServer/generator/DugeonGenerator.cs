/*
    Copyright (C) 2015 creepylava

    This file is part of RotMG Dungeon Generator.

    RotMG Dungeon Generator is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using Json;
using wServer.generator.dungeon;
using wServer.generator.templates;

namespace wServer.generator
{
    public class DungeonGenerator
    {
        private readonly uint seed;
        private readonly Generator gen;
        private Rasterizer ras;

        public DungeonGenerator(uint seed, DungeonTemplate template)
        {
            this.seed = seed;
            gen = new Generator(seed, template);
        }

        public void Generate()
        {
            gen.Generate();
            if (ras == null)
                ras = new Rasterizer(seed, gen.ExportGraph());
            ras.Rasterize(RasterizationStep.Finish);
        }

        private struct TileComparer : IEqualityComparer<DungeonTile>
        {
            public bool Equals(DungeonTile x, DungeonTile y)
            {
                return x.TileType == y.TileType && x.Region == y.Region && x.Object == y.Object;
            }

            public int GetHashCode(DungeonTile obj)
            {
                int code = (int)obj.TileType.Id;
                if (obj.Region != null)
                    code = code * 7 + obj.Region.GetHashCode();
                if (obj.Object != null)
                    code = code * 13 + obj.Object.GetHashCode();
                return code;
            }
        }

        public string ExportToJson()
        {
            var map = ras.ExportMap();
            int w = map.GetUpperBound(0), h = map.GetUpperBound(1);

            var tiles = new JsonArray();
            var indexLookup = new Dictionary<DungeonTile, short>(new TileComparer());
            byte[] data = new byte[w * h * 2];
            int ptr = 0;

            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    var tile = map[x, y];
                    short index;
                    if (!indexLookup.TryGetValue(tile, out index))
                    {
                        indexLookup.Add(tile, index = (short)tiles.Count);
                        tiles.Add(tile);
                    }
                    data[ptr++] = (byte)(index >> 8);
                    data[ptr++] = (byte)(index & 0xff);
                }

            for (int i = 0; i < tiles.Count; i++)
            {
                var tile = (DungeonTile)tiles[i];

                var jsonTile = new JsonObject();
                jsonTile["ground"] = tile.TileType.Name;
                if (!string.IsNullOrEmpty(tile.Region))
                {
                    var region = new JsonObject {
                        { "id", tile.Region }
                    };
                    jsonTile["regions"] = new JsonArray { region };
                }
                if (tile.Object != null)
                {
                    var obj = new JsonObject {
                        { "id", tile.Object.ObjectType.Name }
                    };
                    if (tile.Object.Attributes.Length > 0)
                    {
                        var objAttrs = tile.Object.Attributes.Select(kvp => kvp.Key + ":" + kvp.Value).ToArray();
                        obj["name"] = string.Join(",", objAttrs);
                    }
                    jsonTile["objs"] = new JsonArray { obj };
                }

                tiles[i] = jsonTile;
            }

            var mapObj = new JsonObject();
            mapObj["width"] = w;
            mapObj["height"] = h;
            mapObj["dict"] = tiles;
            mapObj["data"] = Convert.ToBase64String(Zlib.Compress(data));

            return mapObj.ToString();
        }
    }
}