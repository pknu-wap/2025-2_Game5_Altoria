using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System.Linq;
using System.Threading;
using UnityEditor.ShaderGraph.Legacy;

namespace SceneLoad
{
    public class JsonMapLoader : Loader
    {
        CancellationTokenSource cancellationToken;

        readonly List<AsyncOperationHandle> handles = new();
        readonly List<GameObject> instantiatedObjects = new();
        readonly Dictionary<int, GameObject> instantiatedObjectsByID = new();
        readonly Dictionary<string, GameObject> prefabCache = new();
        readonly List<SceneObjectData> failedList = new();

        const int MaxRetryCount = 2;
        int maxTask = 5;
        public JsonMapLoader(string name,int task=5)
        {
            maxTask = task;
            SettingLocator(name);
#if UNITY_EDITOR
            AttachEditorSafeHelper();
#endif
        }

        public void SettingLocator(string sceneName) => Locator = sceneName;
        public override void Load() => _ = LoadAsync();

        async Task LoadAsync()
        {
            await Addressables.InitializeAsync().Task;
            cancellationToken = new CancellationTokenSource();
            var token = cancellationToken.Token;

            string key = Locator;
            string json = await ReadJsonAsync(key);

            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError($"[JsonSceneLoader] JSON 로드 실패: {key}");
                InvokeOnFailure();
                return;
            }

            var dataList = DeserializeJson(json);
            if (dataList == null || dataList.Count == 0)
            {
                Debug.LogError("[JsonSceneLoader] JSON 파싱 실패 또는 오브젝트 없음.");
                InvokeOnFailure();
                return;
            }

            
            await PreloadPrefabs(dataList, token);
            await InstantiateAllAsync(dataList, token);

        

     
            if (failedList.Count > 0)
            {
                Debug.LogWarning($"[JsonSceneLoader] {failedList.Count}개의 프리팹 재시도 시작");
                await RetryFailedPrefabs(token);
            }

            if (failedList.Count > 0)
            {
                Debug.LogError($"[JsonSceneLoader] 여전히 실패한 객체 존재: {failedList.Count}");
                InvokeOnFailure();
            }
            else
            {
                InvokeOnSuccess();
            }
        }

        async Task InstantiateAllAsync(List<SceneObjectData> dataList, CancellationToken token)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                if (token.IsCancellationRequested)
                    return;

                var data = dataList[i];
                Transform parent = GetParent(data.parentID);

                var obj = await InstantiatePrefab(data, parent, token);
                if (obj == null)
                {
                    failedList.Add(data);
                    continue;
                }

