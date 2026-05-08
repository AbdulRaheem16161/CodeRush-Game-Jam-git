using AbdulRaheem.Game.Player;
using AbdulRaheem.Game.Shared;
using UnityEngine;
using System.Collections;

namespace AbdulRaheem.Game.NPC
{
    public class NPCSight : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform eyes;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private LayerMask obstacleLayer;

        [Header("On/off")]
        [SerializeField] private bool sightActivated;
        [SerializeField] private bool showGizmos;

        [SerializeField] private Color gizmosColor;

        [Header("Debug")]
        [SerializeField] private bool targetDetected;

        private NPCDefinition definition;
        [SerializeField] private Transform detectedTarget;
        [SerializeField] private Transform lastDetectedTarget;

        public bool TargetDetected => targetDetected;
        public Transform DetectedTarget => detectedTarget;
        public Transform LastDetectedTarget => lastDetectedTarget;

        [Header("Alert Settings")]
        [SerializeField] private float alertModeSightRange;
        [SerializeField] private float alertModeSightAngle;
        [SerializeField] private float alertDuration;

        [SerializeField] private Health health;
        [SerializeField] private NPCStateMachine stateMachine;

        [Header("Debug Live Values")]
        public float distanceToTarget;

        // ✅ runtime values (THIS is the real fix)
        public float currentSightRange;
        public float currentSightAngle;

        private Coroutine alertCoroutine;

        public GameObject player;
        public void Initialize(NPCDefinition definition)
        {
            this.definition = definition;

            // initialize runtime values from definition
            currentSightRange = definition.SightRange;
            currentSightAngle = definition.SightAngle;

            health.OnDamage += GetAlert;
        }

        public void Tick()
        {
            if (!sightActivated) return;

            Debug.Log("NPCSight is Ticking");

            targetDetected = DetectPlayer();
        }

        private bool DetectPlayer()
        {
            Debug.Log("NPCSight 1");


            if (eyes == null || definition == null) return false;

            Debug.Log("eyes == null || definition == null paassed");

            // Step 1: Range check
            Collider[] hits = Physics.OverlapSphere(transform.position, currentSightRange, targetLayer);

            if (hits.Length == 0)
            {
                detectedTarget = null;
                return false;

                Debug.Log("NPCSight isn't detecting player  because hits.Length == 0");
            }

            Debug.Log("rays are hitting the player, now chekcing for angle");

            Transform target = hits[0].transform;

            // Step 2: Angle check
            Vector3 directionToTarget = (target.position - eyes.position).normalized;
            float angle = Vector3.Angle(eyes.forward, directionToTarget);

            if (angle > currentSightAngle / 2f)
            {
                detectedTarget = null;
                Debug.Log("NPCSight isn't detecting player because angle > currentSightAngle / 2f");
                return false;
            }

            Debug.Log("player is within sight angle, now checking for line of sight");

            // Step 3: Line of sight check
            distanceToTarget = Vector3.Distance(eyes.position, target.position);

            if (Physics.Raycast(eyes.position, directionToTarget, distanceToTarget, obstacleLayer))
            {
                detectedTarget = null;
                return false;
            }

            Debug.Log("NPCSight has detected the player!");

            detectedTarget = target;
            lastDetectedTarget = detectedTarget;
            return true;
        }

        public void HearShound(float damage = 0)
        {
            stateMachine.HasHeardSound = true;
        }

        public void GetAlert(float damage = 0)
        {
            // stop previous alert if running
            if (alertCoroutine != null)
                StopCoroutine(alertCoroutine);

            alertCoroutine = StartCoroutine(AlertRoutine());
        }

        private IEnumerator AlertRoutine()
        {
            // apply alert values
            currentSightRange = alertModeSightRange;
            currentSightAngle = alertModeSightAngle;

            yield return new WaitForSeconds(alertDuration);

            // revert back to default
            currentSightRange = definition.SightRange;
            currentSightAngle = definition.SightAngle;

            alertCoroutine = null;
        }

        private void OnDrawGizmos()
        {
            if (eyes == null || !showGizmos) return;

            float range = Application.isPlaying ? currentSightRange : definition != null ? definition.SightRange : 0f;
            float angle = Application.isPlaying ? currentSightAngle : definition != null ? definition.SightAngle : 0f;

            // Draw sight range
            Gizmos.color = gizmosColor;
            Gizmos.DrawWireSphere(transform.position, range);

            // Draw vision cone
            Vector3 leftBoundary = Quaternion.Euler(0, -angle / 2f, 0) * eyes.forward;
            Vector3 rightBoundary = Quaternion.Euler(0, angle / 2f, 0) * eyes.forward;

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(eyes.position, leftBoundary * range);
            Gizmos.DrawRay(eyes.position, rightBoundary * range);

            if (targetDetected && detectedTarget != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(eyes.position, detectedTarget.position);
            }
        }

        public void NullifyTarget()
        {
            detectedTarget = null;
        }
    }
}