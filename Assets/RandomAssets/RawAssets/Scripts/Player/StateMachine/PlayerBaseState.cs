using AbdulRaheem.Game.Shared;
using UnityEngine;
using Core = AbdulRaheem.Game.Core;

namespace AbdulRaheem.Game.Player
{
    public abstract class PlayerBaseState : Core.State
    {
        protected PlayerStateMachine stateMachine;
        protected AnimationHandler animationHandler; // cashe it cuz its will used in all the states

        protected IPlayerController activeController; // cache
        protected PlayerDefinition defination; // cache


        private float moveSpeed;


        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
            animationHandler = stateMachine.AnimationHandler;
            defination = stateMachine.Definition;
        }

        public override void Enter()
        {
            stateMachine.setCurrentStateString(this.GetType().Name);
        }

        public override void Exit()
        {
        }

        public override void Tick(float deltaTime)
        {
            ChecksForSwitchingState();

            activeController = stateMachine?.ActiveController; // cache it in the Tick() is it stays uptodate

            moveSpeed = stateMachine.WalkPressed ? defination.WalkSpeed : defination.RunSpeed;
            activeController.Move(stateMachine.MovementVector3, moveSpeed);

            activeController.Look(stateMachine.MouseDelta);

            // Animations
            animationHandler.SetSpeed(activeController.CurrentSpeed);
            animationHandler.SetGrounded(activeController.IsGrounded());
        }

        protected abstract void ChecksForSwitchingState();


    }
}
