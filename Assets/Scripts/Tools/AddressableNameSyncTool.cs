#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public static class AddressableNameSyncTool
{
    [MenuItem("Tools/Addressables/선택한 오브젝트 Address를 이름으로 변경")]
    private static void RenameSelectedAddressablesToObjectName()
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            EditorUtility.DisplayDialog(
                "Addressables 설정 없음",
                "AddressableAssetSettings 가 존재하지 않습니다.\nWindow > Asset Management > Addressables > Groups 에서 먼저 설정을 생성하세요.",
                "OK"
            );
            return;
        }

        Object[] selectedObjects = Selection.objects;
        if (selectedObjects == null || selectedObjects.Length == 0)
        {
            EditorUtility.DisplayDialog(
                "선택된 오브젝트 없음",
                "프로젝트 창에서 Addressable로 등록된 에셋을 선택한 뒤 다시 실행하세요.",
                "OK"
            );
            return;
        }

        int changeCount = 0;

        foreach (var obj in selectedObjects)
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(assetPath))
                continue;

            string guid = AssetDatabase.AssetPathToGUID(assetPath);
            if (string.IsNullOrEmpty(guid))
                continue;

            AddressableAssetEntry entry = settings.FindAssetEntry(guid);
            if (entry == null)
                continue;

            string newAddress = obj.name;

            if (entry.address != newAddress)
            {
                Undo.RecordObject(settings, "Rename Addressable Address");
                entry.SetAddress(newAddress);
                changeCount++;
            }
        }

        if (changeCount > 0)
        {
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, null, true);
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog(
                "완료",
                $"선택된 오브젝트 중 {changeCount}개의 Address를 오브젝트 이름으로 변경했습니다.",
                "OK"
            );
        }
        else
        {
            EditorUtility.DisplayDialog(
                "변경 없음",
                "선택된 오브젝트 중 변경할 Address가 없습니다.\n(이미 이름과 Address가 일치하거나 Addressables에 등록되지 않은 에셋일 수 있습니다.)",
                "OK"
            );
        }
    }

    [MenuItem("Tools/Addressables/전체 Address를 이름으로 동기화")]
    private static void RenameAllAddressablesToObjectName()
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            EditorUtility.DisplayDialog(
                "Addressables 설정 없음",
                "AddressableAssetSettings 가 존재하지 않습니다.\nWindow > Asset Management > Addressables > Groups 에서 먼저 설정을 생성하세요.",
                "OK"
            );
            return;
        }

        int changeCount = 0;
        List<AddressableAssetEntry> entries = new List<AddressableAssetEntry>();

        foreach (var group in settings.groups)
        {
            if (group == null) continue;
            group.GatherAllAssets(entries, true, true, true);
        }

        foreach (var entry in entries)
        {
            if (entry == null || string.IsNullOrEmpty(entry.guid))
                continue;

            string assetPath = AssetDatabase.GUIDToAssetPath(entry.guid);
            if (string.IsNullOrEmpty(assetPath))
                continue;

            Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            if (asset == null)
                continue;

            string newAddress = asset.name;

            if (entry.address != newAddress)
            {
                Undo.RecordObject(settings, "Rename Addressable Address (All)");
                entry.SetAddress(newAddress);
                changeCount++;
            }
        }

        if (changeCount > 0)
        {
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, null, true);
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog(
                "완료",
                $"전체 Addressables 중 {changeCount}개의 Address를 오브젝트 이름으로 동기화했습니다.",
                "OK"
            );
        }
        else
        {
            EditorUtility.DisplayDialog(
                "변경 없음",
                "Addressables에 등록된 엔트리의 Address가 이미 오브젝트 이름과 모두 일치합니다.",
                "OK"
            );
        }
    }
}
#endif
