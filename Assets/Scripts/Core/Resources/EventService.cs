using System;
using UnityEngine;

namespace Core.Resources
{
    public class EventService : MonoBehaviour, IEventService
    {
        bool initialised;

        public void OnResourceCreating()
        {
            DontDestroyOnLoad(gameObject);
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