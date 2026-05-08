using UnityEngine;

public class StickToParent : MonoBehaviour
{
    [SerializeField] private GameObject objectToStick;

    // Update is called once per frame
    void Update()
    {
        objectToStick.transform.position = transform.position;
    }
}
