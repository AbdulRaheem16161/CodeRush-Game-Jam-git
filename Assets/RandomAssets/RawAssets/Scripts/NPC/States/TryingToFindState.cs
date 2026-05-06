using UnityEngine;

namespace AbdulRaheem.Game.NPC
{
    public class NPCTryingToFindState : NPCBaseState
    {
        private float timer;
        private bool decisionMade;

        public NPCTryingToFindState(NPCStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();

            agent.speed = 0;
            timer = Random.Range(
                definition.MinFindingStateTimer,
                definition.MaxFindingStateTimer
            );

            decisionMade = false;


        }

        public override void Exit()
        {
            base.Exit();    
            stateMachine.HasHeardSound = false;

        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);   
            timer -= deltaTime;



            if (timer <= 0f && !decisionMade)
            {
                MakeDecision();
            }

        }

        protected override void ChecksForSwitchingState()
        {
            base.ChecksForSwitchingState();
            #region Not Used

            // handled inside Tick

            #endregion

            // random movement to chase state
            if (TargetDetected() && stateMachine.CanChase)
            {
                if (stateMachine.isAHuntingMonster)
                {
                    stateMachine.SwitchState(stateMachine.PatrolState);
                }
                else
                {
                    stateMachine.SwitchState(stateMachine.ChaseState);
                }

            }
        }

        private void MakeDecision()
        {

            decisionMade = true;

            float chance = Random.Range(0f, 100f);

            if (chance <= definition.ChanceOfFindingAfterHearingSound)
            {
                // NPC "finds" something → go alert
                stateMachine.Sight.GetAlert();
                stateMachine.SwitchState(stateMachine.RandomMoveState);
            }
            else
            {
                // NPC gives up → goes random movement
                stateMachine.SwitchState(stateMachine.RandomMoveState);
            }

        }
    }
}