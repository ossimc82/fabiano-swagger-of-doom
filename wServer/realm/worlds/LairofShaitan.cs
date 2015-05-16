#region

using wServer.networking;

#endregion

namespace wServer.realm.worlds
{
    public class LairofShaitan : World
    {
        public LairofShaitan()
        {
            Name = "Lair of Shaitan";
            ClientWorldName = "dungeons.Lair_of_Shaitan";
            Background = 0;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.shaitansmap.wmap", MapType.Wmap);
        }

        public override World GetInstance(Client psr)
        {
            return Manager.AddWorld(new LairofShaitan());
        }
    }
}