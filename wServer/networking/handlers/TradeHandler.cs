#region

using wServer.networking.cliPackets;

#endregion

namespace wServer.networking.handlers
{
    internal class RequestTradeHandler : PacketHandlerBase<RequestTradePacket>
    {
        public override PacketID ID
        {
            get { return PacketID.REQUESTTRADE; }
        }

        protected override void HandlePacket(Client client, RequestTradePacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => client.Player.RequestTrade(t, packet));
        }
    }

    internal class ChangeTradeHandler : PacketHandlerBase<ChangeTradePacket>
    {
        public override PacketID ID
        {
            get { return PacketID.CHANGETRADE; }
        }

        protected override void HandlePacket(Client client, ChangeTradePacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => client.Player.ChangeTrade(t, packet));
        }
    }

    internal class AcceptTradeHandler : PacketHandlerBase<AcceptTradePacket>
    {
        public override PacketID ID
        {
            get { return PacketID.ACCEPTTRADE; }
        }

        protected override void HandlePacket(Client client, AcceptTradePacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => client.Player.AcceptTrade(t, packet));
        }
    }

    internal class CancelTradeHandler : PacketHandlerBase<CancelTradePacket>
    {
        public override PacketID ID
        {
            get { return PacketID.CANCELTRADE; }
        }

        protected override void HandlePacket(Client client, CancelTradePacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => client.Player.CancelTrade(t, packet));
        }
    }
}