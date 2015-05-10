#region

using System;
using System.Collections.Generic;

#endregion

namespace terrain
{
    internal class Decoration
    {
        private static readonly Dictionary<string, Tuple<double, string>[]> decors = new Dictionary
            <string, Tuple<double, string>[]>
        {
            {
                "coast", new[]
                {
                    new Tuple<double, string>(0.05, "Large Lilypads"),
                    new Tuple<double, string>(0.05, "Small Lilypads"),
                    new Tuple<double, string>(1, null)
                }
            },
            {
                "desert", new[]
                {
                    new Tuple<double, string>(0.1/9, "Saguaro Cactus 1"), //0.1
                    new Tuple<double, string>(0.1/9, "Saguaro Cactus 2"),
                    new Tuple<double, string>(0.1/9, "Saguaro Cactus 3"),
                    new Tuple<double, string>(0.1/9, "Small Saguaro Cactus 1"),
                    new Tuple<double, string>(0.1/9, "Small Saguaro Cactus 2"),
                    new Tuple<double, string>(0.1/9, "Prickly Pear Cactus 1"),
                    new Tuple<double, string>(0.1/9, "Prickly Pear Cactus 2"),
                    new Tuple<double, string>(0.1/9, "Succulent Plant 1"),
                    new Tuple<double, string>(0.1/9, "Succulent Plant 2"),
                    new Tuple<double, string>(1, null)
                }
            },
            {
                "dryland", new[]
                {
                    new Tuple<double, string>(0.01, "Tree B"), //0.04
                    new Tuple<double, string>(0.01, "Tree Leafless"),
                    new Tuple<double, string>(0.01, "Bush"),
                    new Tuple<double, string>(0.01, "Shrub"),
                    new Tuple<double, string>(0.005, "Rock Brown"), //0.005

                    new Tuple<double, string>(0.005, "Light Green Bush 1"), //0.04
                    new Tuple<double, string>(0.005, "Light Green Bush 2"),
                    new Tuple<double, string>(0.005, "Light Green Bush 3"),
                    new Tuple<double, string>(0.005, "Light Green Bush 4"),
                    new Tuple<double, string>(0.005, "Light Green Bush 5"),
                    new Tuple<double, string>(0.005, "Light Green Bush 6"),
                    new Tuple<double, string>(0.005, "Light Green Bush 7"),
                    new Tuple<double, string>(0.005, "Light Green Bush 8"),
                    new Tuple<double, string>(0.005, "Yellow Bush 1"), //0.04
                    new Tuple<double, string>(0.005, "Yellow Bush 2"),
                    new Tuple<double, string>(0.005, "Yellow Bush 3"),
                    new Tuple<double, string>(0.005, "Yellow Bush 4"),
                    new Tuple<double, string>(0.005, "Yellow Bush 5"),
                    new Tuple<double, string>(0.005, "Yellow Bush 6"),
                    new Tuple<double, string>(0.005, "Yellow Bush 7"),
                    new Tuple<double, string>(0.005, "Yellow Bush 8"),
                    new Tuple<double, string>(1, null)
                }
            },
            {
                "grassland", new[]
                {
                    new Tuple<double, string>(0.004, "Tree A"), //0.02
                    new Tuple<double, string>(0.004, "Tree B"),
                    new Tuple<double, string>(0.004, "Bush"),
                    new Tuple<double, string>(0.004, "Shrub"),
                    new Tuple<double, string>(0.001, "Tree Leafless"),
                    new Tuple<double, string>(0.005, "Green Bush 1"), //0.04
                    new Tuple<double, string>(0.005, "Green Bush 2"),
                    new Tuple<double, string>(0.005, "Green Bush 3"),
                    new Tuple<double, string>(0.005, "Green Bush 4"),
                    new Tuple<double, string>(0.005, "Green Bush 5"),
                    new Tuple<double, string>(0.005, "Green Bush 6"),
                    new Tuple<double, string>(0.005, "Green Bush 7"),
                    new Tuple<double, string>(0.005, "Green Bush 8"),
                    new Tuple<double, string>(0.005, "Light Green Bush 1"), //0.04
                    new Tuple<double, string>(0.005, "Light Green Bush 2"),
                    new Tuple<double, string>(0.005, "Light Green Bush 3"),
                    new Tuple<double, string>(0.005, "Light Green Bush 4"),
                    new Tuple<double, string>(0.005, "Light Green Bush 5"),
                    new Tuple<double, string>(0.005, "Light Green Bush 6"),
                    new Tuple<double, string>(0.005, "Light Green Bush 7"),
                    new Tuple<double, string>(0.005, "Light Green Bush 8"),
                    new Tuple<double, string>(0.01, "White Flowers"), //0.05
                    new Tuple<double, string>(0.01, "Pink Flowers"),
                    new Tuple<double, string>(0.01, "Large Mushroom"),
                    new Tuple<double, string>(0.01, "Small Mushroom"),
                    new Tuple<double, string>(0.01, "Rock Brown"),
                    new Tuple<double, string>(1, null)
                }
            },
            {
                "forest", new[]
                {
                    new Tuple<double, string>(0.04, "Tree A"), //0.16
                    new Tuple<double, string>(0.04, "Tree B"),
                    new Tuple<double, string>(0.04, "Bush"),
                    new Tuple<double, string>(0.04, "Shrub"),
                    new Tuple<double, string>(0.005, "Tree Leafless"),
                    new Tuple<double, string>(0.005, "Green Bush 1"), //0.04
                    new Tuple<double, string>(0.005, "Green Bush 2"),
                    new Tuple<double, string>(0.005, "Green Bush 3"),
                    new Tuple<double, string>(0.005, "Green Bush 4"),
                    new Tuple<double, string>(0.005, "Green Bush 5"),
                    new Tuple<double, string>(0.005, "Green Bush 6"),
                    new Tuple<double, string>(0.005, "Green Bush 7"),
                    new Tuple<double, string>(0.005, "Green Bush 8"),
                    new Tuple<double, string>(0.002, "White Flowers"), //0.01
                    new Tuple<double, string>(0.002, "Pink Flowers"),
                    new Tuple<double, string>(0.002, "Large Mushroom"),
                    new Tuple<double, string>(0.002, "Small Mushroom"),
                    new Tuple<double, string>(0.002, "Rock Brown"),
                    new Tuple<double, string>(1, null)
                }
            },
            {
                "rainforest", new[]
                {
                    new Tuple<double, string>(0.05, "Tree A"), //0.2
                    new Tuple<double, string>(0.05, "Tree B"),
                    new Tuple<double, string>(0.05, "Bush"),
                    new Tuple<double, string>(0.05, "Shrub"),
                    new Tuple<double, string>(0.005, "Tree Leafless"),
                    new Tuple<double, string>(0.00625, "Green Bush 1"), //0.05
                    new Tuple<double, string>(0.00625, "Green Bush 2"),
                    new Tuple<double, string>(0.00625, "Green Bush 3"),
                    new Tuple<double, string>(0.00625, "Green Bush 4"),
                    new Tuple<double, string>(0.00625, "Green Bush 5"),
                    new Tuple<double, string>(0.00625, "Green Bush 6"),
                    new Tuple<double, string>(0.00625, "Green Bush 7"),
                    new Tuple<double, string>(0.00625, "Green Bush 8"),
                    new Tuple<double, string>(0.002, "White Flowers"), //0.01
                    new Tuple<double, string>(0.002, "Pink Flowers"),
                    new Tuple<double, string>(0.002, "Large Mushroom"),
                    new Tuple<double, string>(0.002, "Small Mushroom"),
                    new Tuple<double, string>(0.002, "Rock Brown"),
                    new Tuple<double, string>(1, null)
                }
            },
            {
                "shrub", new[]
                {
                    new Tuple<double, string>(0.005, "Tree A"), //0.01
                    new Tuple<double, string>(0.005, "Tree B"),
                    new Tuple<double, string>(0.01, "Green Bush 1"), //0.08
                    new Tuple<double, string>(0.01, "Green Bush 2"),
                    new Tuple<double, string>(0.01, "Green Bush 3"),
                    new Tuple<double, string>(0.01, "Green Bush 4"),
                    new Tuple<double, string>(0.01, "Green Bush 5"),
                    new Tuple<double, string>(0.01, "Green Bush 6"),
                    new Tuple<double, string>(0.01, "Green Bush 7"),
                    new Tuple<double, string>(0.01, "Green Bush 8"),
                    new Tuple<double, string>(0.01, "White Flowers"), //0.05
                    new Tuple<double, string>(0.01, "Pink Flowers"),
                    new Tuple<double, string>(0.01, "Large Mushroom"),
                    new Tuple<double, string>(0.01, "Small Mushroom"),
                    new Tuple<double, string>(0.01, "Rock Brown"),
                    new Tuple<double, string>(1, null)
                }
            },
            {
                "taiga", new[]
                {
                    new Tuple<double, string>(0.0025, "Tree A"), //0.01
                    new Tuple<double, string>(0.0025, "Tree B"),
                    new Tuple<double, string>(0.0025, "Tree Dead"),
                    new Tuple<double, string>(0.001, "Tree Leafless"),
                    new Tuple<double, string>(0.005, "Light Green Bush 1"), //0.04
                    new Tuple<double, string>(0.005, "Light Green Bush 2"),
                    new Tuple<double, string>(0.005, "Light Green Bush 3"),
                    new Tuple<double, string>(0.005, "Light Green Bush 4"),
                    new Tuple<double, string>(0.005, "Light Green Bush 5"),
                    new Tuple<double, string>(0.005, "Light Green Bush 6"),
                    new Tuple<double, string>(0.005, "Light Green Bush 7"),
                    new Tuple<double, string>(0.005, "Light Green Bush 8"),
                    new Tuple<double, string>(0.005, "Green Bush 1"), //0.04
                    new Tuple<double, string>(0.005, "Green Bush 2"),
                    new Tuple<double, string>(0.005, "Green Bush 3"),
                    new Tuple<double, string>(0.005, "Green Bush 4"),
                    new Tuple<double, string>(0.005, "Green Bush 5"),
                    new Tuple<double, string>(0.005, "Green Bush 6"),
                    new Tuple<double, string>(0.005, "Green Bush 7"),
                    new Tuple<double, string>(0.005, "Green Bush 8"),
                    new Tuple<double, string>(0.01, "White Flowers"), //0.05
                    new Tuple<double, string>(0.01, "Pink Flowers"),
                    new Tuple<double, string>(0.01, "Large Mushroom"),
                    new Tuple<double, string>(0.01, "Small Mushroom"),
                    new Tuple<double, string>(0.01, "Rock Brown"),
                    new Tuple<double, string>(0.01, "Fir Tree 1"), //0.05
                    new Tuple<double, string>(0.01, "Fir Tree 2"),
                    new Tuple<double, string>(0.01, "Fir Tree 3"),
                    new Tuple<double, string>(0.01, "Small Fir Tree 1"),
                    new Tuple<double, string>(0.01, "Small Fir Tree 2"),
                    new Tuple<double, string>(1, null)
                }
            },
            {
                "mountain", new[]
                {
                    new Tuple<double, string>(0.01, "Rock Grey"), //0.01

                    new Tuple<double, string>(1, null)
                }
            },
        };

