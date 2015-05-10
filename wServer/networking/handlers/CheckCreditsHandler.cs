#region

using db;
using wServer.networking.cliPackets;
using wServer.realm;

#endregion

namespace wServer.networking.handlers
{
    internal class CheckCreditsHandler : PacketHandlerBase<CheckCreditsPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.CHECKCREDITS; }
        }

        protected override void HandlePacket(Client client, CheckCreditsPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
            {
                using (Database db = new Database())
                {
                    db.ReadStats(client.Account);
                    client.Player.Credits = client.Account.Credits;
                    client.Player.UpdateCount++;
                }
            }, PendingPriority.Networking);
        }
    }
}