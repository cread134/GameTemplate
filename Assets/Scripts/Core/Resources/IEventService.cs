using System;

namespace Core.Resources
{
    public interface IEventService : IResourceInstance
    {
        public EventHandler OnLateUpdate { get; set; }
    }
}