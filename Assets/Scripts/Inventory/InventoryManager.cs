using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("아이템 데이터베이스 (모든 아이템 정보)")]
    [SerializeField] private ItemDatabaseSO itemDatabase;  //미완성

    [Header("플레이어 인벤토리 데이터 (보유 중 아이템 목록)")]
    [SerializeField] private InventoryDatabase inventoryData;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    public bool AddItem(string itemID, int count = 1)
    {
        InventoryData existing = inventoryData.rows.Find(x => x.Item != null && x.Item.ID == itemID);

        if (existing != null) //이미 존재하면 
        {
            existing.Count += count;
            Debug.Log($"{itemID} {count}개 추가 (총 {existing.Count})");
            return true;
        }

        if (inventoryData.rows.Count >= inventoryData.maxSlotCount)
        {
            Debug.Log("[InventoryManager] : 인벤토리가 가득참");
            return false;
        }


        ItemData itemData = itemDatabase.GetItemById(itemID);

        if (itemData == null)
        {
            Debug.LogWarning($"[InventoryManager] : {itemID} 아이템 정보를 찾을 수 없음");
            itemData = new ItemData { ID = itemID, Name = "Temp Item" };
        }

        // 새로운 인벤토리 데이터 생성
        InventoryData newItem = new InventoryData
        {
            Item = itemData,
            Count = count,
            IsEquipped = false
        };

        inventoryData.rows.Add(newItem);
        Debug.Log($"[InventoryManager] : {itemID} {count}개 새 슬롯 추가 (현재 슬롯 수: {inventoryData.rows.Count})");
        return true;
    }


    public bool RemoveItem(string itemID, int count = 1)
    {
        InventoryData existing = inventoryData.rows.Find(x => x.Item.ID == itemID);

        if (existing == null)
        {
            Debug.LogWarning($"{itemID} 아이템이 인벤토리에 없습니다.");
            return false;
        }

        existing.Count -= count;
        if (existing.Count <= 0)
        {
            inventoryData.rows.Remove(existing);
            Debug.Log($"{itemID} 전부 삭제됨");
        }
        else
        {
            Debug.Log($"{itemID} {count}개 삭제됨 (남은 수량: {existing.Count})");
        }

        return true;
    }


    public InventoryData GetItemData(string itemID)
    {
        return inventoryData.GetItemById(itemID);
    }


    public List<InventoryData> GetAllItems()
    {
        return inventoryData.rows;
    }

}
