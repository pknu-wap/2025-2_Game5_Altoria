using GameInteract;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SetComponent
{
    [MenuItem("Tools/Prefab/Set Collect Component")]
    public static void SetCollectComponent()
    {
        string filePath = "Assets/Prefabs/Interact/Mineral";

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { filePath });

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

            if (AssetDatabase.LoadAssetAtPath<GameObject>(assetPath).GetComponent<CollectInteractComponent>() != null)
                return;

            var GO = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath).AddComponent<CollectInteractComponent>();
            string id = GO.name[..^2];
            var GOComponent = GO.GetComponent<CollectInteractComponent>();
            GOComponent.SetObjectID(id);
            GOComponent.SetCollectType(Define.ContentType.Mining);
        }
    }
}
