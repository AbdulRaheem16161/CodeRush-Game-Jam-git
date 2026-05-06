//using UnityEngine;

//namespace AbdulRaheem.Game.Weapons
//{
//    public class WeaponRanged
//    {
//        private WeaponRangedDefination defination;

//        private float lastAttackTime;

//        public WeaponRanged(WeaponRangedDefination defination)
//        {
//            this.defination = defination;
//        }

//        //private bool CanShoot()
//        //{
//        //    if (defination.FireRate <= Time.time - lastAttackTime)
//        //    {
//        //        lastAttackTime = Time.time;
//        //        return true;
//        //    }

//        //    return false;
//        //}

//        public void Shoot(Transform firePoint)
//        {
//            //if (!CanShoot()) return false;

//            GameObject proj = Object.Instantiate(defination.ProjectilePrefab, firePoint.position, firePoint.rotation);
//            proj.GetComponent<MagicBall>().Initialize(defination.Speed, defination.Damage);

//            //return true;
//        }
//    }
//}