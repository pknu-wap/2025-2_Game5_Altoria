using GameInteract;
using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
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
    public int ItemGrade;
    [TextArea] public string Description;
}
[System.Serializable]
public class CraftingData
{
    public CraftingType Type;
    public string ID;
    public int Count;
    public float Time;
    public List<Ingredient> Ingredients;
}
[System.Serializable]
public class CraftingSlotData
{
    public CraftingType Type;
    public int SlotIndex;

    public CraftingSlotData(CraftingType type, int slotIndex)
    {
        Type = type;
        SlotIndex = slotIndex;
    }
}
[System.Serializable]
public class InventoryData
{
    public string ID;
    public string Name;
    public bool IsEquipped;
    public int Count;
} 