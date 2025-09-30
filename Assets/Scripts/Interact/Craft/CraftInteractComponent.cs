using System.Collections.Generic;
using UnityEngine;

namespace GameInteract
{
    public enum CraftingType
    {
        Metal,
        Wood,
        Extra,
        Tool,
        Item
    }
    public class CraftInteractComponent : InteractBaseComponent
    {
        
        public override void Interact() => OpenInteractWindow();
        void OpenInteractWindow()
        {
            Manager.UI.ShowPopup<CraftProcessPopUp>(() =>
            {
                EndInteract();
            });
        }

    }
}
