using UnityEngine;

namespace AbdulRaheem.Game.Weapons
{
    public class WeaponMelee
    {
        private WeaponMeleeDefination defination;

        private float lastAttackTime;

        public WeaponMelee(WeaponMeleeDefination defination)
        {
            this.defination = defination;
        }

        public void Slash(MeleeHitbox hitbox, float damage, float range, LayerMask targetLayer)
        {
            hitbox.PerformHit(damage, range);
        }
    }
}