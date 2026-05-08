using AbdulRaheem.Game.NPC;
using UnityEngine;
using UnityEngine.AI;

public class ReactWhenBeingWatched : MonoBehaviour
{
    [SerializeField] private NPCStateMachine stateMachine;
    [SerializeField] private NavMeshAgent Agent;

    public bool IsBeingWatched;

    [Header("what to do when is being Watched?")]
    [SerializeField] private bool getFreezed;
    public bool GetFreezed
    {
        get => getFreezed;
        set
        {
            getFreezed = value;
            WanderAround = !getFreezed;
        }
    }

    [SerializeField] private bool wanderAround;
    public bool WanderAround
    {
        get => wanderAround;
        set
        {
            wanderAround = value;
            getFreezed = !wanderAround;
        }
    }

    private void Update()
    {
        if (getFreezed)
        {
        if (stateMachine.CurrentState != "NPCFleeState")
            Debug.Log("its not in FleeState there for setting agent.isFreez = true");
            Agent.isStopped = IsBeingWatched;
        }
        // else{
        //      Agent.isStopped = false;
        // }

        if (wanderAround)
        {
            stateMachine.IsBeingWatched = IsBeingWatched;
        }
    }
}
