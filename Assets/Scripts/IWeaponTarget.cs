using UnityEngine;

namespace MatchThreePrototype
{

    public interface IWeaponTarget
    {
        //public abstract void TakeDamage(int damage);
        public abstract void TakeDamage(int damage, ContactPoint2D point);

        public abstract Vector3 GetTransformPosition();

        public abstract bool IsActive();

        public WeaponTargetPriority GetTargetPriority();
    }

    public enum WeaponTargetPriority
    {
        None = 0,
        Golem = 1,
        Rocks = 2
    }
}