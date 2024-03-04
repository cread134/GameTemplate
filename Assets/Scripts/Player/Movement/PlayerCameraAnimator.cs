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

        public void OnBehaviourInit(IPlayerController playerController, IPlayerResources playerResources)
        {
            characterController = playerResources.GetComponentResource<CharacterController>();   
            groundMask = playerResources.GetLayerMaskResource("ground");
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
            return Physics.CheckSphere(characterController.transform.position + Vector3.up * (characterController.height - CHECK_CEIL_RADIUS + CHECK_CEIL_RADIUS), CHECK_CEIL_RADIUS, groundMask);
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
    }
}
