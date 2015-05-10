#region

using wServer.networking.cliPackets;
using wServer.realm;
using wServer.realm.entities;

#endregion

namespace wServer.networking.handlers
{
    internal class BuyHandler : PacketHandlerBase<BuyPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.BUY; }
        }

        protected override void HandlePacket(Client client, BuyPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
            {
                if (client.Player.Owner == null) return;
                SellableObject obj = client.Player.Owner.GetEntity(packet.ObjectId) as SellableObject;
                if (obj != null)
                    obj.Buy(client.Player);
            }, PendingPriority.Networking);
        }
    }
}