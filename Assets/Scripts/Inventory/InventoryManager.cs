using System.Collections.Generic;
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
        if (inventoryData.rows.Count >= inventoryData.maxSlotCount)
        {
            Debug.Log("[InventoryManager] : 인벤토리가 가득참");
            return false;
        }

        InventoryData existing = inventoryData.rows.Find(x => x.ID == itemID);

        if (existing != null)
        {
            existing.Count += count;
            Debug.Log($"{itemID} {count}개 추가 (총 {existing.Count})");
            return true;
        }
        else
        {
            InventoryData newItem = new InventoryData
            {
                ID = itemID,
                Count = count,
            };

            inventoryData.rows.Add(newItem);
            Debug.Log($"{itemID} {count}개 새 슬롯에 추가");
            return true;
        }
    }


    public bool RemoveItem(string itemID, int count = 1)
    {
        InventoryData existing = inventoryData.rows.Find(x => x.ID == itemID);

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


    public ItemData GetItemData(string itemID)
    {
        return itemDatabase.GetItemById(itemID);
    }


    public List<InventoryData> GetAllItems()
    {
        // 실제 인벤토리 SO 참조하지 않고 복제본으로 참조하는 것 검토중 (데이터 영구손실 방지)
        return inventoryData.rows;
    }
}
