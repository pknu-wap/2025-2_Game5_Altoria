using GameData;

public static class DataConverter
{
    public static CraftingRecipe ToRecipe(CraftingData data)
    {
        if (data == null)
            return null;

        var requiredItems = new ItemData[data.Ingredients.Count];
        for (int i = 0; i < data.Ingredients.Count; i++)
        {
            string itemId = data.Ingredients[i].ID;
            requiredItems[i] = GameDB.GetItemData(itemId);
        }

        return new CraftingRecipe
        {
            ResultItem = GameDB.GetItemData(data.ID), 
            Time = data.Time,
            RequiredItems = requiredItems
        };
    }
}
