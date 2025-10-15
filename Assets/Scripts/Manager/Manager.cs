using GameData;
using GameInteract;
using GameUI;
using System.Threading;
using UnityEditor.SceneManagement;
using UnityEngine;
using Common;
using SceneLoader;
using Unity.VisualScripting;
public class Manager : MonoBehaviour
{
    private static Manager instance;

    UIController ui; //= new();
    TimeController time; //= new();
    SceneLoader.SceneLoader scene; //= new();
    RandomHellper randomHellper; //= new();
    LifeStatsManager lifeStatsManager; //= new();
    ResourceManager resource; //= new();
    GameSystem system; //= new();
    public static Manager Instance { get { return instance; } }

    public static UIController UI { get { return instance.ui; } }
    public static TimeController Time { get { return instance.time; } }
    public static SceneLoader.SceneLoader SceneLoader { get { return instance.scene; } }
    public static RandomHellper Random { get { return instance.randomHellper;  } }
    public static LifeStatsManager Life { get { return instance.lifeStatsManager; } }
    public static ResourceManager Resource { get { return instance.resource; } }    

    public static GameSystem System { get { return Instance.system; } }   


    //public static void Init()
    //{
    //    if (instance == null)
    //    {
    //        GameObject go = GameObject.Find("Manager");
    //        if (go == null)
    //            go = new GameObject { name = "Manager" };

    //        instance = go.GetOrAddComponent<Manager>();
    //        DontDestroyOnLoad(go);
    //    }
    //}
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
        ui = new();
        time = new();
        scene = new();
        resource = new();
        randomHellper = new();
        lifeStatsManager = new();
        system = new();
    }
}
