using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectHandler", menuName = "Manager/CollectHandler")]
public class CollectHandler : ScriptableObject
{
    [SerializeField] List<FishDataSO> fishSoData;
    [SerializeField] List<CollectDataSO> collectSoData;

    public FishDataSO FishiSO(Define.AreaType areaType) => fishSoData[(int)areaType - 1];
    public CollectDataSO CollectSo(string id) => collectSoData[(Convert.ToInt32(id) % 100) - 1];
}
