using UnityEngine;
using ArcadeVP;

public class CarExitTrigger : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCam;
    public ArcadeVehicleController arcadeVehicleController;

    public Transform playerExitPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Car"))
            return;

        player.transform.position = playerExitPoint.position;
        player.SetActive(true);
        playerCam.SetActive(true);
        arcadeVehicleController.enabled = false;

        this.gameObject.SetActive(false);   
    }
}
