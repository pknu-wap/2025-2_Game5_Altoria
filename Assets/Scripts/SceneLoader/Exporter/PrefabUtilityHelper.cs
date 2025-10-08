using UnityEditor;
using UnityEngine;

namespace SceneLoader
{
    public static class PrefabUtilityHelper
    {
        public static bool IsExportTarget(GameObject obj, bool parentIsExported)
        {
            return IsOutermostPrefabInstanceRoot(obj)
                || !PrefabUtility.IsPartOfAnyPrefab(obj)
                || (IsModified(obj) && !parentIsExported);
        }

        public static bool IsModified(GameObject obj)
        {
            return PrefabUtility.HasPrefabInstanceAnyOverrides(obj, false);
        }

        public static bool IsOutermostPrefabInstanceRoot(GameObject obj)
        {
            return PrefabUtility.IsPartOfPrefabInstance(obj) &&
                   PrefabUtility.GetOutermostPrefabInstanceRoot(obj) == obj;
        }

        public static bool HasSourcePrefab(GameObject obj)
        {
            return PrefabUtility.GetCorrespondingObjectFromSource(obj) != null;
        }

        public static bool NeedsPrefabCreation(GameObject obj, SceneExportSettings settings)
        {
            if (settings.ForcePrefabize) return true;
            return PrefabUtility.GetCorrespondingObjectFromSource(obj) == null;
        }

        public static bool TryGetAddressFromSource(GameObject obj, out string address)
        {
            address = null;
            var prefab = PrefabUtility.GetCorrespondingObjectFromSource(obj);
            if (prefab == null) return false;

            return PrefabAddressableHandler.TryGetAddress(prefab, out address);
        }

        public static SceneObjectData CreatePrefabSceneObjectData(
            Transform transform,
            int currentId,
            int parentId,
            SceneExportSettings settings,
            string prefabPath)
        {
            GameObject obj = transform.gameObject;
            GameObject prefab = GetOrCreatePrefab(obj, settings, prefabPath);
            string address = ResolveAddress(prefab, settings);

            return new SceneObjectData
            {
                ID = currentId,
                parentID = parentId,
                PrefabAddress = address,
                Position = transform.localPosition,
                Rotation = transform.localRotation,
                Scale = transform.localScale,
                SiblingIndex = transform.GetSiblingIndex()
            };
        }

        public static SceneObjectData CreateContainerSceneObjectData(Transform transform, int currentId, int parentId)
        {
            return new SceneObjectData
            {
                ID = currentId,
                parentID = parentId,
                PrefabAddress = null,
                Position = transform.localPosition,
                Rotation = transform.localRotation,
                Scale = transform.localScale,
                SiblingIndex = transform.GetSiblingIndex()
            };
        }

        private static GameObject GetOrCreatePrefab(GameObject obj, SceneExportSettings settings, string fallbackPath)
        {
            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(obj);

            bool shouldCreate = prefab == null || (settings != null && settings.ForcePrefabize);
            if (!shouldCreate)
                return prefab;

            string savePath = (settings != null && settings.UseCustomPath && !string.IsNullOrEmpty(settings.CustomSavePath))
                ? settings.CustomSavePath
                : fallbackPath;

            return PrefabAddressableHandler.ConvertToPrefabOnly(obj, savePath);
        }

        private static string ResolveAddress(GameObject prefab, SceneExportSettings settings)
        {
            string address = null;

            if (settings != null && settings.MakeAddressable)
            {
                address = (settings.UseCustomAddress && !string.IsNullOrEmpty(settings.CustomAddress))
                    ? settings.CustomAddress
                    : prefab.name;

                if (!PrefabAddressableHandler.TryGetAddress(prefab, out _))
                {
                    PrefabAddressableHandler.RegisterAddressable(prefab, address);
                }
            }
            else
            {
                PrefabAddressableHandler.TryGetAddress(prefab, out address);
            }

            return address;
        }

        public static bool HasExportedChild(Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                if (IsExportTarget(parent.GetChild(i).gameObject, true))
                    return true;
            }
            return false;
        }
    }
}