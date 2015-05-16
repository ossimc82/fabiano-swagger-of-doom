namespace wServer.realm.worlds
{
    public class TomboftheAncients : World
    {
        public TomboftheAncients()
        {
            Name = "Tomb of the Ancients";
            ClientWorldName = "dungeons.Tomb_of_the_Ancients";
            Dungeon = true;
            Background = 0;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.tomb.wmap", MapType.Wmap);
        }
    }
}