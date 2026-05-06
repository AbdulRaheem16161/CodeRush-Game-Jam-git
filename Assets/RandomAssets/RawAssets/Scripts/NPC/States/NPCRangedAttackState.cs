using UnityEngine;
using AbdulRaheem.Game.NPC;
using UnityEngine.UIElements;

namespace AbdulRaheem.Game.NPC
{
    public class NPCRangedAttackState : NPCBaseState
    {
        public NPCRangedAttackState(NPCStateMachine stateMachine) : base(stateMachine) { }

        private Vector3 currentTargetToChase;
        private float distanceToTarget;

        private float lastAttackTime;

        public override void Enter()
        {
            base.Enter();

            agent.speed = definition.RangedAttackMoveSpeed;

            animationHandler.RangedAttackAction += PerformAttack;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            if (sight.DetectedTarget != null)
            {
                currentTargetToChase = sight.DetectedTarget.position;
                distanceToTarget = Vector3.Distance(stateMachine.transform.position, currentTargetToChase);
            }

            RotateTowardsTheTarget();


            if (CanAttack()) animationHandler.TriggerRangedAttackAnim();  // the Ranged Attack Animation Clip calls the function AnimationHandler.PerformRangedAttack() which invokes
        }                                                                // RangedAttackAction. this.PerfromAttack() subnscribes to that action and calls  weaponController.TryRangedAttack();

        protected override void ChecksForSwitchingState()
        {
            base.ChecksForSwitchingState();

            if (distanceToTarget > weaponController.RangedWeaponRange)
            {
                stateMachine.SwitchState(stateMachine.ChaseState);
            }

            if (stateMachine.CanMeleeAttack && weaponController.MeleeWeaponDefination != null)
            {
                if (distanceToTarget <= weaponController.MeleeWeaponRange)  
                {
                    stateMachine.SwitchState(stateMachine.MeleeAttackState);
                }
            }

            if (sight.DetectedTarget == null)
            {
                stateMachine.SwitchState(stateMachine.IdleState);
            }
        }

        private bool CanAttack()
        {
            if (weaponController.getFireRate() <= Time.time - lastAttackTime)
            {
                lastAttackTime = Time.time;
                return true;
            }

            return false;
        }

        private void PerformAttack()
        {
            weaponController.PerformRangedAttack();
        }

        private void RotateTowardsTheTarget()
        {
            Vector3 direction = currentTargetToChase - stateMachine.transform.position;

            // Create rotation looking at target
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate toward it
            stateMachine.transform.rotation = Quaternion.Slerp(
                stateMachine.transform.rotation,
                targetRotation,
                Time.deltaTime * 5f
            );
        }
    }
}

