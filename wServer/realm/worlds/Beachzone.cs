namespace wServer.realm.worlds
{
    public class Beachzone : World
    {
        public Beachzone()
        {
            Name = "Beachzone";
            ClientWorldName = "{dungeons.Beachzone}";
            Background = 0;
            Difficulty = 0;
            ShowDisplays = true;
            AllowTeleport = false;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.beachzone.wmap", MapType.Wmap);
        }
    }
}
