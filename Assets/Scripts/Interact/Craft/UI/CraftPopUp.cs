using GameUI;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GameInteract
{
    public class CraftPopUp : UIPopUp
    {
        [SerializeField] Transform listRoot;
        [SerializeField] ProgressSlots progressSlots;

        CraftingType type;
        IDisposable disposable;
        public void SetData(CraftingType type)
        {
            this.type = type;
        }
        void OnEnable()
        {
            SubScribe();
        }
        void OnDisable()
        {
            UnSubscribe();
        }
        void HandleProgress(int slotIndex)
        {
            Debug.Log($"[CraftPopUp]:{slotIndex}");
        }
        #region Events
        void SubScribe()
        {
            progressSlots.OnSlotClicked += HandleProgress;
        }
        void UnSubscribe()
        {
            progressSlots.OnSlotClicked -= HandleProgress;
        }
        #endregion

    }
}