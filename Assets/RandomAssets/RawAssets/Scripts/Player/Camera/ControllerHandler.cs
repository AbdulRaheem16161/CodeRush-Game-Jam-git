using System;
using UnityEngine;
using AbdulRaheem.Game.General;
using Unity.Cinemachine;

namespace AbdulRaheem.Game.Player
{
    [DefaultExecutionOrder(-1)]
    public class PlayerControllerModeHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputReader inputReader;
        [Space(3)]
        [SerializeField] private CinemachineCamera thirdPersonCamera;
        [SerializeField] private CinemachineCamera firstPersonCamera;
        [Space(3)]
        [SerializeField] private FirstPersonPlayerController firstPersonController;
        [SerializeField] private ThirdPersonPlayerController thirdPersonController;
        [Space(3)]
        [SerializeField] private GameObject Visual;

        public IPlayerController ActiveController { get; private set; }

        #region Getters
        public FirstPersonPlayerController FirstPersonPlayerController => firstPersonController;
        public ThirdPersonPlayerController ThirdPersonPlayerController => thirdPersonController;
        #endregion

        [Header("Deubbing")]
        [SerializeField] private bool isFirstPerson;
        [SerializeField] private bool isThirdPerson;


        private void OnEnable()
        {
            inputReader.CameraToggleAction += Toggle;
        }

        private void OnDisable()
        {
            inputReader.CameraToggleAction -= Toggle;
        }

        private void Awake()
        {
            // default to first Person
            SwitchActiveController();
        }

        private void Toggle()
        {
            SwitchActiveController();
        }

        private void SwitchActiveController()
        {
            if (ActiveController == firstPersonController) 
            {
                // switch to third person
                ActiveController = thirdPersonController;
                thirdPersonCamera.Priority = 20;
                firstPersonCamera.Priority = 10;

                Visual.SetActive(true);

                isThirdPerson = true;
                isFirstPerson = false;
            }
            else
            {
                // switch to first person
                ActiveController = firstPersonController;
                thirdPersonCamera.Priority = 10;
                firstPersonCamera.Priority = 20;

                Visual.SetActive(false);

                isFirstPerson = true;
                isThirdPerson = false;
            }
        }
    }
}

