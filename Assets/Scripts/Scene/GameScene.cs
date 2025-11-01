using GameUI;
using SceneLoad;
using SceneLoade;
using UnityEngine;
using UnityEngine.AI;

public class GameScene : BaseScene
{
    protected int task = 15;

    const string PlayerKey = "Player";
    const string NavMeshKey = "NavMesh/GameScene";

    Vector3 PlayerPos = new Vector3(-33.9f, 3.74f, -12.01f);
    NavMeshDataInstance navMeshInstance;

    protected override void Init()
    {
        base.Init();

        sceneType = Define.SceneType.GameScene;
        SceneLoad();
    }

    void SceneLoad()
    {
        var loadingUI = Manager.UI.ShowPopup<LoadingUI>();


        Manager.Resource.LoadAsync<NavMeshData>(NavMeshKey, data =>
        {
            if (data != null)
            {
                navMeshInstance = NavMesh.AddNavMeshData(data);
                Debug.Log(" NavMesh loaded before map.");
            }
            else
            {
                Debug.LogError("Failed to load NavMeshData Addressable!");
            }


            var loader = new JsonMapLoader($"{GetType()}", task);
            loader.Load();

            loadingUI.StartLoding(loader);
            loadingUI.OnClosed += PlayerLoad;
        });
    }

    void PlayerLoad()
    {
        Manager.Resource.Instantiate(PlayerKey,
            new InstantiateOptions { Position = PlayerPos },
            obj =>
            {
                Manager.UI.ShowHUD<UI_GameScene>();
            });
    }

    protected virtual void OnDestroy()
    {
     
        if (navMeshInstance.valid)
        {
            NavMesh.RemoveNavMeshData(navMeshInstance);
            Debug.Log(" NavMesh Unloaded.");
        }
    }
}
