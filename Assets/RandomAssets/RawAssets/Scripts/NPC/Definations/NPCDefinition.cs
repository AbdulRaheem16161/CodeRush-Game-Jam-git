using UnityEngine;

namespace AbdulRaheem.Game.Player
{
    [CreateAssetMenu(menuName = "NPC/NPCDefination")]
    public class NPCDefinition : ScriptableObject
    {
        [Header("Movement")]

        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public float ChaseSpeed { get; private set; }
        [field: SerializeField] public float FleeSpeed { get; private set; }
        [field: SerializeField] public float MeleeAttackMoveSpeed { get; private set; }
        [field: SerializeField] public float RangedAttackMoveSpeed { get; private set; }
        
        [Header("Patrol / Random Movement")]
        [field: SerializeField] public float WaitTimeBetweenPatrolPoints { get; private set; }
        [field: SerializeField] public float WaitTimeBetweenRandomPoints { get; private set; }
        [field: SerializeField] public float RandomMoveRadius { get; private set; }
       
        [Header("Sight")]
        [field: SerializeField] public float SightAngle { get; private set; }
        [field: SerializeField] public float SightRange { get; private set; }
        [field: SerializeField] public float LostPlayerTimeOut { get; private set; }
        
        [Header("Combat Reaction")]
        [field: SerializeField] public float FlinchDuration { get; private set; }
        [field: SerializeField] public float FlinchSpeed { get; private set; }
        
        [Header("Finding State")]
        [field: SerializeField] public float MinFindingStateTimer { get; private set; }
        [field: SerializeField] public float MaxFindingStateTimer { get; private set; }
        [field: SerializeField] public float ChanceOfFindingAfterHearingSound { get; private set; }

        [Header("FleeState")]
        [field: SerializeField] public float FleeTime { get; private set; }
}
}