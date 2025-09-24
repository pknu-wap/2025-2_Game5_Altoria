using System.Collections.Generic;
using System.Security.Cryptography;
using GameUI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : UIPopUp
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
        ui.ClosePopup();
    }
    public override bool Init()
    {
        if (!base.Init()) return false;
        return true;
    }

    public void OnClickResume()
    {
        pauseUI.SetActive(false);
        Debug.Log("Resume Game");
    }

    public void OnClickOption()
    {
        //ui.ShowPopup<SettingUI>();
        popups[0].SetActive(true);
        Debug.Log("Open Option Menu");
    }

    public void OnClickQuit()
    {
        Debug.Log("Quit Game");
    }
}
