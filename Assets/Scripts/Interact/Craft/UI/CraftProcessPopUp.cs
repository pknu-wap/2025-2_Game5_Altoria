using UnityEngine;
using GameUI;
using System;


namespace GameInteract
{
    public class CraftProcessPopUp : UIPopUp 
    {
        IDisposable disposable;
        

        public void OnClickPopUp(int type)
        {
            
            var popUp = Manager.UI.ShowPopup<CraftPopUp>();
            popUp.SetData((CraftingType)type);
        }
    }
}