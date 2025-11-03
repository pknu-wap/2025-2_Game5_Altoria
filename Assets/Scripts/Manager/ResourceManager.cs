using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;
public class InstantiateOptions
{
    public Vector3 Position { get; set; } = Vector3.zero;
    public Quaternion Rotation { get; set; } = Quaternion.identity;
    public Vector3 Scale { get; set; } = Vector3.one;
    public Transform Parent { get; set; } = null;

    public bool DontDestroyOnLoad { get; set; } = false;
    public float LifeTime { get; set; } = -1f; 
    public string Tag { get; set; } = null;
    public int Layer { get; set; } = -1;
    public bool SetActiveOnInstantiate { get; set; } = true;
}
public class ResourceManager
{
    readonly Dictionary<string, Object> cache = new();
    readonly Dictionary<string, AsyncOperationHandle> handles = new();
    readonly HashSet<GameObject> instances = new(); 

    public int Count => handles.Count;

 
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : Object
    {
        if (cache.TryGetValue(key, out var resource))
        {
            callback?.Invoke(resource as T);
            return;
        }

        if (handles.TryGetValue(key, out var existingHandle))
        {
            existingHandle.Completed += op => callback?.Invoke(op.Result as T);
            return;
        }

        var handle = Addressables.LoadAssetAsync<T>(key);
        handles[key] = handle;

        handle.Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                cache[key] = op.Result;
                callback?.Invoke(op.Result as T);
            }
            else
            {
                Debug.LogError($"[ResourceManager] Failed to load: {key}");
            }
        };
    }

   
    public void InstantiateAsync(string key, Transform parent = null, Action<GameObject> callback = null)
    {
        var handle = Addressables.InstantiateAsync(key, parent);
        handle.Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                var obj = op.Result;
                instances.Add(obj);
                callback?.Invoke(obj);
            }
            else
            {
                Debug.LogError($"[ResourceManager] Failed to instantiate: {key}");
            }
        };
    }


    public void Instantiate(string key, InstantiateOptions options = null, Action<GameObject> callback = null)
    {
        options ??= new InstantiateOptions();

        LoadAsync<GameObject>(key, prefab =>
        {
            if (prefab == null) return;

            var obj = Object.Instantiate(prefab, options.Position, options.Rotation, options.Parent);
            obj.name = prefab.name;
            obj.transform.localScale = options.Scale;

            if (!options.SetActiveOnInstantiate)
                obj.SetActive(false);

            if (options.Tag != null)
                obj.tag = options.Tag;

            if (options.Layer >= 0)
                obj.layer = options.Layer;

            if (options.DontDestroyOnLoad)
                Object.DontDestroyOnLoad(obj);

            if (options.LifeTime > 0)
                Object.Destroy(obj, options.LifeTime);

            instances.Add(obj);
            callback?.Invoke(obj);
        });
    }


    public void Destroy(GameObject obj, float delay = 0f)
    {
        if (obj == null) return;

        if (instances.Contains(obj))
        {
          
            Addressables.ReleaseInstance(obj);
            instances.Remove(obj);
        }
        else
        {
            Object.Destroy(obj, delay);
        }
    }

    public void Remove(string key)
    {
        if (cache.ContainsKey(key))
            cache.Remove(key);

        if (handles.TryGetValue(key, out var handle))
        {
            Addressables.Release(handle);
            handles.Remove(key);
        }
    }
    public T Load<T>(string key) where T : Object
    {
        if (cache.TryGetValue(key, out var resource))
            return resource as T;

        var handle = Addressables.LoadAssetAsync<T>(key);
        handle.WaitForCompletion(); 
        cache[key] = handle.Result;
        handles[key] = handle;
        return handle.Result;
    }
    public void Clear()
    {
       
        var objs = new List<GameObject>(instances);
        for (int i = 0; i < objs.Count; i++)
            Addressables.ReleaseInstance(objs[i]);
        instances.Clear();


        var keys = new List<string>(handles.Keys);
        for (int i = 0; i < keys.Count; i++)
            Addressables.Release(handles[keys[i]]);
        handles.Clear();

        cache.Clear();
    }
}
