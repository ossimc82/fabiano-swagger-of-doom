#region

using db;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities.player;
using FailurePacket = wServer.networking.svrPackets.FailurePacket;

#endregion

namespace wServer.networking.handlers
{
    internal class LoadHandler : PacketHandlerBase<LoadPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.LOAD; }
        }

        protected override void HandlePacket(Client client, LoadPacket packet)
        {
            using (Database db = new Database())
            {
                client.Character = db.LoadCharacter(client.Account, packet.CharacterId);
                if (client.Character != null)
                {
                    if (client.Character.Dead)
                    {
                        client.SendPacket(new FailurePacket
                        {
                            ErrorId = 0,
                            ErrorDescription = "Character is dead."
                        });
                    }
                    else
                    {
                        World target = client.Manager.Worlds[client.TargetWorld];
                        client.SendPacket(new Create_SuccessPacket
                        {
                            CharacterID = client.Character.CharacterId,
                            ObjectID =
                                client.Manager.Worlds[client.TargetWorld].EnterWorld(
                                    client.Player = new Player(client.Manager, client))
                        });
                        client.Stage = ProtocalStage.Ready;
                    }
                }
                else
                {
                    client.SendPacket(new FailurePacket
                    {
                        ErrorId = 0,
                        ErrorDescription = "Failed to Load character."
                    });
                    client.Disconnect();
                }
            }
        }
    }
}