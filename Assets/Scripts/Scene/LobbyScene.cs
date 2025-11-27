using GameUI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        sceneType = Define.SceneType.Lobby;
        Manager.UI.ShowHUD<UI_LobbyScene>();
        SoundManager.Instance.PlayBGM(BGM.Lobby);
    }
}
