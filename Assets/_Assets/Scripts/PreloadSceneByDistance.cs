using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PreloadSceneByDistance : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string sceneName;

    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Settings")]
    [SerializeField] private float activationDistance = 3f;

    [SerializeField] private float distance;

    private AsyncOperation preloadOperation;

    private bool isReady = false;
    private bool hasActivated = false;

    private void Start()
    {
        StartCoroutine(PreloadScene());

        distance = 200f;
    }

    private void Update()
    {
        if (!isReady || hasActivated)
            return;

        distance = Vector3.Distance(transform.position, player.position);

        if (distance <= activationDistance)
        {
            hasActivated = true;

            preloadOperation.allowSceneActivation = true;
        }
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
}