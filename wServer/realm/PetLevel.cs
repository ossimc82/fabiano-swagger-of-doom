#region

using System;
using System.Collections.Generic;
using wServer.realm.entities;

#endregion

namespace wServer.realm
{
    public enum AbilityType
    {
        First,
        Second,
        Third
    }

    public class PetLevel
    {
        private static readonly Dictionary<int, int> LevelCap = new Dictionary<int, int>(100)
        {
            {1, 20},
            {2, 42},
            {3, 65},
            {4, 91},
            {5, 118},
            {6, 147},
            {7, 179},
            {8, 213},
            {9, 250},
            {10, 290},
            {11, 333},
            {12, 380},
            {13, 430},
            {14, 485},
            {15, 544},
            {16, 607},
            {17, 676},
            {18, 750},
            {19, 829},
            {20, 916},
            {21, 1009},
            {22, 1110},
            {23, 1218},
            {24, 1336},
            {25, 1463},
            {26, 1600},
            {27, 1748},
            {28, 1907},
            {29, 2080},
            {30, 2266},
            {31, 2467},
            {32, 2685},
            {33, 2920},
            {34, 3173},
            {35, 3447},
            {36, 3743},
            {37, 4062},
            {38, 4407},
            {39, 4779},
            {40, 5182},
            {41, 5616},
            {42, 6085},
            {43, 6592},
            {44, 7139},
            {45, 7731},
            {46, 8369},
            {47, 9059},
            {48, 9803},
            {49, 10607},
            {50, 11476},
            {51, 12414},
            {52, 13427},
            {53, 14521},
            {54, 15703},
            {55, 16979},
            {56, 18357},
            {57, 19846},
            {58, 21453},
            {59, 23190},
            {60, 25065},
            {61, 27090},
            {62, 29277},
            {63, 31639},
            {64, 34190},
            {65, 36945},
            {66, 39921},
            {67, 43135},
            {68, 46605},
            {69, 50354},
            {70, 54402},
            {71, 58774},
            {72, 63496},
            {73, 68596},
            {74, 74103},
            {75, 80052},
            {76, 86476},
            {77, 93414},
            {78, 100907},
            {79, 108999},
            {80, 117739},
            {81, 127178},
            {82, 137373},
            {83, 148382},
            {84, 160273},
            {85, 173115},
            {86, 186984},
            {87, 201962},
            {88, 218139},
            {89, 235610},
            {90, 254479},
            {91, 274858},
            {92, 296866},
            {93, 320635},
            {94, 346306},
            {95, 374031},
            {96, 403973},
            {97, 436311},
            {98, 471236},
            {99, 508954},
        };

        private Pet pet;
        private readonly AbilityType type;


        //a * (1 - Math.pow(r, level)) / (1 - r)
        //points to get to level
        //with a = 20 , r = 1.08
        //
        //first 100%
        //second ~ 70%
        //third ~ 30%

        public PetLevel(AbilityType ability, Ability abilityType, int power, int level, Pet pet)
        {
            this.pet = pet;
            type = ability;
            Ability = abilityType;
            Level = level;
            Power = power;
        }

        public int Level { get; private set; }
        public int Power { get; private set; }
        public Ability Ability { get; private set; }

        public void Incease(IFeedable petFoodNOMNOMNOM)
        {
            int remaining = petFoodNOMNOMNOM.FeedPower;

            if (type == AbilityType.Second)
                remaining = (int)(petFoodNOMNOMNOM.FeedPower * 0.702025);
            if (type == AbilityType.Third)
                remaining = (int)(petFoodNOMNOMNOM.FeedPower * 0.3240117);

            if (Level == pet.MaximumLevel || Level == 100) return;

            while (remaining > 0)
            {
                remaining--;
                Power++;
                if (Power == LevelCap[Level])
                    Level++;
                if (Level == pet.MaximumLevel || Level == 100) break;
            }
        }
    }
}