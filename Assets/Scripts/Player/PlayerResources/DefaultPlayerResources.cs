using Core.Logging;
using Core.Resources;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerResources
{
    public class DefaultPlayerResources : MonoBehaviour, IPlayerResources
    {
        [SerializeField] private List<Component> m_Components;
        [SerializeField] private LayerMaskConfiguration m_PlayerLayerMasks;

        private List<IPlayerBehaviour> m_Behaviours;

        private ILoggingService _loggingService;
        public void ConfigureResources(List<IPlayerBehaviour> playerBehaviours)
        {
            m_Behaviours = playerBehaviours;

            _loggingService = ObjectFactory.ResolveService<ILoggingService>();
        }

        public T GetBehaviourResource<T>() where T : class, IPlayerBehaviour
        {
            var foundBehaviour = m_Behaviours.Find(b => b is T);
            if (foundBehaviour == null)
            {
                _loggingService.LogError($"Behaviour of type {typeof(T)} not found in player resources");
                return default(T);
            }
            return foundBehaviour as T;
        }

        public T GetComponentResource<T>() where T : Component
        {
            var foundComponent = m_Components.Find(c => c is T);
            if (foundComponent == null)
            {
                _loggingService.LogError($"Component of type {typeof(T)} not found in player resources");
                return null;
            }
            return foundComponent as T;
        }

        public LayerMask GetLayerMaskResource(string name)
        {
            var mask = m_PlayerLayerMasks.GetLayerMask(name);
            if (mask == default)
            {
                _loggingService.LogError($"Layer mask {name} not found in player resources");
            }
            return mask;
        }
    }
}