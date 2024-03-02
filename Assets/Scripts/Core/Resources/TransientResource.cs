using System;

namespace Core.Resources
{
    internal class TransientResource<T, U> : IGameResource where U : class where T : class
    {
        public object Resolve()
        {
            return (T)Activator.CreateInstance(typeof(U));
        }
    }
}