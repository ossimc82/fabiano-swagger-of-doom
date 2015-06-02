#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using db;
using db.data;
using log4net;
using wServer.logic;
using wServer.networking;
using wServer.realm.commands;
using wServer.realm.entities;
using wServer.realm.entities.player;
using wServer.realm.worlds;

#endregion

namespace wServer.realm
{
    public enum PendingPriority
    {
        Emergent,
        Destruction,
        Networking,
        Normal,
        Creation,
    }

    public struct RealmTime
    {
        public int thisTickCounts { get; set; }

        public int thisTickTimes { get; set; }

        public long tickCount { get; set; }

        public long tickTimes { get; set; }
    }

    public class RealmManager
    {
        public static List<string> Realms = new List<string>(44)
        {
            "NexusPortal.Lich",
            "NexusPortal.Goblin",
            "NexusPortal.Ghost",
            "NexusPortal.Giant",
            "NexusPortal.Gorgon",
            "NexusPortal.Blob",
            "NexusPortal.Leviathan",
            "NexusPortal.Unicorn",
            "NexusPortal.Minotaur",
            "NexusPortal.Cube",
            "NexusPortal.Pirate",
            "NexusPortal.Spider",
            "NexusPortal.Snake",
            "NexusPortal.Deathmage",
            "NexusPortal.Gargoyle",
            "NexusPortal.Scorpion",
            "NexusPortal.Djinn",
            "NexusPortal.Phoenix",
            "NexusPortal.Satyr",
            "NexusPortal.Drake",
            "NexusPortal.Orc",
            "NexusPortal.Flayer",
            "NexusPortal.Cyclops",
            "NexusPortal.Sprite",
            "NexusPortal.Chimera",
            "NexusPortal.Kraken",
            "NexusPortal.Hydra",
            "NexusPortal.Slime",
            "NexusPortal.Ogre",
            "NexusPortal.Hobbit",
            "NexusPortal.Titan",
            "NexusPortal.Medusa",
            "NexusPortal.Golem",
            "NexusPortal.Demon",
            "NexusPortal.Skeleton",
            "NexusPortal.Mummy",
            "NexusPortal.Imp",
            "NexusPortal.Bat",
            "NexusPortal.Wyrm",
            "NexusPortal.Spectre",
            "NexusPortal.Reaper",
            "NexusPortal.Beholder",
            "NexusPortal.Dragon",
            "NexusPortal.Harpy"
        };
        public static List<string> CurrentRealmNames = new List<string>();
        public const int MAX_REALM_PLAYERS = 85;

        private static readonly ILog log = LogManager.GetLogger(typeof(RealmManager));

        public ConcurrentDictionary<string, Client> Clients { get; private set; }
        public ConcurrentDictionary<int, World> Worlds { get; private set; }
        public ConcurrentDictionary<string, GuildHall> GuildHalls { get; private set; }
        public ConcurrentDictionary<string, World> LastWorld { get; private set; }

        private ConcurrentDictionary<string, Vault> vaults;

        public Random Random { get; }

        private Thread logic;
        private Thread network;
        private int nextClientId;

        private int nextWorldId;

        public RealmManager(int maxClients, int tps)
        {
            MaxClients = maxClients;
            TPS = tps;
            Clients = new ConcurrentDictionary<string, Client>();
            Worlds = new ConcurrentDictionary<int, World>();
            GuildHalls = new ConcurrentDictionary<string, GuildHall>();
            LastWorld = new ConcurrentDictionary<string, World>();
            vaults = new ConcurrentDictionary<string, Vault>();
            Random = new Random();
        }

        public BehaviorDb Behaviors { get; private set; }

        public ChatManager Chat { get; private set; }

        public CommandManager Commands { get; private set; }

        public XmlData GameData { get; private set; }

        public string InstanceId { get; private set; }

        public LogicTicker Logic { get; private set; }

        public int MaxClients { get; private set; }

        public RealmPortalMonitor Monitor { get; private set; }

        public NetworkTicker Network { get; private set; }
        public DatabaseTicker Database { get; private set; }

        public bool Terminating { get; private set; }

        public int TPS { get; private set; }

        //public Database Database { get; private set; }

        public World AddWorld(int id, World world)
        {
            if (world.Manager != null)
                throw new InvalidOperationException("World already added.");
            world.Id = id;
            Worlds[id] = world;
            OnWorldAdded(world);
            return world;
        }

        public World AddWorld(World world)
        {
            if (world.Manager != null)
                throw new InvalidOperationException("World already added.");
            world.Id = Interlocked.Increment(ref nextWorldId);
            Worlds[world.Id] = world;
            OnWorldAdded(world);
            return world;
        }

        public void CloseWorld(World world)
        {
            Monitor.WorldRemoved(world);
        }

        public async void Disconnect(Client client)
        {
            if (client == null) return;
            Client dummy;
            client.Disconnect();
            await client.Save();
            while (!Clients.TryRemove(client.Account.AccountId, out dummy) && Clients.ContainsKey(client.Account.AccountId));
            client.Dispose();
        }

