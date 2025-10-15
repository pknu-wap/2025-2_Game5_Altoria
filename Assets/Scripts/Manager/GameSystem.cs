using GameInteract;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Common
{
    public class GameSystem
    {
        public readonly CraftingController Crafting = new();

        CollectHandler collectHandler;

        public GameSystem()
        {
            Init();
        }

        void Init()
        {
            Manager.Resource.LoadAsync<CollectHandler>("CollectHandler", asset =>
            {
                if (asset == null)
                    Debug.LogError("CollectHandler is not exit");
                else
                    collectHandler = asset;
            });
        }

        public List<CraftingSlot> GetCurrentCraftingSlots(CraftingType type) => Crafting.GetCurrentCraftingSlots(type);
        public bool HaveEmptySlot(CraftingType type) => Crafting.HaveEmptySlot(type);    
        public CraftingSlot GetCraftingSlot(CraftingType type, int slotIndex) => Crafting.GetCraftingSlot(type, slotIndex);  
        public ItemData GetCompletedItem(CraftingType type, int slotIndex) => Crafting.GetCompletedItem(type, slotIndex);

        public FishDataSO GetFishData(Define.AreaType areaType) => collectHandler.FishiSO(areaType);
        public CollectDataSO GetCollectData(string id) => collectHandler.CollectSo(id);
    }
}
