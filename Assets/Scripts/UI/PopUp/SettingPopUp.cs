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

    [Header("Setting Tabs")]
    [SerializeField] List<GameObject> tabs;

    UIController ui;

    [Header("Audio Settings")]
    [SerializeField] SliderInput BGMSlider;  
    [SerializeField] SliderInput SFXSlider;   

    [Header("Control Settings")]
    [SerializeField] TMP_Dropdown screenmode;
    [SerializeField] TMP_Dropdown resolution;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowTab(0);
        
        // 오디오 초기화
        BGMSlider.Value = SoundManager.Instance.GetBGMVolume();
        SFXSlider.Value = SoundManager.Instance.GetSFXVolume();

        // 슬라이더 값 변경 시 사운드 매니저에 반영
        BGMSlider.slider.onValueChanged.AddListener(SoundManager.Instance.SetBGMVolume);
        SFXSlider.slider.onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);

        // 화면모드, 품질
        screenmode.onValueChanged.AddListener(ChangeScreenMode);
        resolution.onValueChanged.AddListener(SetGraphicQuality); 
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
}
