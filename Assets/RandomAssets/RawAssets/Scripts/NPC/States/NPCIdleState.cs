using UnityEngine;
using AbdulRaheem.Game.NPC;

namespace AbdulRaheem.Game.NPC
{
    public class NPCIdleState : NPCBaseState
    {
        public NPCIdleState(NPCStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();

            agent.speed = 0f;
            agent.ResetPath();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Tick(float deltaTime)  
        {
            base.Tick(deltaTime);
        }

        protected override void ChecksForSwitchingState()
        {
            base.ChecksForSwitchingState();

            // Idle to Random Movement
            if (stateMachine.CanRandomMove)
            {
                stateMachine.SwitchState(stateMachine.RandomMoveState);
            }

            // Idle to Patrol Movement
            if (stateMachine.CanPatrol)
            {
                stateMachine.SwitchState(stateMachine.PatrolState);
            }

            // Idle to Chase
            // Idle to Attack
            // Idle to Flee
        }
    }
}

