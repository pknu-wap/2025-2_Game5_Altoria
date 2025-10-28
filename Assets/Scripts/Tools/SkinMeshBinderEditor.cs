using UnityEditor;
using UnityEngine;

public class SkinMeshBinderEditor : EditorWindow
{
    private SkinnedMeshRenderer playerRenderer;
    private SkinnedMeshRenderer targetRenderer;

    [MenuItem("Tools/Skinned Mesh Binder")]
    public static void ShowWindow()
    {
        GetWindow<SkinMeshBinderEditor>("Skinned Mesh Binder");
    }

    private void OnGUI()
    {
        GUILayout.Label(" Skinned Mesh Auto Binder", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        playerRenderer = (SkinnedMeshRenderer)EditorGUILayout.ObjectField(
            "Player Renderer", playerRenderer, typeof(SkinnedMeshRenderer), true);

        targetRenderer = (SkinnedMeshRenderer)EditorGUILayout.ObjectField(
            "Target Renderer", targetRenderer, typeof(SkinnedMeshRenderer), true);

        EditorGUILayout.Space();

        GUI.enabled = playerRenderer && targetRenderer;
        if (GUILayout.Button("Apply Player Bones ¡æ Target"))
        {
            ApplyBones();
        }
        GUI.enabled = true;
    }

    private void ApplyBones()
    {
        if (!playerRenderer || !targetRenderer)
        {
            Debug.LogError("Renderer references are missing!");
            return;
        }

        Undo.RecordObject(targetRenderer, "Bind Skinned Mesh Bones");

        targetRenderer.rootBone = playerRenderer.rootBone;
        targetRenderer.bones = playerRenderer.bones;

        EditorUtility.SetDirty(targetRenderer);

        Debug.Log($" Successfully bound bones from [{playerRenderer.name}] to [{targetRenderer.name}]");
    }
}
