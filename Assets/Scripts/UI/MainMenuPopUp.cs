using UnityEngine;
using UnityEngine.UI;
using GameUI;

public class MainMenuPopUp : UIPopUp
{
    [SerializeField] private GameObject mainMenuPopUp;

    // 추후 이름 변경
    [SerializeField] private Slider stat1;
    [SerializeField] private Slider stat2;
    [SerializeField] private Slider stat3;
    [SerializeField] private Slider stat4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 스탯 표시~
        // stat1.onValueChanged.AddListener( 함수 추가 ); 
    }

    // Update is called once per frame
    void Update()
    {
        /*
       if(settingUI.activeSelf && Input.GetKeyDown(KeyCode.I))
       {

       }
       */
    }

    public void closePopUp()
    {
        mainMenuPopUp.SetActive(false);
    }
}
