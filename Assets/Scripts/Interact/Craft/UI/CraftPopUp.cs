using Common;
using GameData;
using GameUI;
using System;
using System.Collections.Generic;
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
        CraftingHandler handler;



        public void SetTypeAndGetData(CraftingType type)
        {
            this.type = type;
            handler = new CraftingHandler(type);
            RefreshProgressSlots();
            CreateSlots();
            Subscribe();
        }

        void OnDisable() => Unsubscribe();

        void CreateSlots()
        {
            List<CraftSlotInfo> infos = handler.GetSlotInfos();
            if (infos == null || infos.Count == 0) return;
            GameObject prefab = Resources.Load<GameObject>(nameof(CraftItemSlot));

            for (int i = 0; i < infos.Count; i++)
            {
                CraftSlotInfo info = infos[i];

                GameObject obj = UnityEngine.Object.Instantiate(prefab, listRoot);
                if (obj.TryGetComponent<CraftItemSlot>(out var slot))
                {
                    slot.SetSlot(info.Index, info.ItemID, info.Count);
                    slot.OnClicked += OnClickItemSlot;
                }
            }
        }

        void OnClickItemSlot(int slotIndex)
        {
            ItemData resultItem = handler.GetResultItem(slotIndex);
            float time = handler.GetCraftingData(slotIndex).Time;
            List<ItemEntry> ingredients = handler.GetIngredients(slotIndex);

            CraftRecipePopUp  popUp= Manager.UI.ShowPopup<CraftRecipePopUp>();
            popUp.SetRecipe(resultItem, ingredients,time);
            popUp.OnCraftButtonClicked += () =>
            {
                if(CheckEmptySlot())
                OnProgressStart(slotIndex);
                else
                {
                    //todo : 현재 칸이 부족하다는 알림창 
                }
            };
        }
        
        void RefreshProgressSlots()
        {
            List<CraftingSlot> craftingSlots = GameSystem.Instance.GetCurrentCraftingSlots(type);
            for (int index = 0; index < craftingSlots.Count; index++)
            {
                int currentIndex = index;
                CraftingSlot slot = craftingSlots[currentIndex];
                
                Action action = slot.State switch
                {
                    CraftingState.Crafting => () => progressSlots.SetItem(currentIndex,slot),

                    CraftingState.Completed => () =>
                    {
                        progressSlots.SetItem(currentIndex,slot);
                        progressSlots.OnCompleteProgress(currentIndex);
                    },
                    _ => () => progressSlots.ClearSlot(currentIndex)
                };

                action?.Invoke(); 
            }
        }

        void OpenProgressPopUp(CraftingState state,CraftingSlot slot,int slotIndex)
        {
            ItemEntry entry = slot.Recipe.ResultItem;
            CraftProgressPopUp popUp =  Manager.UI.ShowPopup<CraftProgressPopUp>();
            popUp.InitEntry(state, entry.Item.ID, entry.Value);
            if (popUp is IActionClickable button)
            {
                button.OnClicked += () =>
                {
                    handler.GetCompletedItem(slotIndex);
                    Manager.UI.ClosePopup();
                    RefreshProgressSlots();
                };
            };
            
        }
                   
        
        bool CheckEmptySlot() => handler.HaveEmptySlot();


        #region Progress
        void OnClickProgressSlot(int slotIndex)
        {
            CraftingSlot slot = handler.GetCraftingSlot(slotIndex);

            if (slot.State == CraftingState.None) return;

            OpenProgressPopUp(slot.State, slot,slotIndex);
        }
        void OnProgressStart(int slotIndex)
        {
            handler.StartCrafting(slotIndex);
            RefreshProgressSlots();

            Manager.UI.ClosePopup();
        }
        void OnProgressUpdate(CraftingProgressEvent packet)
        {
            if (packet.Type != type) return;
            progressSlots.UpdateProgress(packet.SlotIndex, packet.Progress);
        }

        void OnProgressComplete(CraftingCompletedEvent packet)
        {
            if (packet.Type != type) return;
            progressSlots.OnCompleteProgress(packet.SlotIndex);
        }
        #endregion

        #region Event Binding
        void Subscribe()
        {
            progressSlots.OnSlotClicked += OnClickProgressSlot;
            GlobalEvents.Instance.Subscribe<CraftingProgressEvent>(OnProgressUpdate);
            GlobalEvents.Instance.Subscribe<CraftingCompletedEvent>(OnProgressComplete);
        }

        void Unsubscribe()
        {
            progressSlots.OnSlotClicked -= OnClickProgressSlot;
            GlobalEvents.Instance.Unsubscribe<CraftingProgressEvent>(OnProgressUpdate);
            GlobalEvents.Instance.Unsubscribe<CraftingCompletedEvent>(OnProgressComplete);
        }
        #endregion
    }
}
