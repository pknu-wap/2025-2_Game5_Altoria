using UnityEngine;
using System;

namespace GameInteract
{
    public class ProgressSlots : MonoBehaviour
    {
        [SerializeField] CraftingProgress[] slots;

        public event Action<int> OnSlotClicked;

        void Awake()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].Init(i);
                slots[i].OnClicked += HandleSlotClicked;
            }
        }
        void OnDisable()
        {
            for(int i=0; i<slots.Length; i++)
            {
                slots[i].OnClicked -= HandleSlotClicked;    
            }
        }

        void HandleSlotClicked(int slotIndex)
        {
            Debug.Log(slotIndex);
            OnSlotClicked?.Invoke(slotIndex);
        }
      
        public void StartProgress(int slotIndex, Sprite icon)
            => slots[slotIndex].SetIcon(icon);

        public void UpdateProgress(int slotIndex, float progress)
            => slots[slotIndex].FillProgress(progress);

        public void OnCompleteProgress(int slotIndex)
        {

        }
    }
}
