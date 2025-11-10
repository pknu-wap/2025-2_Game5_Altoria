using Common;
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
            GameSystem.Init();

            // TODO: 시작할 Scene Data에서 받아와서 사용하기
            Manager.Scene.LoadScene(Define.SceneType.GameScene);
        }

        public void OnClickSettingButton()
        {
            //TODO:  설정창 팝업
        }
        public void OnClickExitButton()
        {
            Manager.UserData.SaveAllUserData();
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