using Core.Interaction;
using Core.Logging;
using Core.Resources;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Interaction
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        private ILoggingService loggingService;
        private IInteractionService interactionService;

        public void Activate()
        {
            loggingService = ObjectFactory.ResolveService<ILoggingService>();
            interactionService = ObjectFactory.ResolveService<IInteractionService>();

            interactionService.SubscribeToInput<Vector2>("movement", OnMove);
            interactionService.SubscribeToInput<Vector2>("look_delta", OnLook);
            interactionService.SubscribeToInput("on_jump", OnJumpActionDown, OnJumpActionUp);
            interactionService.SubscribeToInput("on_main", OnMainActionDown, OnMainActionUp);
            interactionService.SubscribeToInput("on_secondary", OnSecondaryActionDown, OnSecondaryActionUp);
            interactionService.SubscribeToInput("on_pause", OnPause);

            loggingService.Log("PlayerController.Awake");
        }

        public void OnLook(Vector2 lookDelta)
        {
            LookDelta?.Invoke(this, lookDelta);
        }
        public void OnMove(Vector2 moveDelta)
        {
            MoveDelta?.Invoke(this, moveDelta);
        }

        public void OnJumpActionDown()
        {
            OnJumpDown?.Invoke(this, EventArgs.Empty);
        }
        public void OnJumpActionUp()
        {
            OnJumpUp?.Invoke(this, EventArgs.Empty);
        }
        public void OnMainActionDown()
        {
            OnMainDown?.Invoke(this, EventArgs.Empty);
        }
        public void OnMainActionUp()
        {
            OnMainUp?.Invoke(this, EventArgs.Empty);
        }
        public void OnSecondaryActionUp()
        {
            OnSecondaryUp?.Invoke(this, EventArgs.Empty);
        }
        public void OnSecondaryActionDown()
        {
            OnSecondaryDown?.Invoke(this, EventArgs.Empty);
        }

        public void OnPause()
        {
            OnPauseButton?.Invoke(this, EventArgs.Empty);
        }
        #region interface entry points

        public EventHandler<Vector2> LookDelta { get; set; }
        public EventHandler<Vector2> MoveDelta { get; set; }

        public EventHandler OnMainDown { get; set; }

        public EventHandler OnMainUp { get; set; }

        public EventHandler OnSecondaryDown { get; set; }

        public EventHandler OnSecondaryUp { get; set; }

        public EventHandler OnJumpDown { get; set; }
        public EventHandler OnJumpUp { get; set; }
        public EventHandler OnPauseButton { get; set; }
        #endregion
    }
}