using GameUI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameInteract
{
    public class CraftRecipePopUp : UIPopUp
    {
        [SerializeField] Transform listRoot;
        [SerializeField] Image resultItem;
        [SerializeField] TextMeshProUGUI text;
        public event Action OnCraftButtonClicked;
        public void SetRecipe(ItemData result, List<ItemEntry> ingredients, float craftTime = 0f)
        {
            SetResultItem(result, craftTime);
            SetIngredients(ingredients);
        }

        void SetIngredients(List<ItemEntry> ingredients)
        {
            GameObject prefab = Resources.Load<GameObject>(nameof(ItemSlot));
            if (prefab == null)
            {
                Debug.LogError("[CraftRecipePopUp] ItemSlot prefab not found in Resources.");
                return;
            }

            for (int i = 0; i < ingredients.Count; i++)
            {
                ItemEntry entry = ingredients[i];

                GameObject obj = Instantiate(prefab, listRoot);
                if (obj.TryGetComponent<ItemSlot>(out var slotUI))
                {
                    slotUI.SetSlot(entry.Item.SpriteAddress, entry.Value);
                }
            }
        }

        void SetResultItem(ItemData resultItem,float craftTime)
        {
            string timeText = FormatTime(craftTime);

            text.text =
                $"<b>이름</b> : {resultItem.Name}\n" +
                $"<b>설명</b> : {resultItem.Description}\n" +
                $"<b>소요시간</b> : {timeText}";
        }
     
        string FormatTime(float seconds)
        {
            int totalSeconds = Mathf.RoundToInt(seconds);
            int minutes = totalSeconds / 60;
            int secs = totalSeconds % 60;

            if (minutes > 0)
                return $"{minutes}분 {secs}초";
            else
                return $"{secs}초";
        }
        public void OnButtonClicked() => OnCraftButtonClicked?.Invoke();

    }
}