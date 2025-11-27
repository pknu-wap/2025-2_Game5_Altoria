using Common;
using GameUI;
using SceneLoad;
using SceneLoade;
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

        SceneLoad();
        CreatDayNight();
        SoundManager.Instance.PlayBGM(BGM.GamePlay);
    }

    void SceneLoad()
    {
        var loadingUI = Manager.UI.ShowPopup<LoadingUI>();


        AsyncOperation op = SceneManager.LoadSceneAsync(EnvironmentSceneName, LoadSceneMode.Additive);

        op.completed += _ =>
        {
            Scene envScene = SceneManager.GetSceneByName(EnvironmentSceneName);
            SceneManager.SetActiveScene(envScene);
            Debug.Log($"[GameScene] Environment Additive Loaded: {EnvironmentSceneName}");

        
            var loader = new JsonMapLoader($"{GetType()}", task);
            loader.Load();


            loadingUI.StartLoding(loader);


            loadingUI.OnEndLoad += PlayerLoad;
            if(!Manager.UserData.GetUserData<UserPlayerData>().GetFirstGift())
                loadingUI.OnClosed += FirstGift;
        };
    }

    void PlayerLoad()
    {
        Manager.Resource.Instantiate(
            PlayerKey,
            new InstantiateOptions
            {
                Position = Manager.UserData.GetUserData<UserPlayerData>().GetPlayerPosition(),
                Rotation = Manager.UserData.GetUserData<UserPlayerData>().GetPlayerQuaternion()
            },
            obj =>
            {
                Manager.UI.ShowHUD<UI_GameScene>();
            });
    }

    void CreatDayNight()
    {
        var go = new GameObject("DayNight");
        var script = go.AddComponent<DayNightCycle>();
        go.transform.SetParent(this.transform);
    }

    protected virtual void OnDestroy()
    {
      
        Debug.Log("GameScene Destroyed.");
    }

    void FirstGift()
    {
        Manager.UserData.GetUserData<UserPlayerData>().SetFirstGift();
        var popUp = Manager.UI.ShowPopup<GetItemPopUp>();
        popUp.SetData("10080072", 10);
        popUp.SetEtcText("선물이 도착했습니다!");
    }

    private void OnDisable()
    {
        SoundManager.Instance.StopBGM();
    }
}
