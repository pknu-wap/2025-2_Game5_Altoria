using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryItemSlot : ItemSlot, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private InventoryData inventory;

    string itemID;
    string itemName;
    int itemCount1;  //?? 

    float lastClickTime = 0f;
    const float doubleClickThreshold = 0.25f;

    [SerializeField] TextMeshProUGUI equippedText;
    [SerializeField] Button deleteButton; //아이템 버리기 버튼
    [SerializeField] GameObject deletePopUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void Initialize(string id, int count)
    {
        inventory = InventoryManager.Instance.GetItemData(id);

        if (inventory == null)
        {
            Debug.Log($"[InventoryItemSlot] : ID {id} 에 해당하는 아이템 데이터를 찾을 수 없음");
            return;
        }
        if (count <= 0) return;

        itemID = id;
        itemName = inventory.Item.Name;
        itemCount1 = count;
        SetSlot(inventory.Item.SpriteAddress, count, inventory.Item.Grade); 
        equippedText.gameObject.SetActive(inventory.IsEquipped);
        //추가 커스터마이징
    }

    public void ClearSlot()
    {
        itemID = null;
        itemName = null;
        inventory = null;
        itemCount1 = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.time - lastClickTime < doubleClickThreshold)
        {
            OnDoubleClick();
        }
        else
        {
            OnSingleClick();
        }

        lastClickTime = Time.time; // 클릭 시간 갱신
    }


    public void OnSingleClick()
    {
    }

    public void OnDoubleClick()
    {
        if (inventory == null)
        {
            Debug.LogWarning("[InventoryItemSlot] : inventory가 아직 초기화되지 않음!");
            return;
        }

        if (inventory.IsEquipped)
        {
            // 아이템 장착 해제 구현
            inventory.IsEquipped = false;
            equippedText.gameObject.SetActive(false);
            Debug.Log("[InventoryItemSlot] : 아이템 장착해제");
        }
        else
        {
            // 아이템 장착 구현
            inventory.IsEquipped = true;
            equippedText.gameObject.SetActive(true);
            Debug.Log("[InventoryItemSlot] : 아이템 장착됨");
        }


    }

    // 마우스 hover -> 버리기 버튼 표시 
    public void OnPointerEnter(PointerEventData eventData)
    {
        deleteButton.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        deleteButton.gameObject.SetActive(false);  
    }

    public void OnClickDelete()
    {
        Debug.Log("[InventoryItemSlot] : 아이템 버리기 버튼 클릭됨");
        InventoryUI.Instance.OnClickItemDelete(itemID, itemCount1);
    }
}
