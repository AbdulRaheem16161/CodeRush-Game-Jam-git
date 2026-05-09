using UnityEngine;

namespace AbdulRaheem.Game.Weapons
{
    public class WeaponRanged
    {
        private WeaponRangedDefination defination;

        private float lastAttackTime;

        public WeaponRanged(WeaponRangedDefination defination)
        {
            this.defination = defination;
        }

        public void Shoot(Transform firePoint, Transform target, LayerMask targetLayer)
        {
            Vector3 direction = (target.position - firePoint.position).normalized;

            Quaternion rotation = Quaternion.LookRotation(direction);

            GameObject proj = Object.Instantiate(
                defination.ProjectilePrefab,
                firePoint.position,
                rotation
            );

            proj.GetComponent<MagicBall>()
                .Initialize(defination.Speed, defination.Damage, targetLayer);
        }
    }
}