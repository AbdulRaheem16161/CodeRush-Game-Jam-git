using UnityEngine;
using AbdulRaheem.Game.NPC;

namespace AbdulRaheem.Game.NPC
{
    public class NPCMeleeAttackState : NPCBaseState
    {
        public NPCMeleeAttackState(NPCStateMachine stateMachine) : base(stateMachine) { }

        private Vector3 currentTargetToChase;
        private float distanceToTarget;

        private float lastAttackTime;

        public override void Enter()
        {
            base.Enter();

            agent.speed = definition.MeleeAttackMoveSpeed;
            animationHandler.MeleeAttackAction += PerfromAttack;
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

            if (CanAttack() && sight.distanceToTarget < 1.5) animationHandler.TriggerMeleeAttackAnim();   // the Melee Attack Animation Clip calls the function AnimationHandler.PerformMeleeAttack() which invokes
                                                                                                                                        // MeleeAttackAction. this.PerfromAttack() subnscribes to that action and calls  weaponController.TryRangedAttack();
            agent.SetDestination(currentTargetToChase);
        }

        protected override void ChecksForSwitchingState()
        {
            base.ChecksForSwitchingState();

            if (stateMachine.CanRangeAttack && distanceToTarget > weaponController.MeleeWeaponRange)
            {
                stateMachine.SwitchState(stateMachine.RangedAttackState);
            }

            if (sight.DetectedTarget == null)
            {
                sight.GetAlert();
                stateMachine.SwitchState(stateMachine.IdleState);
            }

            if (!stateMachine.CanRangeAttack && sight.distanceToTarget > weaponController.MeleeWeaponRange)
            {
                stateMachine.SwitchState(stateMachine.ChaseState);
            }
        }

        private bool CanAttack()
        {
            if (weaponController.GetSlashRate() <= Time.time - lastAttackTime)
            {
                lastAttackTime = Time.time;
                return true;
            }

            return false;
        }

        private void PerfromAttack()
        {
            Debug.Log("NPCMeleeAttackState PerformMeleeAttack");
            weaponController.PerformMeleeAttack();
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

