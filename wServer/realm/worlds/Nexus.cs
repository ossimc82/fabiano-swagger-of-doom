#region

using System.Collections.Generic;
using System.IO;
using wServer.realm.entities;
using wServer.realm.entities.player;
using wServer.realm.terrain;

#endregion

namespace wServer.realm.worlds
{
    public class Nexus : World
    {
        public const string WINTER_RESOURCE = "wServer.realm.worlds.maps.nexus_winter.jm";
        public const string SUMMER_RESOURCE = "wServer.realm.worlds.maps.nexus_summer.jm";

        public Nexus()
        {
            Id = NEXUS_ID;
            Name = "Nexus";
            ClientWorldName = "server.nexus";
            Background = 2;
            AllowTeleport = false;
            Difficulty = -1;
        }

        protected override void Init()
        {
            LoadMap(SUMMER_RESOURCE, MapType.Json);
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time); //normal world tick

            CheckDupers();
            UpdatePortals();
        }

        private void CheckDupers()
        {
            foreach (KeyValuePair<int, World> w in Manager.Worlds)
            {
                foreach (KeyValuePair<int, World> x in Manager.Worlds)
                {
                    foreach (KeyValuePair<int, Player> y in w.Value.Players)
                    {
                        foreach (KeyValuePair<int, Player> z in x.Value.Players)
                        {
                            if (y.Value.AccountId == z.Value.AccountId && y.Value != z.Value)
                            {
                                y.Value.Client.Disconnect();
                                z.Value.Client.Disconnect();
                            }
                        }
                    }
                }
            }
        }

        private void UpdatePortals()
        {
            foreach (var i in Manager.Monitor.portals)
            {
                foreach (var j in RealmManager.CurrentRealmNames)
                {
                    if (i.Value.Name.StartsWith(j))
                    {
                        if (i.Value.Name == j) (i.Value as Portal).PortalName = i.Value.Name;
                        i.Value.Name = j + " (" + i.Key.Players.Count + "/" + RealmManager.MAX_REALM_PLAYERS + ")";
                        i.Value.UpdateCount++;
                        break;
                    }
                }
            }
        }
    }
}