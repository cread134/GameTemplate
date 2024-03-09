using Core.Resources;
using System;
using UnityEngine.InputSystem;

namespace Core.Interaction
{
    public interface IInteractionService : IResourceInstance
    {
        public void SubscribeToInput(string inputName, Action callback);
        public void RegisterInputAction(string name, InteractionMapping inputAction);
        void SubscribeToInput(string inputName, Action<InputAction.CallbackContext> callback);
        void SubscribeToInput<T>(string inputName, Action<T> callback) where T : struct;
        void SubscribeToInput(string inputName, Action onActionCallback, Action onActionUpCallback);
    }
}