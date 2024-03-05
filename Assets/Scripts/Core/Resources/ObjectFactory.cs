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
        private Dictionary<Type, IGameResource> services;

        #endregion

        public static void ConfigureFactory()
        {
            if (_instance is not null) return;
            _instance = new ObjectFactory();

            ResourcesConfiguration.RegisterServices();
        }

        public ObjectFactory()
        {
            services = new Dictionary<Type, IGameResource>();
        }

        public enum ServiceType
        {
            Singleton,
            Transient,
            Monobehaviour
        }

        public static T ResolveService<T>() => Instance.Resolve<T>();
        T Resolve<T>()
        {
            if (services.TryGetValue(typeof(T), out var service))
            {
                Debug.Log($"Service of type {typeof(T)} found");
                return (T)(service.Resolve());
            }
            Debug.LogError($"Service of type {typeof(T)} not found");
            return default;
        }

        public static void RegisterService<T, U>(ServiceType serviceType) where U : class where T : class
        {
            Instance.Register<T, U>(serviceType);
        }
        void Register<T, U>(ServiceType serviceType) where U : class where T : class
        {
            if(IsMonoBehaviour(typeof(T)) && serviceType != ServiceType.Monobehaviour)
            {
                Debug.Log($"registering monobehaviour type {serviceType} to non-monobehaviour is not supported");
                serviceType = ServiceType.Monobehaviour;
            }

            if (services.ContainsKey(typeof(T)))
            {
                Debug.LogError($"Service of type {typeof(T)} already registered");
                return;
            }
            services.Add(typeof(T), CreateServiceInstance<T, U>(serviceType));
            Debug.Log($"Service of type {typeof(T)} registered as {serviceType} : service type {serviceType}");
        }

        bool IsMonoBehaviour(Type type)
        {
            return typeof(MonoBehaviour).IsAssignableFrom(type);
        }

        IGameResource CreateServiceInstance<T, U>(ServiceType serviceType) where U : class where T : class
        {
            switch (serviceType)
            {
                case ServiceType.Singleton:
                    return new SingletonResource<T, U>();
                case ServiceType.Transient:
                    return new TransientResource<T, U>();
                case ServiceType.Monobehaviour:
                    return new MonobehaviourResource<T, U>();
            }
            Debug.LogError($"Service instance for {typeof(T)} failed");
            return null;
        }
    }
}