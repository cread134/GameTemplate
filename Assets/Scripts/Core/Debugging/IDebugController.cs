using Core.Resources;
using System;

namespace Core.Debugging
{
    public interface IDebugController
    {
        public void Unsubscribe(string name);
        public void SubscribeView(string name, Func<string> message);
    }
}