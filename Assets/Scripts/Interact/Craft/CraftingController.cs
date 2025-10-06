using GameInteract;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingSlot
{
    public CraftingRecipe Recipe { get; private set; }
    public readonly CraftingSlotData data;
    public event Action<CraftingSlotData,CraftingRecipe> OnCraftFinished;
    public bool IsCrafting { get; private set; }
    CraftingTimer timer = new();

    public CraftingSlot(CraftingSlotData data)
    {
        this.data = data;
    }
    public void StartCraft(CraftingRecipe recipe)
    {
        Recipe = recipe;
        IsCrafting = true;

        timer.OnProgress += UpdateProgress;
        timer.OnFinished += HandleSlotFinished;
    }

    private void UpdateProgress(float value)
    {
        GlobalEvents.Instance.Publish(new CraftingProgressEvent(data.Type, data.SlotIndex, value));
    }

    private void HandleSlotFinished(ITimer t)
    {
        IsCrafting = false;

        OnCraftFinished?.Invoke(data,Recipe);
       
        timer.OnFinished -= HandleSlotFinished;
        timer.OnProgress -= UpdateProgress; 
    }

    public void Reset()
    {
        Recipe = null;
        IsCrafting = false;
    }
}

public class CraftingController
{
    readonly Dictionary<CraftingType, List<CraftingSlot>> slots;
    readonly Dictionary<CraftingType, Dictionary<int, ItemData>> completedByType = new();

    public CraftingController()
    {
        slots = new();

        foreach (CraftingType type in Enum.GetValues(typeof(CraftingType)))
        {
            slots[type] = new List<CraftingSlot>
            {
                new(new CraftingSlotData(type, 0)),
                new(new CraftingSlotData(type, 1))
            };
        }
    }

    public void StartCraft(CraftingType type, int slotIndex, CraftingRecipe recipe)
    {
        if (!TryGetSlot(type, slotIndex, out var slot))
            return;

        slot.OnCraftFinished += HandleCraftFinished;
        slot.StartCraft(recipe);
    }

    void HandleCraftFinished(CraftingSlotData data, CraftingRecipe recipe)
    {
        if (!completedByType.TryGetValue(data.Type, out var dict))
        {
            dict = new Dictionary<int, ItemData>();
            completedByType[data.Type] = dict;
        }

        dict[data.SlotIndex] = recipe.ResultItem;

        GlobalEvents.Instance.Publish(new CraftingCompletedEvent(
            data.Type,
            data.SlotIndex,
            recipe.ResultItem
        ));

        if (TryGetSlot(data.Type, data.SlotIndex, out var slot))
            slot.OnCraftFinished -= HandleCraftFinished;
    }

    public List<(int slotIndex, ItemData result)> GetCompletedResults(CraftingType type)
    {
        if (!completedByType.TryGetValue(type, out var dict) || dict.Count == 0)
            return new List<(int, ItemData)>(0);

        var keys = new List<int>(dict.Keys);
        var list = new List<(int slotIndex, ItemData result)>(keys.Count);

        for (int i = 0; i < keys.Count; i++)
        {
            int idx = keys[i];
            list.Add((idx, dict[idx]));
        }

        return list;
    }

    public ItemData CollectCompletedItem(CraftingType type, int slotIndex)
    {
        if (!completedByType.TryGetValue(type, out var dict))
            return null;

        if (!dict.TryGetValue(slotIndex, out var item))
            return null;

        dict.Remove(slotIndex);
        if (dict.Count == 0)
            completedByType.Remove(type); 

        slots[type][slotIndex].Reset();
        return item;
    }
    bool TryGetSlot(CraftingType type, int index, out CraftingSlot slot)
    {
        if (slots.TryGetValue(type, out var list) && index >= 0 && index < list.Count)
        {
            slot = list[index];
            return true;
        }

        slot = null;
        return false;
    }

    public List<CraftingSlot> GetCurrentCraftingSlots(CraftingType type)
        => slots.TryGetValue(type, out var list) ? list : new List<CraftingSlot>();
}
