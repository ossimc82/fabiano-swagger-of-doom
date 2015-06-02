using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonGenerator.Templates;

namespace DungeonGenerator
{
    public class DungeonGen
    {
        private readonly int seed;
        private readonly Generator gen;
        private Rasterizer ras;

        public DungeonGen(int seed, DungeonTemplate template)
        {
            this.seed = seed;

            gen = new Generator(seed, template);
        }

        public void GenerateAsync()
        {
            gen.Generate();
            if (ras == null)
                ras = new Rasterizer(seed, gen.ExportGraph());
            ras.Rasterize();
        }

        public string ExportToJson() => JsonMap.Save(ras.ExportMap());
    }
}
