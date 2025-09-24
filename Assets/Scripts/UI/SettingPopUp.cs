using System.Collections.Generic;
using System.ComponentModel;
using GameUI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingPopUp : UIPopUp
{
    [SerializeField] private GameObject settingUI;

    [Header("Setting Tabs")]
    [SerializeField] private List<GameObject> tabs;

    UIController ui;

    [SerializeField] private Slider BGMslider;
    [SerializeField] private Slider SFXslider;

    private void Awake()
    {
        ui = UIController.Instance;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowTab(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ui.ClosePopup();
        }
    }

    public override bool Init()
    {
        if(!base.Init()) return false;
        return true;
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

    public void ShowTab(int index)
    {
        for(int i = 0; i < tabs.Count; i++)
        {
            tabs[i].SetActive(false);
        }
        tabs[index].SetActive(true);
    }

    // 오디오 슬라이더
    public void SetBGMSlider(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
    }

    public void SetSFXSlider(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }
}
