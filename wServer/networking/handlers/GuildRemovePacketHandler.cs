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
    class GuildRemovePacketHandler : PacketHandlerBase<GuildRemovePacket>
    {
        public override PacketID ID { get { return PacketID.GUILDREMOVE; } }

        protected override void HandlePacket(Client client, GuildRemovePacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client, packet));
        }

        void Handle(Client client, GuildRemovePacket packet)
        {
            client.Manager.Database.DoActionAsync(db =>
            {
                try
                {
                    var p = client.Manager.FindPlayer(packet.Name);
                    if (p != null && p.Guild == client.Player.Guild && p.NameChosen)
                    {
                        if(client.Player.Guild[client.Account.AccountId].Rank <= p.Guild[p.AccountId].Rank && p.Guild.Name != client.Player.Guild.Name)
                            return;

                        var g = db.ChangeGuild(p.Client.Account, p.Client.Account.Guild.Id, p.Guild[p.AccountId].Rank, p.Client.Account.Guild.Fame, true);
                        client.Player.Guild.RemoveFromGuild(client.Player, p);
                    }
                    else
                    {
                        Account acc = db.GetAccount(packet.Name, Manager.GameData);
                        if (acc.NameChosen)
                        {
                            try
                            {
                                if (acc.Guild.Name == client.Player.Guild.Name)
                                {
                                    if (client.Player.Guild[client.Account.AccountId].Rank <= acc.Guild.Rank && acc.Guild.Name != client.Player.Guild.Name)
                                        return;

                                    db.ChangeGuild(acc, acc.Guild.Id, acc.Guild.Rank, acc.Guild.Fame, true);
                                    client.Player.Guild.Chat(client.Player, client.Player.Name + " removed " + acc.Name + " from " + client.Player.Guild.Name);
                                }
                            }
                            catch (Exception e)
                            {
                                client.SendPacket(new TextPacket()
                                {
                                    BubbleTime = 0,
                                    Stars = -1,
                                    Name = "*Error*",
                                    Text = e.Message
                                });
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    client.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "*Error*",
                        Text = e.Message
                    });
                }
                client.SendPacket(new CreateGuildResultPacket()
                {
                    Success = true,
                    ErrorText = ""
                });
            });
        }
    }
}
