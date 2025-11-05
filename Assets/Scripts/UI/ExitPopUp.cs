using GameUI;
using UnityEngine;

public class ExitPopUp : UIPopUp
{
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
        Debug.Log("[ExitPopUp] : Game closed");
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
