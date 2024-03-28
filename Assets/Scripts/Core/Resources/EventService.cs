using System;
using UnityEngine;

namespace Core.Resources
{
    internal class EventService : MonobehaviourResourceInstance, IEventService
    {
        bool initialised;

        internal override void OnCreating()
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