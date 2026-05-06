using UnityEngine;

namespace AbdulRaheem.Game.Weapons
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private WeaponDefination initialWeapon;

        [SerializeField] private Transform firePoint;

        [Header("Live Values")]
        [SerializeField] private WeaponRanged currentRangedWeapon;
        [SerializeField] private WeaponMelee currentMeleedWeapon;

        [Header("Debug")]
        [SerializeField] private string holdingWeaponType;

        [SerializeField] private LayerMask targetLayer;

        private void Awake()
        {
            EquipWeapon(initialWeapon);
        }

        private void EquipWeapon(WeaponDefination defination)
        {
            if (defination is WeaponRangedDefination rangedDefination)
            {
                currentRangedWeapon = new WeaponRanged(rangedDefination);
                currentMeleedWeapon = null;

                holdingWeaponType = "Ranged";
            }
            else if (defination is WeaponMeleeDefination meleeDefination)
            {
                currentMeleedWeapon = new WeaponMelee(meleeDefination);
                currentRangedWeapon = null;

                holdingWeaponType = "Melee";
            }
        }

        public void TryAttacking()
        {
            Debug.Log("WeaponController : tried Attacking");
            if (currentRangedWeapon != null)
            {
                currentRangedWeapon.Shoot(firePoint, targetLayer);
            }
            else if (currentMeleedWeapon != null)
            {
                // currentMeleedWeapon.Slash();
            }
        }
    }
}