using GameUI;
using UnityEngine;

public class ExitPopUp : UIPopUp
{
    [SerializeField]
    private GameObject exitUI;

    UIController ui;

    private void Awake()
    {
        ui = UIController.Instance;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ui.ClosePopup();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override bool Init()
    {
        if (!base.Init()) return false;
        return true;
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
        ui.ClosePopup();
    }
}
