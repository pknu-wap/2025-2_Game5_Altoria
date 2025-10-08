using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace GameInteract
{
    public class CraftItemSlot : MonoBehaviour
    {
        [SerializeField] Image itemImage;
        [SerializeField] TextMeshProUGUI countText;

        int slotIndex;
        public Action<int> OnSlotClick;
        public void SetSlot(int slotIndex, string spriteAddress ,int count)
        {
            this.slotIndex = slotIndex;
            countText.text = count.ToString();
        }
        public void OnClickSlotButton()=>OnSlotClick?.Invoke(slotIndex);
    }
}