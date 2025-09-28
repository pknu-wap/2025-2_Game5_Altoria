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
    public static InteractionSystem Interact { get { return Instance.interact; } }

    UIController ui;
    TimeController time;
    SceneLoader scene;
    InteractionSystem interact;

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
        ui = new();
        time = new();
        interact = new();
        scene = Utils.GetOrAddComponent<SceneLoader>(gameObject);
    }
}
