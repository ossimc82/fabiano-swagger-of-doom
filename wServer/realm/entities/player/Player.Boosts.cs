namespace wServer.realm.entities.player
{
    public partial class Player
    {
        private bool lootDropBoostFreeTimer;
        private bool lootTierBoostFreeTimer;
        private bool ninjaShoot;
        private bool ninjaFreeTimer;
        private bool xpFreeTimer;

        public void HandleBoosts()
        {
            if (ninjaShoot && ninjaFreeTimer)
            {
                if (Mp > 0)
                {
                    ninjaFreeTimer = false;
                    Owner.Timers.Add(new WorldTimer(100, (w, t) =>
                    {
                        Mp -= 1;
                        if (Mp <= 0)
                            ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Speedy, DurationMS = 0 });
                        ninjaFreeTimer = true;
                        UpdateCount++;
                    }));
                }
            }

            if (XpBoosted && xpFreeTimer)
            {
                if (XpBoostTimeLeft > 0)
                {
                    xpFreeTimer = false;
                    Owner.Timers.Add(new WorldTimer(1000, (w, t) =>
                    {
                        XpBoostTimeLeft -= 1;
                        if (XpBoostTimeLeft <= 0)
                            XpBoosted = false;
                        xpFreeTimer = true;
                        UpdateCount++;
                    }));
                }
                else
                    XpBoosted = false;
            }

            if (LootDropBoost && lootDropBoostFreeTimer)
            {
                if (LootDropBoostTimeLeft > 0)
                {
                    lootDropBoostFreeTimer = false;
                    Owner.Timers.Add(new WorldTimer(1000, (w, t) =>
                    {
                        LootDropBoostTimeLeft -= 1;
                        lootDropBoostFreeTimer = true;
                        UpdateCount++;
                    }));
                }
            }

            if (LootTierBoost && lootTierBoostFreeTimer)
            {
                if (LootTierBoostTimeLeft > 0)
                {
                    lootTierBoostFreeTimer = false;
                    Owner.Timers.Add(new WorldTimer(1000, (w, t) =>
                    {
                        LootTierBoostTimeLeft -= 1;
                        lootTierBoostFreeTimer = true;
                        UpdateCount++;
                    }));
                }
            }
        }
    }
}
