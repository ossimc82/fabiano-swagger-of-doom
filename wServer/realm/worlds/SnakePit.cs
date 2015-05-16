using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.networking;

namespace wServer.realm.worlds
{
    public class SnakePit : World
    {
        public SnakePit()
        {
            Name = "Snake Pit";
            ClientWorldName = "dungeons.Snake_Pit";
            Dungeon = true;
            Background = 0;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.snakepit.wmap", MapType.Wmap);
        }

        public override World GetInstance(Client client)
        {
            return Manager.AddWorld(new SnakePit());
        }
    }
}
