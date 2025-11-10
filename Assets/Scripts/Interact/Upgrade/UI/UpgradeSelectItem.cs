using GameItem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

namespace GameInteract
{
    public class UpgradeSelectItem : MonoBehaviour
    {
        [SerializeField] Image itemGradeImg;
        [SerializeField] Image itmeImg;
        [SerializeField] GameObject brokeImg;
        [SerializeField] TextMeshProUGUI gradeStep;

        public virtual void Init(ItemData data)
        {
            brokeImg.SetActive(false);

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

        public void SetBrokeImage() => brokeImg.SetActive(true);
    }
}
