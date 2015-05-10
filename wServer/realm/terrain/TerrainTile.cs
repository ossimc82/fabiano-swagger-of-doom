#region

using System;

#endregion

namespace wServer.realm.terrain
{
    internal enum TerrainType
    {
        None,
        Mountains,
        HighSand,
        HighPlains,
        HighForest,
        MidSand,
        MidPlains,
        MidForest,
        LowSand,
        LowPlains,
        LowForest,
        ShoreSand,
        ShorePlains,
    }

    internal struct TerrainTile : IEquatable<TerrainTile>
    {
        public string Biome { get; set; }
        public byte Elevation { get; set; }
        public float Moisture { get; set; }
        public string Name { get; set; }
        public int PolygonId { get; set; }
        public TileRegion Region { get; set; }
        public TerrainType Terrain { get; set; }
        public ushort TileId { get; set; }
        public string TileObj { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public bool Equals(TerrainTile other)
        {
            return
                TileId == other.TileId &&
                TileObj == other.TileObj &&
                Name == other.Name &&
                Terrain == other.Terrain &&
                Region == other.Region;
        }
    }
}