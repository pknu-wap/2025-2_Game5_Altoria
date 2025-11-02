using System.Collections.Generic;
using System.Security.Cryptography;
using GameUI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;

    private void Awake()
    { 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.PlayBGM(BGM.Lobby);
    }

    

    // Update is called once per frame
    void Update()
    {
        /*
        // esc누르면 일시정지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Manager.UI.ShowPopup<PauseMenu>();
        }
        */
    }

    public void OnClickMainMenuPopUp()
    {
        Manager.UI.ShowPopup<MainMenuPopUp>();
        Debug.Log("[PausePopUp] : 메인메뉴패널창");
    }
    
}
