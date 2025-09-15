using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class PrefabRootReplacerTool : EditorWindow
{
    private enum Mode
    {
        CreateNewPrefab,
        ReplaceWithExistingPrefab,
        CreateOrReplaceMatchingPrefab
    }

    private Mode currentMode = Mode.CreateNewPrefab;

    private List<GameObject> createTargets = new List<GameObject>();
    private GameObject replaceTarget;
    private GameObject existingPrefab;
    private string prefabFolder = "Assets/Prefabs";

    [MenuItem("Tools/Prefab/Root Replacer")]
    public static void ShowWindow()
    {
        GetWindow<PrefabRootReplacerTool>("Prefab Root Replacer");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("프리팹 루트 대체 도구 (자식 유지)", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        currentMode = (Mode)EditorGUILayout.EnumPopup("모드 선택", currentMode);
        EditorGUILayout.Space();

        if (currentMode == Mode.CreateNewPrefab || currentMode == Mode.CreateOrReplaceMatchingPrefab)
        {
            DrawNewPrefabMode();
        }
        else if (currentMode == Mode.ReplaceWithExistingPrefab)
        {
            DrawReplaceWithExistingMode();
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("실행"))
        {
            if (createTargets.Count == 0 && currentMode != Mode.ReplaceWithExistingPrefab)
            {
                Debug.LogWarning("대상 오브젝트를 하나 이상 선택해주세요.");
                return;
            }

            if (currentMode == Mode.CreateNewPrefab)
            {
                ProcessCreatePrefabs();
            }
            else if (currentMode == Mode.ReplaceWithExistingPrefab)
            {
                if (replaceTarget == null || existingPrefab == null)
                {
                    Debug.LogWarning("대상 및 기존 프리팹을 설정해주세요.");
                    return;
                }
                ReplaceRootWithExistingPrefab(replaceTarget, existingPrefab);
            }
            else if (currentMode == Mode.CreateOrReplaceMatchingPrefab)
            {
                foreach (var go in createTargets)
                {
                    if (go == null) continue;

                    string path = Path.Combine(prefabFolder, go.name + ".prefab").Replace("\\", "/");
                    GameObject matched = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (matched != null)
                    {
                        ReplaceRootWithExistingPrefab(go, matched);
                    }
                    else
                    {
                        SaveAsPrefabKeepingChildren(go, prefabFolder);
                    }
                }

                createTargets.Clear();
            }
        }
    }

    private void DrawNewPrefabMode()
    {
        EditorGUILayout.LabelField("타겟 오브젝트들", EditorStyles.boldLabel);

        // 드래그 앤 드롭 영역
        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 60.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "이곳에 GameObject들을 드래그하세요", EditorStyles.helpBox);

        Event evt = Event.current;
        if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform)
        {
            if (dropArea.Contains(evt.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    foreach (var obj in DragAndDrop.objectReferences)
                    {
                        if (obj is GameObject go && !createTargets.Contains(go))
                        {
                            createTargets.Add(go);
                        }
                    }
                }
                evt.Use();
            }
        }

        // 리스트 보여주기
        for (int i = 0; i < createTargets.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            createTargets[i] = (GameObject)EditorGUILayout.ObjectField(createTargets[i], typeof(GameObject), true);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                createTargets.RemoveAt(i);
                i--;
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("저장 폴더", GUILayout.Width(70));
        EditorGUILayout.SelectableLabel(prefabFolder, GUILayout.Height(18));
        if (GUILayout.Button("폴더 선택", GUILayout.Width(90)))
        {
            string selectedPath = EditorUtility.OpenFolderPanel("프리팹 저장 폴더 선택", "Assets", "");
            if (!string.IsNullOrEmpty(selectedPath) && selectedPath.StartsWith(Application.dataPath))
            {
                prefabFolder = "Assets" + selectedPath.Substring(Application.dataPath.Length);
            }
            else
            {
                Debug.LogError("Assets 폴더 내부만 선택 가능합니다.");
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawReplaceWithExistingMode()
    {
        replaceTarget = (GameObject)EditorGUILayout.ObjectField("타겟 오브젝트", replaceTarget, typeof(GameObject), true);
        existingPrefab = (GameObject)EditorGUILayout.ObjectField("기존 프리팹", existingPrefab, typeof(GameObject), false);
    }

    private void ProcessCreatePrefabs()
    {
        List<GameObject> successList = new List<GameObject>();

        foreach (var go in createTargets)
        {
            if (go == null) continue;
            SaveAsPrefabKeepingChildren(go, prefabFolder);
            successList.Add(go);
        }

        foreach (var go in successList)
        {
            createTargets.Remove(go);
        }
    }

    private void SaveAsPrefabKeepingChildren(GameObject go, string folderPath)
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        Undo.RegisterFullObjectHierarchyUndo(go, "Create Prefab");

        Transform[] children = new Transform[go.transform.childCount];
        for (int i = 0; i < go.transform.childCount; i++)
            children[i] = go.transform.GetChild(i);

        foreach (Transform child in children)
            child.SetParent(null, true);

        string prefabPath = Path.Combine(folderPath, go.name + ".prefab").Replace("\\", "/");
        prefabPath = AssetDatabase.GenerateUniqueAssetPath(prefabPath);
        GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(go, prefabPath, InteractionMode.UserAction);

        foreach (Transform child in children)
            child.SetParent(go.transform, true);

        Debug.Log($"[프리팹 생성] {prefabPath}");
    }

    private void ReplaceRootWithExistingPrefab(GameObject original, GameObject prefab)
    {
        Undo.RegisterFullObjectHierarchyUndo(original, "Replace With Prefab");

        Transform[] children = new Transform[original.transform.childCount];
        for (int i = 0; i < original.transform.childCount; i++)
            children[i] = original.transform.GetChild(i);

        foreach (Transform child in children)
            child.SetParent(null, true);

        Vector3 position = original.transform.position;
        Quaternion rotation = original.transform.rotation;
        Vector3 scale = original.transform.localScale;
        Transform parent = original.transform.parent;
        int siblingIndex = original.transform.GetSiblingIndex();

        GameObject newRoot = (GameObject)PrefabUtility.InstantiatePrefab(prefab, original.scene);
        newRoot.transform.SetPositionAndRotation(position, rotation);
        newRoot.transform.localScale = scale;
        newRoot.transform.SetParent(parent);
        newRoot.transform.SetSiblingIndex(siblingIndex);

        foreach (Transform child in children)
            child.SetParent(newRoot.transform, true);

        Undo.DestroyObjectImmediate(original);
        Selection.activeGameObject = newRoot;

        Debug.Log($"[루트 교체 완료] {prefab.name}");
    }
}
