using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerResources
{
    public interface IPlayerResources 
    {
        void ConfigureResources(List<IPlayerBehaviour> playerBehaviours);
        T GetBehaviourResource<T>() where T : class, IPlayerBehaviour;
        T GetComponentResource<T>() where T : Component;
        LayerMask GetLayerMaskResource(string name);
    }
}