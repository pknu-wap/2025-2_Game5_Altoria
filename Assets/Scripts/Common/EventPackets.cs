using GameInteract;
using UnityEngine;

public class CraftingStartedEvent
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
