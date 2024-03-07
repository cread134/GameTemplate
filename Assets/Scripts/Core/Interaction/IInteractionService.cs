using Core.Resources;
using System;
using UnityEngine.InputSystem;

namespace Core.Interaction
{
    public interface IInteractionService : IResourceInstance
    {
        public void SubscribeToInput(string inputName, Action callback);
        public void RegisterInputAction(string name, InputAction inputAction);
    }
}