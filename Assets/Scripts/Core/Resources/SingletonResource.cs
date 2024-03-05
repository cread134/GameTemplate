using System;

namespace Core.Resources
{
    internal class SingletonResource<T, U> : IGameResource where U : class where T : class
    {
        private T _resource;
        
        public SingletonResource()
        {
            _resource = (T)Activator.CreateInstance(typeof(U));
            if (_resource is IResourceInstance createdResource)
            {
                createdResource.OnResourceCreating();
            }
        }
        public object Resolve()
        {
            return _resource;
        }
    }
}