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
                return _instance ??= new ObjectFactory();
            }
            private set
            {
                _instance = value;
            }
        }
        private static ObjectFactory _instance;
        private static Dictionary<Type, Func<IGameService>> services;

        #endregion

        public ObjectFactory()
        {
            services = new Dictionary<Type, Func<IGameService>>();
            ResourcesConfiguration.RegisterServices();
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

        public static void RegisterService<TInterface, T>(ServiceType serviceType) where T : TInterface
        {
            Instance.Register<TInterface, T>(serviceType);
        }
        void Register<TInterface, T>(ServiceType serviceType) where T : TInterface
        {
            if (services.ContainsKey(typeof(TInterface)))
            {
                Debug.LogError($"Service of type {typeof(TInterface)} already registered");
                return;
            }
            services.Add(typeof(TInterface), CreateServiceInstance<T>(serviceType));
            Debug.Log($"Service of type {typeof(TInterface)} registered as {serviceType} : service type {serviceType}");
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
            return null;
        }

        IGameService ServiceCreator(Type type)
        {
            IGameService service = null;
            if(typeof(MonoBehaviour).IsAssignableFrom(type))
            {
                GameObject gameObject = new GameObject(type.Name);
                service = gameObject.AddComponent(type) as IGameService;
                service.OnServiceRegistering();
            } else
            {

            }
            return service;
        }
    }
}