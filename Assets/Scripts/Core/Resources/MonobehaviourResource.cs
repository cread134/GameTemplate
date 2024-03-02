using UnityEngine;

namespace Core.Resources
{
    internal class MonobehaviourResource<T, U> : IGameResource where U : class where T : class
    {
        private T _resource;
        public MonobehaviourResource()
        {
            var type = typeof(T);
            var resourceInstance = new GameObject(type.Name, typeof(U));
            _resource = resourceInstance.GetComponent<U>() as T; 
        }
        public object Resolve()
        {
            return _resource;
        }
    }
}