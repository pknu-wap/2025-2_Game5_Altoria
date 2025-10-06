using GameUI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace GameInteract
{
    public class CraftProgressPopUp : UIPopUp
    {
        [SerializeField] ItemSlot itemSlot;
        [SerializeField] Button rewardButton;
        [SerializeField] TextMeshProUGUI progressText;

        Dictionary<CraftingState, string> textDict = new()
        {
            {CraftingState.Crafting, "제작 중..." },
            {CraftingState.Completed,"제작 완료" }
        };
    }
}
