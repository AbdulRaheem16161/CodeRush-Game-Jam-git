using UnityEngine;

namespace AbdulRaheem.Game.Weapons
{
    public class WeaponDefination : ScriptableObject
    {
        [SerializeField] private float damage;
        [SerializeField] private float range;

        public float Damage => damage;
        public float Range => range;
    }
}
