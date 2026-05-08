using UnityEngine;

public class teleport : MonoBehaviour
{
    public Transform teleportPoint;
    public Transform player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.position = teleportPoint.position;
        }
    }
}
