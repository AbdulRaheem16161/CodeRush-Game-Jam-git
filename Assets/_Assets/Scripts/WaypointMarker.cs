using UnityEngine;

public class WaypointMarker : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float fadeDuration = 0.3f;
    
    private bool isCollected = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (isCollected) return;
        if (!collision.CompareTag(playerTag)) return;

        isCollected = true;
        Vanish();
    }

    private void Vanish()
    {
        if (fadeDuration <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(FadeOut());
        }
    }

    private System.Collections.IEnumerator FadeOut()
    {
        var renderer = GetComponent<Renderer>();
        var materials = renderer.materials;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            foreach (var material in materials)
            {
                Color color = material.color;
                color.a = alpha;
                material.color = color;
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}