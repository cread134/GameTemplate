using Core.PauseManagement;
using Player.PlayerResources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Player.Interaction
{
    public class PlayerMouseLook : MonoBehaviour, IPlayerMouseLook, IPlayerBehaviour
    {
        [SerializeField] private Transform mouseLooker;
        [SerializeField] private Transform playerBody;
        [SerializeField] float minAngle, maxAngle;
        [SerializeField] private MouseLookConfiguration mouseLookConfiguration;

        private bool initialized;
        private Vector3 rotation;
        private Vector2 delta;

        public void OnBehaviourInit(IPlayerController playerController, IPlayerResources playerResources)
        {
            CursorManager.LockCursor = true;
            rotation = new Vector3(mouseLooker.localEulerAngles.x, playerBody.localEulerAngles.y);
            playerController.LookDelta += UpdateDelta;
            initialized = true;
        }

        public void StartBehaviour()
        {
        }

        void UpdateLook()
        {
            if (!initialized) { return; }
            if (PauseManager.IsPaused)
                return;
            rotation.x = Mathf.Clamp(rotation.x - delta.y, minAngle, maxAngle);
            rotation.y += delta.x % 360;
            mouseLooker.localEulerAngles = new Vector3(rotation.x, 0);
            playerBody.localEulerAngles += new Vector3(0, delta.x % 360);
        }

        public void UpdateDelta(object sender, Vector2 delta)
        {
            delta.x *= mouseLookConfiguration.lookSensitivity_X;
            delta.y *= mouseLookConfiguration.lookSensitivity_Y;
            this.delta = delta;
            UpdateLook();
        }
    }
}