using UnityEngine;

namespace AbdulRaheem.Game.NPC
{
    public class NPCFlinchState : NPCBaseState
    {
        public NPCFlinchState(NPCStateMachine stateMachine) : base(stateMachine) { }

        public NPCBaseState PreviousState;
        private float flinchTimeStamp;

        public override void Enter()
        {
            base.Enter();

            agent.speed = definition.FlinchSpeed;
            flinchTimeStamp = Time.time;
            animationHandler.TriggerGetHitAnim();
        }

        public override void Exit()
        {
            base.Exit();

            animationHandler.ResetHitAnimTrigger();
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
        }

        protected override void ChecksForSwitchingState()
        {
            base.ChecksForSwitchingState();

            if (stateMachine.isAWeepingAnged)
            {
                stateMachine.SwitchState(stateMachine.FleeState);
            }

            if (Time.time >= flinchTimeStamp + definition.FlinchDuration && !stateMachine.isAWeepingAnged)
            {
                stateMachine.SwitchState(PreviousState);
            }
            
        }
    }
}