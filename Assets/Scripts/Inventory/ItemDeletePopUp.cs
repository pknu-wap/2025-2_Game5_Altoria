using GameUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDeletePopUp : UIPopUp
{
    public static ItemDeletePopUp Instance;
   
    UIController ui;

    [Header("UI 요소")]
    [SerializeField] InventoryItemSlot itemIcon;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] Button plusButton;
    [SerializeField] Button minusButton;
    [SerializeField] TMP_InputField CountInput;

    int currentCount = 1;
    int maxCount = 133;  // 갖고있는 아이템 개수
    string itemID;

    private void Awake()
    {
        Instance = this;   //인스턴스 초기화

        CountInput.onValueChanged.AddListener(OnInputChanged);
        plusButton.onClick.AddListener(OnPlus);
        minusButton.onClick.AddListener(OnMinus);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CountInput.text = "1";
        //ui = Manager.UI;     오류
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool Init()
    {
        return base.Init();
    }

    public void SetItem()
    {
        // 아이템 아이콘에 표시할 정보 가져옴
        //itemIcon.SetSlot(itemID, 1);
        //itemNameText.text = 
    }

    void OnInputChanged(string str)
    {
        if (int.TryParse(str, out int val))
        {
            currentCount = Mathf.Clamp(val, 1, maxCount);
        }
        else
        {
            currentCount = 1; 
        }

        CountInput.text = currentCount.ToString();
    }

    void OnPlus()
    {
        if (currentCount < maxCount)
        {
            currentCount++;
            CountInput.text = currentCount.ToString();
        }
    }

    private void OnMinus()
    {
        if (currentCount > 1)
        {
            currentCount--;
            CountInput.text = currentCount.ToString();
        }
    }

    public void DeleteItem()
    {
        if (int.TryParse(CountInput.text, out int val))
            currentCount = Mathf.Clamp(val, 1, maxCount);
        else
            currentCount = 1;
        // 아이템 버리기 로직 구현
        Debug.Log("[ItemDiscardPopUp] : {} 아이템 {}개 삭제");
        OnClose();
    }
    public void OnOpen()
    {
        gameObject.SetActive(true);
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
        //ui.ClosePopup();  //오류 
    }
}
