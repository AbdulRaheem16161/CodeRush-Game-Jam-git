using UnityEngine;
using AbdulRaheem.Game.NPC;

namespace AbdulRaheem.Game.NPC
{
    public class NPCPatrolState : NPCBaseState
    {
        public NPCPatrolState(NPCStateMachine stateMachine) : base(stateMachine) { }

        private Transform[] trackPoints; // will get referenced from the stateMachine.PatrolPath
        private float waitingTime; // will get referenced from the definition

        // Run time veriables
        private float waitingTimeStartStamp;
        private int currentIndex;
        private bool isWaiting;

        public override void Enter()
        {
            base.Enter();

            if (stateMachine.isAHuntingMonster)
            {
                sight.currentSightRange = 9999;
                sight.currentSightAngle = 360;

            }

            trackPoints = stateMachine.PatrolPath.TrackPoints;
            waitingTime = definition.WaitTimeBetweenPatrolPoints;
            currentIndex = 0;

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

                currentIndex++;

                if (currentIndex >= trackPoints.Length)
                {
                    if(stateMachine.isAHuntingMonster)
                    {
                        stateMachine.SwitchState(stateMachine.ChaseState);
                    }
                    else
                    {
                        currentIndex = 0;
                    }


                }
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

            // patrol movement to chase state
            if (TargetDetected() && stateMachine.CanChase)
            {
                if (stateMachine.isAHuntingMonster) return;
                stateMachine.SwitchState(stateMachine.ChaseState);
            }

            // Patrol to Attack
            // Patrol to Flee
        }

        private void MoveToNextPoint()
        {
            agent.SetDestination(trackPoints[currentIndex].position);
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

