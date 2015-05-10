#region

using System;
using System.Text;
using wServer.networking.cliPackets;

#endregion

namespace wServer.networking.handlers
{
    internal class PlayerTextHandler : PacketHandlerBase<PlayerTextPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.PLAYERTEXT; }
        }

        protected override void HandlePacket(Client client, PlayerTextPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
            {
                if (client.Player.Owner == null) return;

                if (packet.Text[0] == '/')
                    client.Player.Manager.Commands.Execute(client.Player, t, packet.Text);
                else
                {
                    string txt = Encoding.ASCII.GetString(
                        Encoding.Convert(
                            Encoding.UTF8,
                            Encoding.GetEncoding(
                                Encoding.ASCII.EncodingName,
                                new EncoderReplacementFallback(string.Empty),
                                new DecoderExceptionFallback()
                                ),
                            Encoding.UTF8.GetBytes(packet.Text)
                            ));
                    if (client.Player.Muted)
                    {
                        client.Player.SendInfo("{\"key\":\"server.muted\"}");
                        return;
                    }
                    if (!String.IsNullOrWhiteSpace(txt))
                        client.Player.Manager.Chat.Say(client.Player, txt);
                    else
                        client.Player.SendInfo("{\"key\":\"server.invalid_chars\"}");
                }
            });
        }
    }
}