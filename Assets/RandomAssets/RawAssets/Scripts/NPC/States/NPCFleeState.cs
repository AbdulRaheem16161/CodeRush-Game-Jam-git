using UnityEngine;
using UnityEngine.AI;

namespace AbdulRaheem.Game.NPC
{
    public class NPCFleeState : NPCBaseState
    {
        private Vector3 fleePoint;
        private float distanceFromFleePoint;

        private float elapsedTime; // ⏱️ tracks how long we’ve been fleeing

        public NPCFleeState(NPCStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();

           // agent.ResetPath();

            agent.speed = definition.FleeSpeed;

            agent.SetDestination(stateMachine.FleePoint.position);

           // fleePoint = stateMachine.FleePoint.position;

            elapsedTime = 0f; // reset timer when entering state
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            
            agent.isStopped = false;
        
            

            // distanceFromFleePoint = Vector3.Distance(
            //     stateMachine.Agent.transform.position,
            //     fleePoint
            // );

            // ⏱️ keep increasing time
            elapsedTime += deltaTime;
        }

        protected override void ChecksForSwitchingState()
        {
            base.ChecksForSwitchingState();

            // ⏱️ switch after FleeTime duration
            if (elapsedTime >= definition.FleeTime)
            {
                stateMachine.SwitchState(stateMachine.RandomMoveState);
            }
        }
    }
}