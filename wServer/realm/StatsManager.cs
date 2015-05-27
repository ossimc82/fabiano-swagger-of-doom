#region

using System;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm
{
    public class StatsManager
    {
        private readonly Player player;

        public StatsManager(Player player, uint seed)
        {
            this.player = player;
            this.Random = new DamageRandom(seed);
        }

        public DamageRandom Random { get; }

        //from wiki

        public int GetStats(int id)
        {
            return player.Stats[id] + player.Boost[id];
        }

        public float GetAttackDamage(int min, int max)
        {
            return Random.obf6((uint)min, (uint)max) * DamageModifier();
        }

        private float DamageModifier()
        {
            if (player.HasConditionEffect(ConditionEffectIndex.Weak))
                return 0.5f;
            var ret = (0.5f + GetStats(2) / 75F*(2 - 0.5f));

            if (player.HasConditionEffect(ConditionEffectIndex.Damaging))
                ret *= 1.5f;

            return ret;
        }

        public static float GetDefenseDamage(Entity host, int dmg, int def)
        {
            if (host.HasConditionEffect(ConditionEffectIndex.Armored))
                def *= 2;
            if (host.HasConditionEffect(ConditionEffectIndex.ArmorBroken))
                def = 0;

            float limit = dmg*0.15f;

            float ret;
            if (dmg - def < limit) ret = limit;
            else ret = dmg - def;

            if (host.HasConditionEffect(ConditionEffectIndex.Invulnerable) ||
                host.HasConditionEffect(ConditionEffectIndex.Invincible))
                ret = 0;
            return ret;
        }

        public float GetDefenseDamage(int dmg, bool noDef)
        {
            int def = GetStats(3);
            if (player.HasConditionEffect(ConditionEffectIndex.Armored))
                def *= 2;
            if (player.HasConditionEffect(ConditionEffectIndex.ArmorBroken) ||
                noDef)
                def = 0;

            float limit = dmg*0.15f;

            float ret;
            if (dmg - def < limit) ret = limit;
            else ret = dmg - def;

            if (player.HasConditionEffect(ConditionEffectIndex.Invulnerable) ||
                player.HasConditionEffect(ConditionEffectIndex.Invincible))
                ret = 0;
            return ret;
        }

        public static float GetSpeed(Entity entity, float stat)
        {
            float ret = 4 + 5.6f*(stat/75f);
            if (entity.HasConditionEffect(ConditionEffectIndex.Speedy))
                ret *= 1.5f;
            if (entity.HasConditionEffect(ConditionEffectIndex.Slowed))
                ret = 4;
            if (entity.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                ret = 0;
            return ret;
        }

        public float GetSpeed()
        {
            return GetSpeed(player, GetStats(4));
        }

        public float GetHPRegen()
        {
            int vit = GetStats(5);
            if (player.HasConditionEffect(ConditionEffectIndex.Sick))
                vit = 0;
            return 1 + 0.12f * vit;
        }

        public float GetMPRegen()
        {
            int wis = GetStats(6);
            if (player.HasConditionEffect(ConditionEffectIndex.Quiet))
                return 0;
            return 0.5f + 0.06f * wis;
        }

        public float GetDex()
        {
            int dex = GetStats(7);
            if (player.HasConditionEffect(ConditionEffectIndex.Dazed))
                dex = 0;

            float ret = 1.5f + 6.5f*(dex/75f);
            if (player.HasConditionEffect(ConditionEffectIndex.Berserk))
                ret *= 1.5f;
            if (player.HasConditionEffect(ConditionEffectIndex.Stunned))
                ret = 0;
            return ret;
        }

        public static int StatsNameToIndex(string name)
        {
            switch (name)
            {
                case "MaxHitPoints":
                    return 0;
                case "MaxMagicPoints":
                    return 1;
                case "Attack":
                    return 2;
                case "Defense":
                    return 3;
                case "Speed":
                    return 4;
                case "HpRegen":
                    return 5;
                case "MpRegen":
                    return 6;
                case "Dexterity":
                    return 7;
            }
            return -1;
        }

        public static string StatsIndexToName(int index)
        {
            switch (index)
            {
                case 0:
                    return "MaxHitPoints";
                case 1:
                    return "MaxMagicPoints";
                case 2:
                    return "Attack";
                case 3:
                    return "Defense";
                case 4:
                    return "Speed";
                case 5:
                    return "HpRegen";
                case 6:
                    return "MpRegen";
                case 7:
                    return "Dexterity";
            }
            return null;
        }

        public static string StatsIndexToPotName(int index)
        {
            switch (index)
            {
                case 0:
                    return "Life";
                case 1:
                    return "Mana";
                case 2:
                    return "Attack";
                case 3:
                    return "Defense";
                case 4:
                    return "Speed";
                case 5:
                    return "Vitality";
                case 6:
                    return "Wisdom";
                case 7:
                    return "Dexterity";
            }
            return null;
        }

        public class DamageRandom
        {
            public DamageRandom(uint seed = 1)
            {
                Seed = seed;
            }

            public uint Seed { get; private set; }

            public static uint obf1()
            {
                return (uint) Math.Round(new Random().NextDouble()*(uint.MaxValue - 1) + 1);
            }

            public uint obf2()
            {
                return this.obf3();
            }

            public float obf4()
            {
                return this.obf3()/2147483647;
            }

            public float obf5(float param1 = 0.0f, float param2 = 1.0f)
            {
                float _loc3_ = this.obf3()/2147483647;
                float _loc4_ = this.obf3()/2147483647;
                float _loc5_ = (float) Math.Sqrt(-2*(float) Math.Log(_loc3_))*(float) Math.Cos(2*_loc4_*Math.PI);
                return param1 + _loc5_*param2;
            }

            public uint obf6(uint param1, uint param2)
            {
                if (param1 == param2)
                {
                    return param1;
                }
                return param1 + this.obf3()%(param2 - param1);
            }

            public float obf7(float param1, float param2)
            {
                return param1 + (param2 - param1)*this.obf4();
            }

            private uint obf3()
            {
                uint _loc1_ = 0;
                uint _loc2_ = 0;
                _loc2_ = 16807*(this.Seed & 65535);
                _loc1_ = 16807*(this.Seed >> 16);
                _loc2_ = _loc2_ + ((_loc1_ & 32767) << 16);
                _loc2_ = _loc2_ + (_loc1_ >> 15);
                if (_loc2_ > 2147483647)
                {
                    _loc2_ = _loc2_ - 2147483647;
                }
                return this.Seed = _loc2_;
            }
        }
    }
}