using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class UI_LobbyButtons : UIWidget
    {
        [SerializeField] Button startBtn;
        [SerializeField] Button settingBtn;
        [SerializeField] Button eixtBtn;

        public override bool Init()
        {
            if (!base.Init()) return false;

            return true;
        }

        #region OnClick Event
        public void OnClickStartButton()
        {
            Debug.Log($"{GetType()}게임 시작!");
        }

        public void OnClickSettingButton()
        {
            Debug.Log($"{GetType()}설정 창 열기");

            // 팝업 UI 생성 및 표시
            UIController.Instance.ShowPopup<TestSettingPopUp>();
        }
        public void OnClickExitButton()
        {
            Debug.Log($"{GetType()}게임 종료");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion
    }
}