using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.realm.worlds
{
    public class BelladonnasGarden : World
    {
        public BelladonnasGarden()
        {
            Name = "Belladonna's Garden";
            ClientWorldName = "dungeons.BelladonnaAPOSs_Garden";
            Background = 0;
            AllowTeleport = false;
            Difficulty = 5;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.belladonnasGarden.wmap", MapType.Wmap);
        }
    }
}
