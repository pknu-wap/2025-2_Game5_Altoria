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


    UIController ui;
    TimeController time;
    SceneLoader scene;


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
        
        scene = Utils.GetOrAddComponent<SceneLoader>(gameObject);
    }
}
