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

        public void Shoot(Transform firePoint, LayerMask targetLayer)
        {
            GameObject proj = Object.Instantiate(defination.ProjectilePrefab, firePoint.position, firePoint.rotation);
            proj.GetComponent<MagicBall>().Initialize(defination.Speed, defination.Damage, targetLayer);
        }
    }
}