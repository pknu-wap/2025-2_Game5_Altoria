using Common;
using GameData;
using GameItem;
using GameUI;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameInteract
{
    public class UpgradePopUp : UIPopUp
    {
        [SerializeField] Transform slotRoot;
        [SerializeField] Transform selectItemSlotRoot;
        [SerializeField] Transform meterialSlotRoot;
        [SerializeField] TextMeshProUGUI gradeTxt;

        List<GameObject> rightSlotItems = new List<GameObject>();
        GameObject selectedItemGO;
        GameObject meterialsItem;
        ItemData selectItemData;
        string upgradeMaterialID = "10080072";

        public override bool Init()
        {
            if (base.Init() == false) return false;

            SetItemSlot();

            return true;
        }

        void SetItemSlot()
        {
            if (rightSlotItems.Count != 0)
            {
                for (int i = 0; i < rightSlotItems.Count; i++)
                {
                    Destroy(rightSlotItems[i]);
                }
                rightSlotItems.Clear();
            }

            // TODO: 인벤토리에서 EquipItem 아이템만 들고오기
            var tempList = GameSystem.Inventory.GetAllItems()
                    .Where(item => item.item is EquipItem)
                    .ToList();

            for (int i = 0; i < tempList.Count; i++)
            {
                GameObject upgradeSlotPrefab = Resources.Load<GameObject>(nameof(UpgradeSlot));
                var newGO = Instantiate(upgradeSlotPrefab, slotRoot);
                if (newGO.TryGetComponent<UpgradeSlot>(out var slot))
                {
                    slot.Init(tempList[i].item.ItemData, i);
                    slot.OnClickAction = (i) => SetUpgradeData(tempList[i].item.ItemData);
                }
                rightSlotItems.Add(newGO);
            }
        }

        void SetUpgradeData(ItemData itemData)
        {
            selectItemData = itemData;
            Destroy(meterialsItem);
            Destroy(selectedItemGO);

            GameObject selectdSlotPrefab = Resources.Load<GameObject>(nameof(UpgradeSelectItem));
            if (selectdSlotPrefab.TryGetComponent<UpgradeSelectItem>(out var slot))
            {
                var selectdItem = Common.GameSystem.Inventory.GetItem(itemData.ID);
                if (selectdItem?.item is EquipItem selectEquipItem)
                {
                    slot.Init(itemData);
                    gradeTxt.text = $"{(selectEquipItem.Level)}강 -> {(selectEquipItem.Level + 1)}강";

                    GameObject meterialSlotPrefab = Resources.Load<GameObject>(nameof(ItemSlot));
                    var newGO = Instantiate(meterialSlotPrefab, meterialSlotRoot);
                    meterialsItem = newGO;
                    if (newGO.TryGetComponent<ItemSlot>(out var item))
                        item.SetSlot(upgradeMaterialID, GameDB.GetUpgradeData(selectEquipItem.Level).Material);
                }
                else
                {
                    slot.Init(itemData);
                    gradeTxt.text = "";
                }

            }
            selectedItemGO = Instantiate(selectdSlotPrefab, selectItemSlotRoot);
        }

        public void OnClickUpgradeBtn()
        {
            // TODO: 강화석이 충분하다면 삭제, 아니면 강화석 부족 PopUp창 띄우기
            var popUp = Manager.UI.ShowPopup<UpgradeResultPopUp>();
            GameSystem.Life.AddExp<UpgradeInteractComponent>(10);
            popUp.SetResult(selectItemData);
            popUp.OnClosed += SetItemSlot;
            popUp.OnClosed += () => SetUpgradeData(selectItemData);
        }
    }
}
