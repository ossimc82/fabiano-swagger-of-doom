using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.networking.cliPackets;
using wServer.realm;
using wServer.networking.svrPackets;
using db;
using wServer.realm.entities;
using wServer.realm.entities.player;

namespace wServer.networking.handlers
{
    class GuildInvitePacketHandler : PacketHandlerBase<GuildInvitePacket>
    {
        public override PacketID ID { get { return PacketID.GUILDINVITE; } }

        protected override void HandlePacket(Client client, GuildInvitePacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client.Player, packet));
        }

        void Handle(Player player, GuildInvitePacket packet)
        {
            if(player.Guild.IsDefault)
            {
                player.SendInfo("You are not in a guild");
                return;
            }
            if (player.Guild[player.AccountId].Rank >= 20)
            {
                foreach(var i in player.Owner.Players.Values)
                {
                    Player target = player.Owner.GetPlayerByName(packet.Name);

                    if (target == null)
                    {
                        player.SendInfoWithTokens("server.invite_notfound", new KeyValuePair<string, object>[1]
                        {
                            new KeyValuePair<string, object>("player", packet.Name)
                        });
                        return;
                    }
                    if (!target.NameChosen || player.Dist(target) > 20)
                    {
                        player.SendInfoWithTokens("server.invite_notfound", new KeyValuePair<string, object>[1]
                        {
                            new KeyValuePair<string, object>("player", packet.Name)
                        });
                        return;
                    }

                    if (target.Guild.IsDefault)
                    {
                        if (target.Ignored.Contains(Client.Account.AccountId)) return;
                        target.Client.SendPacket(new InvitedToGuildPacket()
                        {
                            Name = player.Name,
                            GuildName = player.Guild[player.AccountId].Name
                        });
                        target.Invited = true;
                        player.SendInfoWithTokens("server.invite_succeed", new KeyValuePair<string, object>[2]
                        {
                            new KeyValuePair<string, object>("player", packet.Name),
                            new KeyValuePair<string, object>("guild", player.Guild[player.AccountId].Name)
                        });
                        return;
                    }
                    else
                        player.SendError("Player is already in a guild!");
                }
            }
            else
                player.SendInfo("Members and initiates cannot invite!");
        }
    }
}
