using GameUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameInventory;
using static UnityEngine.EventSystems.EventTrigger;
using Common;

public class ItemDeletePopUp : UIPopUp
{
    public static ItemDeletePopUp Instance;

    [Header("UI 요소")]
    [SerializeField] InventoryItemSlot itemIcon;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] Button plusButton;
    [SerializeField] Button minusButton;
    [SerializeField] Button maxButton;
    [SerializeField] TMP_InputField CountInput;

    InventoryEntry currentItem;

    int currentCount = 1;
    int maxCount;  // 갖고있는 아이템 개수
    string itemID;

    void Awake()
    {
        //Manager.Init();
        Instance = this;   //인스턴스 초기화

        CountInput.onValueChanged.AddListener(OnInputChanged);
        plusButton.onClick.AddListener(OnPlus);
        minusButton.onClick.AddListener(OnMinus);
        maxButton.onClick.AddListener(OnMax);
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

    void OnEnable()
    {
        //SetItem(itemID, maxCount);

    }

    public override bool Init()
    {
        CountInput.text = "1";
        return base.Init();
    }

    public void SetItem(InventoryEntry data)
    {
        if (data != null)
        {
            currentItem = data;
            itemID = data.item.ItemData.ID;
            maxCount = Mathf.Max(1, data.count);

            itemIcon.SetSlot(itemID, data.count, data.item.ItemData.Grade);  // 아이템 이미지, 테두리, 개수 설정
            itemNameText.text = data.item.ItemData.Name;
        }
        else return;
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

    void OnMinus()
    {
        if (currentCount <= 1) return;
        currentCount--;

        CountInput.onValueChanged.RemoveListener(OnInputChanged);
        CountInput.text = currentCount.ToString();
        CountInput.onValueChanged.AddListener(OnInputChanged);
    }

    public void OnMax()
    {
        currentCount = maxCount;

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

        bool success = GameSystem.Inventory.RemoveItem(currentItem.item.ItemData.ID, currentCount);

        if (success)
        {
            Debug.Log($"[ItemDeletePopUp] : {currentItem.item.ItemData.Name} 아이템 {currentCount}개 삭제 완료");
        }
        else
        {
            Debug.Log($"[ItemDeletePopUp] : {currentItem.item.ItemData.Name} 삭제 실패 - 존재하지 않음");
        }

        InventoryUI.Instance.RefreshInventory();
        OnClose();
    }
    public static ItemDeletePopUp Open(InventoryEntry data)
    {
        var popup = Manager.UI.ShowPopup<ItemDeletePopUp>(); 
        popup.SetUp(data);

        Debug.Log($"[ItemDeletePopUp] : {data.item.ItemData.ID} 아이템 삭제창 오픈 / maxCount={data.count}");
        return popup;
    }

    public void SetUp(InventoryEntry data)
    {
        if (data == null) return;

        currentItem = data;
        itemID = data.item.ItemData.ID;
        maxCount = Mathf.Max(1, data.count);

        itemIcon.SetSlot(itemID, data.count, data.item.ItemData.Grade);
        itemNameText.text = data.item.ItemData.Name;

        currentCount = 1;
       
        CountInput.onValueChanged.RemoveListener(OnInputChanged);  //잠깐 끊기 
        CountInput.text = currentCount.ToString();
        CountInput.onValueChanged.AddListener(OnInputChanged);
    }

    public void OnClose()
    {
        Manager.UI.ClosePopup();  
    }
}
