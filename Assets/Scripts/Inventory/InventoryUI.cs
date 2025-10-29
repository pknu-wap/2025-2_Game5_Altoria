using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

[System.Serializable]
public class TopButtonSet
{
    public Image button;
    public Image focus;
}

public enum SortType
{
    GradeHighToLow,
    GradeLowToHigh
}

public class InventoryUI : MonoBehaviour
{
    #region Singleton
    public static InventoryUI Instance;
    private void Awake()
    {
        Instance = this;   //인스턴스 초기화
    }
    #endregion

    [Header("Top Tabs")]
    [SerializeField] TopButtonSet[] topButtons;  //Weapon, Tool, Consume, Material, Additive
    Define.ItemType currentType = Define.ItemType.None;
    SortType currentSort = SortType.GradeHighToLow;

    [Header("Sort Dropdown")]
    [SerializeField] TMP_Dropdown sortDropdown;
    readonly Dictionary<SortType, string> sortLabels = new Dictionary<SortType, string>()
    {
        { SortType.GradeHighToLow, "등급 높은 순" },
        { SortType.GradeLowToHigh, "등급 낮은 순" }
    };

    [Header("Slots")]
    [SerializeField] InventoryItemSlot itemPrefab;
    [SerializeField] Transform slotsParent;
    List<InventoryItemSlot> displayList = new();
    List<InventoryData> displayListData = new();

    [SerializeField] ItemDeletePopUp deletePopUp;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayListData = new List<InventoryData>(InventoryManager.Instance.GetAllItems());

        InitSortDropdown();
        InitCategoryButtons();
        RefreshInventory();
    }

    void OnEnable()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }



    // 상위 버튼 선택 관리
    public void InitCategoryButtons()
    {
        for (int i = 0; i < topButtons.Length; i++)
        {
            int index = i;
            topButtons[i].button.GetComponent<Button>().onClick.AddListener(() =>
            {
                for (int j = 0; j < topButtons.Length; j++)
                {
                    if (j == index)
                    {
                        OnTypeChanged(j);
                        topButtons[j].focus.gameObject.SetActive(true);
                    }
                    else
                    {
                        topButtons[j].focus.gameObject.SetActive(false);
                    }
                }
                
                SoundManager.Instance.PlaySFX(SFX.ButtonClick);
            });
        }
    }

    // 드롭다운 초기화
    public void InitSortDropdown()
    {
        sortDropdown.ClearOptions();

        List<string> options = new List<string>();
        foreach (SortType type in System.Enum.GetValues(typeof(SortType)))
        {
            options.Add(sortLabels[type]);
        }

        sortDropdown.AddOptions(options);
        sortDropdown.value = 0;
        sortDropdown.RefreshShownValue();

        sortDropdown.onValueChanged.AddListener(OnSortChanged);
    }

    // 보여질 아이템 타입 변경
    public void OnTypeChanged(int index)
    {
        //None, Weapon, Tool, Consume, Material, Additive
        currentType = (Define.ItemType)index;

        if(currentType != Define.ItemType.None)
        {
            var allItems = InventoryManager.Instance.GetAllItems();
            displayListData = allItems.FindAll(item => item.Item != null && item.Item.Type == currentType);
        }
        else
            displayListData = new List<InventoryData>(InventoryManager.Instance.GetAllItems());

        ApplySort();
        RefreshInventory();
        Debug.Log($"[InventoryUI] : {currentType} 아이템만 표시");
    }

    // 정렬 방식 변경
    public void OnSortChanged(int index)
    {
        currentSort = (SortType)index;
        ApplySort();
        RefreshInventory();

        Debug.Log($"[InventoryUI] : 정렬 방식 변경됨 - {sortLabels[currentSort]}");
        SoundManager.Instance.PlaySFX(SFX.ButtonClick);
    }

    //정렬
    private void ApplySort()
    {
        if (displayListData == null || displayListData.Count == 0) return;

        switch (currentSort)
        {
            case SortType.GradeHighToLow:
                displayListData.Sort((a, b) => b.Item.Grade.CompareTo(a.Item.Grade));
                break;
            case SortType.GradeLowToHigh:
                displayListData.Sort((a, b) => a.Item.Grade.CompareTo(b.Item.Grade));
                break;
        }
    }

    // 인벤토리 최신화 - 필터링,정렬된 데이터로 
    public void RefreshInventory()
    {
        foreach (var slot in displayList)
            Destroy(slot.gameObject);
        displayList.Clear();
        
        foreach (var data in displayListData)
        {
            InventoryItemSlot slot = Instantiate(itemPrefab, slotsParent);
            displayList.Add(slot);
            Debug.Log($"[InventoryUI] : 아이템 슬롯 생성 - ID: {data.Item.ID}, Count: {data.Count}");

            InventoryData itemInfo = InventoryManager.Instance.GetItemData(data.Item.ID);

            /* 이미지 연결 
            Sprite icon = Resources.Load<Sprite>(data.Item.SpriteAddress);
            */

            slot.Initialize(data.Item.ID, data.Count);  //실제 UI에 표시 
        }
        Debug.Log("[InventoryUI] : 인벤토리 최신화됨");
    }


    public void OnClickItemDelete(string id, int count)
    {
        deletePopUp.Open(id, count);   //삭제창에 아이템 정보 전달
    }

    //창 켜기
    public void Show()
    {
        gameObject.SetActive(true);
    }

    //창 닫기
    public void Hide()
    {
        gameObject.SetActive(false);
    }


    // -----------------------------------------------------------------------------
    // 아이템 추가 test
    public void debugAddItem()
    {
        int randomID = Random.Range(1, 201);  // 랜덤 아이템 코드 추가
        string itemID = randomID.ToString();

        InventoryManager.Instance.AddItem(itemID, 10);
        RefreshInventory();
    }
}
