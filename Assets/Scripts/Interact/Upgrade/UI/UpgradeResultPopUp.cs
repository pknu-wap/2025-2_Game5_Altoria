using Common;
using GameData;
using GameItem;
using GameUI;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

namespace GameInteract
{
    public class UpgradeResultPopUp : UIPopUp
    {
        [SerializeField] TextMeshProUGUI resultText;
        [SerializeField] Transform resultItemRoot; 

        string result;

        public void SetResult(ItemData data)
        {
            var inventoryEntryData = Common.GameSystem.Inventory.GetItem(data.ID);
            var currentStep = inventoryEntryData?.item is EquipItem currentItem ? currentItem.Level : 0;
            var gradeData = GameDB.GetUpgradeData(currentStep);

            var gradeProb = new List<(string, float)> { ("Success", gradeData.Success), ("Fail", gradeData.Fail), ("Destroy", gradeData.Destroy) };
            result = GameSystem.Random.Pick(gradeProb);
            resultText.text = result;

            if (inventoryEntryData?.item is EquipItem upgradeItem)
            {
                if (result == "Success")
                    upgradeItem.Upgrade();
            }

            GameObject selectdSlotPrefab = Resources.Load<GameObject>(nameof(UpgradeSelectItem));
            if (selectdSlotPrefab.TryGetComponent<UpgradeSelectItem>(out var slot))
            {
                slot.Init(data);
                if (result == "Destroy")
                {
                    // TODO: 인벤토리에서 삭제
                    slot.SetBrokeImage();
                }
            }
            Instantiate(selectdSlotPrefab, resultItemRoot);
        }
    }
}

