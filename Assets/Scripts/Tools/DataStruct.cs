using JetBrains.Annotations;
using System.Collections.Generic;
using System;
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

[Serializable]
public class SerializableDictionary<TKey, TValue>
{
    [Serializable]
    public struct KeyValue
    {
        public TKey Key;
        public TValue Value;
    }

    public List<KeyValue> items = new List<KeyValue>();

    public SerializableDictionary() { }

    public SerializableDictionary(Dictionary<TKey, TValue> dict)
    {
        foreach (var kvp in dict)
        {
            items.Add(new KeyValue { Key = kvp.Key, Value = kvp.Value });
        }
    }

    public Dictionary<TKey, TValue> ToDictionary()
    {
        var dict = new Dictionary<TKey, TValue>();
        foreach (var kv in items)
            dict[kv.Key] = kv.Value;
        return dict;
    }
}
