using UnityEngine;
using UnityEngine.UI;
using GameUI;
using TMPro;

public class MainMenuPopUp : UIPopUp
{
    [SerializeField] private GameObject mainMenuPopUp;
    [SerializeField] private GameObject settingPopUp;

    // ���� �̸� ����
    [SerializeField] private Slider stat1;   [SerializeField] private TextMeshProUGUI stat1Text;
    [SerializeField] private Slider stat2;   [SerializeField] private TextMeshProUGUI stat2Text;
    [SerializeField] private Slider stat3;   [SerializeField] private TextMeshProUGUI stat3Text;
    [SerializeField] private Slider stat4;   [SerializeField] private TextMeshProUGUI stat4Text;

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

       }
       */
    }

    public override bool Init()
    {
        return base.Init();
    }

    public void updateStats()
    {
        // ���� ǥ��~
        // stat1.onValueChanged.AddListener( �Լ� �߰� ); 

        stat1Text.text = stat1.value.ToString() + " / 250";  //���� �̸� ����  ����Ʈ�� �ٲٵ��� 
        stat2Text.text = stat2.value.ToString() + " / 250";
        stat3Text.text = stat3.value.ToString() + " / 250";
        stat4Text.text = stat4.value.ToString() + " / 250";
    }
    public void OnClickInventory()
    {
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
        Debug.Log("[MainMenuPopUp] : ����â");
    }
    public void closePopUp()
    {
        //ui.ClosePopup();
        mainMenuPopUp.SetActive(false);
    }
}
