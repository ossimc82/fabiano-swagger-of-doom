#region

using System;
using wServer.networking.cliPackets;
using wServer.realm.entities;

#endregion

namespace wServer.networking.handlers
{
    internal class PlayerHitHander : PacketHandlerBase<PlayerHitPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.PLAYERHIT; }
        }

        protected override void HandlePacket(Client client, PlayerHitPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client, packet));
        }

        private void Handle(Client client, PlayerHitPacket packet)
        {
            try
            {
                if (client.Player.Owner != null)
                {
                    Projectile proj;
                    if (
                        client.Player.Owner.Projectiles.TryGetValue(
                            new Tuple<int, byte>(packet.ObjectId, packet.BulletId), out proj))
                    {
                        foreach (ConditionEffect effect in proj.Descriptor.Effects)
                        {
                            if (effect.Target == 1)
                            {
                                if (client.Player.Pet != null)
                                    client.Player.Pet.ApplyConditionEffect(effect);
                            }
                            else
                                client.Player.ApplyConditionEffect(effect);
                        }
                        client.Player.Damage(proj.Damage, proj.ProjectileOwner.Self);
                    }
                    else
                        log.Error("Can't register playerhit." + packet.ObjectId + " - " + packet.BulletId);
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error in PlayerHit: {0}", ex);
            }
        }
    }
}
