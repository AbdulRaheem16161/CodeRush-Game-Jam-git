using AbdulRaheem.Game.Shared;
using UnityEngine;

namespace AbdulRaheem.Game.Weapons
{
    public class MagicBall : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float lifeTime = 5f;
        [SerializeField] private LayerMask targetLayer;

        private bool hasHit;

        public void Initialize(float speed, float damage, LayerMask targetLayer)
        {
            this.speed = speed;
            this.damage = damage;
            this.targetLayer = targetLayer;
        }

        private void Awake()
        {
            Debug.Log("0");
        }

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("1");
            if (hasHit)
                return;

            Debug.Log("2");

            if ((targetLayer & (1 << other.gameObject.layer)) == 0)
                return;

            Debug.Log("3");

            hasHit = true;

            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable != null)
            {

                Debug.Log("4");
                damageable.TakeDamage(damage);
            }

            Debug.Log("5");
            Destroy(gameObject);
        }
    }
}