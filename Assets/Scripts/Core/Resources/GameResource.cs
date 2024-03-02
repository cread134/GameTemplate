using System;

namespace Core.Resources
{
    internal class GameResource<T, U> : IGameResource where U : class where T : class
    {
        private T _resource;
        
        public GameResource()
        {
            _resource = (T)Activator.CreateInstance(typeof(U));
        }
        public object Resolve()
        {
            return _resource;
        }
    }
}