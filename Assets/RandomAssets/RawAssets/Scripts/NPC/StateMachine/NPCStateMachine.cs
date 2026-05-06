using UnityEngine;
using AbdulRaheem.Game.Core;
using AbdulRaheem.Game.General;
using AbdulRaheem.Game.Player;
using AbdulRaheem.Game.Weapons;
using UnityEngine.AI;
using AbdulRaheem.Game.Shared;

namespace AbdulRaheem.Game.NPC
{
    public class NPCStateMachine : StateMachine
    {
        public NPCIdleState IdleState { get; private set; }
        public NPCRandomMoveState RandomMoveState { get; private set; }
        public NPCPatrolState PatrolState { get; private set; }
        public NPCChaseState ChaseState { get; private set; }
        public NPCMeleeAttackState MeleeAttackState { get; private set; }
        public NPCRangedAttackState RangedAttackState { get; private set; }
        public NPCFleeState FleeState { get; private set; }
        public NPCFlinchState FlinchState { get; private set; }
        public NPCDeadState DeadState { get; private set; }
        public NPCTryingToFindState TryingToFindState { get; private set; }
        public Transform FleePoint;

        [Header("Allowed States")]
        [SerializeField] private bool canRandomMove;
        [SerializeField] private bool canPatrol;
        [SerializeField] private bool canChase;
        [SerializeField] private bool canMeleeAttack;
        [SerializeField] private bool canRangeAttack;
        [SerializeField] private bool canFlee;
        [SerializeField] public bool canGetAlertOnHearingGunSound; 

        [Header("Modes")]
        [SerializeField] public bool ChaseInZigzag;
        [SerializeField] public bool isAHuntingMonster;
        [SerializeField] public bool isAWeepingAnged;


        #region Getters
        public bool CanRandomMove => canRandomMove;
        public bool CanPatrol => canPatrol;
        public bool CanChase => canChase;
        public bool CanMeleeAttack => canMeleeAttack;
        public bool CanRangeAttack => canRangeAttack;
        public bool CanFlee => canFlee;
        #endregion

        [Header("References")]
        [SerializeField] private NPCDefinition definition;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private AnimationHandler animationHandler;
        [SerializeField] private NPCWeaponController weaponController;
        [SerializeField] private NPCSight sight;
        [SerializeField] private NPCPatrolPath patrolPath;
        [SerializeField] private Health health;

        #region Getters
        public NPCDefinition Defination => definition;
        public NavMeshAgent Agent => agent;
        public NPCWeaponController WeaponController => weaponController;
        public AnimationHandler AnimationHandler => animationHandler;
        public NPCSight Sight => sight;
        public NPCPatrolPath PatrolPath => patrolPath;
        public Health Health => health;
        #endregion

        [Header("Debug")]
        [SerializeField] public string CurrentState;

        #region Setters
        public void setCurrentStateString(string newState) => CurrentState = newState; //  called from PlayerBaseState
        #endregion

        public bool HasHeardSound;
        public bool IsBeingWatched;
        
        protected void Awake()
        {
            // states references
            IdleState = new NPCIdleState(this);
            RandomMoveState = new NPCRandomMoveState(this);
            PatrolState = new NPCPatrolState(this);
            ChaseState = new NPCChaseState(this);
            MeleeAttackState = new NPCMeleeAttackState(this);
            RangedAttackState = new NPCRangedAttackState(this);
            FleeState = new NPCFleeState(this);
            FlinchState = new NPCFlinchState(this);
            DeadState = new NPCDeadState(this);
            TryingToFindState = new NPCTryingToFindState(this);

            // initialize
            if (sight != null) Sight.Initialize(definition);

            if (canRandomMove) canPatrol = false;

            // start from default state
            SwitchState(IdleState);
        }

        protected void Update()
        {
            if (Sight != null) Sight.Tick();
            base.Update();
        }
    }
}
 