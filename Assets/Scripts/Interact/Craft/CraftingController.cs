 using GameInteract;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSlot
{
    public CraftingRecipe Recipe { get; private set; }
    public float Elapsed { get; private set; }
    public bool IsCrafting { get; private set; }

    public void StartCraft(CraftingRecipe recipe)
    {
        Recipe = recipe;
        Elapsed = 0;
        IsCrafting = true;
    }

    public void Tick(float deltaTime)
    {
        if (!IsCrafting) return;
        Elapsed += deltaTime;

        if (Elapsed >= Recipe.Time)
        {
            IsCrafting = false;
        }
    }
}
public class CraftingController
{
    private Dictionary<CraftingType, List<CraftingSlot>> slots;

    public CraftingController()
    {
        slots = new Dictionary<CraftingType, List<CraftingSlot>>();
        foreach (CraftingType type in Enum.GetValues(typeof(CraftingType)))
        {
            slots[type] = new List<CraftingSlot> { new CraftingSlot(), new CraftingSlot() };
        }
    }

    public void StartCraft(CraftingType type, int slotIndex, CraftingRecipe recipe)
    {
        if (slots.TryGetValue(type, out var slotList) && slotIndex < slotList.Count)
        {
            slotList[slotIndex].StartCraft(recipe);
        }
    }

    public void Update(float deltaTime)
    {
        foreach (var slotList in slots.Values)
        {
            foreach (var slot in slotList)
                slot.Tick(deltaTime);
        }
    }
}
