using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ServiceLocator
{
    public static class GlobalServices
    {
        private const string GlobalServicesPrefabName = "GlobalServices";

        public static Dictionary<Type, MonoBehaviour> _instantiatedServices = new Dictionary<Type, MonoBehaviour>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Load()
        {
            GlobalServicesData[] dataPrefabs = Resources.LoadAll<GlobalServicesData>(GlobalServicesPrefabName);

            switch (dataPrefabs.Length)
            {
                case 0:
                    Debug.LogWarning("Global services prefab is missing");
                    break;
                case > 1:
                    Debug.LogError("Multiple global services prefabs detected");
                    break;
                default:
                    GlobalServicesData data = dataPrefabs[0];
                    Load(data);
                    break;
            }
        }

        private static void Load(GlobalServicesData data)
        {
            foreach (MonoBehaviour service in data.Services)
            {
                MonoBehaviour instantiatedService = InstantiateService(service);

                _instantiatedServices.Add(instantiatedService.GetType(), instantiatedService);
            }
        }

        private static MonoBehaviour InstantiateService(MonoBehaviour service)
        {
            GameObject instantiatedServiceObject = Object.Instantiate(service.gameObject);
            Object.DontDestroyOnLoad(instantiatedServiceObject);

            Type serviceType = service.GetType();
            MonoBehaviour instantiatedService = (MonoBehaviour) instantiatedServiceObject.GetComponent(serviceType);

            return instantiatedService;
        }

        public static T Get<T>() where T : MonoBehaviour
        {
            Type serviceType = typeof(T);

            bool isServiceFound = _instantiatedServices.TryGetValue(serviceType, out MonoBehaviour service);

            if (!isServiceFound)
                throw new MissingServiceException(serviceType);

            return (T) service;
        }
    }
}