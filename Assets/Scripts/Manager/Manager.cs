using GameInteract;
using GameUI;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager instance;
    public static Manager Instance { get { return instance; } }

    public static UIController UI { get { return Instance.ui; } }
    public static TimeController Time { get { return Instance.time; } }
    public static SceneLoader SceneLoader { get { return Instance.scene; } }
    public static RandomHellper Random { get { return Instance.randomHellper;  } }
    public static LifeStatsManager Life { get { return Instance.lifeStatsManager; } }

    private UIController ui;
    private TimeController time;
    private SceneLoader scene;
    private RandomHellper randomHellper;
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

    private void InitManagers()
    {
        ui = new ();
        time = Utils.GetOrAddComponent<TimeController>(gameObject);
        scene = new ();
        randomHellper = new ();
        lifeStatsManager = new ();
    }
}
