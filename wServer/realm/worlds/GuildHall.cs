using db;
using wServer.networking;

namespace wServer.realm.worlds
{
    public class GuildHall : World
    {
        public string Guild { get; set; }
        public int GuildLevel { get; set; }

        public GuildHall(string guild)
        {
            Id = GUILD_ID;
            Guild = guild;
            Name = "Guild Hall";
            Background = 0;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            Manager.Database.DoActionAsync(db =>
            {
                switch (Level(db))
                {
                    case 0:
                        LoadMap("wServer.realm.worlds.maps.ghall0.wmap", MapType.Wmap); break;
                    case 1:
                        LoadMap("wServer.realm.worlds.maps.ghall1.wmap", MapType.Wmap); break;
                    case 2:
                        LoadMap("wServer.realm.worlds.maps.ghall2.wmap", MapType.Wmap); break;
                    default:
                        LoadMap("wServer.realm.worlds.maps.ghall3.wmap", MapType.Wmap); break;
                }
            });
        }

        public override World GetInstance(Client client)
        {
            return Manager.AddWorld(new GuildHall(Guild));
        }

        public int Level(Database db)
        {
            return db.GetGuildLevel(db.GetGuildId(Guild));
        }
    }
}
