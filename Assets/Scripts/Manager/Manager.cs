using GameData;
using GameInteract;
using GameUI;
using System.Threading;
using UnityEditor.SceneManagement;
using UnityEngine;
using Common;
using SceneLoader;
public class Manager : MonoBehaviour
{
    private static Manager instance;
    public static Manager Instance { get { return instance; } }

    public static UIController UI { get { return Instance.ui; } }
    public static TimeController Time { get { return Instance.time; } }
    public static SceneLoader.SceneLoader SceneLoader { get { return Instance.scene; } }
    public static RandomHellper Random { get { return Instance.randomHellper;  } }
    public static LifeStatsManager Life { get { return Instance.lifeStatsManager; } }
    public static ResourceManager Resource { get { return Instance.resource; } }    

    public static GameSystem System { get { return Instance.system; } }   

    UIController ui;
    TimeController time;
    SceneLoader.SceneLoader scene;
    RandomHellper randomHellper;
    LifeStatsManager lifeStatsManager;
    ResourceManager resource;
    GameSystem system;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(Time!=null)
        Time.Tick(UnityEngine.Time.deltaTime);  
    }
    async void InitManagers()
    {
        await GameDB.LoadAll();
        system = new();
        ui = new ();
        time = new();
        scene = new();
        resource=new();
        randomHellper = new();
        lifeStatsManager = new ();
    }
}
