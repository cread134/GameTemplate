using System;

namespace Core.Resources
{
    internal class TransientResource<T, U> : IGameResource where U : class where T : class
    {
        public object Resolve()
        {
            var instance = (T)Activator.CreateInstance(typeof(U));
            if (instance is IResourceInstance createdResource)
            {
                createdResource.OnResourceCreating();
            }
            return instance;
        }
    }
}