#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RemoveMissingScriptsWindow : EditorWindow
{
    private Vector2 _scroll;
    private List<string> _logMessages = new();
    private int _totalChecked = 0;
    private int _totalRemoved = 0;

    [MenuItem("Tools/Cleanup/Remove Missing Scripts (Window)")]
    public static void ShowWindow()
    {
        GetWindow<RemoveMissingScriptsWindow>("Remove Missing Scripts");
    }

    private void OnGUI()
    {
        GUILayout.Label("Missing Scripts Cleaner", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (GUILayout.Button("검사 및 제거 실행"))
        {
            RunRemoval();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField($"검사한 오브젝트 수: {_totalChecked}");
        EditorGUILayout.LabelField($"제거된 Missing Script 수: {_totalRemoved}");

        EditorGUILayout.Space();
        GUILayout.Label("결과 로그:");
        _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.Height(300));
        foreach (string msg in _logMessages)
        {
            EditorGUILayout.HelpBox(msg, MessageType.None);
        }
        EditorGUILayout.EndScrollView();
    }

    private void RunRemoval()
    {
        _logMessages.Clear();
        _totalChecked = 0;
        _totalRemoved = 0;

        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = activeScene.GetRootGameObjects();
        List<GameObject> allObjects = new();

        foreach (GameObject root in rootObjects)
        {
            GetAllChildrenRecursive(root, allObjects);
        }

        foreach (GameObject go in allObjects)
        {
            Undo.RegisterCompleteObjectUndo(go, "Remove Missing Scripts");

            int removed = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
            if (removed > 0)
            {
                string path = GetFullPath(go);
                _logMessages.Add($"Removed {removed} missing scripts from: {path}");
                _totalRemoved += removed;
            }

            _totalChecked++;
        }

        Debug.Log($"[MissingScriptCleaner] 완료: {_totalChecked}개 검사, {_totalRemoved}개 제거됨.");
    }

    private static void GetAllChildrenRecursive(GameObject obj, List<GameObject> list)
    {
        list.Add(obj);
        foreach (Transform child in obj.transform)
        {
            GetAllChildrenRecursive(child.gameObject, list);
        }
    }

    private static string GetFullPath(GameObject go)
    {
        string path = go.name;
        Transform current = go.transform;
        while (current.parent != null)
        {
            current = current.parent;
            path = current.name + "/" + path;
        }
        return path;
    }
}
#endif
