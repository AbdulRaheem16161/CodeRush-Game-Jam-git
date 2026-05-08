using System.Collections;
using UnityEngine;
using AbdulRaheem.Game.NPC;

public class SightOnOff : MonoBehaviour
{
    [Header("Reference")]
    public NPCSight npcSight;

    [Header("How long sight stays ON")]
    public float minOnTime = 2f;
    public float maxOnTime = 5f;

    [Header("How long sight stays OFF")]
    public float minOffTime = 1f;
    public float maxOffTime = 3f;

    public NPCStateMachine stateMachine;

    private void Start()
    {
        StartCoroutine(SightRoutine());
    }

    IEnumerator SightRoutine()
    {
        while (true)
        {
            
           stateMachine.ForceToRandomMovement = true;

            float onTime = Random.Range(minOnTime, maxOnTime);
            yield return new WaitForSeconds(onTime);

            stateMachine.ForceToRandomMovement = false;

            float offTime = Random.Range(minOffTime, maxOffTime);
            yield return new WaitForSeconds(offTime);
        }
    }
}