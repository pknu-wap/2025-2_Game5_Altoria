using GameUI;
using SceneLoade;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        sceneType = Define.SceneType.GameScene;
    }

    public void TestMoveScene()
    {
        Manager.Scene.LoadScene(Define.SceneType.TestFC_1);
    }
}
