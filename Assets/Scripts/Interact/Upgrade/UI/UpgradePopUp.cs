using GameUI;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using TMPro;
using GameData;
using UnityEngine;
using Common;

namespace GameInteract
{
    public class UpgradePopUp : UIPopUp
    {
        [SerializeField] Transform slotRoot;
        [SerializeField] Transform selectItemSlotRoot;
        [SerializeField] Transform meterialSlotRoot;
        [SerializeField] TextMeshProUGUI gradeTxt;

        GameObject selectedItemGO;
        GameObject meterialsItem;
        ItemData selectItemData;

        public override bool Init()
        {
            if (base.Init() == false) return false;

            // TODO: 인벤토리 데이터 가져와서 강화 가능한 물품만 정리하기
            GameObject upgradeSlotPrefab = Resources.Load<GameObject>(nameof(UpgradeSlot));
            var newGO = Instantiate(upgradeSlotPrefab, slotRoot);
            if (newGO.TryGetComponent<UpgradeSlot>(out var slot))
            {
                var newData = new ItemData($"12251000", Define.ItemGrade.Nomal);
                Manager.UserData.GetUserData<UserToolData>().SetToolSteop("12251000", 0);
                slot.Init(newData, 0);
                slot.OnClickAction = (i) => SetUpgradeData(newData);
            }

            return true;
        }

        void SetUpgradeData(ItemData itemData)
        {
            Destroy(meterialsItem);
            Destroy(selectedItemGO);

            GameObject selectdSlotPrefab = Resources.Load<GameObject>(nameof(UpgradeSelectItem));
            if(selectdSlotPrefab.TryGetComponent<UpgradeSelectItem>(out var slot))
            {
                slot.Init(itemData, (int)Manager.UserData.GetUserData<UserToolData>().GetToolStep(itemData.ID));
            }
            selectedItemGO = Instantiate(selectdSlotPrefab, selectItemSlotRoot);
            selectItemData = itemData;

            var step = Manager.UserData.GetUserData<UserToolData>().GetToolStep(itemData.ID);
            gradeTxt.text = $"{((int)step)}강 -> {((int)step + 1)}강";

            GameObject meterialSlotPrefab = Resources.Load<GameObject>(nameof(ItemSlot));
            var newGO = Instantiate(meterialSlotPrefab, meterialSlotRoot);
            meterialsItem = newGO;
            if (newGO.TryGetComponent<ItemSlot>(out var item))
                item.SetSlot(itemData.ID, GameDB.GetUpgradeData((int)step).Material);
        }

        public void OnClickUpgradeBtn()
        {
            // TODO: 강화석이 충분하다면 삭제, 아니면 강화석 부족 PopUp창 띄우기
            var popUp = Manager.UI.ShowPopup<UpgradeResultPopUp>();
            GameSystem.Life.AddExp<UpgradeInteractComponent>(10);
            popUp.SetResult(selectItemData);
        }

    }
}
