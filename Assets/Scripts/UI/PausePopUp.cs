using System.Collections.Generic;
using System.Security.Cryptography;
using GameUI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PausePopUp : UIPopUp
{
    [SerializeField] private GameObject pauseUI;

    UIController ui;

    [Header("Popup")]
    [SerializeField] private List<GameObject> popups;

    private void Awake()
    {
        ui = UIController.Instance;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
    public override bool Init()
    {
        if (!base.Init()) return false;
        return true;
    }

    public void OnClickResume()
    {
        ui.ClosePopup();
        Debug.Log("Resume Game");
    }

    public void OnClickOption()
    {
        ui.ShowPopup<SettingPopUp>();
        Debug.Log("Open Option Menu");
    }

    public void OnClickQuit()
    {
        ui.ShowPopup<ExitPopUp>();
        Debug.Log("Quit Game");
    }
}
