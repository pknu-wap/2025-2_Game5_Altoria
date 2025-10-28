using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;

public class UIFontReplacerWindow : EditorWindow
{
    GameObject targetPrefab;
    Font unityFont;
    TMP_FontAsset tmpFont;

    [MenuItem("Tools/UI/Font Replacer")]
    static void OpenWindow()
    {
        GetWindow<UIFontReplacerWindow>("UI Font Replacer");
    }

    void OnGUI()
    {
        GUILayout.Label("Prefab Font Replacer", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        targetPrefab = (GameObject)EditorGUILayout.ObjectField("Target Prefab", targetPrefab, typeof(GameObject), false);
        unityFont = (Font)EditorGUILayout.ObjectField("UI Font (Text)", unityFont, typeof(Font), false);
        tmpFont = (TMP_FontAsset)EditorGUILayout.ObjectField("TMP Font (TextMeshPro)", tmpFont, typeof(TMP_FontAsset), false);

        EditorGUILayout.Space();

        GUI.enabled = targetPrefab != null && (unityFont != null || tmpFont != null);
        if (GUILayout.Button("Replace Fonts"))
        {
            ReplaceFonts();
        }
        GUI.enabled = true;
    }

    void ReplaceFonts()
    {
        if (targetPrefab == null)
        {
            Debug.LogWarning(" Prefab을 지정해주세요.");
            return;
        }

        string path = AssetDatabase.GetAssetPath(targetPrefab);
        GameObject prefabRoot = PrefabUtility.LoadPrefabContents(path);

        int unityCount = 0;
        int tmpCount = 0;

        
        if (unityFont != null)
        {
            foreach (var text in prefabRoot.GetComponentsInChildren<Text>(true))
            {
                text.font = unityFont;
                unityCount++;
                EditorUtility.SetDirty(text);
            }
        }

        if (tmpFont != null)
        {
            foreach (var tmp in prefabRoot.GetComponentsInChildren<TMP_Text>(true))
            {
                tmp.font = tmpFont;
                tmpCount++;
                EditorUtility.SetDirty(tmp);
            }
        }

        // 저장
        PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
        PrefabUtility.UnloadPrefabContents(prefabRoot);

        Debug.Log($" Font 변경 완료: Text {unityCount}개, TMP_Text {tmpCount}개 교체됨");
    }
}
