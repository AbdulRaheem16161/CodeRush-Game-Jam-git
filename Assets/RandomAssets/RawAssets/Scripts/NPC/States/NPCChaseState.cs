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


        private Vector3 lastSetDestination;
        private float destinationUpdateRate = 0.2f;
        private float destinationTimer;
        private float targetChangeThreshold = 2.5f;
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
                HandleNormalChaseMovement(deltaTime);
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
            zigZagTimer += deltaTime;
            zigZagPhase += deltaTime * zigZagSpeed;

            Vector3 direction =
                (currentTargetToChase - npcTransform.position).normalized;

            Vector3 perpendicular =
                Vector3.Cross(direction, Vector3.up);

            float zigzag =
                Mathf.Sin(zigZagPhase) * zigZagStrength;

            // 🔥 NEW FIX: stabilize forward movement (IMPORTANT)
            Vector3 forwardStep = direction * 3.5f;

            Vector3 zigOffset = perpendicular * zigzag;

            Vector3 desiredTarget = npcTransform.position + forwardStep + zigOffset;

            // ======================================================
            // 🔥 CRITICAL FIX: DON'T override if agent is on link
            // ======================================================
         //   if (stateMachine.Agent.isOnOffMeshLink)
              //  return;

            // ======================================================
            // 🔥 CRITICAL FIX #2: avoid spam updates
            // ======================================================
            if (zigZagTimer >= zigZagUpdateRate)
            {
                zigZagTimer = 0f;

                stateMachine.Agent.SetDestination(desiredTarget);
            }
        }

        private void HandleNormalChaseMovement(float deltaTime)
        {
            destinationTimer += deltaTime;

            // how far target moved since last update
            float targetMovedDistance = Vector3.Distance(currentTargetToChase, lastSetDestination);

            if (destinationTimer >= destinationUpdateRate || targetMovedDistance > targetChangeThreshold)
            {
                stateMachine.Agent.SetDestination(currentTargetToChase);

                lastSetDestination = currentTargetToChase;
                destinationTimer = 0f;
            }
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
                    stateMachine.SwitchState(stateMachine.MeleeAttackState);
                }
            }
        }
    }
}