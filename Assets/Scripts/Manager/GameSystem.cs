using GameInteract;
using UnityEngine;

public class GameSystem 
{
    CraftingController craft = new();

    public void StartCraft(CraftingType type, int slotIndex, CraftingRecipe recipe)
        => craft.StartCraft(type, slotIndex, recipe);
  
}
