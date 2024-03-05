using Core.Resources;
using System;

namespace Core.Debugging
{
    public interface IDebugController : IResourceInstance
    {
        public void Unsubscribe(string name);
        public void SubscribeView(string name, Func<string> message);
    }
}