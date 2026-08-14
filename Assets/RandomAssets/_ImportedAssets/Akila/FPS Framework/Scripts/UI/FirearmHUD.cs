using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Akila.FPSFramework
{
    [AddComponentMenu("Akila/FPS Framework/Weapons/Firearm HUD")]
    public class FirearmHUD : MonoBehaviour
    {
        [Header("Text")]
        public TextMeshProUGUI firearmNameText;
        public TextMeshProUGUI ammoTypeNameText;
        public TextMeshProUGUI remainingAmmoText;
        public TextMeshProUGUI remainingAmmoTypeText;
        public GameObject fireModeSwitchAlert;
        public GameObject outOfAmmoAlert;
        public GameObject lowAmmoAlert;

        [Header("Colors")]
        public Color normalColor = Color.white;
        public Color alertColor = Color.red;

        public Firearm firearm { get; set; }

        private CanvasGroup canvasGroup;
        private bool eventRegistered = false;

        private void Awake()
        {
            canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            canvasGroup.alpha = 0;

            if (fireModeSwitchAlert != null)
                fireModeSwitchAlert.SetActive(false);

            PerformTextUpdate();
        }

        private void Start()
        {
            TryInitializeFirearm();

            Invoke(nameof(Show), Time.fixedDeltaTime);
        }

        private void Update()
        {
            if (firearm == null)
            {
                TryInitializeFirearm();
                return;
            }

            PerformTextUpdate();
        }

        private void TryInitializeFirearm()
        {
            if (firearm == null)
                return;

            if (eventRegistered)
                return;

            if (firearm.events == null)
            {
                Debug.LogWarning(
                    $"[FirearmHUD] Firearm '{firearm.name}' exists, but its events object is null."
                );

                return;
            }

            firearm.events.OnFireModeChange.AddListener(OnFireModeChange);

            eventRegistered = true;

            PerformTextUpdate();

            Debug.Log(
                $"[FirearmHUD] Successfully initialized with firearm: {firearm.name}"
            );
        }

        private void OnFireModeChange()
        {
            if (fireModeSwitchAlert == null)
                return;

            fireModeSwitchAlert.SetActive(true);

            TextMeshProUGUI textMeshProUGUI =
                fireModeSwitchAlert.GetComponentInChildren<TextMeshProUGUI>();

            if (textMeshProUGUI != null)
            {
                textMeshProUGUI.text =
                    $"Fire Mode: {firearm.currentFireMode}";
            }

            CancelInvoke(nameof(HideFireModeAlert));

            Invoke(nameof(HideFireModeAlert), 2f);
        }

        private void Show()
        {
            if (canvasGroup != null)
                canvasGroup.alpha = 1;
        }

        private void HideFireModeAlert()
        {
            if (fireModeSwitchAlert != null)
                fireModeSwitchAlert.SetActive(false);
        }

        protected virtual void PerformTextUpdate()
        {
            if (firearm == null)
                return;

            gameObject.SetActive(firearm.isHudActive);

            if (firearmNameText != null)
                firearmNameText.SetText(firearm.Name);

            if (ammoTypeNameText != null &&
                firearm.ammoProfile != null &&
                firearm.ammoProfile.identifier != null)
            {
                ammoTypeNameText.SetText(
                    firearm.ammoProfile.identifier.displayName
                );
            }

            if (remainingAmmoText != null)
            {
                remainingAmmoText.SetText(
                    firearm.remainingAmmoCount.ToString()
                );

                remainingAmmoText.color =
                    firearm.remainingAmmoCount <=
                    firearm.preset.magazineCapacity / 3
                        ? alertColor
                        : normalColor;
            }

            if (remainingAmmoTypeText != null)
            {
                remainingAmmoTypeText.SetText(
                    firearm.remainingAmmoTypeCount.ToString()
                );

                remainingAmmoTypeText.color =
                    firearm.remainingAmmoTypeCount <= 0
                        ? alertColor
                        : normalColor;
            }

            if (outOfAmmoAlert != null)
            {
                outOfAmmoAlert.SetActive(
                    firearm.remainingAmmoCount <= 0
                );
            }

            if (lowAmmoAlert != null)
            {
                lowAmmoAlert.SetActive(
                    firearm.remainingAmmoCount <=
                    firearm.preset.magazineCapacity / 3 &&
                    firearm.remainingAmmoCount > 0
                );
            }
        }

        private void OnDestroy()
        {
            if (firearm != null &&
                firearm.events != null &&
                eventRegistered)
            {
                firearm.events.OnFireModeChange.RemoveListener(
                    OnFireModeChange
                );
            }

            CancelInvoke();
        }
    }
}