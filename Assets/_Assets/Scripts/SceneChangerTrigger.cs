using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PreloadSceneTrigger : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string sceneName;

    private AsyncOperation preloadOperation;

    private bool isReady = false;

    private void Start()
    {
        StartCoroutine(PreloadScene());
    }

    private IEnumerator PreloadScene()
    {
        preloadOperation = SceneManager.LoadSceneAsync(sceneName);

        preloadOperation.allowSceneActivation = false;

        while (preloadOperation.progress < 0.9f)
        {
            yield return null;
        }

        isReady = true;

        Debug.Log("Scene preloaded!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (isReady)
        {
            preloadOperation.allowSceneActivation = true;
        }
    }
}