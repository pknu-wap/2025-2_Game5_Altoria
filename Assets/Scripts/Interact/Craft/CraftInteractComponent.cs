using System.Collections.Generic;
using UnityEngine;

namespace GameInteract
{
    enum CraftingType
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
            Manager.UI.ShowPopup<CraftPopUp>(() =>
            {
                EndInteract();
            });
        }

    }
}
