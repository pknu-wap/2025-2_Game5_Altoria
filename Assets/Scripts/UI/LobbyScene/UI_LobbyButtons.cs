using Common;
using SceneLoade;
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
            Debug.Log($"[{GetType()}] 게임 시작!");
            GameSystem.Init();
            Manager.Scene.LoadScene(Define.SceneType.GameScene);
            var loadingUI = Manager.UI.ShowPopup<LoadingUI>();
            loadingUI.StartLoding(Define.SceneType.GameScene);
            Manager.UI.ShowHUD<UI_GameScene>();
        }

        public void OnClickSettingButton()
        {
            Debug.Log($"[{GetType()}] 설정 창 열기");

            // 팝업 UI 생성 및 표시
            Manager.UI.ShowPopup<TestSettingPopUp>();
        }
        public void OnClickExitButton()
        {
            Debug.Log($"[{GetType()}] 게임 종료");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion
    }
}