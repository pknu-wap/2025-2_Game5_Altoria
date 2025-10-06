using GameInteract;
using System;
using System.Collections.Generic;
using UnityEngine;


public class CraftingSlot
{
    public CraftingRecipe Recipe { get; private set; }
    CraftingSlotData data;
    CraftingTimer timer = new();

    public Action<CraftingSlotData> OnCraftFinished;
    public CraftingSlot(CraftingSlotData data) { this.data = data; }
    public bool IsCrafting { get; private set; }
    public void StartCraft(CraftingRecipe recipe)
    {
        Recipe = recipe;
        timer.OnProgress += UpdateProgress;
        timer.OnFinished += HandleSlotFinished;
        IsCrafting = true;
    }

    private void UpdateProgress(float value)
        => GlobalEvents.Instance.Publish<CraftingProgressEvent>
        (
            new CraftingProgressEvent(data.Type, data.SlotIndex, value)
        );

    void HandleSlotFinished(ITimer timer)
    {
        

    }
    public void Reset() => Recipe = null;
}

public class CraftingController
{
    private Dictionary<CraftingType, List<CraftingSlot>> slots;

    public CraftingController()
    {
        slots = new Dictionary<CraftingType, List<CraftingSlot>>();

        Array types = Enum.GetValues(typeof(CraftingType));
        for (int i = 0; i < types.Length; i++)
        {
            CraftingType type = (CraftingType)types.GetValue(i);
            var slotList = new List<CraftingSlot>
            {
                new CraftingSlot(new CraftingSlotData(type, 0)),
                new CraftingSlot(new CraftingSlotData(type, 1))
            };

            slots[type] = slotList;
        }
       
    }
    
    public void StartCraft(CraftingType type, int slotIndex, CraftingRecipe recipe)
    {
        if (slots.TryGetValue(type, out var slotList) && slotIndex < slotList.Count)
        {
            slotList[slotIndex].StartCraft(recipe);
        }
    }

    public bool CanCrafting(string ID)
    {
        return true;
    }
}
