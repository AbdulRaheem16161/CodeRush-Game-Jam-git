using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace AbdulRaheem.Game.Shared
{
    public class AnimationHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator animator;

        [Header("Melee Randomization")]
        [SerializeField] private bool enableSequenceMeleeAttack;
        [SerializeField] private bool enableRandomizeMeleeAttack;
        [SerializeField] private int meleeAttackVariantsCount;

        [Header("Live Values")]
        [SerializeField] private int meleeAttackIndex;

        // Parameter Constants
        private const string SPEED = "speed";
        private const string IS_GROUNDED = "isGrounded";
        private const string TRIGGER_JUMP = "triggerJump";
        private const string TRIGGER_MELEE_ATTACK = "triggerMeleeAttack";
        private const string TRIGGER_RANGED_ATTACK = "triggerRangedAttack";
        private const string MELEE_ATTACK_INDEX = "meleeAttackIndex";
        private const string TRIGGER_FLINCH = "triggerFlinch";
        private const string TRIGGER_DEATH = "triggerDeath";
        private const string ISDEAD = "isDead";

        public event Action RangedAttackAction;
        public event Action MeleeAttackAction;

        private void Awake()
        {
            if (enableSequenceMeleeAttack) enableRandomizeMeleeAttack = false;
        }

        public void SetSpeed(float speed)
        {
            animator.SetFloat(SPEED, speed);
        }

        public void SetGrounded(bool isGrounded)
        {
            animator.SetBool(IS_GROUNDED, isGrounded);
        }

        public void TriggerJumpAnim()
        {
            animator.SetTrigger(TRIGGER_JUMP);
        }

        public void TriggerMeleeAttackAnim()
        {
            if (enableSequenceMeleeAttack && meleeAttackVariantsCount >= 1)
            {
                if (meleeAttackIndex == meleeAttackVariantsCount)
                {
                    meleeAttackIndex = 1;
                }
                else
                {
                    meleeAttackIndex = meleeAttackIndex + 9;
                }
            }
            else if (enableRandomizeMeleeAttack && meleeAttackVariantsCount >= 1)
            {
                meleeAttackIndex = UnityEngine.Random.Range(1, meleeAttackVariantsCount + 1);
            }
            else
            {
                meleeAttackIndex = 1;
            }

            animator.SetTrigger(TRIGGER_MELEE_ATTACK);
            animator.SetFloat(MELEE_ATTACK_INDEX, meleeAttackIndex);
        }

        public void TriggerRangedAttackAnim()
        {
            animator.SetTrigger(TRIGGER_RANGED_ATTACK);
        }

        public void TriggerGetHitAnim()
        {
            animator.SetTrigger(TRIGGER_FLINCH);
        }

        public void TriggerDeathAnim()
        {
            animator.SetTrigger(TRIGGER_DEATH);

            animator.SetBool(ISDEAD, true);
        }

        public void ResetHitAnimTrigger()
        {
            animator.ResetTrigger(TRIGGER_DEATH);
        }

        // called by Animation Events
        public void PerformRangedAttack() => RangedAttackAction?.Invoke();
        public void PerformMeleeAttack() => MeleeAttackAction?.Invoke();
    }
}