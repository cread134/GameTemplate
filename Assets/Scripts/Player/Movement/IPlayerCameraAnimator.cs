using UnityEngine;

namespace Player.Movement
{
    internal interface IPlayerCameraAnimator
    {
        void ChangePlayerHeadHeight(float duration, float newHeight);
        void DoHeadAnimation(AnimationCurve curve_X, AnimationCurve curve_Y, AnimationCurve curve_Z, float duration, float multiplier = 1f);
        void UpdateHeadBob(Vector3 velocity);
    }
}