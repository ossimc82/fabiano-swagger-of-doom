#region

using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;

#endregion

namespace wServer.networking.handlers
{
    internal class EscapeHander : PacketHandlerBase<EscapePacket>
    {
        public override PacketID ID
        {
            get { return PacketID.ESCAPE; }
        }

        protected override void HandlePacket(Client client, EscapePacket packet)
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