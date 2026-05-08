using UnityEngine;
using AbdulRaheem.Game.Player;

namespace AbdulRaheem.Game.Player
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();

            // cache
            activeController = stateMachine.ActiveController; 

            // animation
            animationHandler.TriggerJumpAnim();  

            // functionality
            activeController.Jump(stateMachine.Definition.JumpHeight);
            stateMachine.JumpTriggered = false;
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
            // Jump to Idle
            if ((activeController.IsGrounded()) && (activeController.VerticalVelocity.y < 0f))
            {
                stateMachine.SwitchState(stateMachine.IdleState);
            }
        }
    }

}

