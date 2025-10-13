using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectDataSO", menuName = "GameData/CollectDataSO")]
public class CollectDataSO : ScriptableObject
{
    public List<CollectData> rows;
}
