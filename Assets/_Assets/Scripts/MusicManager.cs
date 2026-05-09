using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Tracks")]
    public AudioClip musicA;
    public AudioClip musicB;
    public AudioClip musicC;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 2f;

    private AudioSource currentSource;
    private AudioSource nextSource;

    private MusicTrack currentTrack = MusicTrack.A;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        AudioSource[] sources = GetComponents<AudioSource>();

        if (sources.Length < 2)
        {
            Debug.LogError("MusicManager needs TWO AudioSources.");
            return;
        }

        currentSource = sources[0];
        nextSource = sources[1];
    }

    private void Start()
    {
        PlayTrackImmediate(MusicTrack.A);
    }

    public void ChangeMusic(MusicTrack newTrack)
    {
        if (newTrack == currentTrack)
            return;

        currentTrack = newTrack;

        AudioClip clip = GetClip(newTrack);

        StopAllCoroutines();
        StartCoroutine(CrossFade(clip));
    }

    private AudioClip GetClip(MusicTrack track)
    {
        switch (track)
        {
            case MusicTrack.A:
                return musicA;

            case MusicTrack.B:
                return musicB;

            case MusicTrack.C:
                return musicC;
        }

        return musicA;
    }

    private void PlayTrackImmediate(MusicTrack track)
    {
        currentTrack = track;

        currentSource.clip = GetClip(track);
        currentSource.volume = 1f;
        currentSource.loop = true;
        currentSource.Play();
    }

    private IEnumerator CrossFade(AudioClip newClip)
    {
        nextSource.clip = newClip;
        nextSource.volume = 0f;
        nextSource.loop = true;
        nextSource.Play();

        float timer = 0f;

        float startVolume = currentSource.volume;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float t = timer / fadeDuration;

            currentSource.volume = Mathf.Lerp(startVolume, 0f, t);
            nextSource.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        currentSource.Stop();

        AudioSource temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;
    }
}

public enum MusicTrack
{
    A,
    B,
    C
}