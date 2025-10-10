using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "GameData/InventoryDatabase")]
public class InventoryDatabase : ScriptableObject
{
    [Header("아이템 데이터베이스 (모든 아이템 정보)")]
    ItemDatabaseSO itemDatabase;

    public int maxSlotCount = 30;

    public List<InventoryData> rows;
}
