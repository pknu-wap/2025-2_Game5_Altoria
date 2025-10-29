using Common;
using UnityEngine;
using static Define;

public class BaseScene : MonoBehaviour
{
    protected SceneType sceneType = SceneType.None;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Manager.Init();
        //GameSystem.Init();
    }
}
