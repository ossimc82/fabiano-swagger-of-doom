#region

using System;

#endregion

namespace wServer.realm.entities.player
{
    partial class Player
    {
        private int CanTPCooldownTime;
        private float bleeding;
        private int healCount;
        private float healing;
        private int newbieTime;

        public bool IsVisibleToEnemy()
        {
            if (HasConditionEffect(ConditionEffects.Paused))
                return false;
            if (HasConditionEffect(ConditionEffects.Invisible))
                return false;
            if (newbieTime > 0)
                return false;
            return true;
        }

        private void HandleEffects(RealmTime time)
        {
            if (HasConditionEffect(ConditionEffects.Healing))
            {
                if (healing > 1)
                {
                    HP = Math.Min(Stats[0] + Boost[0], HP + (int) healing);
                    healing -= (int) healing;
                    UpdateCount++;
                    healCount++;
                }
                healing += 28*(time.thisTickTimes/1000f);
            }
            if (HasConditionEffect(ConditionEffects.Quiet) &&
                Mp > 0)
            {
                Mp = 0;
                UpdateCount++;
            }
            if (HasConditionEffect(ConditionEffects.Bleeding) &&
                HP > 1)
            {
                if (bleeding > 1)
                {
                    HP -= (int) bleeding;
                    bleeding -= (int) bleeding;
                    UpdateCount++;
                }
                bleeding += 28*(time.thisTickTimes/1000f);
            }

            if (newbieTime > 0)
            {
                newbieTime -= time.thisTickTimes;
                if (newbieTime < 0)
                    newbieTime = 0;
            }
            if (CanTPCooldownTime > 0)
            {
                CanTPCooldownTime -= time.thisTickTimes;
                if (CanTPCooldownTime < 0)
                    CanTPCooldownTime = 0;
            }
        }

        private bool CanHpRegen()
        {
            if (HasConditionEffect(ConditionEffects.Sick) || HasConditionEffect(ConditionEffects.Bleeding) || OxygenBar == 0)
                return false;
            return true;
        }

        private bool CanMpRegen()
        {
            if (HasConditionEffect(ConditionEffects.Quiet) || ninjaShoot)
                return false;
            return true;
        }

        internal void SetNewbiePeriod()
        {
            newbieTime = 3000;
        }

        internal void SetTPDisabledPeriod()
        {
            CanTPCooldownTime = 10*1000; // 10 seconds
        }

        public bool TPCooledDown()
        {
            if (CanTPCooldownTime > 0)
                return false;
            return true;
        }
    }
}