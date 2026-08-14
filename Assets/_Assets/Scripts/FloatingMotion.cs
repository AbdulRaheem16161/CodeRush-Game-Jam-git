using UnityEngine;

public class FloatingMotion : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatHeight = 0.5f;
    [SerializeField] private float rotationSpeed = 30f;
    
    private Vector3 startPosition;
    private Vector3 rotationAxis;
    private float rotationOffset;

    private void Start()
    {
        startPosition = transform.position;
        
        // Randomize rotation direction for organic feel
        int randomDirection = Random.Range(0, 3);
        rotationAxis = randomDirection switch
        {
            0 => Vector3.up,           // Vertical spin
            1 => Vector3.right,        // Horizontal (X-axis)
            _ => Vector3.forward       // Horizontal (Z-axis)
        };
        
        // Random rotation offset for variation
        rotationOffset = Random.Range(0f, 360f);
    }

    private void Update()
    {
        // Organic floating motion
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        
        // Subtle horizontal drift for more organic feel
        float driftX = Mathf.Sin(Time.time * floatSpeed * 0.5f) * (floatHeight * 0.3f);
        float driftZ = Mathf.Cos(Time.time * floatSpeed * 0.3f) * (floatHeight * 0.2f);
        
        transform.position = new Vector3(
            startPosition.x + driftX,
            newY,
            startPosition.z + driftZ
        );
        
        // Organic spinning with slight wobble
        float wobble = Mathf.Sin(Time.time * floatSpeed * 0.7f) * 0.3f;
        transform.rotation = Quaternion.AngleAxis(
            Time.time * rotationSpeed * (1f + wobble) + rotationOffset,
            rotationAxis
        );
    }
}