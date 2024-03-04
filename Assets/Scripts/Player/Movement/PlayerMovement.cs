using Player.Interaction;
using Player.PlayerResources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

namespace Player.Movement
{
    public class PlayerMovement : MonoBehaviour, IPlayerBehaviour
    {
        private CharacterController characterController;
        private IPlayerCameraAnimator playerCameraAnimator;
        [SerializeField] private MovementConfiguration movementConfiguration;

        private LayerMask groundMask;

        private bool initialised = false;
        private bool _useGravity = true;

        private bool _isGrounded;
        private bool _lastGrounded;

        private Vector3 _moveDelta;
        private Vector3 _moveDamper;
        private Vector3 moveVector;

        private Vector3 _slopeDirection;
        private Vector3 _slopeNormal;
        private Vector3 _slopeHorizontal;
        private Vector3 _appliedVelocity;
        private Vector3 _gravityVelocity;

        private float _slopeAngle;
        private float _desiredStepOffset;

        public void OnBehaviourInit(IPlayerController playerController, IPlayerResources playerResources)
        {
            //setup resources
            characterController = playerResources.GetComponentResource<CharacterController>();
            playerCameraAnimator = playerResources.GetBehaviourResource<PlayerCameraAnimator>();
            groundMask = playerResources.GetLayerMaskResource("Ground");

            playerController.MoveDelta += OnMoveDelta;
            playerController.OnJumpDown += OnJumpDown;

            //process behaviours
            _desiredStepOffset = characterController.stepOffset;
            Debug.Log("stepoffset");
            initialised = true;
        }

        public void StartBehaviour()
        {
            playerCameraAnimator.ChangePlayerHeadHeight(0.1f, movementConfiguration.walkHeadHeight);
        }

        private void Update()
        {
            if (!initialised)
                return;
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            GroundCheck();

            UpdateSlopeValues();
            CalculatePhysics();
            BasicMove();

            MovePlayerFinal();

            _lastGrounded = _isGrounded;
        }

        public bool Grounded
            => Physics.CheckSphere(characterController.transform.position + Vector3.up * (movementConfiguration.groundedCheckRadius - movementConfiguration.groundedDistance), movementConfiguration.groundedCheckRadius, groundMask);

        private void GroundCheck()
        {
            _isGrounded = Grounded && _slopeAngle <= movementConfiguration.maxGroundedSlopeAngle;

            if (_isGrounded && !_lastGrounded)
            {
                OnLand();
            }
        }

        #region slope handling

        private void UpdateSlopeValues()
        {
            Vector3 pos = characterController.transform.position + Vector3.up * movementConfiguration.slopeCheckRadius;
            if (Physics.SphereCast(pos, movementConfiguration.slopeCheckRadius, Vector3.down, out RaycastHit hit, movementConfiguration.slopeCheckRadius + movementConfiguration.slopeCheckDistance, groundMask, QueryTriggerInteraction.Ignore))
            {
                // Get horizontal direction of the normal
                _slopeNormal = hit.normal;
                _slopeHorizontal = _slopeNormal;
                _slopeHorizontal.y = 0;
                _slopeHorizontal = _slopeHorizontal.normalized;

                // Flat Ground
                if (_slopeNormal == Vector3.up || Mathf.Approximately(_slopeNormal.y, 1.0f) || Mathf.Abs(Vector3.Dot(_slopeNormal, Vector3.up)) >= Mathf.Cos(movementConfiguration.slopeFlatAngle * Mathf.Deg2Rad))
                {
                    _slopeDirection = Vector3.up;
                    _slopeAngle = 0.0f;
                }
                // Sloped Ground
                else
                {
                    Vector3 right = Quaternion.AngleAxis(90.0f, Vector3.up) * _slopeHorizontal;
                    _slopeAngle = 90.0f - Vector3.Angle(_slopeHorizontal, hit.normal);
                    _slopeDirection = Quaternion.AngleAxis(_slopeAngle, right) * _slopeHorizontal;
                    _slopeDirection = _slopeDirection.normalized;
                }
            }
            else
            {
                _slopeDirection = Vector3.up;
                _slopeHorizontal = Vector3.zero;
                _slopeAngle = 0.0f;
            }
            Debug.DrawLine(characterController.transform.position, characterController.transform.position + _slopeDirection, Color.cyan);
        }
        #endregion

