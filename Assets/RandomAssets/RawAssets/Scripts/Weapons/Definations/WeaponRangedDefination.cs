using UnityEngine;

namespace AbdulRaheem.Game.Weapons
{
    [CreateAssetMenu(menuName = "Weapon/WeaponRangedDefination")]
    public class WeaponRangedDefination : WeaponDefination
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float speed;
        [SerializeField] private float fireRate;

        public GameObject ProjectilePrefab => projectilePrefab;
        public float Speed => speed;
        public float FireRate => fireRate;
    }
}