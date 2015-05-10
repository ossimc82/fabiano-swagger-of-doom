#region

using System;

#endregion

namespace terrain
{
    public enum TerrainType : byte
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
        ShorePlains
    }

    public enum TileRegion : byte
    {
        None,
        Spawn,
        Realm_Portals,
        Store_1,
        Store_2,
        Store_3,
        Store_4,
        Store_5,
        Store_6,
        Vault,
        Loot,
        Defender,
        Hallway,
        Enemy,
        Hallway_1,
        Hallway_2,
        Hallway_3,
        Store_7,
        Store_8,
        Store_9,
        Gifting_Chest,
        Store_10,
        Store_11,
        Store_12,
        Store_13,
        Store_14,
        Store_15,
        Store_16,
        Store_17,
        Store_18,
        Store_19,
        Store_20,
        Store_21,
        Store_22,
        Store_23,
        Store_24,
        PetRegion,
        Outside_Arena,
        Item_Spawn_Point,
        Arena_Central_Spawn,
        Arena_Edge_Spawn,
        Store_25,
        Store_26,
        Store_27,
        Store_28,
        Store_29,
        Store_30,
        Store_31,
        Store_32,
        Store_33,
        Store_34,
        Store_35,
        Store_36,
        Store_37,
        Store_38,
        Store_39,
        Store_40
    }

    internal struct TerrainTile : IEquatable<TerrainTile>
    {
        public string Biome;
        public float Elevation;
        public float Moisture;
        public string Name;
        public int PolygonId;
        public TileRegion Region;
        public TerrainType Terrain;
        public ushort TileId;
        public string TileObj;
        public int X;
        public int Y;

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