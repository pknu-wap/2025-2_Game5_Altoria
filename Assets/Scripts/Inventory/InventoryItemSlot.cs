using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryItemSlot : ItemSlot, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private ItemData itemData;

    string itemID;
    int itemCount1;  //?? 

    float lastClickTime = 0f;
    const float doubleClickThreshold = 0.25f;

    [SerializeField] TextMeshProUGUI equippedText;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] GameObject description;
    [SerializeField] Button deleteButton;
    GameObject deletePopUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*
        deletePopUp = GameObject.Find("ItemDeletePopUp");
        if (deletePopUp != null)
            deleteButton.onClick.AddListener(() => deletePopUp.SetActive(true));
        else
            Debug.Log("????????");
        */
    }

    public void Initialize(string id, int count)
    {
        itemData = InventoryManager.Instance.GetItemData(id);

        itemID = id;
        itemCount1 = count;
        if (itemData == null)
        {
            Debug.LogWarning($"[InventoryItemSlot] : ID {id} 에 해당하는 아이템 데이터를 찾을 수 없습니다!");
            return;
        }
        SetSlot(itemData.Name, count);
        
        //추가 커스터마이징
    }

    public void ClearSlot()
    {
        itemID = null;
        itemData = null;
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
        if (equippedText.gameObject.activeSelf)
        {
            // 아이템 장착 해제 구현
            equippedText.gameObject.SetActive(false);
            Debug.Log("[InventoryItemSlot] : 아이템 장착해제");
        }
        else
        {
            // 아이템 장착 구현
            equippedText.gameObject.SetActive(true);
            Debug.Log("[InventoryItemSlot] : 아이템 장착됨");
        }


    }

    // 마우스 hover -> 버리기 버튼 표시 
    public void OnPointerEnter(PointerEventData eventData)
    {
        description.SetActive(true);
        if (itemData == null) Debug.Log("ffffffffffdnullff");
        itemNameText.text = itemData.Name;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description.SetActive(false);    // 버튼 누르기도 전에 꺼져버려서 나중에 수정
    }

    public void OnClickDelete()
    {
        ItemDeletePopUp.Instance.OnOpen();
    }
}
