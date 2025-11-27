using System.Collections.Generic;
using System.ComponentModel;
using GameUI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using static SettingData;
using System.Text;

public class SettingPopUp : UIPopUp
{
    SettingData setting = SettingData.Instance;
    [SerializeField] SoundManager soundManager;

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
        BGMtext.Value = soundManager.GetBGMVolume();
        SFXtext.Value = soundManager.GetSFXVolume();

        // 슬라이더 값 변경 시 사운드 매니저에 반영
        BGMtext.slider.onValueChanged.AddListener(soundManager.SetBGMVolume);
        SFXtext.slider.onValueChanged.AddListener(soundManager.SetSFXVolume);

        // 화면모드, 품질
        screenmode.onValueChanged.AddListener(ChangeScreenMode);
        resolution.onValueChanged.AddListener(SetGraphicQuality); 

        // 카메라 감도 초기화
        CameraSensitivity.Value = 60f;

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
        soundManager.SetBGMVolume(value);
    }

    public void SetSFXSlider(float value)
    {
        soundManager.SetSFXVolume(value);
    }

    // 화면 모드
    public void ChangeScreenMode(int index)
    {
        setting.SetScreenMode((GameScreenMode)index);
    }

    // 품질 설정
    public void SetGraphicQuality(int index)
    {
        setting.SetQuality(index);
    }

    // 그래픽 설정
    public void SetCameraSensitivitySlider(float sensitivity)
    {
        setting.SetMouseSensitivity(sensitivity);
    }
}
