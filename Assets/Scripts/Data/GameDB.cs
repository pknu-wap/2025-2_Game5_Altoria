using GameInteract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace GameData
{
    public interface ILoadStrategy<T> { Task<IList<T>> LoadAsync(string sourceKey); }

    public class AddressableLoader<T> : ILoadStrategy<T>
    {
        public async Task<IList<T>> LoadAsync(string addressKey)
        {
            var handle = Addressables.LoadAssetsAsync<T>(addressKey, null);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"[AddressableLoader] Failed to load from {addressKey}");
                return Array.Empty<T>();
            }

            var result = handle.Result;
            Addressables.Release(handle);
            return result;
        }
    }

}