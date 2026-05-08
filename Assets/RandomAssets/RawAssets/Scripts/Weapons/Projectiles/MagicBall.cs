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
            IDamageable damageable = other.GetComponent<IDamageable>();


            if (damageable != null)
            {
                
                Debug.Log("4");
                if (damageable.Type != "Player") return;

                Debug.Log("doing damage: " + damage + " on the player");
                damageable.TakeDamage(damage);

                if (other.gameObject.layer == targetLayer){

                    Destroy(gameObject);
                }
            }

            Debug.Log("5");
        }
    }
}