using UnityEngine;

namespace AbdulRaheem.Game.General
{
    public class PuaseManager : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private bool isPaused;

        private void Awake()
        {
            inputReader.PauseAction += TogglePause;
        }

        private void TogglePause()
        {
            isPaused = !isPaused;
        }

        private void Update()
        {
            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}