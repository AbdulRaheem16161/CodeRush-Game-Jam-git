using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AbdulRaheem.Game.NPC
{
    public class CamouflageZombie : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private Transform player;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;

        [Header("Animation")]
        [SerializeField] private AnimationClip getUpClip;

        [Header("Settings")]
        [SerializeField] private float triggerDistance = 5f;
        [SerializeField] private string getUpTriggerName = "GetUp";

        [Header("Gizmos")]
        [SerializeField] private bool showTriggerRadius = true;
        [SerializeField] private Color gizmoColor = Color.red;

        #endregion

        #region Runtime

        private bool hasWokenUp = false;

        #endregion

        private void Start()
        {
            #region Init

            if (agent != null)
            {
                agent.isStopped = true;
            }

            #endregion
        }

        private void Update()
        {
            #region Detection

            if (hasWokenUp) return;

            if (player == null || agent == null) return;

            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= triggerDistance)
            {
                StartCoroutine(WakeUpSequence());
            }

            #endregion
        }

        private IEnumerator WakeUpSequence()
        {
            #region Safety

            if (hasWokenUp) yield break;
            hasWokenUp = true;

            #endregion

            #region Stop Movement

            agent.isStopped = true;

            #endregion

            #region Play Animation

            if (animator != null)
            {
                animator.SetTrigger(getUpTriggerName);
            }

            #endregion

            #region Wait Animation

            if (getUpClip != null)
            {
                yield return new WaitForSeconds(getUpClip.length);
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }

            #endregion

            #region Enable Movement

            if (agent != null)
            {
                agent.isStopped = false;
            }

            #endregion
        }

        private void OnDrawGizmosSelected()
        {
            #region Gizmo Drawing

            if (!showTriggerRadius) return;

            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, triggerDistance);

            #endregion
        }
    }
}