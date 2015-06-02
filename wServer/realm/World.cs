#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using DungeonGenerator;
using DungeonGenerator.Templates;
using log4net;
using wServer.networking;
using wServer.networking.svrPackets;
using wServer.realm.entities;
using wServer.realm.entities.player;
using wServer.realm.worlds;
using wServer.realm.terrain;

#endregion

namespace wServer.realm
{
    public abstract class World : IDisposable
    {
        public const int TUT_ID = -1;
        public const int NEXUS_ID = -2;
        //public const int RAND_REALM = -3;
        public const int NEXUS_LIMBO = -3;
        public const int VAULT_ID = -5;
        public const int TEST_ID = -6;
        public const int GAUNTLET = -7;
        public const int WC = -8;
        public const int ARENA = -9;
        public const int GHALL = -10;
        public const int MARKET = -11;
        public const int PETYARD_ID = -12;
        public const int DAILY_QUEST_ID = -13;
        public const int GUILD_ID = -14;
        protected static readonly ILog Log = LogManager.GetLogger(typeof(World));
        public string ExtraVar = "Default";
        private int entityInc;
        private RealmManager manager;
        private bool canBeClosed;

        protected World()
        {
            Players = new ConcurrentDictionary<int, Player>();
            Enemies = new ConcurrentDictionary<int, Enemy>();
            Quests = new ConcurrentDictionary<int, Enemy>();
            Pets = new ConcurrentDictionary<int, Pet>();
            Projectiles = new ConcurrentDictionary<Tuple<int, byte>, Projectile>();
            StaticObjects = new ConcurrentDictionary<int, StaticObject>();
            Timers = new List<WorldTimer>();
            ClientXml = ExtraXml = Empty<string>.Array;
            AllowTeleport = true;
            ShowDisplays = true;
            MaxPlayers = -1;

            //Mark world for removal after 2 minutes if the 
            //world is a dungeon and if no players in there;
            Timers.Add(new WorldTimer(120 * 1000, (w, t) =>
            {
                canBeClosed = true;
                if (NeedsPortalKey)
                    PortalKeyExpired = true;
            }));
        }

        public bool IsLimbo { get; protected set; }

        public RealmManager Manager
        {
            get { return manager; }
            internal set
            {
                manager = value;
                if (manager == null) return;
                Seed = manager.Random.NextUInt32();
                PortalKey = Utils.RandomBytes(NeedsPortalKey ? 16 : 0);
                Init();
            }
        }

        public int Id { get; internal set; }
        public int Difficulty { get; protected set; }
        public string Name { get; protected set; }
        public string ClientWorldName { get; protected set; }
        public byte[] PortalKey { get; private set; }
        public bool PortalKeyExpired { get; private set; }
        public uint Seed { get; private set; }

        public virtual bool NeedsPortalKey => false;

        public ConcurrentDictionary<int, Player> Players { get; private set; }
        public ConcurrentDictionary<int, Enemy> Enemies { get; private set; }
        public ConcurrentDictionary<int, Pet> Pets { get; }
        public ConcurrentDictionary<Tuple<int, byte>, Projectile> Projectiles { get; private set; }
        public ConcurrentDictionary<int, StaticObject> StaticObjects { get; private set; }
        public List<WorldTimer> Timers { get; }
        public int Background { get; protected set; }

        public CollisionMap<Entity> EnemiesCollision { get; private set; }
        public CollisionMap<Entity> PlayersCollision { get; private set; }
        public byte[,] Obstacles { get; private set; }

        public bool AllowTeleport { get; protected set; }
        public bool ShowDisplays { get; protected set; }
        public string[] ClientXml { get; protected set; }
        public string[] ExtraXml { get; protected set; }

        public bool Dungeon { get; protected set; }
        public bool Cave { get; protected set; }
        public bool Shaking { get; protected set; }

        public int MaxPlayers { get; protected set; }

        public Wmap Map { get; private set; }
        public ConcurrentDictionary<int, Enemy> Quests { get; }

        public virtual World GetInstance(Client psr)
        {
            return null;
        }

        public bool IsPassable(int x, int y)
        {
            var tile = Map[x, y];
            ObjectDesc desc;
            if (Manager.GameData.Tiles[tile.TileId].NoWalk)
                return false;
            if (Manager.GameData.ObjectDescs.TryGetValue(tile.ObjType, out desc))
            {
                if (!desc.Static)
                    return false;
                if (desc.OccupySquare || desc.EnemyOccupySquare || desc.FullOccupy)
                    return false;
            }
            return true;
        }

        public int GetNextEntityId()
        {
            return Interlocked.Increment(ref entityInc);
        }

        public bool Delete()
        {
            lock (this)
            {
                if (Players.Count > 0) return false;
                Id = 0;
            }
            Map = null;
            Players = null;
            Enemies = null;
            Projectiles = null;
            StaticObjects = null;
            return true;
        }

        public virtual void BehaviorEvent(string type)
        {
        }

        protected abstract void Init();

