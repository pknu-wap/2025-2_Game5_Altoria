using GameUI;
using UnityEngine;

public class PauseMenu : UIPopUp
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

    public void OnClickResume()
    {
        Manager.UI.ClosePopup();
        Debug.Log("[PausePopUp] : 게임 재개");
    }

    public void OnClickSetting() //OnClickOption
    {
        Manager.UI.ShowPopup<SettingPopUp>();
        Debug.Log("[PausePopUp] : 설정창");
    }

    public void OnClickQuit()
    {
        Manager.UI.ShowPopup<ExitPopUp>();
        Debug.Log("[PausePopUp] : 게임 종료확인창");
    }
}
