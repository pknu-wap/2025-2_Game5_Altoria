using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class UI_LobbyButtons : UIWidget
    {
        Button startBtn;
        Button settingBtn;
        Button eixtBtn;

        public override bool Init()
        {
            if (!base.Init()) return false;

            startBtn = Utils.FindChild<Button>(gameObject, "Start");
            settingBtn = Utils.FindChild<Button>(gameObject, "Setting");
            eixtBtn = Utils.FindChild<Button>(gameObject, "Exit");

            startBtn.onClick.AddListener(OnClickStartButton);
            settingBtn.onClick.AddListener(OnClickSettingButton);
            eixtBtn.onClick.AddListener(OnClickExitButton);

            return true;
        }

        #region OnClick Event
        public void OnClickStartButton()
        {
            Debug.Log("게임 시작!");
        }

        public void OnClickSettingButton()
        {
            Debug.Log("설정 창 열기");

            // 팝업 UI 생성 및 표시
            UIController.Instance.ShowPopup<TestSettingPopUp>();
        }
        public void OnClickExitButton()
        {
            Debug.Log("게임 종료");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion
    }
}