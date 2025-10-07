using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BamaoUIPack.Scripts
{
    public class EquipmentTabView : MonoBehaviour
    {
        public List<Button> equipmentTabButtons;
        public List<float> TabGapPos;
        public RectTransform TabLineObj;
        public int CurrentTabIndex;
        public List<EquipmentItemPrefab> equipmentItemPrefabs;
        public List<EquipmentSlotPrefab> equipmentSlotPrefabs;
        public Dictionary<int, int> equipmentStatusDic = new Dictionary<int, int>();

        private void Start()
        {
            // 初始化 Tab 按鈕
            for (var index = 0; index < equipmentTabButtons.Count; index++)
            {
                var btn = equipmentTabButtons[index];
                var index1 = index;
                btn.onClick.AddListener(() =>
                {
                    CurrentTabIndex = index1;
                    StartCoroutine(MoveTabLine(new Vector2(TabGapPos[index1], TabLineObj.anchoredPosition.y)));

                    // 設定每個類型的顯示邏輯
                    foreach (var itemPrefab in equipmentItemPrefabs)
                    {
                        itemPrefab.SetTabVisibility(CurrentTabIndex);
                    }
                });
            }

            // 初始化物品功能
            for (var index = 0; index < equipmentItemPrefabs.Count; index++)
            {
                var item = equipmentItemPrefabs[index];
                var index1 = index;

                // 物品點擊行為
                item.OnClickAction += () =>
                {
                    int categoryKey = item.ItemCategory - 1; // 每種類型的 Key (例如: 盔甲 / 武器)

                    if (!equipmentStatusDic.ContainsKey(categoryKey))
                    {
                        // 尚未裝備該類型的物品，直接裝備
                        equipmentStatusDic[categoryKey] = index1;
                        equipmentSlotPrefabs[categoryKey]
                            .SetItem(item.sprite, item.qualitySprites[item.ItemQuality]);
                        equipmentSlotPrefabs[categoryKey].SetActive(true);
                        item.SetEquipped(true); // 設定為已裝備
                    }
                    else if (equipmentStatusDic[categoryKey] == index1)
                    {
                        // 如果已裝備的是同一個物品，則取消裝備
                        equipmentItemPrefabs[categoryKey].SetEquipped(false); // 設定為未裝備
                        equipmentStatusDic.Remove(categoryKey);
                        equipmentSlotPrefabs[categoryKey].SetActive(false); // 停用該 Slot
                    }
                    else
                    {
                        // 如果裝備不同的物品，進行替換
                        int previousIndex = equipmentStatusDic[categoryKey];
                        equipmentItemPrefabs[previousIndex].SetEquipped(false); // 取消之前裝備的物品
                        equipmentStatusDic[categoryKey] = index1; // 更新裝備狀態

                        equipmentSlotPrefabs[categoryKey]
                            .SetItem(item.sprite, item.qualitySprites[item.ItemQuality]);
                        equipmentSlotPrefabs[categoryKey].SetActive(true);
                        item.SetEquipped(true); // 設定當前物品為已裝備
                    }
                };
            }
        }

        private IEnumerator MoveTabLine(Vector2 targetPosition)
        {
            float duration = 0.15f; // 動畫持續時間
            float elapsedTime = 0f;
            Vector2 startPosition = TabLineObj.anchoredPosition; // 改用 RectTransform 的 anchoredPosition

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                TabLineObj.anchoredPosition =
                    Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration); // 使用 Vector2.Lerp 平滑過渡
                yield return null;
            }

            TabLineObj.anchoredPosition = targetPosition; // 確保最終位置精確
        }
    }
}