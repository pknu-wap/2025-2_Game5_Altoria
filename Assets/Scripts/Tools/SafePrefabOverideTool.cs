#if  UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class SafePrefabBatchOverrideTool : EditorWindow
{
    [MenuItem("Tools/Prefab/Safe Prefab Batch Override %#p")]
    static void Open() => GetWindow<SafePrefabBatchOverrideTool>("Safe Prefab Batch Override");

    void OnGUI()
    {
        EditorGUILayout.HelpBox(
            "Unity 6 Prefab Override 병합 시 Terrain 등 Scene 오브젝트가 오염되는 문제를 방지합니다.\n" +
            "이 툴은 선택된 모든 Prefab Instance의 Layer, Tag, Transform을 안전하게 Prefab에 일괄 적용합니다.",
            MessageType.Info);

        EditorGUILayout.Space(10);

        if (GUILayout.Button(" Apply Safe Overrides to All Selected Prefabs", GUILayout.Height(36)))
        {
            ApplyAllSelected();
        }
    }

    void ApplyAllSelected()
    {
        var selection = Selection.gameObjects;
        if (selection == null || selection.Length == 0)
        {
            Debug.LogWarning("[SafeOverride] No objects selected.");
            return;
        }

       
        var prefabInstances = selection
            .Select(PrefabUtility.GetOutermostPrefabInstanceRoot)
            .Where(x => x != null && PrefabUtility.IsPartOfPrefabInstance(x))
            .Distinct()
            .ToList();

        if (prefabInstances.Count == 0)
        {
            Debug.LogWarning("[SafeOverride] No prefab instances found in selection.");
            return;
        }

        int totalApplied = 0;

        foreach (var instance in prefabInstances)
        {
            totalApplied += ApplySafeOverrides(instance);
        }

        AssetDatabase.SaveAssets();

        Debug.Log($" [SafeOverride] Applied safe overrides to {prefabInstances.Count} prefab(s), total {totalApplied} properties updated.");
    }

    int ApplySafeOverrides(GameObject instance)
    {
        var prefabRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(instance);
        var prefabAssetRoot = PrefabUtility.GetCorrespondingObjectFromSource(prefabRoot);
        if (prefabAssetRoot == null)
        {
            Debug.LogWarning($"[SafeOverride] Prefab asset not found for {prefabRoot.name}");
            return 0;
        }

        var targets = prefabRoot.GetComponentsInChildren<Transform>(true)
                                .Select(t => t.gameObject)
                                .ToList();

        int appliedCount = 0;

        foreach (var go in targets)
        {
            if (go.TryGetComponent<Terrain>(out _))
                continue;

            var srcGO = PrefabUtility.GetCorrespondingObjectFromSource(go);
            if (srcGO == null) continue;

            appliedCount += ApplyProperty(go, srcGO, "m_Layer");
            appliedCount += ApplyProperty(go, srcGO, "m_TagString");

            var instTr = go.transform;
            var srcTr = srcGO.transform;
            appliedCount += ApplyProperty(instTr, srcTr, "m_LocalPosition");
            appliedCount += ApplyProperty(instTr, srcTr, "m_LocalRotation");
            appliedCount += ApplyProperty(instTr, srcTr, "m_LocalScale");
        }

        if (appliedCount > 0)
            Debug.Log($"[SafeOverride] {prefabRoot.name}: {appliedCount} property overrides applied.");

        return appliedCount;
    }

    int ApplyProperty(Object instObj, Object srcObj, string propertyPath)
    {
        var instSO = new SerializedObject(instObj);
        var instProp = instSO.FindProperty(propertyPath);
        if (instProp == null) return 0;

        var srcSO = new SerializedObject(srcObj);
        var srcProp = srcSO.FindProperty(propertyPath);
        if (srcProp == null) return 0;

        Undo.RecordObject(srcObj, "Safe Prefab Batch Override");

        srcProp.serializedObject.Update();
        CopySerializedValue(instProp, srcProp);
        srcProp.serializedObject.ApplyModifiedPropertiesWithoutUndo();

        EditorUtility.SetDirty(srcObj);
        return 1;
    }

    void CopySerializedValue(SerializedProperty from, SerializedProperty to)
    {
        switch (from.propertyType)
        {
            case SerializedPropertyType.Integer: to.intValue = from.intValue; break;
            case SerializedPropertyType.Boolean: to.boolValue = from.boolValue; break;
            case SerializedPropertyType.Float: to.floatValue = from.floatValue; break;
            case SerializedPropertyType.String: to.stringValue = from.stringValue; break;
            case SerializedPropertyType.Color: to.colorValue = from.colorValue; break;
            case SerializedPropertyType.ObjectReference: to.objectReferenceValue = from.objectReferenceValue; break;
            case SerializedPropertyType.Vector2: to.vector2Value = from.vector2Value; break;
            case SerializedPropertyType.Vector3: to.vector3Value = from.vector3Value; break;
            case SerializedPropertyType.Vector4: to.vector4Value = from.vector4Value; break;
            case SerializedPropertyType.Quaternion: to.quaternionValue = from.quaternionValue; break;
            default:
                to.serializedObject.CopyFromSerializedProperty(from);
                break;
        }
    }
}
#endif