using UnityEngine;

namespace Core.PauseManagement
{
    public class CursorManager
    {
        public static bool LockCursor
        {
            get => Cursor.lockState == CursorLockMode.Locked;
            set
            {
                Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !value;
            }
        }
    }
}