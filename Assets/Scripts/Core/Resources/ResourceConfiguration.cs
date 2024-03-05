using Core.Audio;
using Core.Debugging;
using Core.Logging;
using UnityEngine;

namespace Core.Resources
{
    public static class ResourcesConfiguration
    {
        public static void RegisterServices()
        {
            ConfigureLogging();
            ObjectFactory.RegisterService<IEventService, EventService>(ObjectFactory.ServiceType.Monobehaviour);
            ObjectFactory.RegisterService<IUiResources, UiResources>(ObjectFactory.ServiceType.Singleton);
            ObjectFactory.RegisterService<IAudioManager, AudioManager>(ObjectFactory.ServiceType.Monobehaviour);
            ObjectFactory.RegisterService<IDebugController, DebugController>(ObjectFactory.ServiceType.Monobehaviour);
        }

        static void ConfigureLogging()
        {
            if (Application.isEditor)
            {
                ObjectFactory.RegisterService<ILoggingService, LoggingService>(serviceType: ObjectFactory.ServiceType.Singleton);
            }
            else
            {
                ObjectFactory.RegisterService<ILoggingService, BuildLoggingService>(serviceType: ObjectFactory.ServiceType.Singleton);
            }
        }
    }
}