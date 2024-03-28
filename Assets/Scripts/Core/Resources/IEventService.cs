using System;

namespace Core.Resources
{
    public interface IEventService
    {
        public EventHandler OnLateUpdate { get; set; }
    }
}