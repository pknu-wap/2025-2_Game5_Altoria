using UnityEngine;
using System;
using Unity.AppUI.UI;

namespace GameInteract
{
    public class ProgressSlots : GameUI.UIBase
    {
        [SerializeField] CraftingProgress[] slots;
        [SerializeField] Sprite testSprite;
        [SerializeField] Sprite emptySprite;
        public event Action<int> OnSlotClicked;

        public override bool Init()
        {
            if (!base.Init()) return false;

            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].Init(i);
                slots[i].OnClicked += HandleClick;
            }
            return true;
        }

        void OnDisable()
        {
            for (int i = 0; i < slots.Length; i++)
                slots[i].OnClicked -= HandleClick;
        }

        void HandleClick(int index)
        {
            Debug.Log($"[ProgressSlots] Click {index}");
            OnSlotClicked?.Invoke(index);
        }
      
        public void SetItem(int index, CraftingSlot slot )
        {
            if (slot.Recipe != null)
            {
                Sprite icon = Manager.Resource.Load<Sprite>(slot.Recipe.ResultItem.Item.ID);
                slots[index].SetIcon(icon);
            }

        }
           

        public void UpdateProgress(int index, float progress)=> slots[index].FillProgress(progress);

        public void OnCompleteProgress(int index)
        {
            slots[index].FillProgress(1f);
            Debug.Log($"[ProgressSlots] Slot {index} completed");
        }

        public void ClearSlot(int index)
        {
            slots[index].SetIcon(emptySprite);
            slots[index].FillProgress(0);
        }
    }
}
