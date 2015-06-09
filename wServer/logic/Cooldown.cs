#region

using System;

#endregion

namespace wServer.logic
{
    public struct Cooldown
    {
        public readonly int CoolDown;
        public readonly int Variance;

        public Cooldown(int cooldown, int variance)
        {
            CoolDown = cooldown;
            Variance = variance;
        }

        public Cooldown Normalize()
        {
            if (CoolDown == 0)
                return 1000;
            return this;
        }

        public Cooldown Normalize(int def)
        {
            if (CoolDown == 0)
                return def;
            return this;
        }

        public int Next(Random rand)
        {
            if (Variance == 0) return CoolDown;
            return CoolDown + rand.Next(-Variance, Variance + 1);
        }

        public static implicit operator Cooldown(int cooldown)
        {
            return new Cooldown(cooldown, 0);
        }
    }
}