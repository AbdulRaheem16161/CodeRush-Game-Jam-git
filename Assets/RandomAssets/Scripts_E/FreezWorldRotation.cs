using UnityEngine;

public class FreezWorldRotation : MonoBehaviour
{
    [Header("Freeze Axes")]
    public bool freezeX = true;
    public bool freezeY = true;
    public bool freezeZ = true;

    private Vector3 initialEuler;

    void Start()
    {
        // Store starting rotation as reference
        initialEuler = transform.eulerAngles;
    }

    void LateUpdate()
    {
        Vector3 currentEuler = transform.eulerAngles;

        // Freeze selected axes by restoring original values
        if (freezeX)
            currentEuler.x = initialEuler.x;

        if (freezeY)
            currentEuler.y = initialEuler.y;

        if (freezeZ)
            currentEuler.z = initialEuler.z;

        transform.rotation = Quaternion.Euler(currentEuler);
    }
}