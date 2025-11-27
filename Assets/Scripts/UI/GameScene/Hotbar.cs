using Common;
using GameItem;
using static Define;
using System.Collections.Generic;
using UnityEngine;
namespace GameUI
{
    public class Hotbar : UIWidget
    {
        [SerializeField] List<HotbarSlot> slots;

        void Awake()
        {
            for(int i = (int)ContentType.Fish; i <= (int)ContentType.Mining; i++)
            {
                var item = GameSystem.Inventory.GetEquipItem((ContentType)i);
                if(item != null)
                    slots[i - (int)ContentType.Fish].Bind(item.ItemData);
            }
        }

        void OnEnable() => Subscribe();
        void OnDisable() => Unsubscribe();

        void Subscribe()
        {
            GameSystem.Inventory.OnItemEquipped += HandleEquipped;
            GameSystem.Inventory.OnItemUnequipped += HandleUnequipped;
        }
        void Unsubscribe()
        {
            GameSystem.Inventory.OnItemEquipped -= HandleEquipped;
            GameSystem.Inventory.OnItemUnequipped -= HandleUnequipped;
        }

        void HandleEquipped(EquipItem item)
        {
            var index = FindSlotIndexByItem(item.ItemData.Content);

            Debug.Log($"Hotbar: HandleEquipped - Item {item.ItemData.ID} equipped to slot {index}");

            if (index >= 0)
                slots[index].Bind(item.ItemData);
        }
        void HandleUnequipped(EquipItem item)
        {
            var index = FindSlotIndexByItem(item.ItemData.Content);

            if (index >= 0) 
                slots[index].Clear();
        }

        public void LevelChanged(ItemData itemData, int newCount)
        {
            for(int i = 0; i < slots.Count; i++)
            {
                if (slots[i].ItemData == null)
                    continue;

                if (slots[i].ItemData.ID == itemData.ID)
                {
                    slots[i].Bind(itemData);
                    return;
                }
            }
        }

        int FindSlotIndexByItem(ContentType type)
        {
            for (int i = (int)ContentType.Fish; i <= (int)ContentType.Mining; i++)
            {
                if ((ContentType)i == type)
                    return i - (int)ContentType.Fish;
            }
            return -1;
        }
    }
}
