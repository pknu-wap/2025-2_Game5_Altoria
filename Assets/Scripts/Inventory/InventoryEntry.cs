using UnityEngine;
using GameItem;
using GameData;

namespace GameInventory
{
    // runtime
    public class InventoryEntry
    {
        public Item item;
        public int count;
        public bool isEquipped;

        public InventoryEntry(string ID, int Count = 1, bool IsEquipped = false)
        {
            item = ItemFactory.CreateItem(GameDB.GetItemData(ID));
            count = Count;
            isEquipped = IsEquipped;
        }
    }
}