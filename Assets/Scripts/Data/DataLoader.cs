using GameData;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace ILoader
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
    [System.Serializable]
    public class JsonWrapper<T>
    {
        public List<T> rows;
    }
    public class JsonLoader<T> : ILoadStrategy<T> where T : new()
    {
        public async Task<IList<T>> LoadAsync(string address)
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(address);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogWarning($"Failed to load JSON Addressable: {address}");
                return new List<T>();
            }

            string json = handle.Result.text.Trim('\uFEFF', '\u200B', '\u0000', '\t', '\r', '\n', ' ');
            try
            {
                var wrapper = JsonUtility.FromJson<JsonWrapper<T>>(json);
                return wrapper?.rows ?? new List<T>();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"JSON parse failed for {address}:\n{e}\nText: {json}");
                return new List<T>();
            }
        }
    }

    public class CsvLoader<T> : ILoadStrategy<T> where T : new()
    {
        public async Task<IList<T>> LoadAsync(string address)
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(address);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogWarning($"Failed to load CSV Addressable: {address}");
                return new List<T>();
            }

            string csvText = handle.Result.text;
            return ParseCsv(csvText);
        }

        private static List<T> ParseCsv(string csvText)
        {
            var result = new List<T>();
            var lines = csvText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2) return result;

            var headers = lines[0].Split(',');

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                var obj = new T();

                for (int j = 0; j < headers.Length && j < values.Length; j++)
                {
                    string header = headers[j].Trim();
                    string value = values[j].Trim();

                    var field = typeof(T).GetField(header, BindingFlags.Public | BindingFlags.Instance);
                    var prop = typeof(T).GetProperty(header, BindingFlags.Public | BindingFlags.Instance);

                    if (field != null)
                    {
                        field.SetValue(obj, ConvertValue(value, field.FieldType));
                    }
                    else if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(obj, ConvertValue(value, prop.PropertyType));
                    }
                }

                result.Add(obj);
            }

            return result;
        }

        private static object ConvertValue(string value, Type type)
        {
            if (type == typeof(string)) return value;
            if (type == typeof(int)) return int.TryParse(value, out var i) ? i : 0;
            if (type == typeof(float)) return float.TryParse(value, out var f) ? f : 0f;
            if (type == typeof(bool)) return bool.TryParse(value, out var b) ? b : false;
            if (type.IsEnum)
            {
                try { return Enum.Parse(type, value, true); } catch { return Activator.CreateInstance(type); }
            }

            return null;
        }
    }
}

