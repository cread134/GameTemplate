using UnityEngine;

namespace Core.Interaction
{
    [CreateAssetMenu]
    public class InteractionConfiguration : ScriptableObject
    {
        public InteractionMapping[] InteractionMappings;

    }
}