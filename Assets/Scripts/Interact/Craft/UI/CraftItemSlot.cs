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
        public void SetSlot(int slotIndex, string sprite ,int count)
        {
            this.slotIndex = slotIndex;
            countText.text = count.ToString();
        }
        public void OnClickSlot() => onClick?.Invoke(slotIndex);

    }
}