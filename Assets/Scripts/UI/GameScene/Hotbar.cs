using Common;
using GameInteract;
using GameItem;
using GameUI;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    [System.Serializable]
    public class HotbarSlotUI
    {
        public string ItemID { private set; get;  }

        public void Bind(ItemData itemData)
        {
            ItemID = itemData.ID;
            // TODO: 슬롯에 아이템 정보 바인딩
        }

        public void Clear()
        {
            // TODO: Default 상태로 초기화
        }
    }

    public class Hotbar : UIWidget
    {
        [SerializeField] List<HotbarSlotUI> slots;

        void Awake()
        {
            // TODO: 데이터 Load하여 장착된 아이템 등록
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
            var index = FindSlotIndexByItem(item.ItemData.ID);
            slots[index].Bind(item.ItemData);
        }
        void HandleUnequipped(EquipItem item)
        {
            var index = FindSlotIndexByItem(item.ItemData.ID);
            if (index >= 0) slots[index].Clear();
        }

        public void LevelChanged(string itemID, int newCount)
        {
            int idx = FindSlotIndexByItem(itemID);
            if (idx < 0) return;

            if (newCount == -1)
                slots[idx].Clear();
            else
            {
                // TODO: 강화 단계만 변경
            }
        }

        int FindSlotIndexByItem(string itemID)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].ItemID == itemID)
                    return i;
            }
            return -1;
        }
    }
}
