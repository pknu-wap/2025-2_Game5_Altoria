using GameInteract;
using UnityEngine;

[System.Serializable]
public struct CraftingStartedEvent
{
    public CraftingType Type;
    public int SlotIndex;
    public ItemData Item;
    public CraftingStartedEvent(CraftingType type, int slotIndex=0, ItemData item =null)
    {
        this.Type = type;
        this.SlotIndex = slotIndex;
        this.Item = item;
    }
}
[System.Serializable]
public struct CraftingProgressEvent
{
    public CraftingType Type;
    public int SlotIndex;
    public float Progress;   
    public CraftingProgressEvent(CraftingType type, int slotIndex = 0, float progress = 0f)
    {
        this.Type = type;
        this.SlotIndex = slotIndex;
        this.Progress = progress;
    }
   
}
[System.Serializable]
public struct CraftingCompletedEvent
{
    public CraftingType Type;
    public int SlotIndex;
    public ItemData Item;
    public CraftingCompletedEvent(CraftingType type, int  slotIndex= 0, ItemData item=null)
    {
        this.Type = type;
        this.SlotIndex = slotIndex;
        this.Item = item;
    }
}

[System.Serializable]
public class CraftingRecipe
{
    public ItemData ResultItem;
    public float Time;
    public ItemData[] RequiredItems;
}
