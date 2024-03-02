using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core.Resources
{
    public class ObjectFactory
    {
        #region singleton
        public static ObjectFactory Instance
        {
            get
            {
                if (_instance is null)
                {
                    ConfigureFactory();
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }
        private static ObjectFactory _instance;
        private Dictionary<Type, Func<IGameService>> services;

        #endregion

        public static void ConfigureFactory()
        {
            if (_instance is not null) return;
            _instance = new ObjectFactory();

            ResourcesConfiguration.RegisterServices();
        }

        public ObjectFactory()
        {
            services = new Dictionary<Type, Func<IGameService>>();
        }

        public enum ServiceType
        {
            Singleton,
            Transient
        }

        public static T ResolveService<T>() => Instance.Resolve<T>();
        T Resolve<T>()
        {
            if (services.TryGetValue(typeof(T), out var service))
            {
                return (T)service();
            }
            Debug.LogError($"Service of type {typeof(T)} not found");
            return default;
        }

        public static void RegisterService<TInterface, T>(ServiceType serviceType) where T : TInterface, IGameService
        {
            Instance.Register<TInterface, T>(serviceType);
        }
        void Register<TInterface, T>(ServiceType serviceType) where T : TInterface, IGameService
        {
            if(IsMonoBehaviour(typeof(T)) && serviceType == ServiceType.Transient)
            {
                Debug.Log($"registering monobehaviour type {serviceType} to transient is not supported");
                serviceType = ServiceType.Singleton;
            }

            if (services.ContainsKey(typeof(TInterface)))
            {
                Debug.LogError($"Service of type {typeof(TInterface)} already registered");
                return;
            }
            services.Add(typeof(TInterface), CreateServiceInstance<T>(serviceType));
            Debug.Log($"Service of type {typeof(TInterface)} registered as {serviceType} : service type {serviceType}");
        }

        bool IsMonoBehaviour(Type type)
        {
            return typeof(MonoBehaviour).IsAssignableFrom(type);
        }

        Func<IGameService> CreateServiceInstance<T>(ServiceType serviceType)
        {
            var type = typeof(T);
            switch (serviceType)
            {
                case ServiceType.Singleton:
                    var instance = ServiceCreator(type);
                    return () => instance;
                case ServiceType.Transient:
                    return () => {
                        return ServiceCreator(type);
                     };
            }
            return () => {
                    Debug.LogError($"Service instance for {typeof(T)} failed");
                    return null;
                };
        }

        IGameService ServiceCreator(Type type)
        {
            IGameService service = null;
            if(IsMonoBehaviour(type))
            {
                GameObject gameObject = new GameObject(type.Name);         
                service = gameObject.AddComponent(type) as IGameService;
                service.OnServiceRegistering();
            } else
            {
                IGameService instance = Activator.CreateInstance(type) as IGameService;
                instance.OnServiceRegistering();
            }
            return service;
        }
    }
}