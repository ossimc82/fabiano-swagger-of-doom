namespace wServer.realm.worlds
{
    public class ClothBazaar : World
    {
        public ClothBazaar()
        {
            Id = MARKET;
            Name = "Cloth Bazaar";
            ClientWorldName = "nexus.Cloth_Bazaar";
            Background = 2;
            AllowTeleport = false;
            Difficulty = 0;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.bazzar.wmap", MapType.WMAP);
        }

        //public override void Tick(RealmTime time)
        //{
        //    base.Tick(time); //normal world tick
        //}
    }
}