using Core.Audio;
using Core.Resources;
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
        [SerializeField] private AudioObject footStepAudio;
        [SerializeField] private float colliderHeightOffset = 0.1f;

        private CharacterController characterController;
        private IAudioManager audioManager;
        private LayerMask groundMask;

        const float CHECK_CEIL_RADIUS = 0.05f;

        [SerializeField] private float _bobAmplitude = 0.05f;
        [SerializeField] private float bobSpeedMultipler = 1f;

        const float ANIMATION_DAMPING = 5f;

        float _bobDamp = 4f;
        float _bobIndex;
        float _lastFootStep;
        Vector3 bobVector;
        Vector3 animModulation;
        Vector3 animationDamper;

        bool initialised = false;
        public void OnBehaviourInit(IPlayerController playerController, IPlayerResources playerResources)
        {
            characterController = playerResources.GetComponentResource<CharacterController>();   
            groundMask = playerResources.GetLayerMaskResource("ground");
            audioManager = ObjectFactory.ResolveService<IAudioManager>();
            initialised = true;
        }

        public void StartBehaviour()
        {
        }

        private void Update()
        {
            if(!initialised)
            {
                return;
            }
            if (Vector3.Magnitude(animationDamper) > 0.05f && !playingAnimation)
            {
                DampAnimation();
            }
        }

        void DampAnimation()
        {
            animationDamper = Vector3.Lerp(animationDamper, animModulation, ANIMATION_DAMPING * Time.deltaTime);
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
            if (rawValue <= -0.95f && Time.time > _lastFootStep && velocityMagnitude > 0.2f)
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
            audioManager.PlaySound(footStepAudio, characterController.transform.position);
        }

        void UpdateLocalHeadPosition()
        {
            targetTransform.localPosition = Vector3.zero;
            targetTransform.localPosition += bobVector;
            targetTransform.localPosition += animationDamper;
        }

        Coroutine headAnimCoroutine;
        private bool playingAnimation;

        public void DoHeadAnimation(AnimationCurve curve_X, AnimationCurve curve_Y, AnimationCurve curve_Z, float duration, float multiplier = 1f)
        {
            if (headAnimCoroutine != null)
            {
                StopCoroutine(headAnimCoroutine);
                playingAnimation = false;
            }
            headAnimCoroutine = StartCoroutine(HeadAnimation(curve_X, curve_Y, curve_Z, duration, multiplier));
        }

        IEnumerator HeadAnimation(AnimationCurve curve_X, AnimationCurve curve_Y, AnimationCurve curve_Z, float duration, float multiplier)
        {
            playingAnimation = true;
            animModulation = Vector3.zero;
            float time = 0;
            while (time < duration)
            {
                float x = curve_X?.Evaluate(time / duration) ?? 0f;
                float y = curve_Y?.Evaluate(time / duration) ?? 0f;
                float z = curve_Z?.Evaluate(time / duration) ?? 0f;
                animModulation = new Vector3(x, y, z) * multiplier;
                DampAnimation();
                time += Time.deltaTime;
                UpdateLocalHeadPosition();
                yield return null;
            }
            animModulation = Vector3.zero;
            UpdateLocalHeadPosition();
            playingAnimation = false;
        }
    }
}
