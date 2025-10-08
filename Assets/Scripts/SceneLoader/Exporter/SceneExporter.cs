// SceneExportWindow.cs
// MARKER 제거 + BLOCKER 기반 + 외부 SceneExportSettings 기반 구조
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using UnityEditor.AddressableAssets;
using System.Linq;
using JetBrains.Annotations;



namespace SceneLoader
{
    public class SceneExportWindow : EditorWindow
    {
        private Vector2 _scroll;
        private List<SceneExportSettings> _settingsList = new();
        private string _exportPath = "";
        private string _commonPrefabPath = "";

        [MenuItem("Tools/Scene Export/Export Scene")]
        public static void ShowWindow()
        {
            var window = GetWindow<SceneExportWindow>();
            window.titleContent = new GUIContent("Scene Export");
            window.minSize = new Vector2(400, 300);
            window.RefreshExportSettings();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            if (GUILayout.Button("Refresh Export List"))
                RefreshExportSettings();

            EditorGUILayout.Space(10);
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            foreach (var settings in _settingsList)
                DrawSettingsUI(settings);
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField($"Total Targets: {_settingsList.Count}", EditorStyles.boldLabel);
            DrawExportPathSelection();

            bool anyNeedsPrefab = _settingsList.Exists(s => s.IncludeExport && PrefabUtilityHelper.NeedsPrefabCreation(s.Target, s));
            if (anyNeedsPrefab)
                DrawCommonPrefabPath();

            GUI.enabled = !string.IsNullOrEmpty(_exportPath);
            if (GUILayout.Button("Export"))
                ExportToFile();
            GUI.enabled = true;
        }

