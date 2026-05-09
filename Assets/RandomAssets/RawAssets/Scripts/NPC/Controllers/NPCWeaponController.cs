//using UnityEditor.Rendering;
using UnityEngine;

namespace AbdulRaheem.Game.Weapons
{
    public class NPCWeaponController : MonoBehaviour
    {
        [Header("References")]

        public GuardArcShooter arcShooter;
        public bool useArcShooter = false;

        [field: SerializeField] public WeaponMeleeDefination MeleeWeaponDefination { get; private set; }
        [field: SerializeField] public WeaponRangedDefination RangedWeaponDefination { get; private set; }

        [field: SerializeField] public float RangedWeaponRange { get; private set; }
        [field: SerializeField] public float MeleeWeaponRange { get; private set; }

        [SerializeField] private MeleeHitbox hitbox;
        [SerializeField] private Transform firePoint;

        [SerializeField] private LayerMask targetLayer;

        [Header("private Values")]
        private WeaponRanged rangedWeapon;
        private WeaponMelee meleeWeapon;

        [Header("Gizmos")]
        [SerializeField] private bool turnOnGizmos = true;
        [SerializeField] private Color meleeGizmoColor = Color.red;
        [SerializeField] private Color rangedGizmoColor = Color.cyan;

        [SerializeField] private AudioSource fireballSound;

        public GameObject player;

        private void Awake()
        {
            if (MeleeWeaponDefination != null)
            {
                MeleeWeaponRange = MeleeWeaponDefination.Range;
                meleeWeapon = new WeaponMelee(MeleeWeaponDefination);
            }

            if (RangedWeaponDefination != null)
            {
                RangedWeaponRange = RangedWeaponDefination.Range;
                rangedWeapon = new WeaponRanged(RangedWeaponDefination);
            }

            player = GameObject.FindWithTag("PlayerPointToTarget");

            fireballSound = GetComponent<AudioSource>();
        }

        public void PerformMeleeAttack()
        {
            Debug.Log("NPCWeaponController PerformMeleeAttack");
            meleeWeapon.Slash(hitbox, MeleeWeaponDefination.Damage, MeleeWeaponRange, targetLayer);
        }

        public void PerformRangedAttack()
        {
            if (useArcShooter && arcShooter != null)
            {
                arcShooter.Shoot();

                fireballSound.Play();
            }
            else
            {
                rangedWeapon.Shoot(firePoint, player.transform, targetLayer);
                fireballSound.Play();
            }
        }

        // Getters
        public float getFireRate()
        {
            return RangedWeaponDefination.FireRate;
        }

        public float GetSlashRate()
        {
            return MeleeWeaponDefination.SlashRate;
        }

        private void OnDrawGizmos()
        {
            if (!turnOnGizmos) return;

            // Melee range
            Gizmos.color = meleeGizmoColor;
            Gizmos.DrawWireSphere(transform.position, MeleeWeaponRange);

            // Ranged range
            if (firePoint != null)
            {
                Gizmos.color = rangedGizmoColor;
                Gizmos.DrawWireSphere(firePoint.position, RangedWeaponRange);
            }
        }
    }
}