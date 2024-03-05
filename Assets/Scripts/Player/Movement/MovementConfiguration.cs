using UnityEngine;

namespace Player.Movement
{
    [CreateAssetMenu]
    public class MovementConfiguration : ScriptableObject
    {
        public float walkHeadHeight = 1.8f;

        public float airControlMultipler = 0.3f;
        public float groundedCheckRadius = 0.05f;
        public float groundedDistance = 0.01f;

        public float gravity = -9.8f;
        public float airDensity = 1.293f;
        public float dragCoefficient;
        public float coefficientOfFriction = 0.1f;

        public float acceleration = 35f;
        public float moveSpeed = 5f;

        public float slopeGravity = -3f;
        public float maxGroundedSlopeAngle = 45f;
        public float slopeCheckRadius = 0.01f;
        public float slopeCheckDistance = 0.01f;
        public float slopeFlatAngle = 15.0f;
        public float jumpHeight = 2f;
        public float jumpForwardForce = 1.2f;
    }
}