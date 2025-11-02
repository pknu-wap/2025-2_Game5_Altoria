using UnityEngine;
using System.Collections.Generic;
using GameInteract;

public class SocketHandler : MonoBehaviour
{
    [SerializeField] private Transform toolSocket;
    [SerializeField] private ContentPrefabDB craftDB;

    private GameObject currentTool;


    public void SpawnTool(int contentType)
    {
        
        var type = (Define.ContentType)contentType;


        if (currentTool != null)
        {
            Destroy(currentTool);
            currentTool = null;
        }

        var entry = craftDB.GetEntry(type);
        if (entry == null || entry.Prefab == null)
            return;

        
        currentTool = Instantiate(entry.Prefab, toolSocket);
        var t = currentTool.transform;
        t.localPosition = entry.Position;
        t.localEulerAngles = entry.Rotation;
        t.localScale = entry.Scale;

    
    }

    public void DespawnTool()
    {
        if (currentTool == null) return;

        Destroy(currentTool);
        currentTool = null;

        
    }

}
