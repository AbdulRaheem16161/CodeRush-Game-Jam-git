using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSceneOnPlayerDisable : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private float resetDelay = 3f;

    [Header("Hold R To Reset")]
    [SerializeField] private KeyCode resetKey = KeyCode.R;
    [SerializeField] private float holdTimeToReset = 3f;

    private bool wasActive = true;
    private bool isResetting;

    private float resetKeyHoldTimer;

    private void Update()
    {
        if (isResetting)
            return;

        CheckPlayerDisable();
        CheckResetKey();
    }

    private void CheckPlayerDisable()
    {
        if (player == null)
            return;

        if (wasActive && !player.activeInHierarchy)
        {
            StartCoroutine(ResetSceneAfterDelay());
        }

        wasActive = player.activeInHierarchy;
    }

    private void CheckResetKey()
    {
        if (Input.GetKey(resetKey))
        {
            resetKeyHoldTimer += Time.deltaTime;

            if (resetKeyHoldTimer >= holdTimeToReset)
            {
                StartCoroutine(ResetSceneAfterDelay());
            }
        }
        else
        {
            resetKeyHoldTimer = 0f;
        }
    }

    private IEnumerator ResetSceneAfterDelay()
    {
        if (isResetting)
            yield break;

        isResetting = true;

        yield return new WaitForSeconds(resetDelay);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
