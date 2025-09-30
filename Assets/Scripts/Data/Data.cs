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
    [TextArea] public string Description;
}
[System.Serializable]   
public class CraftingRecipe
{
    public ItemData ResultItem;
    public float Time;
    public ItemData[] RequiredItems;
}
[System.Serializable]
public class CraftingData
{
    public string ID;
    public int Count;
    public List<string> Ingredients;
}