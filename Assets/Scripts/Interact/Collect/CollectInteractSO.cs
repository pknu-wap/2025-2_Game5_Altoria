using System.Collections.Generic;
using UnityEngine;
using static Define;

[System.Serializable]
public class CollectItem
{
    public string itemID;           // ¾ÆÀÌÅÛ °íÀ¯ ID
    public string content;
    public string areaID;
    public float baseProbability;   // µå¶ø È®·ü
}

[CreateAssetMenu(fileName = "CollectInteractData", menuName = "GameInteract/CollectInteractSO")]
public class CollectInteractSO : ScriptableObject
{
    public CollectType collectType;   // ³¬½Ã, Ã¤±¤, ³ó»ç µî
    public string areaID;           // Áö¿ª ID (A, B, ...)
    public List<CollectItem> drops;
}
