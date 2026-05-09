using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSceneOnPlayerDisable : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private float resetDelay = 3f;

    private bool wasActive = true;

    private bool isResetting;

    private void Update()
    {
        if (player == null || isResetting)
            return;

        if (wasActive && !player.activeInHierarchy)
        {
            StartCoroutine(ResetSceneAfterDelay());
        }

        wasActive = player.activeInHierarchy;
    }

    private IEnumerator ResetSceneAfterDelay()
    {
        isResetting = true;

        yield return new WaitForSeconds(resetDelay);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
    }
}