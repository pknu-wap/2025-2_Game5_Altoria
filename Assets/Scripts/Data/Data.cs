using UnityEngine;
using static Define;

[System.Serializable]
public class ItemData
{
    public string ID;
    public string Name;
    public string SpriteAddress;
    public ItemType Type;
    public int SellCost;
    [TextArea] public string Description;
}