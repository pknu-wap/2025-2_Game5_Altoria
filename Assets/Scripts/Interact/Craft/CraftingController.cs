using GameInteract;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum CraftingState
{
    None,
    Crafting,
    Completed
}
public class CraftingSlot
{
    public CraftingRecipe Recipe { get; private set; }
    public CraftingState State => state;
    public event Action<CraftingSlotData,CraftingRecipe> OnCraftFinished;
    
    CraftingSlotData data;
    CraftingState state =CraftingState.None;    
    CraftingTimer timer = new();

    public CraftingSlot(CraftingSlotData data)
    {
        this.data = data;
    }
    public void StartCraft(CraftingRecipe recipe)
    {
        Recipe = recipe;
        state = CraftingState.Crafting;

        timer.OnProgress += UpdateProgress;
        timer.OnFinished += HandleSlotFinished;

        timer.SetTimer(recipe.Time);
    }

     void UpdateProgress(float value) => GlobalEvents.Instance.Publish(new CraftingProgressEvent(data.Type, data.SlotIndex, value));

    private void HandleSlotFinished(ITimer t)
    {
        state = CraftingState.Completed;

        OnCraftFinished?.Invoke(data,Recipe);
       
        timer.OnFinished -= HandleSlotFinished;
        timer.OnProgress -= UpdateProgress; 
    }
    
    public void Reset()
    {
        Recipe = null;
        state = CraftingState.None;
    }
}

public class CraftingController 
{
    readonly Dictionary<CraftingType, List<CraftingSlot>> slots;
    readonly Dictionary<CraftingType, Dictionary<int, ItemData>> completedByType = new();

    public CraftingController()
    {

        slots = new Dictionary<CraftingType, List<CraftingSlot>>();
        Array types = Enum.GetValues(typeof(CraftingType));
        for (int i = 0; i < types.Length; i++)
        {
            CraftingType type = (CraftingType)types.GetValue(i);
            slots[type] = new List<CraftingSlot>
            {
                new(new CraftingSlotData(type, 0)),
                new(new CraftingSlotData(type, 1))
            };
        }
        GlobalEvents.Instance.Subscribe<CraftingStartedEvent>(StartCraft);

    }
  

    public void StartCraft(CraftingStartedEvent packet)
    {
        if (!slots.TryGetValue(packet.Type, out var list))
            return;
        Debug.Log($"[StartCraft] 호출 / Controller 해시: {GetHashCode()} / Type={packet.Type}");

        CraftingSlot emptySlot = list.FirstOrDefault(s => s.State == CraftingState.None);

        emptySlot.OnCraftFinished += HandleCraftFinished;
        emptySlot.StartCraft(packet.Recipe);
    }

    void HandleCraftFinished(CraftingSlotData data, CraftingRecipe recipe)
    {
        if (!completedByType.TryGetValue(data.Type, out var dict))
        {
            dict = new Dictionary<int, ItemData>();
            completedByType[data.Type] = dict;
        }

        ItemData resultItem = recipe.ResultItem.Item;
        dict[data.SlotIndex] = resultItem;

        GlobalEvents.Instance.Publish(new CraftingCompletedEvent(
            data.Type,
            data.SlotIndex,
            resultItem
        ));

        if (TryGetSlot(data.Type, data.SlotIndex, out var slot))
            slot.OnCraftFinished -= HandleCraftFinished;
    }


    public ItemData GetCompletedItem(CraftingType type, int slotIndex)
    {
        if (!completedByType.TryGetValue(type, out var dict))
            return null;

        if (!dict.TryGetValue(slotIndex, out var item))
            return null;
        //TODO : 인벤과 연결 
        dict[slotIndex] = null;

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
    public bool HaveEmptySlot(CraftingType type)
    {
        if (!slots.TryGetValue(type, out var list)) return false;
        CraftingSlot emptySlot = list.FirstOrDefault(s => s.State == CraftingState.None);
        return emptySlot != null;
    }
    public CraftingSlot GetCraftingSlot(CraftingType type, int slotIndex) => slots[type][slotIndex];
    public List<CraftingSlot> GetCurrentCraftingSlots(CraftingType type) => slots[type];

    
}
