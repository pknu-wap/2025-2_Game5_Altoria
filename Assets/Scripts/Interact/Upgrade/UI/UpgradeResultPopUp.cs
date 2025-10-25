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
        [SerializeField] Image itemImg;

        string currentItemID;
        string result;

        public void SetResult(ItemData data)
        {
            var gradeData = GameDB.GetGradeData((int)data.Grade);

            var gradeProb = new List<(string, float)> { ("Success", gradeData.Success), ("Fail", gradeData.Fail), ("Destroy", gradeData.Destroy) };
            result = GameSystem.Random.Pick(gradeProb);
            resultText.text = result;
        }
    }
}

