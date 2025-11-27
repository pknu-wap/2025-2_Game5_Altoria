using System.Collections.Generic;
using System.Security.Cryptography;
using GameUI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;

    public void OnClickMainMenuPopUp()
    {
        Manager.UI.ShowPopup<MainMenuPopUp>();
        Debug.Log("[PausePopUp] : 메인메뉴패널창");
    }
    
}
