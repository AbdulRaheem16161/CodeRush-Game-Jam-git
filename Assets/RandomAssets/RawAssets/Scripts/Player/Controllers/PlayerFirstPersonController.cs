using UnityEngine;

namespace AbdulRaheem.Game.Player
{
    public class FirstPersonPlayerController : MonoBehaviour, IPlayerController
    {
        [Header("References")]
        [SerializeField] private CharacterController characterController;
        private Transform cameraTransform;

        [Header("Settings")]
        [SerializeField] private float mouseSensitivity = 2f;
        [SerializeField] private float gravity = 9.8f;
        [SerializeField] private float speedSmoothness = 0.2f;

        [Header("Live Values")]
        [SerializeField] private float currentSpeed;

        [Header("Runtime")]
        private Vector3 smoothHorizontalVelocity;
        private Vector3 velocitySmoothRef;
        private Vector3 verticalVelocity;

        #region IPlayerController
        public float CurrentSpeed => currentSpeed;
        public Vector3 VerticalVelocity => verticalVelocity;
        #endregion

       

        private void Awake()
        {
            cameraTransform = Camera.main.transform;
        }

        public void Move(Vector3 input, float speed)
        {
            // get camera forward/right but flatten Y so no vertical drift
            Transform cam = Camera.main.transform;

            Vector3 camForward = cam.forward;
            Vector3 camRight = cam.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = camForward * input.z + camRight * input.x;
            moveDirection.Normalize();

            if (characterController.isGrounded && verticalVelocity.y <= 0f)
                verticalVelocity.y = -2f;
            else
                verticalVelocity.y -= gravity * Time.deltaTime;

            Vector3 horizontal = moveDirection * speed;
            Vector3 vertical = new Vector3(0, verticalVelocity.y, 0);

            smoothHorizontalVelocity = Vector3.SmoothDamp(
                smoothHorizontalVelocity,
                horizontal,
                ref velocitySmoothRef,
                speedSmoothness
            );

            characterController.Move((smoothHorizontalVelocity + vertical) * Time.deltaTime);

            Vector3 horizontalSpeed = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
            currentSpeed = horizontalSpeed.magnitude;
        }

        public void Look(Vector2 mouseDelta)
        {
            //// Cinemachine POV handles vertical
            //// We only rotate the player body horizontally
            //float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
            //transform.Rotate(Vector3.up * mouseX);
        }

        public void Jump(float jumpHeight)
        {
            if (characterController.isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
            }
        }

        public bool IsGrounded()
        {
            return characterController.isGrounded;
        }
    }
}