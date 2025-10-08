using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class TopButtonSet
{
    public Image button;
    public Image focus;
    public GameObject page;
}

public enum SortType
{
    GradeHighToLow,
    GradeLowToHigh
}

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryUI;

    [Header("Top Tabs")]
    [SerializeField] TopButtonSet[] topButtons;

    [Header("Sort Dropdown")]
    [SerializeField] TMP_Dropdown sortDropdown;
    private readonly Dictionary<SortType, string> sortLabels = new Dictionary<SortType, string>()
    {
        { SortType.GradeHighToLow, "등급 높은 순" },
        { SortType.GradeLowToHigh, "등급 낮은 순" }
    };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 아이템 데이터 테이블 가져오기
        //

        InitSortDropdown();
        InitCategoryButtons();
    }

    void OnEnable()
    {
        RefreshInventory();
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
                        topButtons[j].page.SetActive(true);
                    }
                    else
                    {
                        topButtons[j].focus.gameObject.SetActive(false);
                        topButtons[j].page.SetActive(false);
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

    // 정렬 방식 변경
    public void OnSortChanged(int index)
    {
        SortType selectedSort = (SortType)index;

        //ItemSorter.SortItems(아이템데이터, selectedSort);

        RefreshInventory();
        Debug.Log($"[InventoryUI] : 정렬 방식 변경됨 - {sortLabels[selectedSort]}");
        SoundManager.Instance.PlaySFX(SFX.ButtonClick);
    }

    // 인벤토리 최신화
    public void RefreshInventory()
    {
        //////////// 인벤토리 최신화 로직 구현
        Debug.Log("[InventoryUI] : 인벤토리 최신화됨");
    }

    // 인벤토리에 아이템 슬롯 추가
    public void AddItemToInventory(int itemID, int count)
    {
        //////////// 아이템 추가 로직 구현
        Debug.Log($"[InventoryUI] : 아이템 추가됨 ID: {itemID}, Count: {count}");
    }

    // 창 닫기
    public void OnClose()
    {
        inventoryUI.SetActive(false);
        SoundManager.Instance.PlaySFX(SFX.ButtonClick);
    }
}
