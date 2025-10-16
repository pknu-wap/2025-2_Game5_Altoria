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

    UIController ui = new();
    SceneLoader.SceneLoader scene = new();
    ResourceManager resource = new();

    public static Manager Instance { get { return instance; } }
    public static UIController UI { get { return instance.ui; } }
    public static SceneLoader.SceneLoader Scene { get { return instance.scene; } }
    public static ResourceManager Resource { get { return instance.resource; } }

    public static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("Manager");
            if (go == null)
                go = new GameObject { name = "Manager" };

            instance = go.GetOrAddComponent<Manager>();
            DontDestroyOnLoad(go);
        }

        instance.InitManagers();
    }

    //void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //        InitManagers();
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}


    async void InitManagers()
    {
        await GameDB.LoadAll();
        ui = new();
        scene = new();
        resource = new();
    }
}
