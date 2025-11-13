using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "InventoryDatabase", menuName = "GameData/InventoryDatabase")]
public class InventoryDatabase : ScriptableObject
{
    [Header("가지고 있는 아이템")]
    public List<InventoryData> rows;

    public int maxSlotCount;

    public InventoryData GetItemById(string id)
    {
        return rows.Find(x => x.ID== id);
    }
}