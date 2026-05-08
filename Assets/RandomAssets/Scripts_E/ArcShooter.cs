using UnityEngine;

public class GuardArcShooter : MonoBehaviour
{
    [Header("Setup")]
    public Transform firePoint;
    public Transform player;
    public GameObject projectilePrefab;

    [Header("Shooting")]
    public float fireRate = 1.5f; // seconds between shots
    public float projectileSpeed = 25f;

    private float fireTimer;

    // void Update()
    // {
    //     if (player == null) return;

    //     fireTimer -= Time.deltaTime;

    //     if (fireTimer <= 0f)
    //     {
    //         Shoot();
    //         fireTimer = fireRate;
    //     }
    // }

    public void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = proj.GetComponent<Rigidbody>();

        Vector3 velocity = CalculateBallisticVelocity(firePoint.position, player.position, projectileSpeed);

       rb.linearVelocity = velocity; // Unity 6 (use rb.velocity if older Unity)
    }

    Vector3 CalculateBallisticVelocity(Vector3 origin, Vector3 target, float speed)
    {
        Vector3 toTarget = target - origin;

        Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
        float xzDistance = toTargetXZ.magnitude;

        float yOffset = toTarget.y;

        float g = Mathf.Abs(Physics.gravity.y);
        float speedSquared = speed * speed;

        float discriminant =
            speedSquared * speedSquared -
            g * (g * xzDistance * xzDistance + 2f * yOffset * speedSquared);

        if (discriminant < 0f)
        {
            // Target out of range → just brute force it (no physics respect)
            return toTarget.normalized * speed;
        }

        float sqrt = Mathf.Sqrt(discriminant);

        float lowAngle = Mathf.Atan((speedSquared - sqrt) / (g * xzDistance));
        float highAngle = Mathf.Atan((speedSquared + sqrt) / (g * xzDistance));

        float angle = lowAngle; // low arc = more “gun-like” and less cartoon artillery

        Vector3 dirXZ = toTargetXZ.normalized;

        Vector3 velocity =
            dirXZ * speed * Mathf.Cos(angle) +
            Vector3.up * speed * Mathf.Sin(angle);

        return velocity;
    }
}