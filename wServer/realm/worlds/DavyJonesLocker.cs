using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm.entities.player;

namespace wServer.realm.worlds
{
    public class DavyJonesLocker : World
    {
        public DavyJonesLocker()
        {
            Name = "Davy Jones's Locker";
            ClientWorldName = "dungeons.Davy_JonesAPOSs_Locker";
            Dungeon = true;
            Difficulty = 5;
            Background = 0;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.vault.wmap", MapType.Wmap);
        }

        public override int EnterWorld(Entity entity)
        {
            int ret = base.EnterWorld(entity);
            if (entity is Player)
                (entity as Player).Client.SendPacket(new networking.svrPackets.Global_NotificationPacket
                {
                    Text = "showKeyUI",
                    Type = 0
                });
            return ret;
        }

        public override void LeaveWorld(Entity entity)
        {
            if(entity is Player)
                (entity as Player).Client.SendPacket(new networking.svrPackets.Global_NotificationPacket
                {
                    Text = "showKeyUI",
                    Type = 0
                });
            base.LeaveWorld(entity);
        }
    }
}
