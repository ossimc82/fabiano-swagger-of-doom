#region

using System;
using System.Collections.Generic;
using System.Linq;
using wServer.networking.svrPackets;
using wServer.realm.worlds;

#endregion

namespace wServer.realm.entities.player
{
    public partial class Player
    {
        public int UpdatesSend { get; private set; }
        public int UpdatesReceived { get; set; }

        public const int SIGHTRADIUS = 15;

        private static int AppoxAreaOfSight = (int)(Math.PI * SIGHTRADIUS * SIGHTRADIUS + 1);

        private readonly HashSet<Entity> _clientEntities = new HashSet<Entity>();
        private readonly HashSet<IntPoint> _clientStatic = new HashSet<IntPoint>(new IntPointComparer());
        private readonly Dictionary<Entity, int> _lastUpdate = new Dictionary<Entity, int>();
        private int _mapHeight;
        private int _mapWidth;
        private int _tickId;

        private IEnumerable<Entity> GetNewEntities()
        {
            foreach (KeyValuePair<int, Player> i in Owner.Players.Where(i => _clientEntities.Add(i.Value)))
            {
                if (!i.Value.vanished || i.Value == this)
                {
                    yield return i.Value;
                }
            }
            foreach (
                Decoy i in
                    Owner.PlayersCollision.HitTest(X, Y, SIGHTRADIUS).OfType<Decoy>().Where(i => _clientEntities.Add(i))
                )
            {
                yield return i;
            }

            foreach (
                Pet i in
                    Owner.PlayersCollision.HitTest(X, Y, SIGHTRADIUS).OfType<Pet>().Where(i => _clientEntities.Add(i))
                )
            {
                yield return i;
            }

            foreach (Entity i in Owner.EnemiesCollision.HitTest(X, Y, SIGHTRADIUS))
            {
                if (i is Container)
                {
                    string owner = (i as Container).BagOwners != null
                        ? (i as Container).BagOwners.Length == 1 ? (i as Container).BagOwners[0] : null as string
                        : null;
                    if (owner != null && owner != AccountId) continue;

                    if(owner == AccountId)
                        if ((LootDropBoost || LootTierBoost) && (i.ObjectType != 0x500 || i.ObjectType != 0x506))
                            (i as Container).BoostedBag = true; //boosted bag

                }
                if (MathsUtils.DistSqr(i.X, i.Y, X, Y) <= SIGHTRADIUS * SIGHTRADIUS)
                {
                    if (_clientEntities.Add(i))
                        yield return i;
                }
            }
            if (questEntity != null && _clientEntities.Add(questEntity))
                yield return questEntity;
        }

        private IEnumerable<int> GetRemovedEntities()
        {
            foreach (Entity i in _clientEntities.Where(i => i is Player))
            {
                if ((i as Player).vanished && i != this)
                {
                    yield return i.Id;
                }
            }
            foreach (Entity i in _clientEntities.Where(i => !(i is Player) || i.Owner == null))
            {
                if (MathsUtils.DistSqr(i.X, i.Y, X, Y) > SIGHTRADIUS * SIGHTRADIUS &&
                    !(i is StaticObject && (i as StaticObject).Static) &&
                    i != questEntity)
                {
                    if (i is Pet)
                    {
                        continue;
                       //yield return i.Id;
                       //(i as Pet).UpdateNeeded = true;
                    }

                    yield return i.Id;
                }
                else if (i.Owner == null)
                    yield return i.Id;

                if (i is Player)
                {
                    if ((i as Player).vanished && i != this)
                    {
                        yield return i.Id;
                    }
                }
            }
        }

