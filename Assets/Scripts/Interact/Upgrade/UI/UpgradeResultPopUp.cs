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
            var currentSteop = (int)Manager.UserData.GetUserData<UserToolData>().GetToolStep(data.ID);
            var gradeData = GameDB.GetUpgradeData(currentSteop);

            var gradeProb = new List<(string, float)> { ("Success", gradeData.Success), ("Fail", gradeData.Fail), ("Destroy", gradeData.Destroy) };
            result = GameSystem.Random.Pick(gradeProb);
            resultText.text = result;

            Manager.UserData.GetUserData<UserToolData>().SetToolSteop(data.ID, currentSteop + 1);

            GameObject resultItemGO = Resources.Load<GameObject>(nameof(UpgradeSelectItem));
            var newGO = Instantiate(resultItemGO, resultItemRoot);
            if (newGO.TryGetComponent<UpgradeSelectItem>(out var item))
                item.Init(data, currentSteop + 1);
        }
    }
}

