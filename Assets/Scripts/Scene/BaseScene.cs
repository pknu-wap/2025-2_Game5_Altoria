using UnityEngine;
using static Define;

public class BaseScene : MonoBehaviour
{
    protected SceneType SceneType = SceneType.None;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        //Manager.Init();
    }
}
