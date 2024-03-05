using System;
using UnityEngine;

namespace Core.Resources
{
    public class EventService : MonoBehaviour, IEventService
    {
        bool initialised;

        public void OnResourceCreating()
        {
            initialised = true;
        }

        private void LateUpdate()
        {
            if(!initialised)
            {
                return;
            }
            OnLateUpdate?.Invoke(this, EventArgs.Empty);
        }
        public EventHandler OnLateUpdate { get; set; }
    }
}