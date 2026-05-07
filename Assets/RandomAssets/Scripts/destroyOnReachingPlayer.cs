using UnityEngine;
using System.Collections;

public class destroyOnReachingPlayer : MonoBehaviour
{
    public Transform playerPoint;
    public float destroyDistance = 1f;

    public float destroyDelay = 0.1f; 

    void Awake()
    {
        playerPoint = GameObject.FindWithTag("PlayerPointToTarget").transform;
    }

    void Update()
    {
        if (playerPoint == null) return;

        float distance = Vector3.Distance(transform.position, playerPoint.position);

        if (distance <= destroyDistance)
        {
            StartCoroutine(DestroyAfterDelay()); // Optional: small delay for effects
            
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
    
}
