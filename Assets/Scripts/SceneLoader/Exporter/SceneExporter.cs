using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SceneLoader
{
    public class SceneExportWindow : EditorWindow
    {
        Vector2 scroll;
        readonly List<SceneExportSettings> settingsList = new();
        readonly Dictionary<GameObject, CachedSettingState> cacheStates = new();
        string exportPath = "";
        string commonPrefabPath = "";
        double lastRefreshTime;
        bool needRefresh;
        bool isExporting;  

        const float ItemHeight = 70f;

        class CachedSettingState
        {
            public bool IsOutermost;
            public bool HasPrefab;
            public bool HasAddress;
        }

        [MenuItem("Tools/Scene Export/Export Scene")]
        public static void ShowWindow()
        {
            var window = GetWindow<SceneExportWindow>();
            window.titleContent = new GUIContent("Scene Export");
            window.minSize = new Vector2(400, 300);
            window.RefreshExportSettings();
        }

        void OnInspectorUpdate()
        {
            if (needRefresh && EditorApplication.timeSinceStartup - lastRefreshTime > 0.3)
            {
                needRefresh = false;
                RefreshExportSettings();
                Repaint();
            }
        }

        void OnGUI()
        {
            EditorGUILayout.Space();

   
         
            if (GUILayout.Button("Refresh Export List"))
            {
                RefreshExportSettings();
                GUI.FocusControl(null);
            }
            GUI.enabled = true;

            EditorGUILayout.Space(10);

          
            if (isExporting)
            {
                EditorGUILayout.HelpBox("Exporting scene data... Please wait.", MessageType.Info);
            }

            scroll = EditorGUILayout.BeginScrollView(scroll);

            int count = settingsList.Count;
            float totalHeight = count * ItemHeight;

            Rect visible = new Rect(0, scroll.y, position.width, position.height);
            int firstVisible = Mathf.Max(0, Mathf.FloorToInt(visible.y / ItemHeight) - 2);
            int lastVisible = Mathf.Min(count, Mathf.CeilToInt((visible.y + visible.height) / ItemHeight) + 2);

            GUILayout.Space(firstVisible * ItemHeight);

            for (int i = firstVisible; i < lastVisible; i++)
            {
                if (i < 0 || i >= count) continue;
                DrawCachedSettingUI(settingsList[i]);
                GUILayout.Space(2);
            }

            float remainingSpace = Mathf.Max(0, totalHeight - lastVisible * ItemHeight);
            GUILayout.Space(remainingSpace);

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField($"Total Targets: {settingsList.Count}", EditorStyles.boldLabel);
            DrawExportPathSelection();

            bool anyNeedsPrefab = settingsList.Any(s => s.IncludeExport && PrefabUtilityHelper.NeedsPrefabCreation(s.Target, s));
            if (anyNeedsPrefab)
                DrawCommonPrefabPath();

  
            GUI.enabled = !isExporting && !string.IsNullOrEmpty(exportPath);
            if (GUILayout.Button("Export"))
                StartExport();
            GUI.enabled = true;
        }

        void DrawCachedSettingUI(SceneExportSettings s)
        {
            if (s.Target == null)
                return;

            if (!cacheStates.TryGetValue(s.Target, out var cache))
            {
                cache = new CachedSettingState
                {
                    IsOutermost = PrefabUtilityHelper.IsOutermostPrefabInstanceRoot(s.Target),
                    HasPrefab = PrefabUtilityHelper.HasSourcePrefab(s.Target),
                    HasAddress = PrefabUtilityHelper.TryGetAddressFromSource(s.Target, out _)
                };
                cacheStates[s.Target] = cache;
            }

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
            DrawCachedAddressableOptions(s, cache);
            DrawCachedPrefabizeOptions(s, cache);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        void DrawCachedAddressableOptions(SceneExportSettings setting, CachedSettingState cache)
        {
            bool disabled = cache.IsOutermost && cache.HasAddress;
            if (disabled) setting.MakeAddressable = false;

            EditorGUI.BeginDisabledGroup(disabled);
            setting.MakeAddressable = EditorGUILayout.Toggle("Make Addressable", setting.MakeAddressable);
            EditorGUI.EndDisabledGroup();
        }

        void DrawCachedPrefabizeOptions(SceneExportSettings setting, CachedSettingState cache)
        {
            bool disabled = cache.HasPrefab;
            if (disabled) setting.ForcePrefabize = false;

            EditorGUI.BeginDisabledGroup(disabled);
            setting.ForcePrefabize = EditorGUILayout.Toggle("Force Prefabize", setting.ForcePrefabize);
            EditorGUI.EndDisabledGroup();
        }

        void DrawCommonPrefabPath()
        {
            EditorGUILayout.LabelField("Common Prefab Save Path", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            commonPrefabPath = EditorGUILayout.TextField(commonPrefabPath);
            if (GUILayout.Button("Browse", GUILayout.Width(70)))
            {
                string selected = EditorUtility.OpenFolderPanel("Select Prefab Folder", "Assets", "");
                if (PrefabAddressableHandler.TryGetAssetRelativePath(selected, out var assetPath))
                    commonPrefabPath = assetPath;
            }
            EditorGUILayout.EndHorizontal();
        }

        void DrawExportPathSelection()
        {
            EditorGUILayout.LabelField("Export Path", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            exportPath = EditorGUILayout.TextField(exportPath);
            if (GUILayout.Button("Browse", GUILayout.Width(70)))
            {
                string sceneName = SceneManager.GetActiveScene().name;
                exportPath = EditorUtility.SaveFilePanel("Export Scene", "", sceneName + ".json", "json");
            }
            EditorGUILayout.EndHorizontal();
        }

        void RefreshExportSettings()
        {
            lastRefreshTime = EditorApplication.timeSinceStartup;
            settingsList.Clear();
            cacheStates.Clear();

            var roots = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var root in roots)
                CollectExportSettingsRecursive(root.transform, false);
        }

        void CollectExportSettingsRecursive(Transform t, bool parentExported)
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
                settingsList.Add(new SceneExportSettings(obj));

            for (int i = 0; i < t.childCount; i++)
                CollectExportSettingsRecursive(t.GetChild(i), exportTarget);
        }


        void StartExport()
        {
            if (isExporting) return;
            isExporting = true;
            try
            {
                ExportToFile();
            }
            finally
            {
                isExporting = false;
                Repaint();
            }
        }

        void ExportToFile()
        {
            var dataList = new List<SceneObjectData>(4096);
            int nextId = 0;
            HandleExport(dataList, ref nextId);
            WriteExportedDataToJsonFile(dataList);
            RefreshExportSettings();
            Repaint();
        }

        void HandleExport(List<SceneObjectData> dataList, ref int nextId)
        {
            var exportedRoots = new HashSet<Transform>(128);
            var settingLookup = new Dictionary<GameObject, SceneExportSettings>(settingsList.Count);

            for (int i = 0; i < settingsList.Count; i++)
                settingLookup[settingsList[i].Target] = settingsList[i];

            foreach (var setting in settingsList)
            {
                if (!setting.IncludeExport) continue;

                var tr = setting.Target.transform;
                bool skip = false;
                foreach (var r in exportedRoots)
                {
                    if (tr.IsChildOf(r))
                    {
                        skip = true;
                        break;
                    }
                }
                if (skip) continue;

                ExportTransformRecursive(tr, -1, ref nextId, dataList, false, setting, settingLookup);
                exportedRoots.Add(tr);
            }
        }

        void WriteExportedDataToJsonFile(List<SceneObjectData> dataList)
        {
            var wrapper = new SerializationWrapper<SceneObjectData>(dataList);
            string json = JsonUtility.ToJson(wrapper, true);

            if (File.Exists(exportPath))
                File.Copy(exportPath, exportPath + ".bak", true);

            File.WriteAllText(exportPath, json);
            Debug.Log($"[SceneExportWindow] Exported {dataList.Count} objects to: {exportPath}");
            AssetDatabase.Refresh();
        }

        void ExportTransformRecursive(
            Transform t,
            int parentId,
            ref int nextId,
            List<SceneObjectData> resultList,
            bool parentExported,
            SceneExportSettings setting,
            Dictionary<GameObject, SceneExportSettings> settingLookup)
        {
            GameObject obj = t.gameObject;
            bool isOutermost = PrefabUtilityHelper.IsOutermostPrefabInstanceRoot(obj);
            bool exportTarget = PrefabUtilityHelper.IsExportTarget(obj, parentExported);
            bool hasChildExport = !isOutermost && PrefabUtilityHelper.HasExportedChild(t);

            if (!exportTarget && !hasChildExport)
                return;

            int currentId = nextId++;
            SceneObjectData data = (exportTarget && (isOutermost || (setting != null && setting.ForcePrefabize)))
                ? PrefabUtilityHelper.CreatePrefabSceneObjectData(t, currentId, parentId, setting, commonPrefabPath)
                : PrefabUtilityHelper.CreateContainerSceneObjectData(t, currentId, parentId);

            resultList.Add(data);

            for (int i = 0; i < t.childCount; i++)
            {
                Transform child = t.GetChild(i);
                if (!settingLookup.TryGetValue(child.gameObject, out var childSetting))
                    childSetting = null;

                ExportTransformRecursive(child, currentId, ref nextId, resultList, exportTarget, childSetting, settingLookup);
            }
        }
    }
}
