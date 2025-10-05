using GameInteract;
using GameUI;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager instance;
    public static Manager Instance { get { return instance; } }

    public static UIController UI { get { return Instance.ui; } }
    public static TimeController Time { get { return Instance.time; } }
    public static SceneLoader SceneLoader { get { return Instance.scene; } }
    public static CollectDropHellper Collect { get { return Instance.collectDropHellper;  } }
    public static LifeStatsManager Life { get { return Instance.lifeStatsManager; } }

    private UIController ui;
    private TimeController time;
    private SceneLoader scene;
    private CollectDropHellper collectDropHellper;
    private LifeStatsManager lifeStatsManager;

    private void Awake()
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

    private void Update()
    {
        Time.Tick(UnityEngine.Time.deltaTime);  
    }
    private void InitManagers()
    {
        ui = new UIController();
        time = Utils.GetOrAddComponent<TimeController>(gameObject);
        scene = new();
        collectDropHellper = new();
        lifeStatsManager = new ();
    }
}
