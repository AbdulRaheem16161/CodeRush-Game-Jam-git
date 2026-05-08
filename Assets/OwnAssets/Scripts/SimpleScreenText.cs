using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SimpleScreenText : MonoBehaviour
{
    [SerializeField] private string displayMessage = "Find the exit to your house on the screen";
    
    private static TextMeshProUGUI textDisplay;
    private static SimpleScreenText instance;

    private void Start()
    {
        if (textDisplay == null)
        {
            CreateUI();
        }
        
        ShowPersistent(displayMessage);
    }

    private void OnDestroy()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (textDisplay != null)
            Destroy(textDisplay.transform.root.gameObject);
        textDisplay = null;
        instance = null;
    }

    public static void ShowPersistent(string message)
    {
        if (textDisplay == null)
        {
            CreateUI();
        }

        textDisplay.text = message;
        textDisplay.gameObject.SetActive(true);
        instance.StopAllCoroutines();
    }

    public static void Show(string message, float duration = 0f)
    {
        if (textDisplay == null)
        {
            CreateUI();
        }

        textDisplay.text = message;
        textDisplay.gameObject.SetActive(true);

        if (duration > 0)
        {
            instance.StopAllCoroutines();
            instance.StartCoroutine(HideAfterDelay(duration));
        }
    }

    public static void Hide()
    {
        if (textDisplay != null)
            textDisplay.gameObject.SetActive(false);
    }

    private static void CreateUI()
    {
        // Create Canvas
        GameObject canvasObj = new GameObject("SimpleTextCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // Create Text GameObject
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(canvasObj.transform);
        textDisplay = textObj.AddComponent<TextMeshProUGUI>();

        // Setup RectTransform
        RectTransform rectTransform = textObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(800, 200);

        // Setup TextMesh
        textDisplay.text = "";
        textDisplay.fontSize = 36;
        textDisplay.alignment = TextAlignmentOptions.Center;
        textDisplay.color = Color.white;

        // Add outline for readability
        var outline = textObj.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(2, -2);

        instance = canvasObj.AddComponent<SimpleScreenText>();
        DontDestroyOnLoad(canvasObj);
    }

    private static System.Collections.IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Hide();
    }
}