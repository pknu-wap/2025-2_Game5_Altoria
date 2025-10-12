using GameUI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace GameInteract
{
    public class CraftItemSlot : ItemSlot, IClickButton
    {
        int slotIndex;
       
        public event Action<int> OnClicked;


        public void OnClickButton() => OnClicked?.Invoke(slotIndex);

        public void SetSlot(int index, string spriteAddress, int count)
        {
            SetSlot(spriteAddress, count);
            slotIndex = index;
        }
    }


}