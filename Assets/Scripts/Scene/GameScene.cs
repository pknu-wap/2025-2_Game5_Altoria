using GameUI;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        sceneType = Define.SceneType.GameScene;
        Manager.UI.ShowHUD<UI_GameScene>();
    }
}
