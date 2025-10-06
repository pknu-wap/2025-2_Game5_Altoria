using GameInteract;
using System.Collections.Generic;
using UnityEngine;


namespace Common
{
    public class GameSystem
    {
        CraftingController craft = new();


        public List<CraftingSlot> GetCurrentCraftingSlots(CraftingType type) => craft.GetCurrentCraftingSlots(type);
        public bool HaveEmptySlot(CraftingType type)=>craft.HaveEmptySlot(type);    

        public CraftingSlot GetCraftingSlot(CraftingType type, int slotIndex)=>craft.GetCraftingSlot(type, slotIndex);  
    }
}
