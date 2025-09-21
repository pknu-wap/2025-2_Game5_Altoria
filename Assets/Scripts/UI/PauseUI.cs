using GameUI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : UIPopUp
{
    [SerializeField] private GameObject pauseUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickResume()
    {
        Debug.Log("Resume Game");
    }

    public void OnClickOption()
    {
        Debug.Log("Open Option Menu");
    }

    public void OnClickQuit()
    {
        Debug.Log("Quit Game");
    }
}
