using System.Collections.Generic;
using System.ComponentModel;
using GameUI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingPopUp : UIPopUp
{
    [Header("Setting Tabs")]
    [SerializeField] List<GameObject> tabs;

    UIController ui;

    [Header("Audio Settings")]
    [SerializeField] SliderInput BGMtext;  
    [SerializeField] SliderInput SFXtext;   

    [Header("Control Settings")]
    [SerializeField] TMP_Dropdown screenmode;
    [SerializeField] TMP_Dropdown resolution;
    [SerializeField] SliderInput CameraSensitivity;

    void Awake()
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowTab(0);

        // 오디오 초기화
        BGMtext.Value = SoundManager.Instance.GetBGMVolume();
        SFXtext.Value = SoundManager.Instance.GetSFXVolume();

        // 슬라이더 값 변경 시 사운드 매니저에 반영
        BGMtext.GetComponent<Slider>().onValueChanged.AddListener(SoundManager.Instance.SetBGMVolume);
        SFXtext.GetComponent<Slider>().onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);

        // 화면모드, 품질
        screenmode.onValueChanged.AddListener(ChangeScreenMode);
        resolution.onValueChanged.AddListener(SetGraphicQuality); 

        // 카메라 감도 초기화
        CameraSensitivity.Value = 60f;

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Manager.UI.ClosePopup();
        }
        */
    }

    public void OnClickAudioTab()
    {
        ShowTab(0);
        Debug.Log("[SettingPopUp] : 오디오 설정창");
    }

    public void OnClickVideoTab()
    {
        ShowTab(1);
        Debug.Log("[SettingPopUp] : 비디오 설정창");
    }

    public void OnClickControlsTab()
    {
        ShowTab(2);
        Debug.Log("[SettingPopUp] : 컨트롤 설정창");
    }

    public void OnClickEtcTab()
    {
        ShowTab(3);
        Debug.Log("[SettingPopUp] : 고급 설정창");
    }

    public void ShowTab(int index)
    {
        for(int i = 0; i < tabs.Count; i++)
        {
            tabs[i].SetActive(false);
        }
        tabs[index].SetActive(true);
    }

    public void settingHide()
    {
        Manager.UI.ClosePopup();
    }

    // 오디오 설정
    public void SetBGMSlider(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
    }

    public void SetSFXSlider(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }

    // 화면 모드
    public void ChangeScreenMode(int index)
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.fullScreen = true;
                Debug.Log("[SettingPopUp] : 전체 화면 모드");
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Screen.fullScreen = false;
                Debug.Log("[SettingPopUp] : 창 모드");
                break;
        }
    }

    // 품질 설정
    public void SetGraphicQuality(int index)
    {
        //Debug.Log("현재 품질 레벨 인덱스: " + QualitySettings.GetQualityLevel());
        //QualitySettings.SetQualityLevel(2 - index);
        Debug.Log($"[SettingPopUp] : 게임 품질 : {2-index}");
    }

    // 그래픽 설정
    public void SetCameraSensitivitySlider()
    {
        //PlayerCameraController - mouseSensivity 필요
    }

    public void customerClick()
    {
        // 임시 문의 링크
        Application.OpenURL("https://www.youtube.com/watch?v=BlAvNOmBLKY");
    }
}
