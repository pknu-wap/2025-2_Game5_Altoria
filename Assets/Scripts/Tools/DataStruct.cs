using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public struct StringPair
{
    public string V1;
    public string V2;
}
[System.Serializable]
public struct ItemEntry
{
    public ItemData Item;
    public int Value;
    public ItemEntry(ItemData item, int value = 0)
    {
        Item = item;
        Value = value;
    }
}

[System.Serializable]
public class Ingredient
{
    public string ID;
    public int Count;
}
[System.Serializable]
public class FishGroup
{
    public string ID;
    public int Probability;
}
[System.Serializable]
public class CollectGroup
{
    public int Count;
    public int Probability;
}