using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Interaction
{
    [CreateAssetMenu]
    public class InteractionConfiguration : ScriptableObject
    {
        public InteractionMapping[] InteractionMappings;

        [System.Serializable]
        public struct InteractionMapping
        {
            public string Name;
            public InputAction ActionMapping;
        }
    }
}