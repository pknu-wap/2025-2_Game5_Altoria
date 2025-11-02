using GameUI;
using UnityEngine;

public class ExitPopUp : UIPopUp
{
    [SerializeField] private GameObject exitUI;

    private void Awake()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitUI.SetActive(true);
        }
        */
    }

    public void OnClickYes()
    {
        Debug.Log("Game closed");
#if UNITY_EDITOR                 
        UnityEditor.EditorApplication.isPlaying = false;   
#else
        Application.Quit();      
#endif
    }

    public void OnClickNo()
    {
        Manager.UI.ClosePopup();
    }
}
