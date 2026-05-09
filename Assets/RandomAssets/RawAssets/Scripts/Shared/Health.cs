using System;
using UnityEngine;
using AbdulRaheem.Game.Shared;
using System.Collections;
using Akila.FPSFramework;
using UnityEngine.UI;

namespace AbdulRaheem.Game.Shared
{
    public class Health : MonoBehaviour, IDamageable
    {
        [field : SerializeField] public string Type { get; set; }
        [SerializeField] private float currentHealth;
        [SerializeField] private float totalHealth;

        [SerializeField] private bool CantDie;
        [SerializeField] private float disableDelay = 2f;
        [SerializeField] private Damageable AKILLADamageable;

        [SerializeField] private GameObject PlayerParent;

        public UnityEngine.AI.NavMeshAgent navMeshAgent;
        public event Action<float> OnDamage;
        public event Action OnDeath;

        public Image healthBarImage;

        private void Awake()
        {
            currentHealth = totalHealth;

            if (AKILLADamageable != null)
            {
                AKILLADamageable.OnDamage_ += TakeDamage;
                AKILLADamageable.OnDeath_ += Die;
            }
        }

        public void TakeDamage(float damage)
        {
            Debug.Log(gameObject.name + ": take Damage"); 

            currentHealth -= damage;

            if (healthBarImage != null)
            {
                healthBarImage.fillAmount = currentHealth / totalHealth;
            }

            if (currentHealth < 0) currentHealth = 0;

            OnDamage?.Invoke(currentHealth);

            if (currentHealth <= 0) Die();
        }

        private void Die()
        {
            if (CantDie) return;

            OnDeath?.Invoke();

            if (navMeshAgent != null) navMeshAgent.enabled = false;

            StartCoroutine(DisableAfterDelay());

            if(PlayerParent != null) PlayerParent.SetActive(false);
        }

        private IEnumerator DisableAfterDelay()
        {
            yield return new WaitForSeconds(disableDelay);
            gameObject.SetActive(false);
        }
    }
}