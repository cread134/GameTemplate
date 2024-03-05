using UnityEngine;

namespace Player.Movement
{
    internal interface IPlayerCameraAnimator
    {
        void ChangePlayerHeadHeight(float duration, float newHeight);
        void UpdateHeadBob(Vector3 velocity);
    }
}