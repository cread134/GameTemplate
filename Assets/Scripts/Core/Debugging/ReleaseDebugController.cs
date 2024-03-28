using System;
using UnityEngine;

namespace Core.Debugging
{
    internal class ReleaseDebugController : MonoBehaviour, IDebugController
    {
        public void OnResourceCreating()
        {
        }

        public void SubscribeView(string name, Func<string> message)
        {
        }

        public void Unsubscribe(string name)
        {
        }
    }
}