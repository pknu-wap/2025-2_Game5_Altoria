using GameUI;
using SceneLoad;
using SceneLoade;
using UnityEngine;

public class TestMap : BaseScene
{
    protected int task = 1;

    protected override void Init()
    {
        base.Init();

        sceneType = Define.SceneType.GameScene;
        SceneLoad();
    }

    void SceneLoad()
    {
        var loadingUI = Manager.UI.ShowPopup<LoadingUI>();
        var loader = new JsonMapLoader($"{GetType()}", task);
        loader.Load();
        loadingUI.StartLoding(loader);
        loadingUI.OnClosed += () => Manager.UI.ShowHUD<UI_GameScene>();
        loadingUI.OnClosed += () => PlayerLoad();
    }

    void PlayerLoad()
    {

    }
}
