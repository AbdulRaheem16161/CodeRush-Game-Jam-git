using UnityEngine;

namespace AbdulRaheem.Game.Player
{
    [CreateAssetMenu(menuName = "Player/PlayerDefination")]
    public class PlayerDefinition : ScriptableObject
    {
        public float RunSpeed => runSpeed;
        public float WalkSpeed => walkSpeed;
        public float JumpHeight => jumpHeight; 

        [SerializeField] private float runSpeed;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float jumpHeight;
    }
}
