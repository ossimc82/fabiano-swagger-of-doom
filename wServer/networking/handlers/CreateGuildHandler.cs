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
    class CreateGuildHandler : PacketHandlerBase<CreateGuildPacket>
    {
        public override PacketID ID { get { return PacketID.CREATEGUILD; } }

        protected override void HandlePacket(Client client, CreateGuildPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client, packet));
        }

        void Handle(Client client, CreateGuildPacket packet)
        {
            try
            {
                client.Manager.Database.DoActionAsync(db =>
                {
                    Player player = client.Player;
                    var name = packet.Name.ToString();
                    if (player.Client.Account.Stats.Fame >= 1000)
                    {
                        if (name != "")
                        {
                            if (db.GetGuild(name) != null)
                            {
                                player.Client.SendPacket(new CreateGuildResultPacket()
                                {
                                    Success = false,
                                    ErrorText = "{\"key\":\"server.create_guild_error\",\"tokens\":{\"error\":\"Guild already exists.\"}}"
                                });
                                return;
                            }
                            try
                            {
                                if (player.Client.Account.Guild.Name == "")
                                {
                                    if (packet.Name != "")
                                    {
                                        var g = db.CreateGuild(player.Client.Account, packet.Name);
                                        player.Client.Account.Guild.Name = g.Name;
                                        player.Client.Account.Guild.Rank = g.Rank;
                                        player.Guild = GuildManager.Add(player, g);
                                        player.Client.SendPacket(new CreateGuildResultPacket()
                                        {
                                            Success = true,
                                            ErrorText = "{\"key\":\"server.buy_success\"}"
                                        });
                                        player.CurrentFame = player.Client.Account.Stats.Fame = db.UpdateFame(player.Client.Account, -1000);
                                        player.UpdateCount++;
                                        return;
                                    }
                                    else
                                    {
                                        player.Client.SendPacket(new CreateGuildResultPacket()
                                        {
                                            Success = false,
                                            ErrorText = "{\"key\":\"server.create_guild_error\",\"tokens\":{\"error\":\"Guild name cannot be blank.\"}}"
                                        });
                                        return;
                                    }
                                }
                                else
                                {
                                    player.Client.SendPacket(new CreateGuildResultPacket()
                                    {
                                        Success = false,
                                        ErrorText = "{\"key\":\"server.create_guild_error\",\"tokens\":{\"error\":\"You cannot create a guild as a guild member.\"}}"
                                    });
                                    return;
                                }
                            }
                            catch (Exception e)
                            {
                                player.Client.SendPacket(new CreateGuildResultPacket()
                                {
                                    Success = false,
                                    ErrorText = "{\"key\":\"server.create_guild_error\",\"tokens\":{\"error\":\"" + e.Message + "\"}}"
                                });
                                return;
                            }
                        }
                        else
                        {
                            player.Client.SendPacket(new CreateGuildResultPacket()
                            {
                                Success = false,
                                ErrorText = "{\"key\":\"server.create_guild_error\",\"tokens\":{\"error\":\"Guild name cannot be blank.\"}}"
                            });
                        }
                    }
                    else
                    {
                        player.Client.SendPacket(new CreateGuildResultPacket()
                        {
                            Success = false,
                            ErrorText = "{\"key\":\"server.not_enough_fame\"}"
                        });
                    }
                });
            }
            catch (Exception e)
            {
                client.SendPacket(new CreateGuildResultPacket()
                {
                    Success = false,
                    ErrorText = "{\"key\":\"server.create_guild_error\",\"tokens\":{\"error\":\"" + e.Message + "\"}}"
                });
            }
        }
    }
}
