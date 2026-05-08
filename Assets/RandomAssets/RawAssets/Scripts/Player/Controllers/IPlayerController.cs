using UnityEngine;

namespace AbdulRaheem.Game.Player
{
    public interface IPlayerController
    {
        float CurrentSpeed { get; }
        Vector3 VerticalVelocity { get; }

        void Move(Vector3 input, float speed);
        void Jump(float jumpHeight);
        bool IsGrounded();

        void Look(Vector2 mouseDelta);
    }
}