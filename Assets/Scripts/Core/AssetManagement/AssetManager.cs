using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace Core.AssetManagement
{
    public class AssetManager
    {
        /// <summary>
        /// Loads syncronously an asset from the addressable path. Do not use when loading large / multiple assets
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="addressablePath"></param>
        /// <returns></returns>
        public static T Load<T>(string addressablePath, out AsyncOperationHandle<T> handle)
        {
            T instance = default;
            try { 
                handle = Addressables.LoadAssetAsync<T>(addressablePath);
                handle.Completed += (op) =>
                {
                    instance = op.Result;
                };
                handle.WaitForCompletion();
            }
            catch (System.Exception e)
            {
                handle = default;
                UnityEngine.Debug.LogError($"Error loading asset {addressablePath} - {e.Message}");
            }
            return instance;
        }
    }
}