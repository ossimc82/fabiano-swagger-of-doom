#region

using wServer.realm;

#endregion

namespace wServer.logic.behaviors
{
    public class ConditionalEffect : Behavior
    {
        //State storage: none

        private readonly ConditionEffectIndex effect;
        private readonly bool perm;

        public ConditionalEffect(ConditionEffectIndex effect, bool perm = false)
        {
            this.effect = effect;
            this.perm = perm;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            host.ApplyConditionEffect(new ConditionEffect
            {
                Effect = effect,
                DurationMS = -1
            });
        }

        protected override void OnStateExit(Entity host, RealmTime time, ref object state)
        {
            if (!perm)
            {
                host.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = effect,
                    DurationMS = 0
                });
            }
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
        }
    }
}