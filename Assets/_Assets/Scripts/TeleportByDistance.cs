using UnityEngine;

public class TeleportByDistance : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform teleportPoint;

    [Header("Settings")]
    [SerializeField] private float teleportDistance = 1f;

    [SerializeField] private float distance;

    private bool hasTeleported = false;

    void Start()
    {
        distance = 200f;
    }

    private void Update()
    {
      //  if (hasTeleported)
          //  return;

        distance = Vector3.Distance(transform.position, player.position);

        if (distance <= teleportDistance)
        {
            CharacterController cc = player.GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false;
            }

            player.SetPositionAndRotation(
                teleportPoint.position,
                teleportPoint.rotation
            );

            if (cc != null)
            {
                cc.enabled = true;
            }

            hasTeleported = true;
        }
    }
}