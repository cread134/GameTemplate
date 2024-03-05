using Player.Interaction;
using Player.PlayerResources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Movement
{
    public class PlayerCameraAnimator : MonoBehaviour, IPlayerCameraAnimator, IPlayerBehaviour
    {
        [SerializeField] private Transform headHolder;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private float colliderHeightOffset = 0.1f;

        private CharacterController characterController;
        private LayerMask groundMask;

        const float CHECK_CEIL_RADIUS = 0.05f;

        [SerializeField] private float _bobAmplitude = 0.05f;
        [SerializeField] private float bobSpeedMultipler = 1f;

        float _bobDamp = 4f;
        float _bobIndex;
        private Vector3 bobVector;
        float _lastFootStep;

        bool initialised = false;
        public void OnBehaviourInit(IPlayerController playerController, IPlayerResources playerResources)
        {
            characterController = playerResources.GetComponentResource<CharacterController>();   
            groundMask = playerResources.GetLayerMaskResource("ground");
            initialised = true;
        }

        public void StartBehaviour()
        {
        }

        #region HeadHeight
        public void ChangePlayerHeadHeight(float duration, float newHeight)
        {
            if (_changeHeadCoroutine != null)
            {
                StopCoroutine(_changeHeadCoroutine);
            }
            _changeHeadCoroutine = StartCoroutine(SetheadHeight(duration, newHeight));
        }

        public bool GetCeilingFree()
        {
            return Physics.CheckSphere(characterController.transform.position + Vector3.up * (characterController.height), CHECK_CEIL_RADIUS, groundMask);
        }

        private Coroutine _changeHeadCoroutine;

        IEnumerator SetheadHeight(float duration, float targetHeight)
        {
            float time = 0;
            float changeValue = headHolder.localPosition.y;
            while (time < duration)
            {
                bool ceiling = targetHeight > changeValue && GetCeilingFree();
                if (!ceiling)
                {
                    changeValue = Mathf.Lerp(changeValue, targetHeight, time / duration);
                    UpdateHeadHeight(changeValue);
                    time += Time.deltaTime;
                }
                yield return null;
            }
            changeValue = targetHeight;
            UpdateHeadHeight(changeValue);
        }

        void UpdateHeadHeight(float newHeight)
        {
            characterController.height = newHeight + colliderHeightOffset;
            characterController.center = new Vector3(0f, (newHeight + colliderHeightOffset) / 2f, 0f);
            headHolder.localPosition = new Vector3(0f, newHeight, 0f);
        }
        #endregion

        private void OnDrawGizmos()
        {
            if(!initialised)
            {
                return;
            }
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(characterController.transform.position + Vector3.up * (characterController.height), CHECK_CEIL_RADIUS);
        }

        public void UpdateHeadBob(Vector3 velocity)
        {
            if(!initialised)
            {
                return;
            }
            var velocityMagnitude = new Vector2(velocity.x, velocity.z).magnitude;
            targetTransform.localPosition = new Vector3(0f, Mathf.Sin(Time.time * 10) * 0.05f, 0f);
            _bobIndex += Time.deltaTime * bobSpeedMultipler * Mathf.Clamp(1f + velocityMagnitude, -2f, 2f);
            float rawValue = Mathf.Sin(_bobIndex);
            if (rawValue <= -0.95f && Time.time > _lastFootStep)
            {
                _lastFootStep = Time.time + 0.1f;
                OnBobPeak();
            }
            rawValue *= _bobAmplitude * 0.1f * velocityMagnitude;
            _bobDamp = Mathf.Lerp(_bobDamp, rawValue, 5f * Time.deltaTime);
            bobVector = new Vector3(0f, _bobDamp, 0f);
            UpdateLocalHeadPosition();
        }

        void OnBobPeak()
        {
            
        }

        void UpdateLocalHeadPosition()
        {
            targetTransform.localPosition = Vector3.zero + bobVector;
        }
    }
}
