#region

using wServer.networking;

#endregion

namespace wServer.realm.worlds
{
    public class OryxCastle : World
    {
        public OryxCastle()
        {
            Name = "Oryx's Castle";
            ClientWorldName = "server.Oryx's_Castle";
            Background = 0;
            AllowTeleport = false;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.OryxCastle.wmap", MapType.WMAP);
        }

        public override World GetInstance(Client psr)
        {
            return Manager.AddWorld(new OryxCastle());
        }
    }
}