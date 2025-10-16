using Common;
using GameData;
using System.Collections.Generic;

namespace GameInteract
{
    [System.Serializable]
    public struct CraftSlotInfo
    {
        public int Index;
        public string ItemID;
        public int Count;
        public string SpriteAddress;

        public CraftSlotInfo(int index, string itemID, int count, string spriteAddress)
        {
            Index = index;
            ItemID = itemID;
            Count = count;
            SpriteAddress = spriteAddress;
        }
    }
    public class CraftingHandler
    {
        readonly CraftingType type;
        readonly List<CraftingData> craftingList = new();

        public CraftingHandler(CraftingType type)
        {
            this.type = type;
            var dict = GameDB.GetCraftTypeData(type);

            if (dict == null)
            {
                UnityEngine.Debug.LogWarning($"[CraftingHandler] No data found for {type}");
                return;
            }

            craftingList.AddRange(dict.Value.Values);
        }

        public int Count => craftingList.Count;

        public CraftingData GetCraftingData(int index)
        {
            if (index < 0 || index >= craftingList.Count)
                return null;
            return craftingList[index];
        }
      
       
        public ItemData GetResultItem(int index)
        {
            CraftingData data = GetCraftingData(index);
            return data == null ? null : GameDB.GetItemData(data.ID);
        }

        public List<ItemEntry> GetIngredients(int index)
        {
            CraftingData data = GetCraftingData(index);
            if (data == null || data.Ingredients == null || data.Ingredients.Count == 0)
                return null;

            List<ItemEntry> list = new(data.Ingredients.Count);

            for (int i = 0; i < data.Ingredients.Count; i++)
            {
                Ingredient ingredient = data.Ingredients[i];
                ItemData mat = GameDB.GetItemData(ingredient.ID);
                if (mat == null) continue;

                ItemEntry entry = new ItemEntry(mat, ingredient.Count);
                list.Add(entry);
            }

            return list;
        }

        
        public List<CraftSlotInfo> GetSlotInfos()
        {
            List<CraftSlotInfo> infos = new ();
            for (int index = 0; index < craftingList.Count; index++)
            {
                CraftingData data = craftingList[index];
                ItemData itemData = GameDB.GetItemData(data.ID);
                if (itemData == null) continue;

                infos.Add(new CraftSlotInfo(index, data.ID, data.Count, itemData.SpriteAddress));
            }
            return infos;
        }

        public void StartCrafting(int index)
        {
            CraftingData craftData = GetCraftingData(index);

            if (craftData == null)
            {
                UnityEngine.Debug.LogWarning($"[CraftingHandler] No valid result for slot {index}");
                return;
            }
            CraftingRecipe recipe = DataConverter.ToRecipe(craftData);
            GlobalEvents.Instance.Publish(new CraftingStartedEvent
            {
                Type = type,
                SlotIndex = index,
                Recipe = recipe
            });
        }
        public void GetCompletedItem(int index)
        {
            GameSystem.Instance.GetCompletedItem(type, index);
        }
        #region Facade 
        public bool HaveEmptySlot() => GameSystem.Instance.HaveEmptySlot(type);
        public CraftingSlot GetCraftingSlot(int slotIndex) => GameSystem.Instance.GetCraftingSlot(type, slotIndex);
        #endregion
    }
}
