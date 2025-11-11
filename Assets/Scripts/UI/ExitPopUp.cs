using GameUI;
using UnityEngine;
using TMPro;

public enum ExitPopUpType
{
    ExitGame,
    GoToMainMenu
}

public class ExitPopUp : UIPopUp
{
    [SerializeField] TextMeshProUGUI information;
    ExitPopUpType currentType;

    public void SetPopUpType(ExitPopUpType type)
    {
        currentType = type;

        switch (type)
        {
            case ExitPopUpType.ExitGame:
                information.text = "진짜로 종료하시겠습니까?";
                break;
            case ExitPopUpType.GoToMainMenu:
                information.text = "메인 메뉴로 돌아가시겠습니까?";
                break;
        }
    }
    public void OnClickYes()
    {
        switch (currentType)
        {
            case ExitPopUpType.ExitGame:
                // Manager.UserData.SaveAllUserData();
                Debug.Log("[ExitPopUp] : 게임 종료");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;

            case ExitPopUpType.GoToMainMenu:
                // Manager.UserData.SaveAllUserData();
                Manager.UI.CloseAllPopup();
                Manager.Scene.LoadScene(Define.SceneType.Lobby);
                break;
        }
    }


    public void OnClickNo()
    {
        Manager.UI.ClosePopup();
    }
}
