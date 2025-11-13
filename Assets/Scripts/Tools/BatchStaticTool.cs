#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BatchStaticTool : EditorWindow
{
   
    StaticEditorFlags staticFlags =
        StaticEditorFlags.BatchingStatic |
        StaticEditorFlags.NavigationStatic |
        StaticEditorFlags.OccludeeStatic |
        StaticEditorFlags.OccluderStatic |
        StaticEditorFlags.ReflectionProbeStatic;

    bool includeChildren = true;

    [MenuItem("Tools/Batch/Set Static On Selected Prefabs")]
    public static void ShowWindow()
    {
        GetWindow<BatchStaticTool>("Batch Static");
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Batch Set Static on Selected Prefabs", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        staticFlags = (StaticEditorFlags)EditorGUILayout.EnumFlagsField("Static Flags", staticFlags);
        includeChildren = EditorGUILayout.Toggle("Include Children", includeChildren);

        EditorGUILayout.Space();

        if (GUILayout.Button("Apply To Selected Prefabs"))
        {
            ApplyStaticToSelectedPrefabs();
        }
    }

    void ApplyStaticToSelectedPrefabs()
    {
     
        GameObject[] selected = Selection.GetFiltered<GameObject>(SelectionMode.Assets);

        if (selected == null || selected.Length == 0)
        {
            EditorUtility.DisplayDialog("Batch Static", "선택된 프리팹이 없습니다.", "OK");
            return;
        }

        int count = 0;

        try
        {
            AssetDatabase.StartAssetEditing();

            foreach (var obj in selected)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path))
                    continue;

          
                GameObject prefabRoot = PrefabUtility.LoadPrefabContents(path);
                if (prefabRoot == null)
                    continue;

               
                SetStaticFlags(prefabRoot, staticFlags, includeChildren);

      
                PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
                PrefabUtility.UnloadPrefabContents(prefabRoot);

                count++;
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.SaveAssets();
        }

        EditorUtility.DisplayDialog("Batch Static",
            $"Static 적용 완료\n처리된 프리팹 개수: {count}", "OK");
    }

    static void SetStaticFlags(GameObject root, StaticEditorFlags flags, bool includeChildren)
    {
        GameObjectUtility.SetStaticEditorFlags(root, flags);

        if (!includeChildren)
            return;

        foreach (Transform child in root.transform)
        {
            SetStaticFlags(child.gameObject, flags, true);
        }
    }
}
#endif