using System;
using UnityEngine.InputSystem;

namespace Core.Interaction
{
    public class InteractionMappingInstance
    {
        public InteractionMappingInstance(InteractionMapping interactionMapping)
        {
            Mapping = interactionMapping;
        }
        public Func<T> GetValueCallback<T>(InputAction.CallbackContext context) where T : struct
        {
            return context.ReadValue<T>;
        }
        public InteractionMapping Mapping;
        public EventHandler<InputAction.CallbackContext> ActionEvent;
    }
}
