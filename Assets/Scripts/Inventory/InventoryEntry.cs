using UnityEngine;
using GameItem;

namespace GameInventory
{
    // runtime
    public class InventoryEntry
    {
        public Item item;
        public int count;
        public bool isEquipped;

        public InventoryEntry(Item Item, int Count = 1, bool IsEquipped = false)
        {
            item = Item;
            count = Count;
            isEquipped = IsEquipped;
        }
    }
}
