using GameData;
using System.Collections.Generic;

public static class DataConverter
{
    public static CraftingRecipe ToRecipe(CraftingData data)
    {
        if (data == null)
            return null;

        var requiredItems = new ItemEntry[data.Ingredients.Count];
        for (int i = 0; i < data.Ingredients.Count; i++)
        {
            string itemId = data.Ingredients[i].ID;
            int itemCount = data.Ingredients[i].Count;

            ItemData itemData = GameDB.GetItemData(itemId);
            requiredItems[i] = new ItemEntry(itemData, itemCount);
        }

        ItemData resultItemData = GameDB.GetItemData(data.ID);
        var resultEntry = new ItemEntry(resultItemData, data.Count);


        return new CraftingRecipe
        {
            ResultItem = resultEntry,
            Time = data.Time,
            RequiredItems = requiredItems
        };
    }
}
