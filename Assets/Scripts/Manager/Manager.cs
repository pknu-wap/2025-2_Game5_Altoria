using GameUI;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager instance;
    public static Manager Instance { get { return instance; } }

    public static UIController UI { get { return Instance.ui; } }
    public static TimeController Time { get { return Instance.time; } }
    public static SceneLoader SceneLoader { get { return Instance.scene; } }

    private UIController ui;
    private TimeController time;
    private SceneLoader scene;

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
        ui = new UIController();
        time = Utils.GetOrAddComponent<TimeController>(gameObject);
        scene = Utils.GetOrAddComponent<SceneLoader>(gameObject);
    }
}
