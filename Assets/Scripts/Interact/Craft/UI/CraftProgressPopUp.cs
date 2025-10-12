using GameUI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace GameInteract
{
    public class CraftProgressPopUp : UIPopUp,IClickButton,IActionClickable
    {
        public event Action OnClicked;

        [SerializeField] ItemSlot itemSlot;
        [SerializeField] Button rewardButton;
        [SerializeField] TextMeshProUGUI progressText;

        Dictionary<CraftingState, string> textDict = new()
        {
            {CraftingState.Crafting, "제작 중..." },
            {CraftingState.Completed,"제작 완료" }
        };


        public void InitEntry(CraftingState state,string spriteAddress, int count)
        {
            itemSlot.SetSlot(spriteAddress, count);
            rewardButton.gameObject.SetActive(state==CraftingState.Completed);
            progressText.text=textDict[state];  
        }

        public void OnClickButton() => OnClicked?.Invoke();
    }
}
