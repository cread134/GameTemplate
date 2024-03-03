using UnityEngine;

namespace Player.Interaction
{
    [CreateAssetMenu]
    public class MouseLookConfiguration : ScriptableObject
    {
        public float lookSensitivity_X = 0.1f;
        public float lookSensitivity_Y = 0.1f;
    }
}