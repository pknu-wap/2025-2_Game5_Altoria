using GameInteract;
using System.Collections.Generic;
using UnityEngine;


namespace Common
{
    public class GameSystem
    {
        public readonly CraftingController Crafting = new();

    
        public List<CraftingSlot> GetCurrentCraftingSlots(CraftingType type) => Crafting.GetCurrentCraftingSlots(type);
        public bool HaveEmptySlot(CraftingType type) => Crafting.HaveEmptySlot(type);    
        public CraftingSlot GetCraftingSlot(CraftingType type, int slotIndex) => Crafting.GetCraftingSlot(type, slotIndex);  

        public ItemData GetCompletedItem(CraftingType type, int slotIndex) => Crafting.GetCompletedItem(type, slotIndex);
    }
}
