using UnityEngine;
using UnityEngine.UI;
using GameUI;
using TMPro;

[System.Serializable]
public class Stat
{
    public Slider slider;
    public TextMeshProUGUI statText;
}

public class MainMenuPopUp : UIPopUp
{
    [SerializeField] GameObject mainMenuPopUp;
    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject settingPopUp;

    [SerializeField] Stat[] stats = new Stat[4];

    private UIController ui;

    private void Awake()
    {
        ui = Manager.UI; 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        updateStats();
    }

    private void OnEnable()
    {
        updateStats();
    }

    // Update is called once per frame
    void Update()
    {
        /*
       if(settingUI.activeSelf && Input.GetKeyDown(KeyCode.I))
       {
            창 닫기  
       }
       */
    }

    public override bool Init()
    {
        return base.Init();
    }

    public void updateStats()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            int index = i;

            stats[i].slider.onValueChanged.AddListener((value) =>
            {
                stats[index].statText.text = value.ToString("F0") + " / 250";
            });
            stats[i].statText.text = stats[i].slider.value.ToString("F0") + " / 250";
        }
    }
    public void OnClickInventory()
    {
        //ui.ShowPopup<InventoryUI>();
        inventoryUI.SetActive(true);
    }
    public void OnClickCraft()
    {
    }

    public void OnClickUpgrade()
    {
    }

    public void OnClickSetting()
    {
        //ui.ShowPopup<SettingPopUp>();
        settingPopUp.SetActive(true);
        Debug.Log("[MainMenuPopUp] : 설정창");
    }

    public void closePopUp()
    {
        //ui.ClosePopup();
        mainMenuPopUp.SetActive(false);
    }
}
