using UnityEngine;

public class TriggerRotation : MonoBehaviour
{
    public SmoothRotateToTarget smoothRotateToTarget;
    public Vector3 targetRotation = new Vector3(0f, 0f, 0f);  

    public string name;

    private void OntriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name + " on trigger: " + name);
        
        if (other.CompareTag("Player"))
        {
           smoothRotateToTarget.targetRotation = targetRotation;  
        }
    }
}