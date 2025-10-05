using GameUI;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        sceneType = Define.SceneType.Lobby;
        Manager.UI.ShowHUD<UI_LobbyScene>();
    }
}
