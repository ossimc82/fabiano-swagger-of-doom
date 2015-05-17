#region

using System;
using System.Collections.Generic;
using System.Linq;
using wServer.networking.svrPackets;

#endregion

namespace wServer.realm.entities.player
{
    public partial class Player
    {
        public int UpdatesSend { get; private set; }
        public int UpdatesReceived { get; set; }

        public const int SIGHTRADIUS = 15;

        private const int APPOX_AREA_OF_SIGHT = (int)(Math.PI * SIGHTRADIUS * SIGHTRADIUS + 1);

        private readonly HashSet<Entity> clientEntities = new HashSet<Entity>();
        private readonly HashSet<IntPoint> clientStatic = new HashSet<IntPoint>(new IntPointComparer());
        private readonly Dictionary<Entity, int> lastUpdate = new Dictionary<Entity, int>();
        private int mapHeight;
        private int mapWidth;
        private int tickId;

        private IEnumerable<Entity> GetNewEntities()
        {
            foreach (var i in Owner.Players.Where(i => clientEntities.Add(i.Value)))
                yield return i.Value;

            foreach (var i in Owner.PlayersCollision.HitTest(X, Y, SIGHTRADIUS).OfType<Decoy>().Where(i => clientEntities.Add(i)))
                yield return i;

            foreach (var i in Owner.PlayersCollision.HitTest(X, Y, SIGHTRADIUS).OfType<Pet>().Where(i => clientEntities.Add(i)))
                yield return i;

            foreach (var i in Owner.EnemiesCollision.HitTest(X, Y, SIGHTRADIUS))
            {
                if (i is Container)
                {
                    var owner = (i as Container).BagOwners?.Length == 1 ? (i as Container).BagOwners[0] : null;
                    if (owner != null && owner != AccountId) continue;

                    if (owner == AccountId)
                        if ((LootDropBoost || LootTierBoost) && (i.ObjectType != 0x500 || i.ObjectType != 0x506))
                            (i as Container).BoostedBag = true; //boosted bag

                }
                if (!(MathsUtils.DistSqr(i.X, i.Y, X, Y) <= SIGHTRADIUS * SIGHTRADIUS)) continue;
                if (clientEntities.Add(i))
                    yield return i;
            }
            if (Quest != null && clientEntities.Add(Quest))
                yield return Quest;
        }

        private IEnumerable<int> GetRemovedEntities()
        {
            foreach (var i in clientEntities.Where(i => !(i is Player) || i.Owner == null))
            {
                if (MathsUtils.DistSqr(i.X, i.Y, X, Y) > SIGHTRADIUS * SIGHTRADIUS &&
                    !(i is StaticObject && (i as StaticObject).Static) &&
                    i != Quest)
                {
                    if (i is Pet) continue;
                    yield return i.Id;
                }
                else if (i.Owner == null)
                    yield return i.Id;

                if (!(i is Player)) continue;
                if (i != this)
                    yield return i.Id;
            }
        }

        private IEnumerable<ObjectDef> GetNewStatics(int xBase, int yBase)
        {
            var ret = new List<ObjectDef>();
            foreach (var i in Sight.GetSightCircle(SIGHTRADIUS))
            {
                var x = i.X + xBase;
                var y = i.Y + yBase;
                if (x < 0 || x >= mapWidth ||
                    y < 0 || y >= mapHeight) continue;

                var tile = Owner.Map[x, y];

                if (tile.ObjId == 0 || tile.ObjType == 0 || !clientStatic.Add(new IntPoint(x, y))) continue;
                var def = tile.ToDef(x, y);
                var cls = Manager.GameData.ObjectDescs[tile.ObjType].Class;
                if (cls == "ConnectedWall" || cls == "CaveWall")
                {
                    if (def.Stats.Stats.Count(_ => _.Key == StatsType.ObjectConnection && _.Value != null) == 0)
                    {
                        var stats = def.Stats.Stats.ToList();
                        stats.Add(new KeyValuePair<StatsType, object>(StatsType.ObjectConnection, (int)ConnectionComputer.Compute((xx, yy) => Owner.Map[x + xx, y + yy].ObjType == tile.ObjType).Bits));
                        def.Stats.Stats = stats.ToArray();
                    }
                }
                ret.Add(def);
            }
            return ret;
        }

