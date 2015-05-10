#region

using System;
using wServer.networking.cliPackets;
using wServer.realm;

#endregion

namespace wServer.networking.handlers
{
    internal class GroundDamageHandler : PacketHandlerBase<GroundDamagePacket>
    {
        public override PacketID ID
        {
            get { return PacketID.GROUNDDAMAGE; }
        }

        protected override void HandlePacket(Client client, GroundDamagePacket packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
            {
                if (client.Player.HasConditionEffect(ConditionEffects.Paused) ||
                    client.Player.HasConditionEffect(ConditionEffects.Invincible))
                    return;

                try
                {
                    if (client.Player.Owner == null) return;
                    WmapTile tile = client.Player.Owner.Map[(int) client.Player.X, (int) client.Player.Y];
                    ObjectDesc objDesc = tile.ObjType == 0 ? null : client.Manager.GameData.ObjectDescs[tile.ObjType];
                    TileDesc tileDesc = client.Manager.GameData.Tiles[tile.TileId];
                    if (tileDesc.Damaging && (objDesc == null || !objDesc.ProtectFromGroundDamage))
                    {
                        int dmg = client.Player.Random.Next(tileDesc.MinDamage, tileDesc.MaxDamage);
                        dmg = (int) client.Player.StatsManager.GetDefenseDamage(dmg, true);

                        client.Player.HP -= dmg;
                        client.Player.UpdateCount++;
                        if (client.Player.HP <= 0)
                            client.Player.Death(tileDesc.ObjectId);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }, PendingPriority.Networking);
        }
    }
}