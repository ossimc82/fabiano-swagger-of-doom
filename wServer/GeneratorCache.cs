using System.Collections.Generic;
using System.Threading.Tasks;
using DungeonGenerator;
using DungeonGenerator.Templates;
using DungeonGenerator.Templates.Abyss;
using DungeonGenerator.Templates.Lab;
using DungeonGenerator.Templates.PirateCave;
using log4net;

namespace wServer
{
    public static class GeneratorCache
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GeneratorCache)); 
        private static Dictionary<string, List<string>> cachedMaps;

        public static void Init()
        {
            cachedMaps = new Dictionary<string, List<string>>();
            createCache("Abyss of Demons", new AbyssTemplate());
            createCache("Mad Lab", new LabTemplate());
            createCache("Pirate Cave", new PirateCaveTemplate());
        }

        public static string NextAbyss(uint seed) => nextMap(seed, "Abyss of Demons", new AbyssTemplate());
        public static string NextLab(uint seed) => nextMap(seed, "Mad Lab", new LabTemplate());
        public static string NextPirateCave(uint seed) => nextMap(seed, "Pirate Cave", new PirateCaveTemplate());

        private static string nextMap(uint seed, string key, DungeonTemplate template)
        {
            var map = cachedMaps[key][0];
            cachedMaps[key].RemoveAt(0);
            log.Info($"Generating new map for dungeon: {key}");
            Task.Factory.StartNew(() => cachedMaps[key].Add(generateNext(seed, template)));
            return map;
        }

        private static string generateNext(uint seed, DungeonTemplate template)
        {
            var gen = new DungeonGen((int)seed, template);
            gen.GenerateAsync();
            return gen.ExportToJson();
        }

        private static void createCache(string key, DungeonTemplate template)
        {
            log.Info($"Generating cache for dungeon: {key}");
            cachedMaps.Add(key, new List<string>());
            for (var i = 0; i < 3; i++) //Keep at least 3 maps in cache
                cachedMaps[key].Add(generateNext(0, template));
        }
    }
}
