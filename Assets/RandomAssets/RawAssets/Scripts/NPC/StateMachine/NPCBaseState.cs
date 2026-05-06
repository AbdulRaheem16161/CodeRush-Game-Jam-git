using AbdulRaheem.Game.Player;
using AbdulRaheem.Game.Shared;
using AbdulRaheem.Game.Weapons;
using UnityEngine;
using UnityEngine.AI;
using Core = AbdulRaheem.Game.Core;

namespace AbdulRaheem.Game.NPC
{
    public abstract class NPCBaseState : Core.State
    {
        protected NPCStateMachine stateMachine;
        protected NPCBaseState currentState;
        protected NPCWeaponController weaponController;
        protected NPCDefinition definition;
        protected AnimationHandler animationHandler;
        protected NavMeshAgent agent;
        protected NPCSight sight;
        protected Health health;

        private bool gotHit;
        private bool died;

        public NPCBaseState(NPCStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
            weaponController = stateMachine.WeaponController;
            definition = stateMachine.Defination;
            animationHandler = stateMachine.AnimationHandler;
            agent = stateMachine.Agent;
            sight = stateMachine.Sight;
            health = stateMachine.Health;

            health.OnDamage += GotHit;
            health.OnDeath += Died;
        }

        public override void Enter()
        {
            stateMachine.setCurrentStateString(this.GetType().Name);
            currentState = this;
        }

        public override void Exit()
        {
        }

        public override void Tick(float deltaTime)
        {
            ChecksForSwitchingState();

            // animation
            float currentSpeed = agent.velocity.magnitude;
            animationHandler.SetSpeed(currentSpeed);

            Debug.Log(currentState.GetType().Name);
        }

        protected virtual void ChecksForSwitchingState()
        {
            if (died)
            {
                stateMachine.SwitchState(stateMachine.DeadState);
            }

            if (gotHit && currentState != stateMachine.DeadState)
            {
                gotHit = false;

                if (currentState != stateMachine.FlinchState) 
                     stateMachine.FlinchState.PreviousState = currentState;

                stateMachine.SwitchState(stateMachine.FlinchState);
            }

            
        }

        protected bool TargetDetected()
        {
            return (stateMachine.CanChase) && (stateMachine.Sight.TargetDetected);
        }

        private void GotHit(float damage)
        {
            gotHit = true;
        }

        private void Died()
        {
            died = true;
        }


    }
}
