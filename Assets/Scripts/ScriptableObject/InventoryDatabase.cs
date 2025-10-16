using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "GameData/InventoryDatabase")]
public class InventoryDatabase : ScriptableObject
{
    [Header("모든 아이템 정보 데이터베이스")]
    [SerializeField] ItemDatabaseSO itemDatabase;

    [Header("가지고 있는 아이템")]
    public List<InventoryData> rows;

    public int maxSlotCount;

    public InventoryData GetItemById(string id)
    {
        return rows.Find(item => item.ID == id);
    }
}
