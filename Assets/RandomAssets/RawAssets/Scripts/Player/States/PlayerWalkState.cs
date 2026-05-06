using UnityEngine;
using AbdulRaheem.Game.Player;

namespace AbdulRaheem.Game.Player
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }
        
        // stats
        private float moveSpeed;

        public override void Enter()
        {
            base.Enter();
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
            // Move to Idle
            if (stateMachine.MovementVector3 == Vector3.zero)
            {
                stateMachine.SwitchState(stateMachine.IdleState);
            }

            // Move to Jump
            if (stateMachine.JumpTriggered)
            {
                if (!stateMachine.CanJump)
                {
                    stateMachine.JumpTriggered = false;
                    return;
                }
                stateMachine.SwitchState(stateMachine.JumpState);
            }

            // Move to Attack
            if (stateMachine.AttackTriggered)
            {
                if (!stateMachine.CanAttack)
                {
                    stateMachine.AttackTriggered = false;
                    return;
                }
                stateMachine.SwitchState(stateMachine.AttackState);
            }
        }
    }
}
