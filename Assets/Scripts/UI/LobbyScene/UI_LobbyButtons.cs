using Common;
using System;
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
            SoundManager.Instance.StopBGM();
            GameSystem.Init();
            if (!Manager.UserData.GetUserData<UserPlayerData>().GetCustomed())
            {
                Manager.UserData.GetUserData<UserPlayerData>().SetCustomed();
                Manager.UI.ShowHUD<CustomizingMenu>();
            }
            else
                Manager.Scene.LoadScene(Define.SceneType.GameScene);
        }

        public void OnClickSettingButton()
        {
            Manager.UI.ShowPopup<SettingPopUp>();
        }
        public void OnClickExitButton()
        {
            Manager.UserData.SaveAllUserData();
            Manager.UI.ShowPopup<ExitPopUp>();
        }
        #endregion
    }
}