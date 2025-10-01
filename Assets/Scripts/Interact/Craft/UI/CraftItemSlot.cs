using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace GameInteract
{
    public class CraftItemSlot : MonoBehaviour
    {
        [SerializeField] Image itemImage;
        [SerializeField] TextMeshProUGUI countText;

        int slotIndex;
        public Action<int> onClick;
        public void SetSlot(int slotIndex, Sprite sprite ,int count)
        {
            this.slotIndex = slotIndex;
            itemImage.sprite = sprite;
        }
        public void OnClickSlot() => onClick?.Invoke(slotIndex);

    }
}