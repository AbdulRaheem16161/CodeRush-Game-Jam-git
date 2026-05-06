using UnityEngine;
using AbdulRaheem.Game.NPC;

namespace AbdulRaheem.Game.NPC
{
    public class NPCDeadState : NPCBaseState
    {
        public NPCDeadState(NPCStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            animationHandler.TriggerDeathAnim();
        }

        public override void Exit()
        {
        }

        public override void Tick(float deltaTime)
        {
        }

        protected override void ChecksForSwitchingState()
        {

        }
    }
}

