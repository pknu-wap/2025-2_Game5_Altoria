using GameUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDeletePopUp : UIPopUp
{
    public static ItemDeletePopUp Instance;

    [Header("UI 요소")]
    [SerializeField] InventoryItemSlot itemIcon;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] Button plusButton;
    [SerializeField] Button minusButton;
    [SerializeField] TMP_InputField CountInput;

    InventoryData data;

    int currentCount = 1;
    int maxCount;  // 갖고있는 아이템 개수
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        SetItem(itemID, maxCount);
        CountInput.onValueChanged.RemoveListener(OnInputChanged);
        CountInput.text = currentCount.ToString();
        CountInput.onValueChanged.AddListener(OnInputChanged);
    }

    public override bool Init()
    {
        return base.Init();
    }

    public void SetItem(string id, int count)
    {
        // 아이템 아이콘에 표시할 정보 가져옴 
        data = InventoryManager.Instance.GetItemData(id);

        if (data != null)
        {
            itemIcon.SetSlot(data.Item.SpriteAddress, count);  // 아이템 이미지, 테두리, 개수 설정.현재는 개수만됨
            itemNameText.text = data.Item.Name;
        }
    }

    void OnInputChanged(string str)
    {
        CountInput.onValueChanged.RemoveListener(OnInputChanged);

        if (int.TryParse(str, out int val))
            currentCount = Mathf.Clamp(val, 1, maxCount);
        else
            currentCount = 1;


        CountInput.text = currentCount.ToString();
        CountInput.onValueChanged.AddListener(OnInputChanged);
    }

    void OnPlus()
    {
        if (currentCount >= maxCount) return;
        currentCount++;

        CountInput.onValueChanged.RemoveListener(OnInputChanged);
        CountInput.text = currentCount.ToString();
        CountInput.onValueChanged.AddListener(OnInputChanged);
    }

    private void OnMinus()
    {
        if (currentCount <= 1) return;
        currentCount--;

        CountInput.onValueChanged.RemoveListener(OnInputChanged);
        CountInput.text = currentCount.ToString();
        CountInput.onValueChanged.AddListener(OnInputChanged);
    }

    public void DeleteItem()
    {
        if (int.TryParse(CountInput.text, out int val))
            currentCount = Mathf.Clamp(val, 1, maxCount);
        else
            currentCount = 1;

        bool success = InventoryManager.Instance.RemoveItem(itemID, currentCount);

        if (success)
        {
            Debug.Log($"[ItemDeletePopUp] : {data.Item.Name} 아이템 {currentCount}개 삭제 완료");
        }
        else
        {
            Debug.Log($"[ItemDeletePopUp] : {data.Item.Name} 삭제 실패 - 존재하지 않음");
        }

        InventoryUI.Instance.RefreshInventory();

        OnClose();
    }

    public void Open(string id, int count)
    {
        itemID = id;
        maxCount = Mathf.Max(1, count);  
        currentCount = 1;

        gameObject.SetActive(true);
        CountInput.onValueChanged.RemoveListener(OnInputChanged);  //잠깐 끊기 
        CountInput.text = currentCount.ToString();
        CountInput.onValueChanged.AddListener(OnInputChanged);
        Debug.Log($"[ItemDeletePopUp] : {itemID} 아이템 삭제창 오픈 / maxCount={maxCount}");
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
        //Manager.UI.ClosePopup();  //오류 
    }
}
