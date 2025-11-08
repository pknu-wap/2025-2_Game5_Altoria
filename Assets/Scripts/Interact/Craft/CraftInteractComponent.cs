using System.Collections.Generic;
using UnityEngine;

namespace GameInteract
{
    public enum CraftingType
    {
        None=0,
        Metal,
        Wood,
        Extra,
        Tool,
        Item
    }
    public class CraftInteractComponent : InteractBaseComponent
    {
        public override void Interact(IEntity entity)
        {
            base.Interact(entity);
            OpenInteractWindow();
        }
        void OpenInteractWindow()
        {
            Manager.UI.ShowPopup<CraftProcessPopUp>(() => {EndInteract();});
        }

    }
}
