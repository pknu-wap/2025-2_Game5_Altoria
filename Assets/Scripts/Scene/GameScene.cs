using Common;
using GameUI;
using SceneLoad;
using SceneLoade;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : BaseScene
{
    protected int task = 15;

    const string PlayerKey = "Player";
    const string EnvironmentSceneName = "GameEnvironment";

    bool isInit;

    protected override void Init()
    {
        if (isInit) return;
        isInit = true;

        base.Init();
        GameSystem.Init();
        sceneType = Define.SceneType.GameScene;

        LoadEnvironmentScene();
        CreateDayNight();
        SoundManager.Instance.PlayBGM(BGM.GamePlay);
    }

    void LoadEnvironmentScene()
    {
        var loadingUI = Manager.UI.ShowPopup<LoadingUI>();

        // Additive 환경 씬 로드
        AsyncOperation op = SceneManager.LoadSceneAsync(EnvironmentSceneName, LoadSceneMode.Additive);

        op.completed += _ =>
        {
            Scene envScene = SceneManager.GetSceneByName(EnvironmentSceneName);
            SceneManager.SetActiveScene(envScene);
            Debug.Log($"[GameScene] Environment Additive Loaded: {EnvironmentSceneName}");
          

            // 로딩 데이터
            var loader = new JsonMapLoader($"{GetType()}", task);
            loader.Load();

            // 이벤트 먼저 등록 → StartLoding보다 항상 먼저
            loadingUI.OnEndLoad += PlayerLoad;

            if (!Manager.UserData.GetUserData<UserPlayerData>().GetFirstGift())
                loadingUI.OnClosed += FirstGift;

            // 로딩 시작 (이제 이벤트 절대 안 놓침)
            loadingUI.StartLoding(loader);
        };
    }

    void PlayerLoad()
    {
        Debug.Log($"[GameScene] Try spawn player. Key={PlayerKey}");


        // Resources Test Mode
        var prefab = Resources.Load<GameObject>(PlayerKey);
        if (prefab == null)
        {
            Debug.LogError($"[GameScene] Resources.Load FAILED! Path=Resources/{PlayerKey}.prefab");
            return;
        }

        var obj = GameObject.Instantiate(prefab);
        Debug.Log("[GameScene] Player Instance Created (Resources)!");
        InitPlayer(obj);

    }

    void InitPlayer(GameObject obj)
    {
        Debug.Log("[GameScene] Initialize Player Transform...");

        var data = Manager.UserData.GetUserData<UserPlayerData>();
        obj.transform.position = data.GetPlayerPosition();
        obj.transform.rotation = data.GetPlayerQuaternion();

        Debug.Log($"[GameScene] Player Spawned at {obj.transform.position}");
        Manager.UI.ShowHUD<UI_GameScene>();
    }

    void CreateDayNight()
    {
        var go = new GameObject("DayNight");
        go.AddComponent<DayNightCycle>();
        go.transform.SetParent(this.transform);
    }

    protected virtual void OnDestroy()
    {
        Debug.Log("GameScene Destroyed.");
    }

    void FirstGift()
    {
        var data = Manager.UserData.GetUserData<UserPlayerData>();
        data.SetFirstGift();

        var popUp = Manager.UI.ShowPopup<GetItemPopUp>();
        popUp.SetData("10080072", 10);
        popUp.SetEtcText("선물이 도착했습니다!");
    }
}