        #region physics handling
        private void CalculatePhysics()
        {
            //calculate friction
            if (_isGrounded == false)
            {
                characterController.stepOffset = 0f;
            }
            else
            {
                characterController.stepOffset = _desiredStepOffset;
            }
            //calculate gravity
            if (_useGravity)
            {
                if (_lastGrounded == true && !_isGrounded && _appliedVelocity.y < -1f)
                {
                    _appliedVelocity.y = 0f;
                }
                if (_isGrounded)
                {
                    Vector3 grav = Gravity(movementConfiguration.gravity);
                    _appliedVelocity += grav * Time.deltaTime;
                    _appliedVelocity.y = Mathf.Clamp(_appliedVelocity.y, grav.y, float.MaxValue);
                }
                else
                {
                    _appliedVelocity += Gravity(movementConfiguration.gravity) * Time.deltaTime;
                }
            }
            float resistanceMultipler = _isGrounded ? 3f : 1f;
            Vector3 lerped = Vector3.Lerp(_appliedVelocity, Vector3.zero, resistanceMultipler * (movementConfiguration.airDensity + AirResistance(characterController.velocity).magnitude) * Time.deltaTime);
            _appliedVelocity.x = lerped.x;
            _appliedVelocity.z = lerped.z;

            // Shift gravity towards the steep slope's direction
            Vector3 slopeAdjustment = _appliedVelocity;
            if (_slopeAngle > movementConfiguration.maxGroundedSlopeAngle)
            {
                slopeAdjustment.y = 0.0f;
                slopeAdjustment += -_slopeDirection * _appliedVelocity.y;
            }

            Vector3 useVelocity = slopeAdjustment + _gravityVelocity;
            moveVector += useVelocity;
        }

        Vector3 AirResistance(Vector3 velocity)
        {
            Vector3 resistanceDirection = -velocity.normalized;
            float magnitude = movementConfiguration.airDensity * 0.5f * movementConfiguration.dragCoefficient * Vector3.SqrMagnitude(velocity);
            return resistanceDirection * magnitude * Time.deltaTime;
        }

        Vector3 Gravity(float gravityAmount)
        {
            Vector3 baseGravity = new Vector3(0f, gravityAmount, 0f);
            baseGravity.y += movementConfiguration.slopeGravity;
            return baseGravity;
        }
        #endregion

        #region moveCalculation
        private void BasicMove()
        {
            Vector3 moveDelta = _moveDelta;
            Vector3 move = characterController.transform.right * moveDelta.x + characterController.transform.forward * moveDelta.y;
            move.y = 0f;

            // Prevent movement from counteracting against the slope
            if (move != Vector3.zero && _slopeAngle > movementConfiguration.maxGroundedSlopeAngle && _slopeHorizontal != Vector3.zero)
            {
                Vector3 right = Quaternion.AngleAxis(90.0f, Vector3.up) * _slopeHorizontal;
                float side = Vector3.Dot(move, right);
                side = side >= 0.0f ? 1.0f : -1.0f;

                float angle = Vector3.Angle(right * side, move) * Mathf.Deg2Rad;
                move = side * Mathf.Cos(angle) * right;
            }

            _moveDamper = Vector3.Lerp(_moveDamper, move, movementConfiguration.acceleration * Time.deltaTime);
            Vector3 targetVelocity = (_moveDamper * movementConfiguration.moveSpeed);
            float multipler = _isGrounded ? 1f : movementConfiguration.airControlMultipler;
            moveVector += (targetVelocity * multipler);
        }

        public void MovePlayerFinal()
        {
            characterController.Move(moveVector * Time.deltaTime);
            moveVector = Vector3.zero;
        }
        #endregion

        private void OnJumpDown(object sender, EventArgs e)
        {

        }

        private void OnMoveDelta(object sender, Vector2 e)
        {
            _moveDelta = e;   
        }

        private void OnLand()
        {
        }
    }
}