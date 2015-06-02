#region

using System.Threading;
using System.Threading.Tasks;
using DungeonGenerator;
using DungeonGenerator.Templates.Abyss;
using wServer.networking;

#endregion

namespace wServer.realm.worlds
{
    public class AbyssofDemons : World
    {
        public AbyssofDemons()
        {
            Name = "Abyss of Demons";
            ClientWorldName = "{dungeons.Abyss_of_Demons}";
            Dungeon = true;
            Background = 0;
            AllowTeleport = true;
        }

        public override bool NeedsPortalKey => true;

        protected override void Init()
        {
            LoadMap(GeneratorCache.NextAbyss(Seed));
        }

        public override World GetInstance(Client psr) => Manager.AddWorld(new AbyssofDemons());
    }
}