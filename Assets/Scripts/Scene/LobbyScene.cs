using GameUI;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.SceneType.LobbyScene;
        UIController.Instance.ShowHUD<UI_LobbyScene>();
    }
}
