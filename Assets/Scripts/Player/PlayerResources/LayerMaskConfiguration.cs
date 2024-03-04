using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerResources
{
    [CreateAssetMenu]
    public class LayerMaskConfiguration : ScriptableObject
    {
        [System.Serializable]
        private struct LayerMaskConfig
        {
            public string layerMaskName;
            public LayerMask mask;
        }

        [SerializeField] private LayerMaskConfig[] layerMasks;

        private Dictionary<string, LayerMask> _layerMaskDictionary;
        private Dictionary<string, LayerMask> MaskDictionary
        {
            get
            {
                if (_layerMaskDictionary == null)
                {
                    _layerMaskDictionary = new Dictionary<string, LayerMask>();
                    foreach (var mask in layerMasks)
                    {
                        _layerMaskDictionary.Add(mask.layerMaskName.ToLowerInvariant(), mask.mask);
                    }
                }
                return _layerMaskDictionary;
            }
        }
        public LayerMask GetLayerMask(string maskName)
        {
            if (MaskDictionary.TryGetValue(maskName.ToLowerInvariant(), out var layerMask))
            {
                return layerMask;
            }
            return default;
        }

        public static int GetLayerMask(params string[] layerNames)
        {
            int layerMask = 0;
            foreach (var layerName in layerNames)
            {
                layerMask |= 1 << LayerMask.NameToLayer(layerName);
            }
            return layerMask;
        }
    }
}