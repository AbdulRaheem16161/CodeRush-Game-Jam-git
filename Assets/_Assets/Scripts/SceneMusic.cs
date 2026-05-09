// using UnityEngine;

// namespace AbdulRaheem.Audio
// {
//     public class SceneMusic : MonoBehaviour
//     {
//         [SerializeField] private AudioClip musicClip;

//         [SerializeField] private bool continueCurrentMusic;

//         private void Start()
//         {
//             if (continueCurrentMusic)
//                 return;

//             if (MusicManager.Instance == null)
//             {
//                 Debug.LogError("No MusicManager found.");
//                 return;
//             }

//             MusicManager.Instance.PlayMusic(musicClip);
//         }
//     }
// }