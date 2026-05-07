using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    [Header("Falling")]
    public float waitBeforeFalling = 1f;
    public float waitBeforeResetting = 3f;

    [Header("Shake")]
    public float shakeIntensity = 0.05f;
    public float shakeDuration = 0.5f;
    public float shakeSpeed = 20f;
    public float shakeSmoothness = 8f;

    [Header("Reset")]
    public float resetSpeed = 2f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Rigidbody rb;

    private bool isFalling;

    public float extraGravity = 25f;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    private void Update()
    {
        // Testing key
        if (Input.GetKeyDown(KeyCode.R))
        {
            TryActivate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TryActivate();
        }
    }

    private void TryActivate()
    {
        if (isFalling)
            return;

        isFalling = true;

        StartCoroutine(ShakePlatform());
        StartCoroutine(FallAndReset());
    }

    private IEnumerator FallAndReset()
    {
        // Wait before falling
        yield return new WaitForSeconds(waitBeforeFalling);

        // Enable physics
        rb.isKinematic = false;

        // Wait before resetting
        yield return new WaitForSeconds(waitBeforeResetting);

        // Stop physics
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Smooth reset
        yield return StartCoroutine(SmoothReset());

        isFalling = false;
    }

    private IEnumerator ShakePlatform()
    {
        Vector3 originalPosition = transform.position;

        float elapsed = 0f;
        float currentAmount = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            // Smoothly blend shake intensity
            currentAmount = Mathf.Lerp(
                currentAmount,
                shakeIntensity,
                Time.deltaTime * shakeSmoothness
            );

            // Smooth wave movement
            float x = Mathf.Sin(elapsed * shakeSpeed) * currentAmount;
            float y = Mathf.Cos(elapsed * shakeSpeed * 0.8f) * currentAmount;

            transform.position = originalPosition + new Vector3(x, y, 0);

            yield return null;
        }

        // Smoothly return to original position
        float returnElapsed = 0f;
        Vector3 currentPosition = transform.position;

        while (returnElapsed < 0.15f)
        {
            returnElapsed += Time.deltaTime;

            transform.position = Vector3.Lerp(
                currentPosition,
                originalPosition,
                returnElapsed / 0.15f
            );

            yield return null;
        }

        transform.position = originalPosition;
    }

    private IEnumerator SmoothReset()
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * resetSpeed;

            transform.position = Vector3.Lerp(
                startPosition,
                initialPosition,
                elapsed
            );

            transform.rotation = Quaternion.Slerp(
                startRotation,
                initialRotation,
                elapsed
            );

            yield return null;
        }

        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    private void FixedUpdate()
{
    if (!rb.isKinematic)
    {
        rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
    }
}
}