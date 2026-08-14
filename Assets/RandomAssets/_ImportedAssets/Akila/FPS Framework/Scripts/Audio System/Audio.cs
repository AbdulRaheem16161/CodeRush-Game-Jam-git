using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Akila.FPSFramework
{
    /// <summary>
    /// Handles audio playback, control, and custom events in the FPS Framework.
    /// </summary>
    public class Audio
    {
        /// <summary>
        /// The main active audio listener in the scene.
        /// </summary>
        public AudioListener audioListener
        {
            get
            {
                if (_audioListener == null)
                {
                    AudioListener[] listeners =
                        GameObject.FindObjectsByType<AudioListener>(
                            FindObjectsSortMode.None
                        );

                    foreach (AudioListener listener in listeners)
                    {
                        if (listener == null)
                            continue;

                        if (listener.TryGetComponent<Camera>(out Camera camera))
                        {
                            if (camera != null &&
                                camera.enabled &&
                                listener.enabled)
                            {
                                _audioListener = listener;
                                break;
                            }
                        }
                    }
                }

                return _audioListener;
            }
        }

        private AudioListener _audioListener;

        /// <summary>
        /// The audio profile containing settings for the audio.
        /// </summary>
        public AudioProfile audioProfile;

        private AudioSource m_audioSource;

        /// <summary>
        /// The AudioSource component used for playing audio.
        /// </summary>
        public AudioSource audioSource
        {
            get
            {
                if (!isSetup)
                    return null;

                try
                {
                    // Check whether the current audio holder was destroyed.
                    if (audioHolder == null)
                    {
                        audioHolder = null;
                        m_audioSource = null;

                        if (sourceGameObject == null)
                            return null;

                        audioHolder = new GameObject(
                            $"{sourceGameObject.name}",
                            typeof(AudioObject)
                        );

                        AudioObject holder =
                            audioHolder.GetComponent<AudioObject>();

                        if (holder == null)
                            return null;

                        if (sourceGameObject != null)
                        {
                            holder.audioTarget = sourceGameObject;
                        }

                        if (holder != null)
                            component = holder;

                        if (audioHolder != null && component != null)
                        {
                            string profileName =
                                audioProfile != null
                                    ? audioProfile.name
                                    : "Audio";

                            audioHolder.name +=
                                $"'s {profileName} Audio Holder";

                            audioHolder.transform.SetPositionAndRotation(
                                component.transform.position,
                                component.transform.rotation
                            );

                            audioHolder.hideFlags =
                                HideFlags.HideAndDontSave;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(
                        $"Failed to create Audio Holder. Error: {e}"
                    );

                    return null;
                }

                if (audioHolder == null)
                    return null;

                // Recreate AudioSource if it was destroyed.
                if (m_audioSource == null)
                {
                    try
                    {
                        m_audioSource =
                            audioHolder.GetComponent<AudioSource>();

                        if (m_audioSource == null)
                        {
                            m_audioSource =
                                audioHolder.AddComponent<AudioSource>();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(
                            $"Failed to create AudioSource. Error: {e}"
                        );

                        m_audioSource = null;
                    }
                }

                return m_audioSource;
            }
        }

        private bool isSetup;

        /// <summary>
        /// The GameObject to which the audio is attached.
        /// </summary>
        public MonoBehaviour component;

        public GameObject GameObject =>
            component != null ? component.gameObject : null;

        private float _eventsDuration;

        public float eventsDuration
        {
            get => _eventsDuration;
            set => _eventsDuration = value;
        }

        /// <summary>
        /// List of custom audio events triggered at specific times.
        /// </summary>
        protected List<CustomAudioEvent> events =
            new List<CustomAudioEvent>();

        public bool isEventsEnabled;

        protected float randomizedPitchOffset { get; set; }

        protected float sixDimensionsPitchOffset { get; set; }

        private float timeScaleSyncedPitch
        {
            get
            {
                if (audioProfile != null &&
                    audioProfile.syncPitchWithTimeScale)
                {
                    return Time.timeScale;
                }

                return 1;
            }
        }

        public float totalPitch
        {
            get
            {
                return sixDimensionsPitchOffset +
                       timeScaleSyncedPitch +
                       randomizedPitchOffset;
            }
        }

        public float distanceFromListener;

        private GameObject audioHolder;
        private GameObject sourceGameObject;

        /// <summary>
        /// Sets up the Audio class with a target GameObject
        /// and an audio profile.
        /// </summary>
        public void Setup(GameObject obj, AudioProfile profile)
        {
            if (obj == null)
            {
                Debug.LogError(
                    "Target object is null. Audio Setup aborted."
                );

                return;
            }

            if (profile == null)
            {
                Debug.LogError(
                    "AudioProfile is null. Audio Setup aborted.",
                    obj
                );

                return;
            }

            sourceGameObject = obj;

            audioProfile = profile;

            eventsDuration = profile.audioLayersDuration;

            isSetup = true;

            // Create the AudioSource immediately.
            AudioSource source = audioSource;

            if (source == null)
            {
                Debug.LogError(
                    $"[Audio] Failed to create AudioSource for {obj.name}.",
                    obj
                );

                return;
            }

            ApplySettings(profile);

            foreach (var layer in profile.audioLayers)
            {
                if (layer.audioClip == null)
                    continue;

                if (layer.time < 0)
                {
                    Debug.LogError(
                        "[Audio] Audio Profile's sound layer time " +
                        "can't be less than zero. Resetting to 0.",
                        obj
                    );
                }

                float time =
                    Mathf.Clamp(layer.time, 0.001f, float.MaxValue);

                AudioClip layerClip = layer.audioClip;

                AddCustomEvent(
                    () =>
                    {
                        AudioSource currentSource = audioSource;

                        if (currentSource != null &&
                            layerClip != null)
                        {
                            currentSource.PlayOneShot(layerClip);
                        }
                    },
                    time
                );
            }
        }

        private void Update()
        {
            if (!Application.isPlaying)
                return;

            if (component == null)
                return;

            if (audioProfile == null)
                return;

            if (component.gameObject == null)
                return;

            if (!component.gameObject.activeSelf &&
                audioProfile.stopOnDisabled)
            {
                Stop();
                return;
            }

            if (component.gameObject.activeInHierarchy)
            {
                if (audioProfile.useRandomPitchOffset)
                {
                    CalculateRandomPitch();
                }

                AudioSource source = audioSource;

                if (source != null)
                {
                    source.pitch =
                        (audioProfile.pitch +
                         randomizedPitchOffset +
                         sixDimensionsPitchOffset) *
                        timeScaleSyncedPitch;
                }
            }

            AudioListener listener = audioListener;

            if (listener == null)
                return;

            if (component == null)
                return;

            float distance =
                Vector3.Distance(
                    component.transform.position,
                    listener.transform.position
                );

            float blendVal =
                audioProfile.maxDistance > 0
                    ? distance / audioProfile.maxDistance
                    : 0;

            Vector3 direction =
                (listener.transform.position -
                 component.transform.position).normalized;

            float forwardDot =
                Mathf.Max(0, Vector3.Dot(
                    direction,
                    Vector3.forward
                ));

            float backwardDot =
                Mathf.Max(0, Vector3.Dot(
                    direction,
                    -Vector3.forward
                ));

            float rightDot =
                Mathf.Max(0, Vector3.Dot(
                    direction,
                    Vector3.right
                ));

            float leftDot =
                Mathf.Max(0, Vector3.Dot(
                    direction,
                    -Vector3.right
                ));

            float upDot =
                Mathf.Max(0, Vector3.Dot(
                    direction,
                    Vector3.up
                ));

            float downDot =
                Mathf.Max(0, Vector3.Dot(
                    direction,
                    -Vector3.up
                ));

            float totalDot =
                forwardDot +
                backwardDot +
                rightDot +
                leftDot +
                upDot +
                downDot;

            float dirValue = 0;

            if (totalDot > 0)
            {
                dirValue +=
                    (
                        forwardDot * audioProfile.forwardFactor +
                        backwardDot * audioProfile.backwardFactor +
                        rightDot * audioProfile.rightFactor +
                        leftDot * audioProfile.leftFactor +
                        upDot * audioProfile.upFactor +
                        downDot * audioProfile.downFactor
                    ) / totalDot;
            }

            if (audioProfile._6DSoundCurve != null)
            {
                sixDimensionsPitchOffset =
                    Mathf.Lerp(
                        0,
                        dirValue,
                        audioProfile._6DSoundCurve.Evaluate(
                            blendVal
                        )
                    );
            }
        }

        /// <summary>
        /// Plays the audio.
        /// </summary>
        public async void Play(
            bool useOneShot = false,
            AudioClip clipOverride = null)
        {
            if (!Application.isPlaying)
                return;

            if (audioProfile == null)
            {
                Debug.LogError(
                    "[Audio] AudioProfile is null. Cannot play audio."
                );

                return;
            }

            if (component == null)
                return;

            if (component.gameObject == null)
                return;

            if (!component.gameObject.activeSelf)
                return;

            AudioSource source = audioSource;

            if (source == null)
            {
                Debug.LogWarning(
                    $"[Audio] AudioSource is missing for " +
                    $"{component.gameObject.name}. Cannot play audio."
                );

                return;
            }

            stopRequested = false;

            if (audioProfile.spatialBlend > 0 &&
                audioProfile.simulateAcousticLatency)
            {
                float time = 0f;

                float distanceFromListener = 1f;

                AudioListener listener = audioListener;

                if (listener != null && source != null)
                {
                    distanceFromListener =
                        Vector3.Distance(
                            listener.transform.position,
                            source.transform.position
                        ) / 343f;
                }

                while (time < distanceFromListener)
                {
                    if (!Application.isPlaying)
                        return;

                    if (component == null)
                        return;

                    if (source == null)
                        return;

                    if (!component.gameObject)
                        return;

                    if (!component.gameObject.activeInHierarchy)
                        return;

                    time += Time.deltaTime;

                    await Task.Yield();
                }
            }

            // IMPORTANT:
            // The object may have been destroyed while awaiting.
            // Re-check everything before touching the AudioSource.
            if (!Application.isPlaying)
                return;

            if (component == null)
                return;

            if (component.gameObject == null)
                return;

            if (source == null)
                return;

            AudioSource currentSource = audioSource;

            if (currentSource == null)
                return;

            if (!currentSource)
                return;

            EnableEvents();

            ApplySettings(audioProfile);

            // ApplySettings can potentially recreate the source.
            currentSource = audioSource;

            if (currentSource == null)
                return;

            CalculateRandomPitch();

            InvokeCustomEvents();

            if (useOneShot)
            {
                AudioClip clipToPlay =
                    clipOverride != null
                        ? clipOverride
                        : audioProfile.audioClip;

                if (clipToPlay == null)
                    return;

                if (currentSource == null)
                    return;

                currentSource.PlayOneShot(clipToPlay);
            }
            else
            {
                if (currentSource == null)
                    return;

                currentSource.Play();
            }
        }

        public void Pause()
        {
            if (audioProfile == null)
                return;

            AudioSource source = audioSource;

            if (source == null)
                return;

            if (audioHolder == null)
                return;

            if (!audioHolder.activeInHierarchy)
                return;

            isPaused = true;

            source.Pause();
        }

        public void Unpause()
        {
            if (audioProfile == null)
                return;

            AudioSource source = audioSource;

            if (source == null)
                return;

            if (component == null)
                return;

            if (!component.gameObject.activeSelf)
                return;

            isPaused = false;

            source.UnPause();
        }

        public void Stop()
        {
            if (audioProfile == null)
                return;

            AudioSource source = audioSource;

            if (source == null)
                return;

            if (component == null)
                return;

            if (!component.gameObject.activeSelf)
                return;

            stopRequested = true;

            source.Stop();
        }

        public void EnableEvents()
        {
            isEventsEnabled = true;
        }

        public void DisableEvents()
        {
            isEventsEnabled = false;
        }

        /// <summary>
        /// Updates AudioSource settings.
        /// </summary>
        public void ApplySettings(AudioProfile profile)
        {
            if (profile == null)
            {
                Debug.LogError(
                    "[Audio] AudioProfile is null. " +
                    "Cannot update AudioSource settings."
                );

                return;
            }

            audioProfile = profile;

            AudioSource source = audioSource;

            if (source == null)
                return;

            source.clip = profile.audioClip;
            source.outputAudioMixerGroup = profile.output;
            source.mute = profile.mute;
            source.bypassEffects = profile.bypassEffects;
            source.bypassListenerEffects =
                profile.bypassListenerEffects;
            source.bypassReverbZones =
                profile.bypassReverbZones;

            source.playOnAwake = profile.playOnAwake;
            source.loop = profile.loop;
            source.priority = profile.priority;

            source.volume =
                profile.volume *
                FPSFrameworkSettings.globalAudioVolume;

            source.pitch = profile.pitch;
            source.panStereo = profile.stereoPan;
            source.spatialBlend = profile.spatialBlend;
            source.reverbZoneMix = profile.reverbZoneMix;
            source.dopplerLevel = profile.dopplerLevel;
            source.spread = profile.spread;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = profile.minDistance;
            source.maxDistance = profile.maxDistance;
        }

        public void CalculateRandomPitch()
        {
            if (audioProfile == null)
                return;

            if (component == null)
                return;

            if (component.gameObject == null)
                return;

            if (!component.gameObject.activeSelf)
                return;

            randomizedPitchOffset =
                UnityEngine.Random.Range(
                    0,
                    audioProfile.randomPitchOffset
                );
        }

        private bool stopRequested;
        private bool isPaused;

        private async void InvokeCustomEvents()
        {
            if (!Application.isPlaying)
                return;

            if (audioProfile == null)
                return;

            if (events.Count == 0)
                return;

            stopRequested = false;

            float time = -Time.deltaTime;
            float currentTime = 0;
            float previousTime = 0;

            while (time < eventsDuration + Time.deltaTime)
            {
                if (!Application.isPlaying)
                    return;

                if (component == null)
                    return;

                if (component.gameObject == null)
                    return;

                if (this.stopRequested)
                    break;

                if (!isPaused)
                {
                    time += Time.deltaTime;
                }

                currentTime = time;

                AudioSource source = audioSource;

                if (source != null &&
                    audioProfile != null)
                {
                    source.pitch =
                        Time.timeScale *
                        audioProfile.pitch;
                }

                foreach (CustomAudioEvent customEvent in events)
                {
                    if (customEvent == null)
                        continue;

                    if (currentTime > customEvent.time &&
                        previousTime < customEvent.time)
                    {
                        if (!stopRequested &&
                            isEventsEnabled &&
                            component != null &&
                            component.gameObject != null &&
                            component.gameObject.activeInHierarchy)
                        {
                            customEvent.Invoke();
                        }
                    }
                }

                previousTime = currentTime;

                await Task.Yield();
            }
        }

        public CustomAudioEvent GetCustomAudioEvent(int index)
        {
            if (index < 0 || index >= events.Count)
                return null;

            return events[index];
        }

        public CustomAudioEvent GetCustomAudioEvent(float time)
        {
            return events.Find(e => e != null && e.time == time);
        }

        public void AddCustomEvent(
            UnityAction action,
            float time)
        {
            events.Add(
                new CustomAudioEvent(time, action)
            );

            CalculateCustomEventDuration();
        }

        public void RemoveCustomEvent(
            CustomAudioEvent audioEvent)
        {
            if (audioEvent == null)
                return;

            events.Remove(audioEvent);

            CalculateCustomEventDuration();
        }

        public void ClearCustomEvents()
        {
            events.Clear();

            eventsDuration = 0;
        }

        private float CalculateCustomEventDuration()
        {
            if (events.Count == 0)
            {
                eventsDuration = 0;
                return 0;
            }

            eventsDuration =
                events.Max(e => e.time);

            return eventsDuration;
        }
    }

    /// <summary>
    /// Represents a custom audio event triggered at a specific time.
    /// </summary>
    public class CustomAudioEvent
    {
        public float time;
        public UnityAction action;

        public CustomAudioEvent(
            float time,
            UnityAction action)
        {
            this.time = time;
            this.action = action;
        }

        public void Invoke()
        {
            if (action == null)
                return;

            action.Invoke();
        }
    }
}
