using UnityEngine;

namespace Player.Interaction
{
    internal interface IPlayerMouseLook
    {
        public void SetCursorLock(bool value);
        public bool GetCursorLock();
        public void UpdateDelta(object sender, Vector2 delta);
    }
}