        private void FromWorldMap(Stream dat)
        {
            var map = new Wmap(Manager.GameData);
            Map = map;
            entityInc = 0;
            entityInc += Map.Load(dat, 0);

            int w = Map.Width, h = Map.Height;
            Obstacles = new byte[w, h];
            for (var y = 0; y < h; y++)
                for (var x = 0; x < w; x++)
                {
                    try
                    {
                        var tile = Map[x, y];
                        ObjectDesc desc;
                        if (Manager.GameData.Tiles[tile.TileId].NoWalk)
                            Obstacles[x, y] = 3;
                        if (Manager.GameData.ObjectDescs.TryGetValue(tile.ObjType, out desc))
                        {
                            if (desc.Class == "Wall" ||
                                desc.Class == "ConnectedWall" ||
                                desc.Class == "CaveWall")
                                Obstacles[x, y] = 2;
                            else if (desc.OccupySquare || desc.EnemyOccupySquare)
                                Obstacles[x, y] = 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                }
            EnemiesCollision = new CollisionMap<Entity>(0, w, h);
            PlayersCollision = new CollisionMap<Entity>(1, w, h);

            Projectiles.Clear();
            StaticObjects.Clear();
            Enemies.Clear();
            Players.Clear();
            foreach (var i in Map.InstantiateEntities(Manager))
            {
                if (i.ObjectDesc != null &&
                    (i.ObjectDesc.OccupySquare || i.ObjectDesc.EnemyOccupySquare))
                    Obstacles[(int)(i.X - 0.5), (int)(i.Y - 0.5)] = 2;
                EnterWorld(i);
            }
        }

        //public void FromJsonMap(string file)
        //{
        //    if (File.Exists(file))
        //    {
        //        var wmap = Json2Wmap.Convert(File.ReadAllText(file));

        //        FromWorldMap(new MemoryStream(wmap));
        //    }
        //    else
        //    {
        //        throw new FileNotFoundException("Json file not found!", file);
        //    }
        //}

        //public void FromJsonStream(Stream dat)
        //{
        //    byte[] data = { };
        //    dat.Read(data, 0, (int)dat.Length);
        //    var json = Encoding.ASCII.GetString(data);
        //    var wmap = Json2Wmap.Convert(json);
        //    FromWorldMap(new MemoryStream(wmap));
        //} //not working

        public virtual int EnterWorld(Entity entity)
        {
            var player = entity as Player;
            if (player != null)
            {
                try
                {
                    player.Id = GetNextEntityId();
                    entity.Init(this);
                    Players.TryAdd(player.Id, player);
                    PlayersCollision.Insert(player);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
            else
            {
                var enemy = entity as Enemy;
                if (enemy != null)
                {
                    enemy.Id = GetNextEntityId();
                    entity.Init(this);
                    Enemies.TryAdd(enemy.Id, enemy);
                    EnemiesCollision.Insert(enemy);
                    if (enemy.ObjectDesc.Quest)
                        Quests.TryAdd(enemy.Id, enemy);
                }
                else
                {
                    var projectile = entity as Projectile;
                    if (projectile != null)
                    {
                        projectile.Init(this);
                        var prj = projectile;
                        Projectiles[new Tuple<int, byte>(prj.ProjectileOwner.Self.Id, prj.ProjectileId)] = prj;
                    }
                    else
                    {
                        var staticObject = entity as StaticObject;
                        if (staticObject != null)
                        {
                            staticObject.Id = GetNextEntityId();
                            staticObject.Init(this);
                            StaticObjects.TryAdd(staticObject.Id, staticObject);
                            if (entity is Decoy)
                                PlayersCollision.Insert(staticObject);
                            else
                                EnemiesCollision.Insert(staticObject);
                        }
                        else
                        {
                            var pet = entity as Pet;
                            if (pet == null) return entity.Id;
                            if (pet.IsPet)
                            {
                                pet.Id = GetNextEntityId();
                                pet.Init(this);
                                if (!Pets.TryAdd(pet.Id, pet))
                                    Log.Error("Failed to add pet!");

                                PlayersCollision.Insert(pet);
                            }
                            else
                                Log.WarnFormat("This is not a real pet! {0}", pet.Name);
                        }
                    }
                }
            }
            return entity.Id;
        }

        public virtual void LeaveWorld(Entity entity)
        {
            if (entity is Player)
            {
                Player dummy;
                if (!Players.TryRemove(entity.Id, out dummy))
                    Log.WarnFormat("Could not remove {0} from world {1}", entity.Name, Name);
                PlayersCollision.Remove(entity);
            }
            else if (entity is Enemy)
            {
                Enemy dummy;
                Enemies.TryRemove(entity.Id, out dummy);
                EnemiesCollision.Remove(entity);
                if (entity.ObjectDesc.Quest)
                    Quests.TryRemove(entity.Id, out dummy);
            }
            else
            {
                var projectile = entity as Projectile;
                if (projectile != null)
                {
                    var p = projectile;
                    Projectiles.TryRemove(new Tuple<int, byte>(p.ProjectileOwner.Self.Id, p.ProjectileId), out p);
                }
                else if (entity is StaticObject)
                {
                    StaticObject dummy;
                    StaticObjects.TryRemove(entity.Id, out dummy);
                    if (entity is Decoy)
                        PlayersCollision.Remove(entity);
                    else
                        EnemiesCollision.Remove(entity);
                }
                else if (entity is Pet)
                {
                    if (entity.IsPet)
                    {
                        Pet dummy2;
                        Pets.TryRemove(entity.Id, out dummy2);
                        PlayersCollision.Remove(entity);
                    }
                }
            }
            entity.Owner = null;
            entity.Dispose();
        }

        public Entity GetEntity(int id)
        {
            Player ret1;
            if (Players.TryGetValue(id, out ret1)) return ret1;
            Enemy ret2;
            if (Enemies.TryGetValue(id, out ret2)) return ret2;
            StaticObject ret3;
            if (StaticObjects.TryGetValue(id, out ret3)) return ret3;
            return null;
        }

        public Player GetPlayerByName(string name)
        {
            return (from i in Players where i.Value.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) select i.Value).FirstOrDefault();
        }

        public Player GetUniqueNamedPlayerRough(string name)
        {
            return (from i in Players where i.Value.CompareName(name) select i.Value).FirstOrDefault();
        }

        public void BroadcastPacket(Packet pkt, Player exclude)
        {
            foreach (var i in Players.Where(i => i.Value != exclude))
                i.Value.Client.SendPacket(pkt);
        }

        public void BroadcastPacketSync(Packet pkt, Predicate<Player> exclude)
        {
            foreach (var i in Players.Where(i => exclude(i.Value)))
                i.Value.Client.SendPacket(pkt);
        }

        public void BroadcastPackets(IEnumerable<Packet> pkts, Player exclude)
        {
            foreach (var i in Players.Where(i => i.Value != exclude))
                i.Value.Client.SendPackets(pkts);
        }

        public void BroadcastPacketsSync(IEnumerable<Packet> pkts, Predicate<Player> exclude)
        {
            foreach (var i in Players.Where(i => exclude(i.Value)))
                i.Value.Client.SendPackets(pkts);
        }

        public virtual void Tick(RealmTime time)
        {
            try
            {
                if (IsLimbo) return;

                for (var i = 0; i < Timers.Count; i++)
                {
                    try
                    {
                        if (Timers[i] == null) continue;
                        if (!Timers[i].Tick(this, time)) continue;
                        Timers.RemoveAt(i);
                        i--;
                    }
                    catch
                    {
                        // ignored
                    }
                }

                foreach (var i in Players)
                    i.Value.Tick(time);

                foreach (var i in Pets)
                    i.Value.Tick(time);

                if (EnemiesCollision != null)
                {
                    foreach (var i in EnemiesCollision.GetActiveChunks(PlayersCollision))
                        i.Tick(time);
                    foreach (var i in StaticObjects.Where(x => x.Value is Decoy))
                        i.Value.Tick(time);
                }
                else
                {
                    foreach (var i in Enemies)
                        i.Value.Tick(time);
                    foreach (var i in StaticObjects)
                        i.Value.Tick(time);
                }
                foreach (var i in Projectiles)
                    i.Value.Tick(time);

                if (Players.Count != 0 || !canBeClosed || !IsDungeon()) return;
                var vault = this as Vault;
                if (vault != null) Manager.RemoveVault(vault.AccountId);
                Manager.RemoveWorld(this);
            }
            catch (Exception e)
            {
                Log.Error("World: " + Name + "\n" + e);
            }
        }

        public bool IsFull => MaxPlayers != -1 && Players.Keys.Count >= MaxPlayers;

        public bool IsDungeon()
        {
            return !(this is Nexus) && !(this is GameWorld) && !(this is ClothBazaar) && !(this is Test) && !(this is GuildHall) && !(this is Tutorial) && !(this is DailyQuestRoom) && !IsLimbo;
        }

        protected void LoadMap(string embeddedResource, MapType type)
        {
            if(embeddedResource == null) return;
            var stream = typeof(RealmManager).Assembly.GetManifestResourceStream(embeddedResource);
            if (stream == null) throw new ArgumentException("Resource not found", nameof(embeddedResource));

            switch (type)
            {
                case MapType.Wmap:
                    FromWorldMap(stream);
                    break;
                case MapType.Json:
                    FromWorldMap(new MemoryStream(Json2Wmap.Convert(Manager, new StreamReader(stream).ReadToEnd())));
                    break;
                default:
                    throw new ArgumentException("Invalid MapType");
            }
        }

        protected void LoadMap(string json)
        {
            FromWorldMap(new MemoryStream(Json2Wmap.Convert(Manager, json)));
        }

        public void ChatReceived(string text)
        {
            foreach (var en in Enemies)
                en.Value.OnChatTextReceived(text);
            foreach (var en in StaticObjects)
                en.Value.OnChatTextReceived(text);
        }

        public virtual void Dispose()
        {
            Map.Dispose();
            Players.Clear();
            Enemies.Clear();
            Quests.Clear();
            Pets.Clear();
            Projectiles.Clear();
            StaticObjects.Clear();
            Timers.Clear();
            EnemiesCollision = null;
            PlayersCollision = null;
        }
    }

    public enum MapType
    {
        Wmap,
        Json
    }
}