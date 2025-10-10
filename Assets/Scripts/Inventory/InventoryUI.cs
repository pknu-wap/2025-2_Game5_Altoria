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

    [Header("Top Tabs")]
    [SerializeField] TopButtonSet[] topButtons;
    private Define.ItemType currentType = Define.ItemType.None;


    [Header("Sort Dropdown")]
    [SerializeField] TMP_Dropdown sortDropdown;
    private readonly Dictionary<SortType, string> sortLabels = new Dictionary<SortType, string>()
    {
        { SortType.GradeHighToLow, "등급 높은 순" },
        { SortType.GradeLowToHigh, "등급 낮은 순" }
    };

    [Header("Slots")]
    [SerializeField] InventoryItemSlot itemPrefab;
    [SerializeField] Transform slotsParent;
    List<InventoryItemSlot> displayList = new List<InventoryItemSlot>();

    [SerializeField] GameObject ItemDeleteUI;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
                        topButtons[j].focus.gameObject.SetActive(true);
                    }
                    else
                    {
                        topButtons[j].focus.gameObject.SetActive(false);
                    }
                }
                //currentType = (Define.ItemType)index;     -----> 인벤토리 상위 버튼 순서와 ItemType에 있는 분류 순서가 달라서 지금 못함 
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

    // 정렬 방식 변경
    public void OnSortChanged(int index)
    {
        SortType selectedSort = (SortType)index;

        //displayList = ItemSorter.SortItems(아이템데이터, selectedSort);

        //RefreshInventory();
        Debug.Log($"[InventoryUI] : 정렬 방식 변경됨 - {sortLabels[selectedSort]}");
        SoundManager.Instance.PlaySFX(SFX.ButtonClick);
    }

    // 인벤토리 최신화
    public void RefreshInventory()
    {
        foreach (var slot in displayList)
            Destroy(slot.gameObject);
        displayList.Clear();

        // 현재 인벤토리 데이터 가져오기
        List<InventoryData> items = InventoryManager.Instance.GetAllItems();

        // 슬롯 생성 및 세팅
        foreach (var data in items)
        {
            InventoryItemSlot slot = Instantiate(itemPrefab, slotsParent);
            displayList.Add(slot);

            // itemID로 아이템 상세정보 가져오기
            ItemData itemInfo = InventoryManager.Instance.GetItemData(data.ID);

            Sprite icon = null;
            if (itemInfo != null && !string.IsNullOrEmpty(itemInfo.SpriteAddress))
                icon = Resources.Load<Sprite>(itemInfo.SpriteAddress);

            slot.SetSlot(itemInfo?.Name ?? data.ID, data.Count);
        }
        Debug.Log("[InventoryUI] : 인벤토리 최신화됨");
    }


    public void OnClickItemDelete()
    {
        ItemDeleteUI.SetActive(true);   //UIController 이용하여 수정 
    }
    //창 닫기
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
