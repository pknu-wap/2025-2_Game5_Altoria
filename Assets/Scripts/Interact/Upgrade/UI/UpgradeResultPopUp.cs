using GameUI;
using GameData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections.Generic;
using Common;

namespace GameInteract
{
    public class UpgradeResultPopUp : UIPopUp
    {
        [SerializeField] TextMeshProUGUI resultText;
        [SerializeField] Transform resultItemRoot; 


        string currentItemID;
        string result;

        public void SetResult(ItemData data)
        {
            var gradeData = GameDB.GetUpgradeData(1);

            var gradeProb = new List<(string, float)> { ("Success", gradeData.Success), ("Fail", gradeData.Fail), ("Destroy", gradeData.Destroy) };
            result = GameSystem.Random.Pick(gradeProb);
            resultText.text = result;

            // TODO: 도구 ID에 따른 강화 수치 변경

            GameObject resultItemGO = Resources.Load<GameObject>(nameof(UpgradeSelectItem));
            var newGO = Instantiate(resultItemGO, resultItemRoot);
            if (newGO.TryGetComponent<UpgradeSelectItem>(out var item))
                item.Init(data);
        }
    }
}

