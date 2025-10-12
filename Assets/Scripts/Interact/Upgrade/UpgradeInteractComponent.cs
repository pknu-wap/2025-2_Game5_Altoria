using UnityEngine;

namespace GameInteract
{
    public class UpgradeInteractComponent : InteractBaseComponent
    {
        public override void Interact() => OpenUpgradeUI();

        void OpenUpgradeUI() => Manager.UI.ShowPopup<UpgradePopUp>(() => {EndInteract();});
        
    }
}
