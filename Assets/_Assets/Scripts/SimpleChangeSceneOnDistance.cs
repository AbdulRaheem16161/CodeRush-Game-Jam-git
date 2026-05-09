using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnDistance : MonoBehaviour
{
    public Transform player;
    public float triggerDistance = 5f;
    public string sceneName;
    public float distance;
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.position);

        if (distance < triggerDistance)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}