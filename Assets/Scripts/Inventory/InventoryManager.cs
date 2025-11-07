using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;
using GameItem;
using GameInventory;
using UnityEditor;
using GameData;

public class InventoryManager 
{


 

    [Header("플레이어 인벤토리 데이터 (보유 중 아이템 목록)")]
   InventoryDatabase inventoryData; // 데이터 저장용

    List<InventoryEntry> inventory = new(); // 런타임용

   
    public void Init()
    {

        inventoryData = Resources.Load<InventoryDatabase>(nameof(InventoryDatabase));
        InventoryLoad();
    }

    public bool AddItem(string itemID, int count = 1)
    {
        InventoryEntry existing = inventory.Find(x => x.item.ItemData.ID == itemID);

        if (existing != null) //이미 존재하면 
        {
            existing.count += count;
            Debug.Log($"[InventoryManager] : {itemID} {count}개 추가 (총 {existing.count})");
            return true;
        }

        if (inventory.Count >= inventoryData.maxSlotCount)
        {
            Debug.Log("[InventoryManager] : 인벤토리가 가득참");
            return false;
        }


        ItemData itemData = GameDB.GetItemData(itemID);

        if (itemData == null)
        {
            Debug.LogWarning($"[InventoryManager] : {itemID} 아이템 정보를 찾을 수 없음");
            itemData = new ItemData { ID = itemID, Name = "Temp Item" };
        }

        // 새로운 아이템 생성
        InventoryEntry newItem = new InventoryEntry(ItemFactory.CreateItem(itemData), count);

        inventory.Add(newItem);
        Debug.Log($"[InventoryManager] : {itemID} {count}개 새 슬롯 추가 (현재 슬롯 수: {inventory.Count})");
        return true;
    }


    public bool RemoveItem(string itemID, int count = 1)
    {
        InventoryEntry existing = inventory.Find(x => x.item.ItemData.ID == itemID);

        if (existing == null)
        {
            Debug.LogWarning($"[InventoryManager] : {itemID} - 잘못된 ItemID");
            return false;
        }

        existing.count -= count;
        if (existing.count <= 0)
        {
            inventory.Remove(existing);
            Debug.Log($"[InventoryManager] : {itemID} 전부 삭제됨");
        }
        else
        {
            Debug.Log($"[InventoryManager] : {itemID} {count}개 삭제됨 (남은 수량: {existing.count})");
        }

        return true;
    }


    public InventoryEntry GetItem(string itemID)
    {
        return inventory.Find(x => x.item.ItemData.ID == itemID);
    }


    public List<InventoryEntry> GetAllItems()
    {
        return inventory;
    }

    // Load & Save
    // inventoryData에서 꺼내기 
    public void InventoryLoad()  // 일단 public으로 해둠
    {
        inventory.Clear();

        foreach (var data in inventoryData.rows)
        {
            if (data.Item == null) continue;

            Item item = ItemFactory.CreateItem(data.Item);
            InventoryEntry newItem = new InventoryEntry(item, data.Count, data.IsEquipped);

            inventory.Add(newItem);
        }
        Debug.Log("[InventoryManager] : 인벤토리 데이터 로드 완료");
    }

    // inventoryData에 넣기
    public void InventorySave()
    {
        inventoryData.rows.Clear();
        //EditorUtility.SetDirty();
        foreach (var entry in inventory)
        {
            if (entry.item == null) continue;

            InventoryData data = new InventoryData
            {
                Item = entry.item.ItemData,
                Count = entry.count,
                IsEquipped = entry.isEquipped
            };

            inventoryData.rows.Add(data);
        }
        Debug.Log("[InventoryManager] : 인벤토리 데이터 세이브 완료");
    }
}
