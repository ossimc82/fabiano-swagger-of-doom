using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;

namespace wServer.networking.handlers
{
    internal class LeaveArenaHandler : PacketHandlerBase<LeaveArenaPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.LEAVEARENA; }
        }

        protected override void HandlePacket(Client client, LeaveArenaPacket packet)
        {
            if (client.Player.Owner == null) return;
            World world = client.Manager.GetWorld(client.Player.Owner.Id);
            if (world.Id == World.NEXUS_ID)
            {
                client.SendPacket(new TextPacket
                {
                    Stars = -1,
                    BubbleTime = 0,
                    Name = "",
                    Text = "server.already_nexus"
                });
                return;
            }
            client.Reconnect(new ReconnectPacket
            {
                Host = "",
                Port = Program.Settings.GetValue<int>("port"),
                GameId = World.NEXUS_ID,
                Name = "nexus.Nexus",
                Key = Empty<byte>.Array,
            });
        }
    }
}
