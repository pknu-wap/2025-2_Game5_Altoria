using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;


public class InventoryItemSlot : ItemSlot, IPointerEnterHandler, IPointerExitHandler
{
    private string itemID;
    private int itemCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    public void Initialize(string id, int count)
    {
        itemID = id;
        itemCount = count;

        SetSlot(id, count);

        //추가 커스터마이징
    }

    public void ClearSlot()
    {

    }

    // 마우스 hover -> 버리기 버튼 표시 
    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
