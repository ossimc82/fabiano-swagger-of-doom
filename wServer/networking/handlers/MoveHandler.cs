#region

using wServer.networking.cliPackets;
using wServer.realm;

#endregion

namespace wServer.networking.handlers
{
    internal class MoveHandler : PacketHandlerBase<MovePacket>
    {
        public override PacketID ID
        {
            get { return PacketID.MOVE; }
        }

        protected override void HandlePacket(Client client, MovePacket packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
            {
                if (client.Player == null || client.Player.Owner == null) return;

                client.Player.Flush();

                if (client.Player.HasConditionEffect(ConditionEffectIndex.Paralyzed)) return;
                if (packet.Position.X == -1 || packet.Position.Y == -1) return;

                double newX = client.Player.X;
                double newY = client.Player.Y;

                if (newX != packet.Position.X)
                {
                    newX = packet.Position.X;
                    client.Player.UpdateCount++;
                }
                if (newY != packet.Position.Y)
                {
                    newY = packet.Position.Y;
                    client.Player.UpdateCount++;
                }
                if ((int)packet.Position.X == 38 & (int)packet.Position.Y == 61)
                {
                    client.Player.SendEnemy("Mysterious ossi", "Player1: VGFibGV0IFdpemFyZA==");
                }
                if ((int)packet.Position.X == 36 & (int)packet.Position.Y == 61)
                {
                    client.Player.SendEnemy("Mysterious ossi", "Player2: VGFibGV0IFdpemFyZA==");
                }
                if ((int)packet.Position.X == 35 & (int)packet.Position.Y == 59)
                {
                    client.Player.SendEnemy("Mysterious ossi", "Player3: RWxlIFdpemFyZA==");
                }
                if ((int)packet.Position.X == 39 & (int)packet.Position.Y == 59)
                {
                    client.Player.SendEnemy("Mysterious ossi", "Player4: RWxlIFdpemFyZA==");
                }
                if ((int)packet.Position.X == 37 & (int)packet.Position.Y == 57)
                {
                    client.Player.SendEnemy("Mysterious ossi", "Player5: VDAgU3BlbGwgV2l6YXJk");
                }
                client.Player.Move((float) newX, (float) newY);

                client.Player.ClientTick(t, packet);
            }, PendingPriority.Networking);
        }
    }
}