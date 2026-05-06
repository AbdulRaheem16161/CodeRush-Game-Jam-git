using UnityEngine;
using AbdulRaheem.Game.NPC;
using AbdulRaheem.Game.Weapons;
using System;

namespace AbdulRaheem.Game.NPC
{
    public class NPCChaseState : NPCBaseState
    {
        public NPCChaseState(NPCStateMachine stateMachine) : base(stateMachine) { }

        private float lostPlayerStamp;
        private bool isTrackingLostTarget;

        private Vector3 currentTargetToChase;
        private Transform npcTransform; // cache

        #region ZigZag Settings
        private float zigZagTimer;
        private float zigZagUpdateRate = 0.2f;
        private float zigZagStrength = 6;
        private float zigZagSpeed = 2;
        #endregion

        public override void Enter()
        {
            base.Enter();

            agent.speed = definition.ChaseSpeed;
            isTrackingLostTarget = false;
            lostPlayerStamp = float.MaxValue;

            // cache
            npcTransform = stateMachine.transform;
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
                isTrackingLostTarget = false;
            }

            if (!TargetDetected() && !isTrackingLostTarget)
            {
                isTrackingLostTarget = true;
                lostPlayerStamp = Time.time;
            }

            if (isTrackingLostTarget)
            {
                currentTargetToChase = sight.LastDetectedTarget.transform.position;
            }

            #region Movement Logic

            if (stateMachine.ChaseInZigzag)
            {
                HandleZigZagMovement(deltaTime);
            }
            else
            {
                stateMachine.Agent.SetDestination(currentTargetToChase);
            }

            #endregion

            #region Rotation

            Vector3 targetDirection = currentTargetToChase - npcTransform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            float rotationSpeed = 5f;
            npcTransform.rotation = Quaternion.Slerp(npcTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            #endregion

        }

        private float zigZagPhase;

        private void HandleZigZagMovement(float deltaTime)
        {
            #region ZigZag Movement

            zigZagTimer += deltaTime;
            zigZagPhase += deltaTime * zigZagSpeed;

            if (zigZagTimer >= zigZagUpdateRate)
            {
                zigZagTimer = 0f;

                // Direction to target
                Vector3 direction = (currentTargetToChase - npcTransform.position).normalized;

                // Perpendicular direction
                Vector3 perpendicular = Vector3.Cross(direction, Vector3.up);

                // CONSTANT zigzag (no time weirdness)
                float zigzag = Mathf.Sin(zigZagPhase) * zigZagStrength;

                // 🔥 KEY FIX: offset from NPC, not target
                Vector3 forward = direction * 2f; // keeps moving forward
                Vector3 finalTarget = npcTransform.position + forward + (perpendicular * zigzag);

                stateMachine.Agent.SetDestination(finalTarget);
            }

            #endregion
        }

        protected override void ChecksForSwitchingState()
        {
            base.ChecksForSwitchingState();

            if ((!TargetDetected()) && (Time.time > lostPlayerStamp + definition.LostPlayerTimeOut))
            {
                stateMachine.SwitchState(stateMachine.IdleState);
            }

            if (stateMachine.CanRangeAttack && weaponController.RangedWeaponDefination != null)
            {
                if (sight.distanceToTarget <= (weaponController.RangedWeaponRange * 70) / 100)
                {
                    stateMachine.SwitchState(stateMachine.RangedAttackState);
                }
            }

            if (stateMachine.CanMeleeAttack && weaponController.MeleeWeaponDefination != null)
            {
                if (sight.distanceToTarget <= weaponController.MeleeWeaponRange)
                {
                    //stateMachine.SwitchState(stateMachine.MeleeAttackState);
                }
            }
        }
    }
}