using UnityEngine;

namespace GameInteract
{
    public class CraftInteractComponent : InteractBaseComponent
    {
        public override void Interact() => OpenInteractWindow();
        void OpenInteractWindow()
        {
            Manager.UI.ShowPopup<PageHandler>(() =>
            {
                EndInteract();
            });
        }

    }
}
