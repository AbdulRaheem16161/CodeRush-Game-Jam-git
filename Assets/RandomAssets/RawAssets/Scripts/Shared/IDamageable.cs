using UnityEngine;

namespace AbdulRaheem.Game.Shared
{
    public interface IDamageable
    {
        public string Type { get; set; }
        public void TakeDamage(float damage);
    }
}
