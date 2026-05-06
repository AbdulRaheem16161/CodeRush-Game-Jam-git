using UnityEngine;
using AbdulRaheem.Game.Shared;

namespace AbdulRaheem.Game.Weapons
{
    public class MeleeHitbox : MonoBehaviour
    {
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float gizmosDuration = 0.2f;

        private float lastHitTime = -999f;
        private float lastHitRange;

        public void PerformHit(float damage, float range)
        {
            Debug.Log("PerformHit");
            lastHitTime = Time.time;
            lastHitRange = range;

            Collider[] hits = Physics.OverlapSphere(transform.position, range, targetLayer);

            foreach (Collider hit in hits)
            {
                Debug.Log("hit in hits");
                Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
                float dot = Vector3.Dot(transform.forward, directionToTarget);

                if (dot < 0.5f) continue;

                IDamageable damageable = hit.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    Debug.Log("TakeDamage");
                }
                damageable?.TakeDamage(damage);
            }
        }

        private void OnDrawGizmos()
        {
            if (Time.time - lastHitTime <= gizmosDuration)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.4f);

                // dot < 0.5 = 60 degree cone, so half angle is 30 degrees
                float halfAngle = 30f;
                int rayCount = 20; // smoothness of the arc

                Vector3 origin = transform.position;
                Vector3 forward = transform.forward;

                // draw the edge rays of the cone
                Vector3 leftEdge = Quaternion.Euler(0, -halfAngle, 0) * forward * lastHitRange;
                Vector3 rightEdge = Quaternion.Euler(0, halfAngle, 0) * forward * lastHitRange;

                Gizmos.DrawRay(origin, leftEdge);
                Gizmos.DrawRay(origin, rightEdge);

                // draw the arc connecting the edges
                Vector3 prevPoint = origin + leftEdge;
                for (int i = 1; i <= rayCount; i++)
                {
                    float angle = -halfAngle + (halfAngle * 2f / rayCount) * i;
                    Vector3 nextDir = Quaternion.Euler(0, angle, 0) * forward * lastHitRange;
                    Vector3 nextPoint = origin + nextDir;

                    Gizmos.DrawLine(prevPoint, nextPoint);
                    prevPoint = nextPoint;
                }
            }
        }
    }
}