#region

using wServer.networking.cliPackets;
using wServer.realm;
using wServer.realm.entities.player;

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
                if (client.Player?.Owner == null) return;

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

                CheckLabConditions(client.Player, packet);

                client.Player.Move((float)newX, (float)newY);

                client.Player.ClientTick(t, packet);
            }, PendingPriority.Networking);
        }

        private static void CheckLabConditions(Entity player, MovePacket packet)
        {
            var tile = player.Owner.Map[(int) packet.Position.X, (int) packet.Position.Y];
            switch (tile.TileId)
            {
                //Green water
                case 0xa9:
                case 0x82:
                    if(tile.ObjId != 0) return;
                    if (!player.HasConditionEffect(ConditionEffectIndex.Hexed) ||
                        !player.HasConditionEffect(ConditionEffectIndex.Stunned) ||
                        !player.HasConditionEffect(ConditionEffectIndex.Speedy))
                    {
                        player.ApplyConditionEffect(ConditionEffectIndex.Hexed);
                        player.ApplyConditionEffect(ConditionEffectIndex.Stunned);
                        player.ApplyConditionEffect(ConditionEffectIndex.Speedy);
                    }
                    break;
                //Blue water
                case 0xa7:
                case 0x83:
                    if (tile.ObjId != 0) return;
                    if (player.HasConditionEffect(ConditionEffectIndex.Hexed) ||
                        player.HasConditionEffect(ConditionEffectIndex.Stunned) ||
                        player.HasConditionEffect(ConditionEffectIndex.Speedy))
                    {
                        player.ApplyConditionEffect(ConditionEffectIndex.Hexed, 0);
                        player.ApplyConditionEffect(ConditionEffectIndex.Stunned, 0);
                        player.ApplyConditionEffect(ConditionEffectIndex.Speedy, 0);
                    }
                    break;
            }
        }
    }
}