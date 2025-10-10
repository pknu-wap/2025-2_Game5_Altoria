using GameUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDeletePopUp : UIPopUp
{
    UIController ui;

    [Header("UI 요소")]
    [SerializeField] InventoryItemSlot itemIcon;
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] Button plusButton;
    [SerializeField] Button minusButton;
    [SerializeField] TMP_InputField CountInput;

    int currentCount = 1;
    int maxCount = 1;  // 갖고있는 아이템 개수
    string itemID;

    private void Awake()
    {
        CountInput.onValueChanged.AddListener(OnInputChanged);
        plusButton.onClick.AddListener(OnPlus);
        minusButton.onClick.AddListener(OnMinus);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CountInput.text = "1";
        ui = Manager.UI;
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
        // 아이템 버리기 로직 구현
        Debug.Log("[ItemDiscardPopUp] : {} 아이템 {}개 삭제");
        OnClose();
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
        //ui.ClosePopup();  //오류 
    }
}
