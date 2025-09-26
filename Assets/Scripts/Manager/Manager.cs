using GameUI;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get { return instance; } }
    public static UIController UI { get { return instance.ui; } }
    public static TimeController Time { get { return instance.time; } }


    private static Manager instance;

    UIController ui = new UIController();
    TimeController time = new TimeController();

    public void Init()
    {
        if (instance == null)
        {
            GameObject ob = GameObject.Find("Manager");
            if (ob == null)
                ob = new GameObject("Manager");
            DontDestroyOnLoad(ob);

            instance = Utils.GetOrAddComponent<Manager>(ob);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
