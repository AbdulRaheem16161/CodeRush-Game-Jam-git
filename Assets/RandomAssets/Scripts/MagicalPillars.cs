using UnityEngine;

public class MagicalPillars : MonoBehaviour
{

    public Vector3 LiftedPosition;
    public Vector3 DefaultPosition;
    public Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3f;

    public Transform player;
    public float distanceFromPlayer;

    public float activationDistance = 5f;

    public Transform pointToCaculateDistanceFrom;   

    void Awake()
    {
        LiftedPosition = transform.position;
        DefaultPosition = transform.position - new Vector3(0, 40f, 0);
        transform.position = DefaultPosition;
    }

    void Update()
    {
        distanceFromPlayer = Vector3.Distance(player.position, pointToCaculateDistanceFrom.transform.position);

        Vector3 targetPosition;

        if (distanceFromPlayer < activationDistance)
        {
            targetPosition = LiftedPosition;
        }
        else
        {
            targetPosition = DefaultPosition;
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }



}
