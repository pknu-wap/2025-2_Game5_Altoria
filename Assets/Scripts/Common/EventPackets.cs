using GameInteract;
using UnityEngine;

public struct CraftingStartedEvent
{
    public CraftingType Type;
    public int SlotIndex;
    public ItemData Item;
}

public struct CraftingProgressEvent
{
    public CraftingType Type;
    public int SlotIndex;
    public float Progress;   
    public ItemData Item;
}

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
