using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStart()
    {

    }
    public void OnClickContinue()
    {

    }
    public void OnClickSetting()
    {

    }
    public void OnClickExit()
    {
        Debug.Log("Game closed");
#if UNITY_EDITOR                  
        UnityEditor.EditorApplication.isPlaying = false;       
        Application.Quit();      
#endif
    }
}
