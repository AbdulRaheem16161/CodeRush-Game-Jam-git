using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private string gameSceneName = "GameScene";

    private void Start()
    {
        // Subscribe to button click events
        startButton.onClick.AddListener(OnStartButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        Debug.Log("Start button clicked! Loading scene: " + gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }

    private void OnQuitButtonClicked()
    {
        Debug.Log("Quit button clicked!");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}