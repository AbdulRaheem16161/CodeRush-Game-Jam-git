using UnityEngine;
using ArcadeVP;

public class CarEnterTrigger : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCam;
    public ArcadeVehicleController arcadeVehicleController;

    public void Start()
    {
        arcadeVehicleController.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        player.SetActive(false);
        playerCam.SetActive(false);
        arcadeVehicleController.enabled = true;

        this.gameObject.SetActive(false);   
    }
}
