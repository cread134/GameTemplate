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

        public void Activate()
        {
            loggingService = ObjectFactory.ResolveService<ILoggingService>();
            Debug.Log($"LoggingService null: {loggingService is null}");
            loggingService.Log("PlayerController.Awake");

        }

        public void OnLook(InputAction.CallbackContext context)
        {
            var vector2 = context.ReadValue<Vector2>();
            LookDelta?.Invoke(this, vector2);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            var vector2 = context.ReadValue<Vector2>();
            MoveDelta?.Invoke(this, vector2);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnJumpDown?.Invoke(this, EventArgs.Empty);
            }
            if(context.canceled)
            {
                OnJumpUp?.Invoke(this, EventArgs.Empty);
            }
        }

        public void OnMain(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnMainDown?.Invoke(this, EventArgs.Empty);
            }
            if (context.canceled)
            {
                OnMainUp?.Invoke(this, EventArgs.Empty);
            }
        }

        public void OnSecondary(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnSecondaryDown?.Invoke(this, EventArgs.Empty);
            }
            if (context.canceled)
            {
                OnSecondaryUp?.Invoke(this, EventArgs.Empty);
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnPauseButton?.Invoke(this, EventArgs.Empty);
            }
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