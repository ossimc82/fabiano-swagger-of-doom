namespace wServer.realm.worlds
{
    public class Kitchen : World
    {
        public Kitchen()
        {
            Name = "Kitchen";
            ClientWorldName = "server.Kitchen";
            Background = 0;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.kitchen.wmap", MapType.Wmap);
        }
    }
}