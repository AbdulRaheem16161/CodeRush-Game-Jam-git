using UnityEngine;
using AbdulRaheem.Game.NPC;
using UnityEngine.AI;
using Akila.FPSFramework;

namespace AbdulRaheem.Game.NPC
{
    public class NPCRandomMoveState : NPCBaseState
    {
        public NPCRandomMoveState(NPCStateMachine stateMachine) : base(stateMachine) { }

        private float randomMoveRadius;  // will get referenced from the definition
        private float waitingTime; // will get referenced from the definition

        // Run time veriables
        private float waitingTimeStartStamp;
        private bool isWaiting;

        public override void Enter()
        {
            base.Enter();

            waitingTime = definition.WaitTimeBetweenRandomPoints;
            randomMoveRadius = definition.RandomMoveRadius;

            agent.speed = definition.MoveSpeed;

            isWaiting = false;
            MoveToNextPoint();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            if (HasReachedAPoint() && !isWaiting)
            {
                isWaiting = true;
                waitingTimeStartStamp = Time.time;
            }

            if (isWaiting)
            {
                if (waitingTime < Time.time - waitingTimeStartStamp)
                {
                    isWaiting = false;

                    MoveToNextPoint();
                }
            }
        }

        protected override void ChecksForSwitchingState()
        {
            base.ChecksForSwitchingState();

            // random movement to chase state
            if (TargetDetected() && stateMachine.CanChase)
            {
                if (stateMachine.isAHuntingMonster)
                {
                    stateMachine.SwitchState(stateMachine.PatrolState);
                }
                else
                {
                    stateMachine.SwitchState(stateMachine.ChaseState);
                }
                  
            }

            if (stateMachine.HasHeardSound && stateMachine.canGetAlertOnHearingGunSound)
            {
                stateMachine.SwitchState(stateMachine.TryingToFindState);
            }

            // Patrol to Attack
            // Patrol to Flee
        }

        private void MoveToNextPoint()
        {
            // first find a random point inside a specified radius
            float randomX = Random.Range(-randomMoveRadius, randomMoveRadius);
            float randomY = Random.Range(-randomMoveRadius, randomMoveRadius);

            Vector3 randomPoint = new(randomX, 0, randomY);

            randomPoint += agent.transform.position;
            

            NavMeshHit hit;

            // check if that point is walkable according to the NavMesh Surface
            bool foundValidPoint = NavMesh.SamplePosition(randomPoint, out hit, randomMoveRadius, NavMesh.AllAreas);

            if (foundValidPoint)
            {
                agent.SetDestination(hit.position);
            }
            else
            {
                Debug.LogWarning("No valid NavMesh point found");
            }
        }

        private bool HasReachedAPoint()
        {
            if ((!agent.pathPending) && (agent.hasPath) && (agent.remainingDistance <= agent.stoppingDistance))
            {
                return true;
            }

            return false;

            #region Summary
            // Returns true if the agent has a path, the path is fully calculated, and it’s close enough to the target
            // Explanation:
            // 1. !pathPending → ensures the path calculation is complete; we don’t want to check distance while Unity is still figuring out the route
            // 2. hasPath → ensures the agent actually has a path assigned; prevents false positives when no destination is set
            // 3. remainingDistance <= stoppingDistance → confirms the agent is physically close enough to the target to consider it "reached"
            #endregion
        }
    }
}