        private void DrawSettingsUI(SceneExportSettings s)
        {
            if (s.Target == null) return;

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            s.IncludeExport = EditorGUILayout.Toggle(s.IncludeExport, GUILayout.Width(20));
            if (GUILayout.Button(s.Target.name, EditorStyles.label))
            {
                Selection.activeGameObject = s.Target;
                EditorGUIUtility.PingObject(s.Target);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            DrawAddressableOptions(s);
            DrawPrefabizeOptions(s);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        private void DrawAddressableOptions(SceneExportSettings setting)
        {
            bool disabled =
                PrefabUtilityHelper.IsOutermostPrefabInstanceRoot(setting.Target) && PrefabUtilityHelper.TryGetAddressFromSource(setting.Target, out _);
            if (disabled)
            {
                setting.MakeAddressable = false;
                setting.UseCustomAddress = false;
                setting.CustomAddress = string.Empty;
            }

            EditorGUI.BeginDisabledGroup(disabled);
            setting.MakeAddressable = EditorGUILayout.Toggle("Make Addressable", setting.MakeAddressable);
            if (setting.MakeAddressable)
            {
                setting.UseCustomAddress = EditorGUILayout.Toggle("Use Custom Address", setting.UseCustomAddress);
                if (setting.UseCustomAddress)
                {
                    setting.CustomAddress = EditorGUILayout.TextField("Custom Address", setting.CustomAddress);
                    if (!string.IsNullOrEmpty(setting.CustomAddress) && PrefabAddressableHandler.IsAddressAlreadyUsed(setting.CustomAddress))
                        EditorGUILayout.HelpBox("This address is already used.", MessageType.Warning);
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        private void DrawPrefabizeOptions(SceneExportSettings setting)
        {
            bool disabled = PrefabUtilityHelper.HasSourcePrefab(setting.Target);
            if (disabled)
            {
                setting.ForcePrefabize = false;
                setting.UseCustomPath = false;
                setting.CustomSavePath = string.Empty;
            }

            EditorGUI.BeginDisabledGroup(disabled);
            setting.ForcePrefabize = EditorGUILayout.Toggle("Force Prefabize", setting.ForcePrefabize);
            if (setting.ForcePrefabize)
            {
                setting.UseCustomPath = EditorGUILayout.Toggle("Use Custom Path", setting.UseCustomPath);
                if (setting.UseCustomPath)
                    DrawCustomSavePath(setting);
            }
            EditorGUI.EndDisabledGroup();
        }

        private void DrawCustomSavePath(SceneExportSettings s)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Custom Path");
            if (GUILayout.Button("Browse", GUILayout.Width(70)))
            {
                string selected = EditorUtility.OpenFolderPanel("Select Folder", "Assets", "");
                if (!string.IsNullOrEmpty(selected) && selected.StartsWith(Application.dataPath))
                    s.CustomSavePath = "Assets" + selected.Substring(Application.dataPath.Length);
            }
            EditorGUILayout.TextField(s.CustomSavePath);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawCommonPrefabPath()
        {
            EditorGUILayout.LabelField("Common Prefab Save Path", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            _commonPrefabPath = EditorGUILayout.TextField(_commonPrefabPath);
            if (GUILayout.Button("Browse", GUILayout.Width(70)))
            {
                string selected = EditorUtility.OpenFolderPanel("Select Prefab Folder", "Assets", "");
                if (PrefabAddressableHandler.TryGetAssetRelativePath(selected, out var assetPath))
                    _commonPrefabPath = assetPath;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawExportPathSelection()
        {
            EditorGUILayout.LabelField("Export Path", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            _exportPath = EditorGUILayout.TextField(_exportPath);
            if (GUILayout.Button("Browse", GUILayout.Width(70)))
            {
                string sceneName = SceneManager.GetActiveScene().name;
                _exportPath = EditorUtility.SaveFilePanel("Export Scene", "", sceneName + ".json", "json");
            }
            EditorGUILayout.EndHorizontal();
        }

        private void RefreshExportSettings()
        {
            _settingsList.Clear();

            var roots = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var root in roots)
            {
                CollectExportSettingsRecursive(root.transform, false);
            }
        }

        private void CollectExportSettingsRecursive(Transform t, bool parentExported)
        {
            if (t.GetComponent<SceneExportBlocker>() != null)
                return;

            GameObject obj = t.gameObject;
            bool isOutermost = PrefabUtilityHelper.IsOutermostPrefabInstanceRoot(obj);
            bool exportTarget = PrefabUtilityHelper.IsExportTarget(obj, parentExported);
            bool hasExportedChild = !isOutermost && PrefabUtilityHelper.HasExportedChild(t);

            if (!exportTarget && !hasExportedChild)
                return;

            if (exportTarget)
                _settingsList.Add(new SceneExportSettings(obj));

            for (int i = 0; i < t.childCount; i++)
            {
                CollectExportSettingsRecursive(t.GetChild(i), exportTarget);
            }
        }
        private void ExportToFile()
        {
            var dataList = new List<SceneObjectData>();
            int nextId = 0;

            HandleExportWithUndo(dataList, ref nextId);
            WriteExportedDataToJsonFile(dataList);
        }
        private void HandleExportWithUndo(List<SceneObjectData> dataList, ref int nextId)
        {
            const int MaxObjectPerUndoGroup = 10;

            int undoCounter = 0;
            int groupNumber = 1;
            int group = Undo.GetCurrentGroup();

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Scene Export");

            HashSet<Transform> exportedRoots = new();

            foreach (var setting in _settingsList)
            {
                if (!setting.IncludeExport) continue;
                if (exportedRoots.Any(r => setting.Target.transform.IsChildOf(r)))
                    continue;

                if (undoCounter % MaxObjectPerUndoGroup == 0)
                {
                    Undo.CollapseUndoOperations(group);
                    group = Undo.GetCurrentGroup();
                    Undo.IncrementCurrentGroup();
                    Undo.SetCurrentGroupName($"Export Group {groupNumber}");
                    Debug.LogError(groupNumber);

                }

                Undo.RegisterCompleteObjectUndo(setting.Target, "Export Root");
                ExportTransformRecursive(setting.Target.transform, -1, ref nextId, dataList, false, setting);
                exportedRoots.Add(setting.Target.transform);
                undoCounter++;
            }

            Undo.CollapseUndoOperations(group);
        }
        private void WriteExportedDataToJsonFile(List<SceneObjectData> dataList)
        {
            var wrapper = new SerializationWrapper<SceneObjectData>(dataList);
            string json = JsonUtility.ToJson(wrapper, true);

            if (File.Exists(_exportPath))
                File.Copy(_exportPath, _exportPath + ".bak", overwrite: true);

            File.WriteAllText(_exportPath, json);
            Debug.Log($"[SceneExportWindow] Exported to: {_exportPath}");

            AssetDatabase.Refresh();
        }


        private void ExportTransformRecursive(Transform t,
            int parentId,
            ref int nextId,
            List<SceneObjectData> resultList
            , bool parentExported,
            SceneExportSettings setting)
        {
            GameObject obj = t.gameObject;
            bool isOutermost = PrefabUtilityHelper.IsOutermostPrefabInstanceRoot(obj);
            bool exportTarget = PrefabUtilityHelper.IsExportTarget(obj, parentExported);
            bool hasChildExport = !isOutermost && PrefabUtilityHelper.HasExportedChild(t);

            if (!exportTarget && !hasChildExport)
                return;

            int currentId = nextId++;
            SceneObjectData data = exportTarget && (isOutermost || (setting != null && setting.ForcePrefabize))
                ? PrefabUtilityHelper.CreatePrefabSceneObjectData(t, currentId, parentId, setting, _commonPrefabPath)
                : PrefabUtilityHelper.CreateContainerSceneObjectData(t, currentId, parentId);

            resultList.Add(data);

            int newParentId = currentId;

            for (int i = 0; i < t.childCount; i++)
            {
                Transform child = t.GetChild(i);
                SceneExportSettings childSetting = null;
                if (PrefabUtilityHelper.IsExportTarget(child.gameObject, exportTarget))
                {
                    childSetting = _settingsList.FirstOrDefault(s => s.Target == child.gameObject);
                }

                ExportTransformRecursive(child, newParentId, ref nextId, resultList, exportTarget, childSetting);
            }
        }
    }
}