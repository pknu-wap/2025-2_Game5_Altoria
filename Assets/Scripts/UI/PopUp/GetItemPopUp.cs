using Common;
using GameData;
using System;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class GetItemPopUp : UIPopUp
    {
        [SerializeField] TextMeshProUGUI itemName;
        [SerializeField] ItemSlot itemSlot;
        [SerializeField] TextMeshProUGUI etcTxt;


        public void SetData(string id, int count)
        {
            itemSlot.SetSlot(id, count);
            itemName.text = $"'{GameDB.GetItemData(id).Name}' 획득";
            GameSystem.Inventory.AddItem(id, count);
        }

        public void SetEtcText(string text)
        {
            etcTxt.text = text;
            etcTxt.gameObject.SetActive(true);
        }
    }
}