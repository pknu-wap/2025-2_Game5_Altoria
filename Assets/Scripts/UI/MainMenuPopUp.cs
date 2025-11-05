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
    [SerializeField] Stat[] stats = new Stat[4];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        UpdateStats();
    }

    private void OnEnable()
    {
        UpdateStats();
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

    public void UpdateStats()
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
        Manager.UI.ShowPopup<InventoryUI>();
    }
    public void OnClickCraft()
    {
    }

    public void OnClickUpgrade()
    {
    }

    public void OnClickSetting()
    {
        Manager.UI.ShowPopup<SettingPopUp>();
        Debug.Log("[MainMenuPopUp] : 설정창");
    }

    public void OnClickExit()
    {
        Manager.UI.ShowPopup<ExitPopUp>();
        Debug.Log("[MainMenuPopUp] : 종료창");
    }

    public void ClosePopUp()
    {
        Manager.UI.ClosePopup();
    }
}
