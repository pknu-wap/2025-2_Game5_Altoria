using GameUI;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.SceneType.Lobby;
        Manager.UI.ShowHUD<UI_LobbyScene>();
    }
}