                InvokeOnProgress((float)(i + 1) / dataList.Count);

            
                if (i % maxTask == 0)
                    await Task.Yield();
            }
        }

        async Task RetryFailedPrefabs(CancellationToken token)
        {
            var retryTarget = new List<SceneObjectData>(failedList);
            failedList.Clear();

            foreach (var data in retryTarget)
            {
                int retry = 0;
                bool success = false;

                while (retry < MaxRetryCount && !success)
                {
                    var parent = GetParent(data.parentID);
                    var obj = await InstantiatePrefab(data, parent, token);

                    if (obj != null)
                    {
                        success = true;
                        break;
                    }

                    retry++;
                    await Task.Delay(100, token); 
                }

                if (!success)
                {
                    failedList.Add(data);
                    Debug.LogError($"[JsonSceneLoader] 재시도 실패: {data.PrefabAddress}");
                }

                await Task.Yield();
            }
        }

        Transform GetParent(int id)
        {
            return instantiatedObjectsByID.TryGetValue(id, out var parentObject)
                ? parentObject.transform
                : null;
        }

        async Task<string> ReadJsonAsync(string key)
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(key);
            handles.Add(handle);
            await handle.Task;
            return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result.text : null;
        }

        List<SceneObjectData> DeserializeJson(string json)
        {
            try
            {
                var wrapper = JsonUtility.FromJson<SerializationWrapper<SceneObjectData>>(json);
                return wrapper?.Items ?? new List<SceneObjectData>();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JsonSceneLoader] JSON 파싱 오류: {ex.Message}");
                return new List<SceneObjectData>();
            }
        }

        async Task PreloadPrefabs(List<SceneObjectData> dataList, CancellationToken token)
        {
            var addresses = dataList
                .Where(d => !string.IsNullOrEmpty(d.PrefabAddress))
                .Select(d => d.PrefabAddress)
                .Distinct()
                .Where(addr => !prefabCache.ContainsKey(addr))
                .ToList();

            var preloadTasks = addresses.Select(async address =>
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(address);
                handles.Add(handle);
                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                    prefabCache[address] = handle.Result;
                else
                    Debug.LogError($"[JsonSceneLoader] 프리팹 프리로드 실패: {address}");
            }).ToList();

            await Task.WhenAll(preloadTasks);
        }

        async Task<GameObject> InstantiatePrefab(SceneObjectData data, Transform parent, CancellationToken token)
        {
            if (token.IsCancellationRequested) return null;


            GameObject instance = null;

            if (!string.IsNullOrEmpty(data.PrefabAddress))
            {
                instance = await InstantiatePrefabObject(data, token);

                if (instance == null) return null;


                if (parent != null)
                {
                    instance.transform.SetParent(parent, false);
                    instance.transform.SetSiblingIndex(data.SiblingIndex);
                }

                ApplyTransform(instance.transform, data);
            }
            else if (data.SiblingIndex >= 0 && parent != null)
            {
                instance = FindContainer(data, parent);
            }

            if (instance != null)
                instantiatedObjectsByID[data.ID] = instance;

            return instance;
        }

        async Task<GameObject> InstantiatePrefabObject(SceneObjectData data, CancellationToken token)
        {
            if (prefabCache.TryGetValue(data.PrefabAddress, out var prefab))
            {
                var instance = GameObject.Instantiate(prefab);
                instantiatedObjects.Add(instance);
                return instance;
            }
            else
            {
                var handle = Addressables.InstantiateAsync(data.PrefabAddress);
                handles.Add(handle);
                await handle.Task;

                if (token.IsCancellationRequested || handle.Status != AsyncOperationStatus.Succeeded)
                    return null;

                var instance = handle.Result;
                instantiatedObjects.Add(instance);
                return instance;
            }
        }

        GameObject FindContainer(SceneObjectData data, Transform parent)
        {
            if (instantiatedObjectsByID.TryGetValue(data.parentID, out var parentObj)
                && data.SiblingIndex >= 0
                && data.SiblingIndex < parentObj.transform.childCount)
            {
                var container = parentObj.transform.GetChild(data.SiblingIndex);
                ApplyTransform(container, data);
                return container.gameObject;
            }

            Debug.LogWarning($"[JsonSceneLoader] 잘못된 컨테이너 인덱스: {data.SiblingIndex}");
            return null;
        }

        void ApplyTransform(Transform t, SceneObjectData data)
        {
            t.localPosition = data.Position;
            t.localRotation = data.Rotation;
            t.localScale = data.Scale;
        }

        #region UnLoad

        public override void UnLoad()
        {
            CancelLoading();
            ReleaseInstantiatedObjects();
            ReleaseLoadedAssets();
            instantiatedObjectsByID.Clear();
            prefabCache.Clear();
        }

        void CancelLoading()
        {
            if (cancellationToken == null)
                return;

            try
            {
                if (!cancellationToken.IsCancellationRequested)
                    cancellationToken.Cancel();
            }
            catch { }
            finally
            {
                cancellationToken.Dispose();
                cancellationToken = null;
            }
        }

        void ReleaseInstantiatedObjects()
        {
            foreach (var obj in instantiatedObjects)
            {
                if (obj == null) continue;

                try
                {
                    if (Addressables.ResourceManager != null)
                        Addressables.ReleaseInstance(obj);
                    else
                        GameObject.Destroy(obj);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"[JsonSceneLoader] ReleaseInstance 실패: {e.Message}");
                }
            }
            instantiatedObjects.Clear();
        }

        void ReleaseLoadedAssets()
        {
            foreach (var handle in handles)
            {
                if (!handle.IsValid()) continue;

                try { Addressables.Release(handle); }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[JsonSceneLoader] 핸들 해제 예외: {ex.Message}");
                }
            }
            handles.Clear();
        }
        #endregion

        #region UnityEditor
#if UNITY_EDITOR
        void AttachEditorSafeHelper()
        {
            if (GameObject.Find(nameof(JsonMapLoaderComponent)) != null)
                return;

            var safeGo = new GameObject(nameof(JsonMapLoaderComponent));
            UnityEngine.Object.DontDestroyOnLoad(safeGo);
            safeGo.AddComponent<JsonMapLoaderComponent>().SetLoader(this);
        }
#endif
        #endregion
    }
}
