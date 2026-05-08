using UnityEngine;
using UnityEngine.InputSystem;

namespace AbdulRaheem.Game.Player
{
    public class ThirdPersonPlayerController : MonoBehaviour, IPlayerController
    {
        [Header("References")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform tpCameraTransform;

        [Space(10)]

        [Header("Settings")]
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float speedSmoothness = 0.2f;
        [SerializeField] private float gravity = 9.8f;

        [Space(10)]

        [Header("Live Values")]
        [SerializeField] private float currentSpeed;
        public float CurrentSpeed => currentSpeed;

        [Space(10)]

        [Header("Runtime Private Variables")]
        private Vector3 smoothHorizontalVelocity;
        private Vector3 velocitySmoothRef;
        private Vector3 verticalVelocity;

        #region Getters
        public Vector3 VerticalVelocity => verticalVelocity;
        #endregion

        [Space(10)]

        [Header("References")]
        [SerializeField] private const string IS_GROUNDED = "isGrounded";
        [SerializeField] private const string VELOCITY = "velocity";

        private void Awake()
        {
            tpCameraTransform = Camera.main.transform;
        }

        public void Move(Vector3 movementInputVector3, float speed) // Called from states such as moveState
        {
            Vector3 camForward = tpCameraTransform.forward;
            Vector3 camRight = tpCameraTransform.right;

            camRight.y = 0f;
            camForward.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = camForward * movementInputVector3.z + camRight * movementInputVector3.x;
            moveDirection.Normalize();

            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Applying Gravity
            if (characterController.isGrounded && verticalVelocity.y <= 0f) verticalVelocity.y = -2f;
            else verticalVelocity.y -= gravity * Time.deltaTime;

            // separating horizontal (movement) and Vertical (gravity & jump) Vectors so that moveSpeed dont effect the vertical motion
            Vector3 horizontal = moveDirection * speed;
            Vector3 vertical = new Vector3(0, verticalVelocity.y, 0);

            // smooth transition of velocity
            smoothHorizontalVelocity = Vector3.SmoothDamp(
                smoothHorizontalVelocity,
                horizontal,
                ref velocitySmoothRef,
                speedSmoothness
            );
            characterController.Move((smoothHorizontalVelocity + vertical) * Time.deltaTime);

            Vector3 horizontalSpeed = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
            currentSpeed = horizontalSpeed.magnitude; // ACTUAL CALCULATED VELOCITY
        }

        public void Look(Vector2 mouseDelta) { } // rotation handled by the Cinemachine in Third Person

        //public void RotateTowardsCursor()
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        //    if (Physics.Raycast(ray, out RaycastHit hit))
        //    {
        //        Vector3 lookTarget = hit.point;
        //        lookTarget.y = transform.position.y;

        //        transform.LookAt(lookTarget);
        //        transform.Rotate(0, 180f, 0);

        //        Vector3 direction = (lookTarget - transform.position);
        //        if (direction.magnitude < 0.1f) return;

        //        Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180f, 0);
        //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //    }
        //}

        public void Jump(float jumpHeight)  // Called from jumpState
        {
            if (characterController.isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
            }
        }

        // Getter functions
        public bool IsGrounded()
        {
            return characterController.isGrounded;
        }
    }
}
