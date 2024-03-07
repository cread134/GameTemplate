using UnityEngine.AddressableAssets;
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
        public static T Load<T>(string addressablePath)
        {
            T instance = default;
            try { 
                var handle = Addressables.LoadAssetAsync<T>(addressablePath);
                handle.Completed += (op) =>
                {
                    instance = op.Result;
                };
                handle.WaitForCompletion();
                Addressables.Release(handle);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError($"Error loading asset {addressablePath} - {e.Message}");
            }
            return instance;
        }
    }
}