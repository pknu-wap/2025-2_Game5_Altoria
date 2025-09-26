using GameUI;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject ob = GameObject.Find("Manager");
                if (ob == null)
                    ob = new GameObject("Manager");
                instance = Utils.GetOrAddComponent<Manager>(ob);
                DontDestroyOnLoad(ob);
            }
            return instance;
        }
    }
    public static UIController UI { get { return Instance.ui; } }
    public static TimeController Time { get { return Instance.time; } }
    public static SceneLoader SceneLoader { get { return Instance.scene; } }

    private static Manager instance;

    UIController ui;
    TimeController time;
    SceneLoader scene;

    private void Awake()
    {
        Init();

        ui = new UIController();
        time = new TimeController();
        scene = new SceneLoader();
    }

    public void Init()
    {
        if (instance == null)
        {
            GameObject ob = GameObject.Find("Manager");
            if (ob == null)
                ob = new GameObject("Manager");
            instance = Utils.GetOrAddComponent<Manager>(ob);
            DontDestroyOnLoad(ob);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
