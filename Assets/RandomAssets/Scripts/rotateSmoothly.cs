using UnityEngine;

public class SmoothRotateToTarget : MonoBehaviour
{
    [Header("Target Rotation (Euler Angles)")]
    public Vector3 targetRotation;

    [Header("Settings")]
    public float rotationSpeed = 5f;

    private bool triggered = false;

    private void Update()
    {
        RotateTowardsTarget();
    }

    void RotateTowardsTarget()
    {
        // Convert target Euler angles into a Quaternion
        Quaternion targetRot = Quaternion.Euler(targetRotation);

        // Smoothly rotate towards it
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );
    }
}