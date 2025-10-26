using GameInteract;
using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

[System.Serializable]
public class ItemData
{
    public string ID;
    public string Name;
    public string SpriteAddress;
    public Content content;
    public ItemGrade Grade;
    public ItemType Type;
    [TextArea] public string Description;
    public ItemData() { }
    public ItemData(string id, ItemGrade grade)
    {
        ID = id;
        Grade = grade;
    }
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
[System.Serializable]
public class UpgradeData
{
    public int CurrentGrade;
    public int Success;
    public int Fail;
    public int Destroy;
    public int Bous;
    public int Material;
}
[System.Serializable]
public class CollectData
{
    public string ID;
    public List<CollectGroup> CollectGroup;
}
[System.Serializable]
public class FishData
{
    public AreaType Area;
    public List<FishGroup> FishGroups;
}