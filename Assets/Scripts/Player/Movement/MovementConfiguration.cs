using UnityEngine;

namespace Player.Movement
{
    [CreateAssetMenu]
    public class MovementConfiguration : ScriptableObject
    {
        [Header("Misc")]
        public float walkHeadHeight = 1.8f;

        [Header("Gravity")]
        public float airControlMultipler = 0.3f;
        public float groundedCheckRadius = 0.05f;
        public float groundedDistance = 0.01f;

        [Header("Physics")]
        public float gravity = -9.8f;
        public float airDensity = 1.293f;
        public float dragCoefficient;
        public float coefficientOfFriction = 0.1f;

        [Header("Base Move")]
        public float acceleration = 35f;
        public float moveSpeed = 5f;

        [Header("Slope")]
        public float slopeGravity = -3f;
        public float maxGroundedSlopeAngle = 45f;
        public float slopeCheckRadius = 0.01f;
        public float slopeCheckDistance = 0.01f;
        public float slopeFlatAngle = 15.0f;
        public float jumpHeight = 2f;
        public float jumpForwardForce = 1.2f;
        public float jumpForwardMultiplier = 0.4f;

        [Header("Visuals")]
        public float jumpCurveDuration = 0.4f;
        public float jumpCurveMultiplier = 0.7f;
        public AnimationCurve jumpCurve_Z;
        public AnimationCurve jumpCurve_Y;
        public float landCurveDuration = 1f;
        public float landCurveMultiplier = 0.7f;
        public AnimationCurve landCurve_Z;
        public AnimationCurve landCurve_Y;
    }
}