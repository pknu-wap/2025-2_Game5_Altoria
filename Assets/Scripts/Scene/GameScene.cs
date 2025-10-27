using GameUI;
using SceneLoad;
using SceneLoade;

public class GameScene : BaseScene
{
    protected int task = 15;

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
    }
}
