using UnityEngine.InputSystem;

namespace Core.Interaction
{
    [System.Serializable]
    public class InteractionMapping
    {
        public string Name;
        public enum MappingType { upDownAction, triggered, performed, canceled, value }  
        public MappingType ActionCallbackType;
        public InputAction ActionMapping;
    }
}