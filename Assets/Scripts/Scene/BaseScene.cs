using UnityEngine;
using static Define;

public class BaseScene : MonoBehaviour
{
    protected SceneType sceneType = SceneType.None;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        
    }
}
