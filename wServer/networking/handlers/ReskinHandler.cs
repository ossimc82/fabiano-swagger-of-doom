#region

using wServer.networking.cliPackets;
using wServer.realm;

#endregion

namespace wServer.networking.handlers
{
    internal class ReskinHandler : PacketHandlerBase<ReskinPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.RESKIN; }
        }

        protected override void HandlePacket(Client client, ReskinPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
            {
                if (packet.SkinId == 0)
                    client.Player.PlayerSkin = 0;
                else if (client.Account.OwnedSkins.Contains(packet.SkinId))
                    client.Player.PlayerSkin = packet.SkinId;
                else
                    client.Player.SendError("You do not have this skin");
                client.Player.UpdateCount++;
                client.Player.SaveToCharacter();
                client.Save();
            }, PendingPriority.Networking);
        }
    }
}