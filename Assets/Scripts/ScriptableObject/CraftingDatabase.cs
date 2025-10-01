using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingData", menuName = "GameData/CraftingData")]
public class CraftingDataSO : ScriptableObject
{
    public List<CraftingData> rows;
}