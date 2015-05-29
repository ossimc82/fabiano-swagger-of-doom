using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors.PetBehaviors
{
    public abstract class PetBehavior : Behavior
    {
        protected Pet Pet { get; private set; }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            Pet pet;
            if (!isValidPet(host, out pet)) return;
            if (Pet == null) Pet = pet;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            Pet pet;
            if (!isValidPet(host, out pet)) return;
            if (Pet == null) Pet = pet;
            TickCore(pet, time, ref state);
        }

        private bool isValidPet(Entity host, out Pet p)
        {
            p = null;
            if (!(host is Pet)) return false;
            var pet = (Pet)host;
            if (PlayerOwnerRequired && pet.PlayerOwner == null) return false;
            p = pet;
            return true;
        }

        protected PetLevel GetLevel(Ability type) => Pet?.GetLevels().FirstOrDefault(_ => _.Ability == type);

        protected bool HasAbility(Ability type) => Pet?.GetLevels().Any(_ => _.Ability == type) ?? false;

        protected abstract bool PlayerOwnerRequired { get; }
        protected abstract void TickCore(Pet pet, RealmTime time, ref object state);

        protected static class Curve
        {
            public static float ExponentialIncrease(float min, float max, float ratio, int level)
            {
                // Todo: Add exponential increase formula
                return -1;
            }
            public static float ExponentialDecrease(float min, float max, float ratio, int level)
            {
                // Todo: Add exponential decrease formula
                return -1;
            }

            public static float LinearGrowth(float min, float max, float ratio, int level)
            {
                // Todo: Add linear growth formula
                return -1;
            }

            public static float DiminishingReturns(float min, float max, float ratio, int level)
            {
                // Todo: Add diminishing returns formula
                return -1;
            }
        }
    }
}
