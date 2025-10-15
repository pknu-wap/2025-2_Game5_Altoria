using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishDataSO", menuName = "GameData/FishDataSO")]
public class FishDataSO : ScriptableObject
{
    public List<FishData> rows;
}
