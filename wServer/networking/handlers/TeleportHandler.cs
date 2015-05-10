#region

using wServer.networking.cliPackets;
using wServer.realm;

#endregion

namespace wServer.networking.handlers
{
    internal class TeleportHandler : PacketHandlerBase<TeleportPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.TELEPORT; }
        }

        protected override void HandlePacket(Client client, TeleportPacket packet)
        {
            if (client.Player.Owner == null) return;

            client.Manager.Logic.AddPendingAction(t => client.Player.Teleport(t, packet),
                PendingPriority.Networking);
        }
    }
}