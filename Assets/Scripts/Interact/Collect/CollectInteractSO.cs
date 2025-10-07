using System.Collections.Generic;
using UnityEngine;
using static Define;

[System.Serializable]
public class CollectItem
{
    public int areaID;
    public int itemID;           // ¾ÆÀÌÅÛ °íÀ¯ ID
    public float baseProbability;   // µå¶ø È®·ü
}

[CreateAssetMenu(fileName = "CollectInteractData", menuName = "GameInteract/CollectInteractSO")]
public class CollectInteractSO : ScriptableObject
{
    public CollectType collectType;   // ³¬½Ã, Ã¤±¤, ³ó»ç µî
    public int areaID;           // Áö¿ª ID (A, B, ...)
    public List<CollectItem> drops;
}
