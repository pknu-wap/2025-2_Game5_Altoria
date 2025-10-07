using UnityEngine;
using System;
using System.Reflection;
using GameUI;

public class PopUpHandler : MonoBehaviour
{
    [SerializeField] PagePopUpSO popUpData;

    void OpenPopUP(int index)
    {
        var popupType = popUpData.GetPopUpType(index);
        if (popupType == null) return;
        
        Manager.UI.ShowPopup(popupType);
      
    }

    public void OnClickPopUpButton(int index) => OpenPopUP(index);
}
