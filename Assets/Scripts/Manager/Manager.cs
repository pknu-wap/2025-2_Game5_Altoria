using GameData;
using GameUI;
using UnityEngine;
using Unity.VisualScripting;
public class Manager : MonoBehaviour
{
    static Manager instance;

    UserDataManager userDataManager = new ();
    UIController ui = new();
    SceneLoader.SceneLoader scene = new();
    ResourceManager resource = new();

    public static Manager Instance { get { return instance; } }
    public static UserDataManager UserData { get { return instance.userDataManager; } }
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

    async void InitManagers()
    {
        await GameDB.LoadAll();
        UserData.Init();
    }



    public static void AllSave()
    {
        UserData.SaveAllUserData();
    }


    public static void AllLoad()
    {
        UserData.LoadAllUserData();
    }


    public static void AllSetDefaultData()
    {
        UserData.SetAllDefaultUserData();
    }
}