        public Player FindPlayer(string name)
        {
            if (name.Split(' ').Length > 1)
                name = name.Split(' ')[1];

            return (from i in Worlds
                    where i.Key != 0
                    from e in i.Value.Players
                    where String.Equals(e.Value.Client.Account.Name, name, StringComparison.CurrentCultureIgnoreCase)
                    select e.Value).FirstOrDefault();
        }

        public Player FindPlayerRough(string name)
        {
            Player dummy;
            foreach (KeyValuePair<int, World> i in Worlds)
                if (i.Key != 0)
                    if ((dummy = i.Value.GetUniqueNamedPlayerRough(name)) != null)
                        return dummy;
            return null;
        }

        public World GetWorld(int id)
        {
            World ret;
            if (!Worlds.TryGetValue(id, out ret)) return null;
            if (ret.Id == 0) return null;
            return ret;
        }

        //public List<Player> GuildMembersOf(string guild)
        //{
        //    return (from i in Worlds where i.Key != 0 from e in i.Value.Players where String.Equals(e.Value.Guild, guild, StringComparison.CurrentCultureIgnoreCase) select e.Value).ToList();
        //}

        public void Initialize()
        {
            log.Info("Initializing Realm Manager...");

            GameData = new XmlData();
            Behaviors = new BehaviorDb(this);
            GeneratorCache.Init();
            MerchantLists.InitMerchatLists(GameData);

            AddWorld(World.NEXUS_ID, Worlds[0] = new Nexus());
            AddWorld(World.MARKET, new ClothBazaar());
            AddWorld(World.TEST_ID, new Test());
            AddWorld(World.TUT_ID, new Tutorial(true));
            AddWorld(World.DAILY_QUEST_ID, new DailyQuestRoom());
            Monitor = new RealmPortalMonitor(this);

            Task.Factory.StartNew(() => GameWorld.AutoName(1, true)).ContinueWith(_ => AddWorld(_.Result), TaskScheduler.Default);

            Chat = new ChatManager(this);
            Commands = new CommandManager(this);

            log.Info("Realm Manager initialized.");
        }

        public Vault PlayerVault(Client processor)
        {
            Vault v;
            if(!vaults.TryGetValue(processor.Account.AccountId, out v))
                vaults.TryAdd(processor.Account.AccountId, v = (Vault)AddWorld(new Vault(false, processor)));
            else
                v.Reload(processor);
            return v;
        }

        public bool RemoveVault(string accountId)
        {
            Vault dummy;
            return vaults.TryRemove(accountId, out dummy);
        }

        public bool RemoveWorld(World world)
        {
            if (world.Manager == null)
                throw new InvalidOperationException("World is not added.");
            World dummy;
            if (Worlds.TryRemove(world.Id, out dummy))
            {
                try
                {
                    OnWorldRemoved(world);
                    world.Dispose();
                    GC.Collect();
                }
                catch (Exception e)
                { log.Fatal(e); }
                return true;
            }
            return false;
        }

        public void Run()
        {
            log.Info("Starting Realm Manager...");

            Network = new NetworkTicker(this);
            Logic = new LogicTicker(this);
            Database = new DatabaseTicker();
            network = new Thread(Network.TickLoop)
            {
                Name = "Network",
                CurrentCulture = CultureInfo.InvariantCulture
            };
            logic = new Thread(Logic.TickLoop)
            {
                Name = "Logic",
                CurrentCulture = CultureInfo.InvariantCulture
            };
            //Start logic loop first
            logic.Start();
            network.Start();

            log.Info("Realm Manager started.");
        }

        public void Stop()
        {
            log.Info("Stopping Realm Manager...");

            Terminating = true;
            List<Client> saveAccountUnlock = new List<Client>();
            foreach (Client c in Clients.Values)
            {
                saveAccountUnlock.Add(c);
                c.Disconnect();
            }
            //To prevent a buggy Account in use.
            using(var db = new Database())
                foreach (Client c in saveAccountUnlock)
                    db.UnlockAccount(c.Account);

            GameData.Dispose();
            logic.Join();
            network.Join();

            log.Info("Realm Manager stopped.");
        }

        public bool TryConnect(Client psr)
        {
            Account acc = psr.Account;
            if (Clients.Count >= MaxClients)
                return false;
            if (acc.Banned)
                return false;
            psr.Id = Interlocked.Increment(ref nextClientId);
            Client dummy;
            if (Clients.ContainsKey(acc.AccountId))
                if (!Clients[acc.AccountId].Socket.Connected)
                    Clients.TryRemove(acc.AccountId, out dummy);
            bool ret = Clients.TryAdd(psr.Account.AccountId, psr);
            return ret;
        }

        private void OnWorldAdded(World world)
        {
            if(world.Manager == null)
                world.Manager = this;
            if (world is GameWorld)
                Monitor.WorldAdded(world);
            log.InfoFormat("World {0}({1}) added.", world.Id, world.Name);
        }

        private void OnWorldRemoved(World world)
        {
            world.Manager = null;
            if (world is GameWorld)
                Monitor.WorldRemoved(world);
            log.InfoFormat("World {0}({1}) removed.", world.Id, world.Name);
        }
    }

    public class TimeEventArgs : EventArgs
    {
        public TimeEventArgs(RealmTime time)
        {
            Time = time;
        }

        public RealmTime Time { get; private set; }
    }
}