        private static readonly Dictionary<string, Tuple<int, int, int>> decorSizes = new Dictionary
            <string, Tuple<int, int, int>>
        {
            {"Tree A", new Tuple<int, int, int>(130, 160, 5)},
            {"Tree B", new Tuple<int, int, int>(130, 160, 5)},
            {"Tree Leafless", new Tuple<int, int, int>(130, 160, 5)},
            {"Tree Dead", new Tuple<int, int, int>(130, 160, 5)},
            {"Fir Tree 1", new Tuple<int, int, int>(130, 160, 5)},
            {"Fir Tree 2", new Tuple<int, int, int>(130, 160, 5)},
            {"Fir Tree 3", new Tuple<int, int, int>(130, 160, 5)},
            {"Small Fir Tree 1", new Tuple<int, int, int>(130, 160, 5)},
            {"Small Fir Tree 2", new Tuple<int, int, int>(130, 160, 5)},
            {"Dead Fir Tree 1", new Tuple<int, int, int>(130, 160, 5)},
            {"Dead Fir Tree 2", new Tuple<int, int, int>(130, 160, 5)},
            {"Dead Fir Tree 3", new Tuple<int, int, int>(130, 160, 5)},
            {"Dead Fir Tree 4", new Tuple<int, int, int>(130, 160, 5)},
            {"Saguaro Cactus 1", new Tuple<int, int, int>(130, 160, 5)},
            {"Saguaro Cactus 2", new Tuple<int, int, int>(130, 160, 5)},
            {"Saguaro Cactus 3", new Tuple<int, int, int>(130, 160, 5)},
            {"Small Saguaro Cactus 1", new Tuple<int, int, int>(130, 160, 5)},
            {"Small Saguaro Cactus 2", new Tuple<int, int, int>(130, 160, 5)},
            {"Prickly Pear Cactus 1", new Tuple<int, int, int>(130, 160, 5)},
            {"Prickly Pear Cactus 2", new Tuple<int, int, int>(130, 160, 5)},
            {"Succulent Plant 1", new Tuple<int, int, int>(130, 160, 5)},
            {"Succulent Plant 2", new Tuple<int, int, int>(130, 160, 5)},
        };

        public static string GetDecor(string biome, Random rand)
        {
            Tuple<double, string>[] dat;
            if (!decors.TryGetValue(biome, out dat)) return null;

            double val = rand.NextDouble();
            double c = 0;
            foreach (Tuple<double, string> i in dat)
            {
                c += i.Item1;
                if (val < c) return i.Item2;
            }
            return null;
        }

        public static int? GetSize(string id, Random rand)
        {
            Tuple<int, int, int> dat;
            if (!decorSizes.TryGetValue(id, out dat))
                return null;
            int min = dat.Item1;
            int max = dat.Item2;
            int range = max - min;
            int s = range/dat.Item3;
            return min + rand.Next(0, s + 1)*dat.Item3;
        }
    }
}