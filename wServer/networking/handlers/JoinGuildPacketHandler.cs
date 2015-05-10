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
    class JoinGuildPacketHandler : PacketHandlerBase<JoinGuildPacket>
    {
        public override PacketID ID { get { return PacketID.JOINGUILD; } }

        protected override void HandlePacket(Client client, JoinGuildPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client, packet));
        }

        void Handle(Client client, JoinGuildPacket packet)
        {
            if(!client.Player.Invited)
            {
                client.Player.SendInfoWithTokens("server.guild_not_invited", new KeyValuePair<string, object>[1]
                {
                    new KeyValuePair<string, object>("guild", packet.GuildName)
                });
                return;
            }
            client.Manager.Database.DoActionAsync(db =>
            {
                var gStruct = db.GetGuild(packet.GuildName);
                if (client.Player.Invited == false)
                {
                    client.Player.SendInfo("You need to be invited to join a guild!");
                }
                if (gStruct != null)
                {
                    var g = db.ChangeGuild(client.Account, gStruct.Id, 0, 0, false);
                    if (g != null)
                    {
                        client.Account.Guild = g;
                        GuildManager.CurrentManagers[packet.GuildName].JoinGuild(client.Player);
                    }
                }
                else
                {
                    client.Player.SendInfoWithTokens("server.guild_join_fail", new KeyValuePair<string, object>[1]
                    {
                        new KeyValuePair<string, object>("error", "Guild does not exist")
                    });
                }
            });
        }
    }
}
