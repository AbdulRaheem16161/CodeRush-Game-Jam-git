using UnityEngine;
using AbdulRaheem.Game.Player;

namespace AbdulRaheem.Game.Player
{
    public class PlayerDefaultState : PlayerBaseState
    {
        public PlayerDefaultState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        // private Runtime Veriables

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

            // Default to Jump
            if (stateMachine.JumpTriggered)
            {
                if (!stateMachine.CanJump)
                {
                    stateMachine.JumpTriggered = false;
                    return;
                }
                stateMachine.SwitchState(stateMachine.JumpState);
            }

            // Default to Attack
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
