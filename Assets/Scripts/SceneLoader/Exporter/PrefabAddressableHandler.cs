using UnityEditor.AddressableAssets;
using UnityEditor;
using UnityEngine;

namespace SceneLoader
{
    public static class PrefabAddressableHandler
    {
        public static GameObject ConvertToPrefabOnly(GameObject obj, string customSavePath)
        {
            EnsureFolderExists(customSavePath);

            string path = AssetDatabase.GenerateUniqueAssetPath($"{customSavePath}/{obj.name}.prefab");
            GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(obj, path, InteractionMode.AutomatedAction);
            Debug.Log($"[PrefabAddressableHandler] Prefab created at: {path}");
            return prefab;
        }
        public static bool TryGetAssetRelativePath(string fullPath, out string assetPath)
        {
            assetPath = null;
            if (string.IsNullOrEmpty(fullPath)) return false;
            if (!fullPath.StartsWith(Application.dataPath)) return false;

            assetPath = "Assets" + fullPath.Substring(Application.dataPath.Length);
            return true;
        }
        public static bool TryGetAddress(GameObject prefab, out string address)
        {
            address = null;
            if (prefab == null) return false;

            string assetPath = AssetDatabase.GetAssetPath(prefab);
            string guid = AssetDatabase.AssetPathToGUID(assetPath);
            address = AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(guid)?.address;

            return !string.IsNullOrEmpty(address);
        }

        public static void RegisterAddressable(GameObject prefab, string address)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("[PrefabAddressableHandler] Addressable settings not found.");
                return;
            }

            string path = AssetDatabase.GetAssetPath(prefab);
            string guid = AssetDatabase.AssetPathToGUID(path);
            if (string.IsNullOrEmpty(guid))
            {
                Debug.LogError("[PrefabAddressableHandler] GUID not found for prefab.");
                return;
            }

            var entry = settings.FindAssetEntry(guid) ?? settings.CreateOrMoveEntry(guid, settings.DefaultGroup);
            entry.address = address;
            Debug.Log($"[PrefabAddressableHandler] Registered addressable: {address}");
        }

        public static bool IsAddressAlreadyUsed(string address)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null) return false;

            foreach (var group in settings.groups)
            {
                foreach (var entry in group.entries)
                {
                    if (entry.address == address)
                        return true;
                }
            }

            return false;
        }

        private static void EnsureFolderExists(string folderPath)
        {
            if (AssetDatabase.IsValidFolder(folderPath)) return;

            string[] parts = folderPath.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = $"{current}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }
                current = next;
            }
        }
    }
}