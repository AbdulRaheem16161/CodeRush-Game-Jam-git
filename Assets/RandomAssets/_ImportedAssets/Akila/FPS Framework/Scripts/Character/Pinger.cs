using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Akila.FPSFramework
{
    [AddComponentMenu("Akila/FPS Framework/Player/Pinger")]
    public class Pinger : MonoBehaviour
    {
        public InputAction inputAction;
        public LayerMask pingableLayers = -1;
        public Ping ping;
        public Canvas canvas;
        public float range = 100;
        public float pingLifetime = 15;
        public float maxPings = 5;

        public List<Ping> pings = new List<Ping>();

        private void OnEnable()
        {
            if (inputAction == null)
            {
                Debug.LogError(
                    $"[Pinger] InputAction is not assigned on {gameObject.name}."
                );

                return;
            }

            inputAction.Enable();
            inputAction.performed += OnPingInput;
        }

        private void OnDisable()
        {
            if (inputAction == null)
                return;

            inputAction.performed -= OnPingInput;
            inputAction.Disable();
        }

        private void OnDestroy()
        {
            if (inputAction == null)
                return;

            inputAction.performed -= OnPingInput;
            inputAction.Disable();
        }

        private void OnPingInput(InputAction.CallbackContext context)
        {
            if (!this)
                return;

            if (!isActiveAndEnabled)
                return;

            LookAndPing();
        }

        private void LookAndPing()
        {
            if (canvas == null)
            {
                Debug.LogError(
                    $"[Pinger] Canvas is not assigned on {gameObject.name}. " +
                    "Assign a Canvas in the Inspector."
                );

                return;
            }

            if (ping == null)
            {
                Debug.LogError(
                    $"[Pinger] Ping prefab is not assigned on {gameObject.name}."
                );

                return;
            }

            if (Physics.Raycast(
                transform.position,
                transform.forward,
                out RaycastHit hit,
                range,
                pingableLayers))
            {
                Ping newPing = Instantiate(ping, canvas.transform);

                if (newPing == null)
                    return;

                FloatingRect floatingRect =
                    newPing.GetComponent<FloatingRect>();

                if (floatingRect != null)
                {
                    floatingRect.position = hit.point;
                }
                else
                {
                    Debug.LogWarning(
                        $"[Pinger] Ping '{newPing.name}' does not have a FloatingRect component."
                    );
                }

                pings.Add(newPing);

                while (pings.Count > maxPings)
                {
                    if (pings[0] != null)
                    {
                        Destroy(pings[0].gameObject);
                    }

                    pings.RemoveAt(0);
                }

                StartCoroutine(AutoDestroyPing(newPing));

                OnPinged(newPing);
            }
        }

        private IEnumerator AutoDestroyPing(Ping pingToDestroy)
        {
            yield return new WaitForSeconds(pingLifetime);

            if (pingToDestroy != null)
            {
                pings.Remove(pingToDestroy);

                Destroy(pingToDestroy.gameObject);
            }
        }

        public virtual void OnPinged(Ping ping)
        {
        }
    }
}