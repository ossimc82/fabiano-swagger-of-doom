#region

using System;
using log4net;
using wServer.realm.entities;
using wServer.realm.entities.player;
using wServer.realm.setpieces;

#endregion

namespace wServer.realm.worlds
{
    internal class GameWorld : World
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (GameWorld));

        private readonly int mapId;
        private readonly bool oryxPresent;
        private string displayname;

        public GameWorld(int mapId, string name, bool oryxPresent)
        {
            displayname = name;
            Name = name;
            ClientWorldName = name;
            Background = 0;
            Difficulty = -1;
            this.oryxPresent = oryxPresent;
            this.mapId = mapId;
        }

        public Oryx Overseer { get; private set; }

        protected override void Init()
        {
            log.InfoFormat("Initializing Game World {0}({1}) from map {2}...", Id, Name, mapId);
            LoadMap("wServer.realm.worlds.maps.world" + mapId + ".wmap", MapType.Wmap);
            SetPieces.ApplySetPieces(this);
            if (oryxPresent)
                Overseer = new Oryx(this);
            else
                Overseer = null;
            log.Info("Game World initalized.");
        }

        public static GameWorld AutoName(int mapId, bool oryxPresent)
        {
            string name = RealmManager.Realms[new Random().Next(RealmManager.Realms.Count)];
            RealmManager.Realms.Remove(name);
            RealmManager.CurrentRealmNames.Add(name);
            return new GameWorld(mapId, name, oryxPresent);
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);
            if (Overseer != null)
                Overseer.Tick(time);
        }

        public void EnemyKilled(Enemy enemy, Player killer)
        {
            if (Overseer != null)
                Overseer.OnEnemyKilled(enemy, killer);
        }

        public override int EnterWorld(Entity entity)
        {
            int ret = base.EnterWorld(entity);
            if (entity is Player)
                Overseer.OnPlayerEntered(entity as Player);
            return ret;
        }

        public override void Dispose()
        {
            if (Overseer != null)
                Overseer.Dispose();
            base.Dispose();
        }
    }
}