        private IEnumerable<ObjectDef> GetNewStatics(int _x, int _y)
        {
            List<ObjectDef> ret = new List<ObjectDef>();
            foreach (var i in Sight.GetSightCircle(SIGHTRADIUS))
            {
                int x = i.X + _x;
                int y = i.Y + _y;
                if (x < 0 || x >= _mapWidth ||
                    y < 0 || y >= _mapHeight) continue;

                var tile = Owner.Map[x, y];

                if (tile.ObjId != 0 && tile.ObjType != 0 && _clientStatic.Add(new IntPoint(x, y)))
                {
                    ObjectDef def = tile.ToDef(x, y);
                    string cls = Manager.GameData.ObjectDescs[tile.ObjType].Class;
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
            }
            return ret;
        }

        private IEnumerable<IntPoint> GetRemovedStatics(int _x, int _y)
        {
            return from i in _clientStatic
                   let dx = i.X - _x
                   let dy = i.Y - _y
                   let tile = Owner.Map[i.X, i.Y]
                   where dx * dx + dy * dy > SIGHTRADIUS * SIGHTRADIUS ||
                         tile.ObjType == 0
                   let objId = Owner.Map[i.X, i.Y].ObjId
                   where objId != 0
                   select i;
        }

        public void SendUpdate(RealmTime time)
        {
            _mapWidth = Owner.Map.Width;
            _mapHeight = Owner.Map.Height;
            var map = Owner.Map;
            int _x = (int)X; int _y = (int)Y;

            var sendEntities = new HashSet<Entity>(GetNewEntities());

            var list = new List<UpdatePacket.TileData>(AppoxAreaOfSight);
            int sent = 0;
            foreach (var i in Sight.GetSightCircle(SIGHTRADIUS))
            {
                int x = i.X + _x;
                int y = i.Y + _y;

                WmapTile tile;
                if (x < 0 || x >= _mapWidth ||
                    y < 0 || y >= _mapHeight ||
                    tiles[x, y] >= (tile = map[x, y]).UpdateCount) continue;

                var world = Manager.GetWorld(this.Owner.Id);
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
            _clientEntities.RemoveWhere(_ => Array.IndexOf(dropEntities, _.Id) != -1);

            List<Entity> toRemove = new List<Entity>();
            foreach (Entity i in _lastUpdate.Keys.Where(i => !_clientEntities.Contains(i)))
                toRemove.Add(i);
            toRemove.ForEach(i => _lastUpdate.Remove(i));

            foreach (var i in sendEntities)
                _lastUpdate[i] = i.UpdateCount;

            var newStatics = GetNewStatics(_x, _y).ToArray();
            var removeStatics = GetRemovedStatics(_x, _y).ToArray();
            List<int> removedIds = new List<int>();
            foreach (var i in removeStatics)
            {
                removedIds.Add(Owner.Map[i.X, i.Y].ObjId);
                _clientStatic.Remove(i);
            }

            if (sendEntities.Count > 0 || list.Count > 0 || dropEntities.Length > 0 ||
                newStatics.Length > 0 || removedIds.Count > 0)
            {
                UpdatePacket packet = new UpdatePacket()
                {
                    Tiles = list.ToArray(),
                    NewObjects = sendEntities.Select(_ => _.ToDefinition()).Concat(newStatics).ToArray(),
                    RemovedObjectIds = dropEntities.Concat(removedIds).ToArray()
                };
                Client.SendPacket(packet);
                UpdatesSend++;
            }
        }

        private void SendNewTick(RealmTime time)
        {
            List<Entity> sendEntities = new List<Entity>();
            try
            {
                foreach (Entity i in _clientEntities.Where(i => i.UpdateCount > _lastUpdate[i]))
                {
                    sendEntities.Add(i);
                    _lastUpdate[i] = i.UpdateCount;
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            if (questEntity != null &&
                (!_lastUpdate.ContainsKey(questEntity) || questEntity.UpdateCount > _lastUpdate[questEntity]))
            {
                sendEntities.Add(questEntity);
                _lastUpdate[questEntity] = questEntity.UpdateCount;
            }
            NewTickPacket p = new NewTickPacket();
            _tickId++;
            p.TickId = _tickId;
            p.TickTime = time.thisTickTimes;
            p.UpdateStatuses = sendEntities.Select(_ => _.ExportStats()).ToArray();
            Client.SendPacket(p);
        }
    }
}