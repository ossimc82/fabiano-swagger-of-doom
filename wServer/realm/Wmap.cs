#region

using System;
using System.Collections.Generic;
using System.IO;
using db.data;
using Ionic.Zlib;
using wServer.realm.entities;
using log4net;

#endregion

namespace wServer.realm
{
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
        Arena_Edge_Spawn
    }

    public enum WmapTerrain : byte
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
        BeachTowels
    }

    public struct WmapTile
    {
        public byte Elevation;
        public string Name;
        public int ObjId;
        public ushort ObjType;
        public TileRegion Region;
        public WmapTerrain Terrain;
        public ushort TileId;
        public byte UpdateCount;

        public ObjectDef ToDef(int x, int y)
        {
            List<KeyValuePair<StatsType, object>> stats = new List<KeyValuePair<StatsType, object>>();
            if (!string.IsNullOrEmpty(Name))
                foreach (string item in Name.Split(';'))
                {
                    string[] kv = item.Split(':');
                    switch (kv[0])
                    {
                        case "name":
                            stats.Add(new KeyValuePair<StatsType, object>(StatsType.Name, kv[1]));
                            break;
                        case "size":
                            stats.Add(new KeyValuePair<StatsType, object>(StatsType.Size, Utils.FromString(kv[1])));
                            break;
                        case "eff":
                            stats.Add(new KeyValuePair<StatsType, object>(StatsType.Effects, Utils.FromString(kv[1])));
                            break;
                        case "conn":
                            stats.Add(new KeyValuePair<StatsType, object>(StatsType.ObjectConnection,
                                Utils.FromString(kv[1])));
                            break;
                        case "hp":
                            stats.Add(new KeyValuePair<StatsType, object>(StatsType.HP, Utils.FromString(kv[1])));
                            break;
                        case "mcost":
                            stats.Add(new KeyValuePair<StatsType, object>(StatsType.SellablePrice,
                                Utils.FromString(kv[1])));
                            break;
                        case "mcur":
                            stats.Add(new KeyValuePair<StatsType, object>(StatsType.SellablePriceCurrency,
                                Utils.FromString(kv[1])));
                            break;
                        case "mtype":
                            stats.Add(new KeyValuePair<StatsType, object>(StatsType.MerchantMerchandiseType,
                                Utils.FromString(kv[1])));
                            break;
                        case "mcount":
                            stats.Add(new KeyValuePair<StatsType, object>(StatsType.MerchantRemainingCount,
                                Utils.FromString(kv[1])));
                            break;
                        case "mtime":
                            stats.Add(new KeyValuePair<StatsType, object>(StatsType.MerchantRemainingMinute,
                                Utils.FromString(kv[1])));
                            break;
                        case "stars":
                            stats.Add(new KeyValuePair<StatsType, object>(StatsType.SellableRankRequirement,
                                Utils.FromString(kv[1])));
                            break;
                            //case "nstar":
                            //    entity.Stats[StatsType.NameChangerStar] = Utils.FromString(kv[1]); break;
                    }
                }
            return new ObjectDef
            {
                ObjectType = ObjType,
                Stats = new ObjectStats
                {
                    Id = ObjId,
                    Position = new Position
                    {
                        X = x + 0.5f,
                        Y = y + 0.5f
                    },
                    Stats = stats.ToArray()
                }
            };
        }

        public WmapTile Clone()
        {
            return new WmapTile
            {
                UpdateCount = (byte) (UpdateCount + 1),
                TileId = TileId,
                Name = Name,
                ObjType = ObjType,
                Terrain = Terrain,
                Region = Region,
                ObjId = ObjId,
            };
        }
    }

    public class Wmap : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Wmap));

        private readonly XmlData data;
        private Tuple<IntPoint, ushort, string>[] entities;
        private WmapTile[,] tiles;

        public Wmap(XmlData data)
        {
            this.data = data;
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public WmapTile this[int x, int y]
        {
            get { try { return tiles[x, y]; } catch { return new WmapTile(); } }
            set { tiles[x, y] = value; }
        }

        public int Load(Stream stream, int idBase)
        {
            int ver = stream.ReadByte();
            using (BinaryReader rdr = new BinaryReader(new ZlibStream(stream, CompressionMode.Decompress)))
            {
                if (ver == 0) return LoadV0(rdr, idBase);
                if (ver == 1) return LoadV1(rdr, idBase);
                if (ver == 2) return LoadV2(rdr, idBase);
                throw new NotSupportedException("WMap version " + ver);
            }
        }

        private int LoadV0(BinaryReader reader, int idBase)
        {
            List<WmapTile> dict = new List<WmapTile>();
            ushort c = (ushort)reader.ReadInt16();
            for (ushort i = 0; i < c; i++)
            {
                WmapTile tile = new WmapTile();
                tile.TileId = (ushort)reader.ReadInt16();
                string obj = reader.ReadString();
                tile.ObjType = String.IsNullOrEmpty(obj) ? (ushort) 0 : data.IdToObjectType[obj];
                tile.Name = reader.ReadString();
                tile.Terrain = (WmapTerrain) reader.ReadByte();
                tile.Region = (TileRegion) reader.ReadByte();
                dict.Add(tile);
            }
            Width = reader.ReadInt32();
            Height = reader.ReadInt32();
            tiles = new WmapTile[Width, Height];
            int enCount = 0;
            List<Tuple<IntPoint, ushort, string>> entities = new List<Tuple<IntPoint, ushort, string>>();
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    WmapTile tile = dict[reader.ReadInt16()];
                    tile.UpdateCount = 1;

                    ObjectDesc desc;
                    if (tile.ObjType != 0 &&
                        (!data.ObjectDescs.TryGetValue(tile.ObjType, out desc) ||
                         !desc.Static || desc.Enemy))
                    {
                        entities.Add(new Tuple<IntPoint, ushort, string>(new IntPoint(x, y), tile.ObjType, tile.Name));
                        tile.ObjType = 0;
                    }

                    if (tile.ObjType != 0)
                    {
                        enCount++;
                        tile.ObjId = idBase + enCount;
                    }


                    tiles[x, y] = tile;
                }
            this.entities = entities.ToArray();
            return enCount;
        }

        private int LoadV1(BinaryReader reader, int idBase)
        {
            List<WmapTile> dict = new List<WmapTile>();
            ushort c = (ushort)reader.ReadInt16();
            for (ushort i = 0; i < c; i++)
            {
                WmapTile tile = new WmapTile();
                tile.TileId = (ushort)reader.ReadInt16();
                string obj = reader.ReadString();
                tile.ObjType = string.IsNullOrEmpty(obj) ? (ushort) 0 : data.IdToObjectType[obj];
                tile.Name = reader.ReadString();
                tile.Terrain = (WmapTerrain) reader.ReadByte();
                tile.Region = (TileRegion) reader.ReadByte();
                tile.Elevation = reader.ReadByte();
                dict.Add(tile);
            }
            Width = reader.ReadInt32();
            Height = reader.ReadInt32();
            tiles = new WmapTile[Width, Height];
            int enCount = 0;
            List<Tuple<IntPoint, ushort, string>> entities = new List<Tuple<IntPoint, ushort, string>>();
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    WmapTile tile = dict[reader.ReadInt16()];
                    tile.UpdateCount = 1;

                    ObjectDesc desc;
                    if (tile.ObjType != 0 &&
                        (!data.ObjectDescs.TryGetValue(tile.ObjType, out desc) ||
                         !desc.Static || desc.Enemy))
                    {
                        entities.Add(new Tuple<IntPoint, ushort, string>(new IntPoint(x, y), tile.ObjType, tile.Name));
                        tile.ObjType = 0;
                    }

                    if (tile.ObjType != 0)
                    {
                        enCount++;
                        tile.ObjId = idBase + enCount;
                    }


                    tiles[x, y] = tile;
                }
            this.entities = entities.ToArray();
            return enCount;
        }

        private int LoadV2(BinaryReader reader, int idBase)
        {
            List<WmapTile> dict = new List<WmapTile>();
            ushort c = (ushort)reader.ReadInt16();
            for (ushort i = 0; i < c; i++)
            {
                WmapTile tile = new WmapTile();
                tile.TileId = (ushort)reader.ReadInt16();
                string obj = reader.ReadString();
                try
                {
                    tile.ObjType = string.IsNullOrEmpty(obj) ? (ushort)0 : data.IdToObjectType[obj];
                }
                catch (Exception ex) { log.Error(ex); }
                tile.Name = reader.ReadString();
                tile.Terrain = (WmapTerrain)reader.ReadByte();
                tile.Region = (TileRegion)reader.ReadByte();
                dict.Add(tile);                
            }
            Width = reader.ReadInt32();
            Height = reader.ReadInt32();
            tiles = new WmapTile[Width, Height];
            int enCount = 0;
            List<Tuple<IntPoint, ushort, string>> entities = new List<Tuple<IntPoint, ushort, string>>();
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    WmapTile tile = dict[reader.ReadInt16()];
                    tile.UpdateCount = 1;
                    tile.Elevation = reader.ReadByte();

                    ObjectDesc desc;
                    if (tile.ObjType != 0 &&
                        (!data.ObjectDescs.TryGetValue(tile.ObjType, out desc) || isGuildMerchant(tile.ObjType) ||
                         !desc.Static || desc.Enemy))
                    {
                        entities.Add(new Tuple<IntPoint, ushort, string>(new IntPoint(x, y), tile.ObjType, tile.Name));
                        tile.ObjType = 0;
                    }

                    if (tile.ObjType != 0)
                    {
                        enCount++;
                        tile.ObjId = idBase + enCount;
                    }


                    tiles[x, y] = tile;
                }
            this.entities = entities.ToArray();
            return enCount;
        }

        private bool isGuildMerchant(ushort objId)
        {
            return objId == 1846 || objId == 1847 || objId == 1848;
        }

        public IEnumerable<Entity> InstantiateEntities(RealmManager manager)
        {
            foreach (Tuple<IntPoint, ushort, string> i in entities)
            {
                Entity entity = Entity.Resolve(manager, i.Item2);
                entity.Move(i.Item1.X + 0.5f, i.Item1.Y + 0.5f);
                if (i.Item3 != null)
                    foreach (string item in i.Item3.Split(';'))
                    {
                        string[] kv = item.Split(':');
                        switch (kv[0])
                        {
                            case "name":
                                entity.Name = kv[1];
                                break;
                            case "size":
                                entity.Size = Utils.FromString(kv[1]);
                                break;
                            case "eff":
                                entity.ConditionEffects = (ConditionEffects) Utils.FromString(kv[1]);
                                break;
                            case "conn":
                                (entity as ConnectedObject).Connection =
                                    ConnectionInfo.Infos[(uint) Utils.FromString(kv[1])];
                                break;
                                //case "mtype":
                                //    (entity as Merchants).custom = true;
                                //    (entity as Merchants).mType = Utils.FromString(kv[1]);
                                //    break;
                                //case "mcount":
                                //    entity.Stats[StatsType.MerchantRemainingCount] = Utils.FromString(kv[1]); break;    NOT NEEDED FOR NOW
                                //case "mtime":
                                //    entity.Stats[StatsType.MerchantRemainingMinute] = Utils.FromString(kv[1]); break;
                            case "mcost":
                                (entity as SellableObject).Price = Utils.FromString(kv[1]);
                                break;
                            case "mcur":
                                (entity as SellableObject).Currency = (CurrencyType) Utils.FromString(kv[1]);
                                break;
                            case "stars":
                                (entity as SellableObject).RankReq = Utils.FromString(kv[1]);
                                break;
                                //case "nstar":
                                //    entity.Stats[StatsType.NameChangerStar] = Utils.FromString(kv[1]); break;
                        }
                    }
                yield return entity;
            }
        }

        public void Dispose()
        {
            entities = null;
            tiles = null;
        }
    }
}