using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Interaction
{
    public class PlayerMouseLook : MonoBehaviour, IPlayerMouseLook, IPlayerBehaviour
    {
        private bool cursorLocked;

        public void OnBehaviourInit(IPlayerController playerController)
        {
            SetCursorLock(true);

            playerController.LookDelta += UpdateDelta;
        }

        public bool GetCursorLock()
        {
            return cursorLocked;
        }

        public void SetCursorLock(bool value)
        {
            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !value;
            cursorLocked = value;
        }

        public void UpdateDelta(object sender, Vector2 delta)
        {
        }
    }
}