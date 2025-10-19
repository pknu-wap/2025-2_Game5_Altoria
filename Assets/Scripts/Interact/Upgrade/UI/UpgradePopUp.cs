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
        ItemData selectItemData;
        List<GameObject> meterialsItem = new List<GameObject>();

        public override bool Init()
        {
            if (base.Init() == false) return false;

            // TODO: 인벤토리 데이터 가져와서 강화 가능한 물품만 정리하기


            GameObject upgradeSlotPrefab = Resources.Load<GameObject>(nameof(UpgradeSlot));
            for (int i = 1; i <= 3; i++)
            {
                var newGO = Instantiate(upgradeSlotPrefab, slotRoot);
                if (newGO.TryGetComponent<UpgradeSlot>(out var slot))
                {
                    var newData = new ItemData($"1221{i}000", (Define.ItemGrade)i);
                    slot.Init(newData, i);
                    slot.OnClickAction = (i) => SetUpgradeData(newData);
                }
            }

            return true;
        }

        private void SetUpgradeData(ItemData data)
        {
            for (int i = 0; i < meterialsItem.Count; i++)
                Destroy(meterialsItem[i]);
            meterialsItem.Clear();
            Destroy(selectedItemGO);

            // TODO: 선택된 item을 selectItemSlotRoot에 설정
            GameObject selectdSlotPrefab = Resources.Load<GameObject>(nameof(UpgradeSelectItem));
            selectedItemGO = Instantiate(selectdSlotPrefab, selectItemSlotRoot);
            selectItemData = data;

            gradeTxt.text = $"{((int)data.Grade)}강 -> {((int)data.Grade + 1)}강";

            // TODO : 강화재료 Data를 받아야한다.
            // TEST
            GameObject meterialSlotPrefab = Resources.Load<GameObject>(nameof(UpgradeMeterialSlot));
            for (int i = 0; i < (Convert.ToInt32(data.ID) / 1000) % 10; i++)
            {
                var newGO = Instantiate(meterialSlotPrefab, meterialSlotRoot);
                meterialsItem.Add(newGO);
                if (newGO.TryGetComponent<UpgradeMeterialSlot>(out var item))
                {
                    item.Init(new ItemData(data.ID, (Define.ItemGrade)i), i);
                }
            }
        }

        public void OnClickUpgradeBtn()
        {
            // TODO: 위에서 가져온 강화 재료(meterialsItem)로부터 재료가 있는지 확인고, 재료를 인벤에서 소진시킨다.
            // 재료가 없다면 재료 없다고 popUp창 띄우기
            var popUp = Manager.UI.ShowPopup<UpgradeResultPopUp>();
            GameSystem.Life.AddExp<UpgradeInteractComponent>(10);
            popUp.SetResult(selectItemData);
        }

    }
}
