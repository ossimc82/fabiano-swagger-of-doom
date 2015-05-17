#region

using db;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.worlds;

#endregion

namespace wServer.networking.handlers
{
    internal class EnterArenaPacketHandler : PacketHandlerBase<EnterArenaPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.ENTER_ARENA; }
        }

        protected override void HandlePacket(Client client, EnterArenaPacket packet)
        {
            using (Database db = new Database())
            {
                if (packet.Currency == 1)
                {
                    client.Player.CurrentFame = client.Account.Stats.Fame = db.UpdateFame(client.Account, -500);
                    client.Player.UpdateCount++;
                }
                else
                {
                    client.Player.Credits = client.Account.Credits = db.UpdateCredit(client.Account, -50);
                    client.SendPacket(new BuyResultPacket
                    {
                        Result = 0,
                        Message = "{server.buy_success}"
                    });
                    client.Player.UpdateCount++;
                }
            }
            client.Save();

            World world = client.Player.Manager.AddWorld(new Arena());

            client.Reconnect(new ReconnectPacket
            {
                Host = "",
                Port = Program.Settings.GetValue<int>("port"),
                GameId = world.Id,
                Name = world.Name,
                Key = Empty<byte>.Array,
            });
        }
    }
}
