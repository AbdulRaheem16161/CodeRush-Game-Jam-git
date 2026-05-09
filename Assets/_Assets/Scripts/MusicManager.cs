using UnityEngine;

namespace AbdulRaheem.Audio
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance { get; private set; }

        [SerializeField] private AudioSource musicSource;

        private AudioClip currentClip;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        public void PlayMusic(AudioClip clip, bool restartIfSame = false)
        {
            if (clip == null)
                return;

            // Same music already playing
            if (currentClip == clip)
            {
                if (!restartIfSame)
                    return;
            }

            currentClip = clip;

            musicSource.clip = clip;
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
            currentClip = null;
        }
    }
}