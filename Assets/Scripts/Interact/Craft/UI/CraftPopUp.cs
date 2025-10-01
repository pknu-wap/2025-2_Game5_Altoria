using GameUI;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace GameInteract
{
    
    public class CraftPopUp : UIPopUp
    {
        [SerializeField] Transform listRoot;
        [SerializeField] ProgressSlots progressSlots;
        [SerializeField] Image titleImage;

        CraftingType type = CraftingType.None;
        
        public override bool Init()
        {
            if (base.Init() == false) return false;

            return true;
        }
        public void SetData(CraftingType type)
        {
            this.type = type;
              
            SubScribe();
        }
      
        void OnDisable()
        {
            UnSubscribe();
        }
        void StartProgress(int slotIndex, ItemData data)
        {
            var packet = new CraftingStartedEvent
            {
                Type =type,
                SlotIndex = slotIndex,
                Item = data
            };

            GlobalEvents.Instance.Publish(packet);
        }
        void OnClickProgress(int slotIndex)
        {
            Debug.Log($"[CraftPopUp]:{slotIndex}");
        }
        void OnCompleteProgress(CraftingCompletedEvent packet)
        {
            if (packet.Type != this.type) return;

        }
        void UpdateProgress(CraftingProgressEvent packet)
        {
            if (packet.Type != this.type) return;
            progressSlots.UpdateProgress(packet.SlotIndex, packet.Progress);    
        }

        #region Events
        void SubScribe()
        {
            progressSlots.OnSlotClicked += OnClickProgress;
            GlobalEvents.Instance.Subscribe<CraftingProgressEvent>(UpdateProgress);
            GlobalEvents.Instance.Subscribe<CraftingCompletedEvent>(OnCompleteProgress);
        }

      
        void UnSubscribe()
        {
            progressSlots.OnSlotClicked -= OnClickProgress;
            GlobalEvents.Instance.Unsubscribe<CraftingProgressEvent>(UpdateProgress);
            GlobalEvents.Instance.Unsubscribe<CraftingCompletedEvent>(OnCompleteProgress);
        }
        #endregion

    }
}