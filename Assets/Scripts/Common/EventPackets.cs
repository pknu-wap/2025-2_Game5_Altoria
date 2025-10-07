using GameInteract;
using UnityEngine;

[System.Serializable]
public struct CraftingStartedEvent
{
    public CraftingType Type;
    public int SlotIndex;
    public ItemData Item;
}
[System.Serializable]
public struct CraftingProgressEvent
{
    public CraftingType Type;
    public int SlotIndex;
    public float Progress;   
    public CraftingProgressEvent(CraftingType type, int slotIndex = 0, float progress = 0f)
    {
        Type = type;
        SlotIndex = slotIndex;
        Progress = progress;
    }
   
}
[System.Serializable]
public struct CraftingCompletedEvent
{
    public CraftingType Type;
    public int SlotIndex;
    public ItemData Item;
}

[System.Serializable]
public class CraftingRecipe
{
    public ItemData ResultItem;
    public float Time;
    public ItemData[] RequiredItems;
}
