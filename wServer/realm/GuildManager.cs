using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm.entities.player;
using wServer.networking.svrPackets;
using wServer.realm.worlds;

namespace wServer.realm
{
    public class GuildManager : List<Player>
    {
        public static Dictionary<string, GuildManager> CurrentManagers { get { return _currentManagers; } }
        private static Dictionary<string, GuildManager> _currentManagers = new Dictionary<string, GuildManager>();

        public static GuildManager Add(Player player, Guild guildStruct)
        {
            if(guildStruct == null) return null;
            GuildManager ret = null;
            if (CurrentManagers.ContainsKey(guildStruct.Name))
            {
                ret = CurrentManagers[guildStruct.Name];
                if (ret._guildStructs.ContainsKey(player.AccountId))
                    ret._guildStructs[player.AccountId] = guildStruct;
                else
                    ret._guildStructs.Add(player.AccountId, guildStruct);

                ret.Add(player);
            }
            else
            {
                ret = new GuildManager(guildStruct, player.Manager);
                ret._guildStructs.Add(player.AccountId, guildStruct);
                ret.Add(player);
                _currentManagers.Add(guildStruct.Name, ret);
            }
            return ret;
        }

        public static void Tick(RealmTime time)
        {
            foreach (var i in CurrentManagers)
            {
                foreach (var p in i.Value)
                {
                    if (!p.Manager.Clients.ContainsKey(p.AccountId))
                    {
                        if (p.Client != null)
                            p.Client.Disconnect();
                        else
                            p.Dispose();
                    }
                }
            }
        }

        public static void RemovePlayerWithId(string accId)
        {
            foreach (var i in CurrentManagers.Select(_ => _.Value))
            {
                if (i.Contains(accId))
                    i.Remove(accId);
                if (i.GuildStructs.ContainsKey(accId))
                    i.GuildStructs.Remove(accId);
            }
        }

        private Dictionary<string, Guild> _guildStructs;
        public Dictionary<string, Guild> GuildStructs
        {
            get { return _guildStructs; }
        }
        public Guild this[string accountId] { get { return _guildStructs.ContainsKey(accountId) ? _guildStructs[accountId] : new Guild { Name = "" }; } }

        private readonly string name;
        public string Name
        {
            get { return name; }
        }

        private readonly long id;
        public long Id
        {
            get { return id; }
        }


        public bool UpgradeInProgress { get; private set; }
        public World GuildHall { get; private set; }

        public GuildManager(Guild guildInfo, RealmManager manager)
        {
            _guildStructs = new Dictionary<string, Guild>();
            GuildHall = guildInfo.Name == "" ? null : manager.AddWorld(new GuildHall(guildInfo.Name));
            this.name = guildInfo.Name;
            this.id = guildInfo.Id;
        }

        public bool Contains(string accountId)
        {
            return this.Where(_ => _.AccountId == accountId).Count() > 0;
        }

        public void Remove(string accountId)
        {
            _guildStructs.Remove(accountId);
            base.RemoveAt(this.FindIndex(_ => _.AccountId == accountId));
        }

        public new void Remove(Player player)
        {
            _guildStructs.Remove(player.AccountId);
            base.Remove(player);
        }

        public void UpdateGuildHall()
        {
            WorldTimer ghallTimer = null;
            UpgradeInProgress = true;
            GuildHall.Timers.Add(ghallTimer = new WorldTimer(60 * 1000, (w, t) =>
            {
                if (w.Players.Count > 0)
                {
                    ghallTimer.Reset();
                    GuildHall.Manager.Logic.AddPendingAction(_ => w.Timers.Add(ghallTimer), PendingPriority.Creation);
                }
                else
                {
                    RealmManager manager = GuildHall.Manager;
                    GuildHall.Manager.RemoveWorld(GuildHall);
                    GuildHall = manager.AddWorld(new GuildHall(Name));
                    UpgradeInProgress = false;
                }
            }));
        }

        public void JoinGuild(Player player)
        {
            _guildStructs.Add(player.AccountId, new Guild
            {
                Name = Name,
                Fame = 0,
                Rank = 0,
                Id = Id
            });
            player.Guild = this;
            this.Add(player);
            player.UpdateCount++;
            foreach (Player p in this)
            {
                p.SendInfoWithTokens("server.guild_join", new KeyValuePair<string, object>[2]
                {
                    new KeyValuePair<string, object>("name", player.Name),
                    new KeyValuePair<string, object>("guild", Name)
                });
            }
        }

        public void RemoveFromGuild(Player sender, Player player)
        {
            if (player.Name == sender.Name)
                foreach (Player p in this)
                    p.SendInfo(sender.Name + " has left " + Name);

            else
                foreach (Player p in this)
                    p.SendInfo(sender.Name + " removed " + player.Name + " from " + Name);

            player.Guild = GetDefaultGuild();
            this.Remove(player);
            player.UpdateCount++;
        }

        private GuildManager GetDefaultGuild()
        {
            if (!CurrentManagers.ContainsKey(""))
                CurrentManagers.Add("", new GuildManager(new Guild { Name = "", Rank = 0, Fame = 0, Id = 0 }, null));

            return CurrentManagers[""];
        }

        public void Chat(Player sender, string text)
        {
            foreach (Player p in this)
            {
                p.Client.SendPacket(new TextPacket
                {
                    BubbleTime = 10,
                    CleanText = "",
                    Name = sender.Name,
                    ObjectId = p.Owner == sender.Owner ? sender.Id : -1,
                    Recipient = "*Guild*",
                    Stars = p.Stars,
                    Text = text
                });
            }
        }

        public bool IsDefault{ get { return _guildStructs.Values.ToArray()[0].Name == ""; } }
    }
}
