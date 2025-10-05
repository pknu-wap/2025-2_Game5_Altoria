using GameData;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class JsonLoader<T> : ILoadStrategy<T> where T : new()
{
    [System.Serializable]
    private class Wrapper { public List<T> rows; }

    public async Task<IList<T>> LoadAsync(string address)
    {
        var handle = Addressables.LoadAssetAsync<TextAsset>(address);
        await handle.Task;

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogWarning($"Failed to load JSON Addressable: {address}");
            return new List<T>();
        }

        string json = handle.Result.text;
        var wrapper = JsonUtility.FromJson<Wrapper>(json);

        return wrapper?.rows ?? new List<T>();
    }
}
public static class SoLoader<T> where T : ScriptableObject
{
    public static async Task<List<T>> LoadAsync(string labelOrAddress)
    {
        var handle = Addressables.LoadAssetsAsync<T>(labelOrAddress, null);
        await handle.Task;

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogWarning($"Failed to load ScriptableObjects: {labelOrAddress}");
            return new List<T>();
        }

        return new List<T>(handle.Result);
    }

    public static void Save(string path, IList<T> data)
    {
        Debug.LogWarning("Saving ScriptableObject at runtime is not supported. Use UnityEditor API instead.");
    }
}
