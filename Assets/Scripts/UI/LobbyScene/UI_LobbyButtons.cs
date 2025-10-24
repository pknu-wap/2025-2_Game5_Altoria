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

        Define.SceneType startScene = Define.SceneType.GameScene;

        public override bool Init()
        {
            if (!base.Init()) return false;

            return true;
        }

        #region OnClick Event
        public void OnClickStartButton()
        {
            GameSystem.Init();
            Manager.Scene.LoadScene(startScene);

            var loadingUI = Manager.UI.ShowPopup<LoadingUI>();
            loadingUI.StartLoding(startScene);
            loadingUI.OnClosed += () => Manager.UI.ShowHUD<UI_GameScene>();
        }

        public void OnClickSettingButton()
        {
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