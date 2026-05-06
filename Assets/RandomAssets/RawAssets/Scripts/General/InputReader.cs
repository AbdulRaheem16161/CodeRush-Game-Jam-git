using System;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AbdulRaheem.Game.General
{
    public class InputReader : MonoBehaviour, PlayerInputAction.IPlayerActions
    {
        private Vector2 movementInputVector2;
        private Vector2 mouseDelta;
        private PlayerInputAction action;

        public event Action JumpAction;
        public event Action AttackAction;
        public event Action PauseAction;
        public event Action CameraToggleAction;

        [field : SerializeField] public bool WalkKey { get; private set; } // this is used change movement speed to walk speed  

        // these are just fot Debugging
        [SerializeField] private bool movementKeys;
        [SerializeField] private bool mouseMovement;
        [SerializeField] private bool jumpKey;
        [SerializeField] private bool attackKey;
        [SerializeField] private bool pauseKey;
        [SerializeField] private bool cameraToggleKey;

        private void Awake()
        {
            action = new PlayerInputAction();
            action.Player.SetCallbacks(this);
        }

        private void OnEnable()
        {
            action?.Player.Enable();
        }

        private void OnDisable()
        {
            action?.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context) // ASDW
        {
            movementInputVector2 = context.ReadValue<Vector2>();

            if (context.performed) movementKeys = true;
            else if (context.canceled) movementKeys = false;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            mouseDelta = context.ReadValue<Vector2>();

            if (context.performed) mouseMovement = true;
            else if (context.canceled) mouseMovement = false;
        }

        public void OnSprint(InputAction.CallbackContext context) // shift
        {
            if (context.performed)
            {
                WalkKey = true;
            }
            else if (context.canceled) WalkKey = false;
        }

        public void OnJump(InputAction.CallbackContext context) // space
        {
            if (context.performed)
            {
                jumpKey = true;
                JumpAction?.Invoke();
            }
            else if (context.canceled) jumpKey = false;
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                attackKey = true;
                AttackAction?.Invoke();
            }
            else if (context.canceled) attackKey = false;
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                pauseKey = true;
                PauseAction?.Invoke();
            }
            else if (context.canceled) pauseKey = false;
        }

        public void OnCameraToggle(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                cameraToggleKey = true;
                CameraToggleAction?.Invoke();
            }
            else if (context.canceled) cameraToggleKey = false;
        }

        // Getters:
        public Vector3 GetMovementInputVector3()
        {
            Vector3 movementVector3 = new Vector3(movementInputVector2.x, 0, movementInputVector2.y);
            return movementVector3.normalized;
        }

        public Vector2 MouseDelta => mouseDelta;

    }
}