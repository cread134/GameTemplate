using Core.Audio;
using Core.Logging;
using UnityEngine;

namespace Core.Resources
{
    public static class ResourcesConfiguration
    {
        public static void RegisterServices()
        {
            ObjectFactory.RegisterService<IAudioManager, AudioManager>(serviceType: ObjectFactory.ServiceType.Singleton);
            
            ConfigureLogging();
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