        private IEnumerable<IntPoint> GetRemovedStatics(int xBase, int yBase)
        {
            return from i in clientStatic
                   let dx = i.X - xBase
                   let dy = i.Y - yBase
                   let tile = Owner.Map[i.X, i.Y]
                   where dx * dx + dy * dy > SIGHTRADIUS * SIGHTRADIUS ||
                         tile.ObjType == 0
                   let objId = Owner.Map[i.X, i.Y].ObjId
                   where objId != 0
                   select i;
        }

        public void SendUpdate(RealmTime time)
        {
            mapWidth = Owner.Map.Width;
            mapHeight = Owner.Map.Height;
            var map = Owner.Map;
            var xBase = (int)X;
            var yBase = (int)Y;

            var sendEntities = new HashSet<Entity>(GetNewEntities());

            var list = new List<UpdatePacket.TileData>(APPOX_AREA_OF_SIGHT);
            var sent = 0;
            foreach (var i in Sight.GetSightCircle(SIGHTRADIUS))
            {
                var x = i.X + xBase;
                var y = i.Y + yBase;

                WmapTile tile;
                if (x < 0 || x >= mapWidth ||
                    y < 0 || y >= mapHeight ||
                    tiles[x, y] >= (tile = map[x, y]).UpdateCount) continue;

                var world = Manager.GetWorld(Owner.Id);
                if (world.Dungeon)
                {
                    //Todo add blocksight
                }

                list.Add(new UpdatePacket.TileData()
                {
                    X = (short)x,
                    Y = (short)y,
                    Tile = tile.TileId
                });
                tiles[x, y] = tile.UpdateCount;
                sent++;
            }
            FameCounter.TileSent(sent);
             
            var dropEntities = GetRemovedEntities().Distinct().ToArray();
            clientEntities.RemoveWhere(_ => Array.IndexOf(dropEntities, _.Id) != -1);

            var toRemove = lastUpdate.Keys.Where(i => !clientEntities.Contains(i)).ToList();
            toRemove.ForEach(i => lastUpdate.Remove(i));

            foreach (var i in sendEntities)
                lastUpdate[i] = i.UpdateCount;

            var newStatics = GetNewStatics(xBase, yBase).ToArray();
            var removeStatics = GetRemovedStatics(xBase, yBase).ToArray();
            var removedIds = new List<int>();
            foreach (var i in removeStatics)
            {
                removedIds.Add(Owner.Map[i.X, i.Y].ObjId);
                clientStatic.Remove(i);
            }

            if (sendEntities.Count <= 0 && list.Count <= 0 && dropEntities.Length <= 0 && newStatics.Length <= 0 &&
                removedIds.Count <= 0) return;
            var packet = new UpdatePacket()
            {
                Tiles = list.ToArray(),
                NewObjects = sendEntities.Select(_ => _.ToDefinition()).Concat(newStatics).ToArray(),
                RemovedObjectIds = dropEntities.Concat(removedIds).ToArray()
            };
            Client.SendPacket(packet);
            UpdatesSend++;
        }

        private void SendNewTick(RealmTime time)
        {
            var sendEntities = new List<Entity>();
            try
            {
                foreach (var i in clientEntities.Where(i => i.UpdateCount > lastUpdate[i]))
                {
                    sendEntities.Add(i);
                    lastUpdate[i] = i.UpdateCount;
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            if (Quest != null &&
                (!lastUpdate.ContainsKey(Quest) || Quest.UpdateCount > lastUpdate[Quest]))
            {
                sendEntities.Add(Quest);
                lastUpdate[Quest] = Quest.UpdateCount;
            }
            var p = new NewTickPacket();
            tickId++;
            p.TickId = tickId;
            p.TickTime = time.thisTickTimes;
            p.UpdateStatuses = sendEntities.Select(_ => _.ExportStats()).ToArray();
            Client.SendPacket(p);
        }
    }
}