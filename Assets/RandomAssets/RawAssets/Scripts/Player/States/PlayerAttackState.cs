using UnityEngine;
using AbdulRaheem.Game.Player;
using System;

namespace AbdulRaheem.Game.Player
{
    public class PlayerAttackState : PlayerBaseState
    {
        public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();

            stateMachine.WeaponController.TryAttacking();
            stateMachine.AttackTriggered = false;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            // stateMachine.PlayerThirdPersonController.RotateTowardsCursor();
        }

        protected override void ChecksForSwitchingState()
        {
            // Attack to Idle
            if (!stateMachine.AttackTriggered)
            {
               stateMachine.SwitchState(stateMachine.IdleState);
            }
        }
    }
}
