using System.Collections.Generic;
using System.Security.Cryptography;
using GameUI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;

    [Header("Pages")]
    [SerializeField] private GameObject mainMenuPopUpUI;
    [SerializeField] private GameObject SettingUI; 
    [SerializeField] private GameObject ExitUI;

    private void Awake()
    { 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.PlayBGM(BGM.Lobby);
    }

    private void OnEnable()
    {
        /////////////////// 시간 정지 부분
    }

    private void OnDisable()
    {
        /////////////////// 시간 재개 부분
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClickMainMenuPopUp()
    {
        mainMenuPopUpUI.SetActive(true);
        Debug.Log("[PausePopUp] : 메인메뉴패널창");
    }
    public void OnClickResume()
    {
        pauseUI.SetActive(false);
        Debug.Log("[PausePopUp] : 게임 재개");
    }

    public void OnClickOption() //OnClickSetting
    {
        SettingUI.SetActive(true);
        Debug.Log("[PausePopUp] : 설정창");
    }

    public void OnClickQuit()
    {
        ExitUI.SetActive(true);
        Debug.Log("[PausePopUp] : 게임 종료확인창");
    }
}
