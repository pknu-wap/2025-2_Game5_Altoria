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

        public override bool Init()
        {
            if (base.Init() == false) return false;
            RefreshProgressSlots();


            return true;
        }

        public void SetTypeAndGetData(CraftingType type)
        {
            this.type = type;
            handler = new CraftingHandler(type);
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
                    slot.SetSlot(info.Index, info.SpriteAddress, info.Count);
                    slot.OnSlotClick += OnClickItemSlot;
                }
            }
        }

        void OnClickItemSlot(int slotIndex)
        {
            ItemData resultItem = handler.GetResultItem(slotIndex);
            List<ItemEntry> ingredients = handler.GetIngredients(slotIndex);

            CraftRecipePopUp  popUp= Manager.UI.ShowPopup<CraftRecipePopUp>();
            popUp.SetRecipe(resultItem, ingredients,10);
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
            List<CraftingSlot> craftingSlots = Manager.System.GetCurrentCraftingSlots(type);

            for (int index = 0; index < craftingSlots.Count; index++)
            {
                var slot = craftingSlots[index];

                if (slot.State!=CraftingState.None)
                    progressSlots.SetItemIcon(index); 
                else
                    progressSlots.ClearSlot(index);
            }
        }

        void OnClickProgressSlot(int slotIndex)
        {
            CraftingSlot slot = handler.GetCraftingSlot(slotIndex);

            Action action = slot.State switch
            {
                CraftingState.Crafting => () => Debug.Log("Crafting"),
                CraftingState.Completed => () => Debug.Log("Complete"),
                _ => () => Debug.Log("None")
            };
        }
       
        void  OnProgressStart(int slotIndex)
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
        bool CheckEmptySlot() => handler.HaveEmptySlot();

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
