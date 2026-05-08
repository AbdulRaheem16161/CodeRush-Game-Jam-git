using UnityEngine;

namespace AbdulRaheem.Game.Weapons
{
    [CreateAssetMenu(menuName = "Weapon/WeaponMeleeDefination")]
    public class WeaponMeleeDefination : WeaponDefination
    {
       [field : SerializeField]  public float SlashRate { get; private set; }    
    }
}