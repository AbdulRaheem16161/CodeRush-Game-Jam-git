using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float rotationSpeed = 5f;

    void Update()
    {
        if (player == null) return;

        // Direction from this object to player
        Vector3 direction = player.position - transform.position;

        // Optional: Ignore vertical rotation
        direction.y = 0f;

        // Prevent weird Quaternion error when too close
        if (direction == Vector3.zero) return;

        // Create target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smooth rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}