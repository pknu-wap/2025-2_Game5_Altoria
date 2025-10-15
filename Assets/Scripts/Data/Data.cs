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

    // 임시 아이템을 생성하기 위해 생성자를 추가했습니다.
    // 추후 인벤토리가 완성되어 인벤토리에서 아이템을 추출할 때 삭제하겠습니다.
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
    public ItemData Item;
    public int Count;
}
[System.Serializable]
public class UpgradeData
{

}

[System.Serializable]
public class GradeData
{
    public int CurrentGrade;
    public int Success;
    public int Fail;
    public int Destroy;
    public int bous;
    public int Durability;
}
[System.Serializable]
public class CollectData
{
    public string ID;
    public string Count;
    public int Probability;
}
[System.Serializable]
public class FishData
{
    public string ID;
    public string Area;
    public int Probability;
}