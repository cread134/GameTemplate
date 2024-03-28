using Core.Application;
using Core.Audio;
using Core.Debugging;
using Core.Interaction;
using Core.Logging;
using Core.SceneManagement;
using Core.Vfx;
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
            ObjectFactory.RegisterService<IVfxManager, VfxManager>(ObjectFactory.ServiceType.Monobehaviour);
            ObjectFactory.RegisterService<IInteractionService, InteractionService>(ObjectFactory.ServiceType.Monobehaviour);
            ConfigureDebug();
            ObjectFactory.RegisterService<ISceneLoader, SceneLoader>(ObjectFactory.ServiceType.Monobehaviour);
        }

        static void ConfigureDebug()
        {
            if (AppSettings.IsDebugMode)
            {
                ObjectFactory.RegisterService<IDebugController, DebugController>(ObjectFactory.ServiceType.Monobehaviour);
            } else
            {
                ObjectFactory.RegisterService<IDebugController, ReleaseDebugController>(ObjectFactory.ServiceType.Monobehaviour);
            }
        }

        static void ConfigureLogging()
        {
            if (UnityEngine.Application.isEditor)
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