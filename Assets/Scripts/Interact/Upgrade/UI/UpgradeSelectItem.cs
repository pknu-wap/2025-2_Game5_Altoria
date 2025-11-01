using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameInteract
{
    public class UpgradeSelectItem : MonoBehaviour
    {
        [SerializeField] Image itemGradeImg;
        [SerializeField] Image itmeImg;
        [SerializeField] TextMeshProUGUI upgtadeStepTxt;
        const string path = "UI/ItemFrame/";

        public void Init(ItemData data)
        {
            itemGradeImg.sprite = Resources.Load<Sprite>(path + data.Grade);
            // itmeImg.sprite = 
            //upgtadeStepTxt.text =   
        }
    }
}
