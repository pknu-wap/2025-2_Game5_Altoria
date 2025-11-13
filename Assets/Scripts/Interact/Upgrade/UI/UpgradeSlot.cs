using GameItem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameInteract
{
    public class UpgradeSlot : MonoBehaviour
    {
        [SerializeField] Image itemGradeImg;
        [SerializeField] Image itmeImg;
        [SerializeField] TextMeshProUGUI gradeStep;
        int slotIndex;

        public Action<int> OnClickAction;

        public void Init(ItemData data, int slotIndex)
        {
            this.slotIndex = slotIndex;

            Sprite gradeSprite = Manager.Resource.Load<Sprite>(data.Grade.ToString());
            if (gradeSprite != null)
                itemGradeImg.sprite = gradeSprite;

            Sprite itemnSprite = Manager.Resource.Load<Sprite>(data.ID);
            if (itemnSprite != null)
                itmeImg.sprite = itemnSprite;

            var inventoryItem = Common.GameSystem.Inventory.GetItem(data.ID);
            if (inventoryItem?.item is EquipItem equipItem)
            {
                gradeStep.text = "+" + equipItem.Level.ToString();
            }
            else
            {
                gradeStep.text = "";
            }
        }

        public void OnClickSlot() => OnClickAction?.Invoke(slotIndex);
    }
}
