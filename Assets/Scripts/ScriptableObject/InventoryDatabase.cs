using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "GameData/InventoryDatabase")]
public class InventoryDatabase : ScriptableObject
{
    public int maxSlotCount = 30;

    public List<InventoryData> rows;
}
