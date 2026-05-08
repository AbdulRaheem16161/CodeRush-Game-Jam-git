using System.ComponentModel;
using UnityEngine;
using AbdulRaheem.Game.Core;
using AbdulRaheem.Game.General;
using AbdulRaheem.Game.Weapons;
using AbdulRaheem.Game.Shared;

namespace AbdulRaheem.Game.Player
{
    public class PlayerStateMachine : StateMachine
    {
        public PlayerDefaultState IdleState { get; private set; }
        public PlayerMoveState moveState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerAttackState AttackState { get; private set; }

        [Header("Allowed States")]
        [SerializeField] private bool canMove;
        [SerializeField] private bool canJump;
        [SerializeField] private bool canAttack;

        #region Getters;
        public bool CanMove => canMove;
        public bool CanJump => canJump;
        public bool CanAttack => canAttack;
        #endregion

        [Header("References")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private PlayerDefinition definition;
        [SerializeField] private AnimationHandler animationHandler;
        [SerializeField] private PlayerWeaponController weaponController;
        [SerializeField] private PlayerControllerModeHandler controllerModeHandler;

        #region Getters
        public IPlayerController ActiveController => controllerModeHandler.ActiveController;
        public AnimationHandler AnimationHandler => animationHandler;
        public PlayerWeaponController WeaponController => weaponController;
        public PlayerDefinition Definition => definition;
        #endregion

        [Header("Input Intent Flags")]
        public bool JumpTriggered;
        public bool AttackTriggered;
        public bool CameraTogglePressed;
        public bool WalkPressed => inputReader.WalkKey; // just for debugging

        [Header("Movement & Mouse Input")]
        public Vector3 MovementVector3 => inputReader.GetMovementInputVector3();
        public Vector3 MouseDelta => inputReader.MouseDelta;

        [Header("Debug")]
        [SerializeField] private string currentState;

        #region Setters
        public void setCurrentStateString(string newState) => currentState = newState; //  called from PlayerBaseState
        #endregion

        private void OnEnable()
        {
            inputReader.JumpAction += OnJumpPressed;
            inputReader.AttackAction += OnAttackPressed;
        }

        private void OnDisable()
        {
            inputReader.JumpAction -= OnJumpPressed;
            inputReader.AttackAction -= OnAttackPressed;
        }

        protected void Awake()
        {
            IdleState = new PlayerDefaultState(this);
            moveState = new PlayerMoveState(this);
            JumpState = new PlayerJumpState(this);
            AttackState = new PlayerAttackState(this);

            SwitchState(IdleState);
        }

        protected void Update()
        {
            base.Update();
        }

        private void OnJumpPressed()
        {
            if (!ActiveController.IsGrounded()) return;

            JumpTriggered = true;
        }

        private void OnAttackPressed()
        {
            AttackTriggered = true;
        }
    }
}
