using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Game/Database/ItemDatabase", order = 0)]
public class ItemDatabaseSO : ScriptableObject
{
    [Header("아이템 데이터 테이블")]
    public List<ItemData> rows = new List<ItemData>();

    public ItemData? GetItemById(string id)
    {
        return rows.Find(item => item.ID == id);
    }
}
