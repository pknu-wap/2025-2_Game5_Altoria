using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "GameData/InventoryDatabase")]
public class InventoryDatabase : ScriptableObject
{
    

    [Header("가지고 있는 아이템")]
    public List<InventoryData> rows;

    public int maxSlotCount;

    public InventoryData GetItemById(string id)
    {
        return rows.Find(x => x.Item.ID== id);
    }
}