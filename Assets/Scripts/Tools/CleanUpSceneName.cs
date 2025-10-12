using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public static class CleanUpSceneName
{
    [MenuItem("Tools/Scene Export/Cleanup Object Names")]
    public static void CleanupSceneObjectNames()
    {
        if (!EditorUtility.DisplayDialog(
            "Cleanup Object Names",
            "This will remove '(1)', '(2)' style suffixes from all GameObjects in the current scene.\nContinue?",
            "Yes", "No"))
            return;

        int renameCount = 0;
        var roots = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (var root in roots)
            RenameRecursive(root.transform, ref renameCount);

        EditorUtility.DisplayDialog("Cleanup Complete", $"Renamed {renameCount} GameObjects.", "OK");
        Debug.Log($"[SceneExport] Cleaned up {renameCount} GameObject names in scene '{SceneManager.GetActiveScene().name}'.");
    }

    private static void RenameRecursive(Transform t, ref int renameCount)
    {
        string originalName = t.name;
        string cleanedName = CleanName(originalName);

        if (cleanedName != originalName)
        {
            Undo.RecordObject(t.gameObject, "Cleanup Name");
            t.name = cleanedName;
            renameCount++;
        }

        for (int i = 0; i < t.childCount; i++)
            RenameRecursive(t.GetChild(i), ref renameCount);
    }

    private static string CleanName(string name)
    {
       
        return Regex.Replace(name, @"\s*\(\d+\)$", "").Trim();
    }
}
