#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm.commands;
using wServer.realm.entities;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.commands
{
    internal class GuildChatCommand : Command
    {
        public GuildChatCommand() : base("guild") { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (!player.Guild.IsDefault)
            {
                try
                {
                    var saytext = string.Join(" ", args);

                    if (String.IsNullOrWhiteSpace(saytext))
                    {
                        player.SendHelp("Usage: /guild <text>");
                        return false;
                    }
                    else
                    {
                        player.Guild.Chat(player, saytext.ToSafeText());
                        return true;
                    }
                }
                catch
                {
                    player.SendInfo("Cannot guild chat!");
                    return false;
                }
            }
            else
                player.SendInfo("You need to be in a guild to use guild chat!");
            return false;
        }
    }

    class GChatCommand : Command
    {
        public GChatCommand() : base("g") { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if(!player.Guild.IsDefault)
            {
                try
                {
                    var saytext = string.Join(" ", args);

                    if (String.IsNullOrWhiteSpace(saytext))
                    {
                        player.SendHelp("Usage: /g <text>");
                        return false;
                    }
                    else
                    {
                        player.Guild.Chat(player, saytext.ToSafeText());
                        return true;
                    }
                }
                catch
                {
                    player.SendInfo("Cannot guild chat!");
                    return false;
                }
            }
            else
                player.SendInfo("You need to be in a guild to use guild chat!");
            return false;
        }
    }

    class GuildInviteCommand : Command
    {
        public GuildInviteCommand() : base("invite") { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (String.IsNullOrWhiteSpace(args[0]))
            {
                player.SendInfo("Usage: /invite <player name>");
                return false;
            }

            if (player.Guild[player.AccountId].Rank >= 20)
            {
                foreach (var i in player.Owner.Players.Values)
                {
                    Player target = player.Owner.GetPlayerByName(args[0]);

                    if (target == null)
                    {
                        player.SendInfoWithTokens("server.invite_notfound", new KeyValuePair<string, object>[1]
                        {
                            new KeyValuePair<string, object>("player", args[0])
                        });
                        return false;
                    }
                    if (!target.NameChosen || player.Dist(target) > 20)
                    {
                        player.SendInfoWithTokens("server.invite_notfound", new KeyValuePair<string, object>[1]
                        {
                            new KeyValuePair<string, object>("player", args[0])
                        });
                        return false;
                    }

                    if (target.Guild.IsDefault)
                    {
                        target.Client.SendPacket(new InvitedToGuildPacket()
                        {
                            Name = player.Name,
                            GuildName = player.Guild[player.AccountId].Name
                        });
                        target.Invited = true;
                        player.SendInfoWithTokens("server.invite_succeed", new KeyValuePair<string, object>[2]
                        {
                            new KeyValuePair<string, object>("player", args[0]),
                            new KeyValuePair<string, object>("guild", player.Guild[player.AccountId].Name)
                        });
                        return true;
                    }
                    else
                    {
                        player.SendError("Player is already in a guild!");
                        return false;
                    }
                }
            }
            else
            {
                player.Client.SendPacket(new TextPacket()
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "",
                    Text = "Members and initiates cannot invite!"
                });
            }
            return false;
        }
    }

    class GuildJoinCommand : Command
    {
        public GuildJoinCommand() : base("join") { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if(String.IsNullOrWhiteSpace(args[0]))
            {
                player.SendInfo("Usage: /join <guild name>");
                return false;
            }
            if (!player.Invited)
            {
                player.SendInfoWithTokens("server.guild_not_invited", new KeyValuePair<string, object>[1]
                {
                    new KeyValuePair<string, object>("guild", args[0])
                });
                return false;
            }
            player.Manager.Database.DoActionAsync(db =>
            {
                var gStruct = db.GetGuild(args[0]);
                if (player.Invited == false)
                {
                    player.SendInfo("You need to be invited to join a guild!");
                }
                if (gStruct != null)
                {
                    var g = db.ChangeGuild(player.Client.Account, gStruct.Id, 0, 0, false);
                    if (g != null)
                    {
                        player.Client.Account.Guild = g;
                        GuildManager.CurrentManagers[args[0]].JoinGuild(player);
                    }
                }
                else
                {
                    player.SendInfoWithTokens("server.guild_join_fail", new KeyValuePair<string, object>[1]
                    {
                        new KeyValuePair<string, object>("error", "Guild does not exist")
                    });
                }
            });
            return true;
        }
    }
}
