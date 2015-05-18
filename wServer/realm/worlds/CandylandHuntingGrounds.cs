namespace wServer.realm.worlds
{
    public class CandylandHuntingGrounds : World
    {
        public CandylandHuntingGrounds()
        {
            Name = "Candyland Hunting Grounds";
            ClientWorldName = "dungeons.Candyland_Hunting_Grounds";
            Background = 0;
            Difficulty = 3;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.candyland.wmap", MapType.Wmap);
        }
    }
}
