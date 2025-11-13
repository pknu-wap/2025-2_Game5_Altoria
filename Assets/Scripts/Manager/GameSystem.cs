using GameInteract;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace Common
{
    public class GameSystem : MonoBehaviour
    {
        private static GameSystem instance;

        TimeController time = new();
        RandomHellper randomHellper = new();
        LifeStatsManager lifeStatsManager = new();
        CraftingController crafting = new();
        InventoryManager inventory = new();

        public ETimeType timeType;

        public static GameSystem Instance { get { return instance; } }
        public static TimeController Time { get { return instance.time; } }
        public static RandomHellper Random { get { return instance.randomHellper; } }
        public static LifeStatsManager Life { get { return instance.lifeStatsManager; } }
        public static CraftingController Crafting {  get { return instance.crafting; } }

        public static InventoryManager Inventory { get { return instance.inventory; } }
        public static void Init()
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("GameSystem");
                if (go == null)
                    go = new GameObject { name = "GameSystem" };

                instance = go.GetOrAddComponent<GameSystem>();
                DontDestroyOnLoad(go);
             
            }
        }

        private void Start()
        {
            Inventory.Init();
        }

        public void Update()
        {
            if (Time != null)
                Time.Tick(UnityEngine.Time.deltaTime);
        }

        public void DestroyObject()
        {
            Destroy(this);
        }
      
        public List<CraftingSlot> GetCurrentCraftingSlots(CraftingType type) => Crafting.GetCurrentCraftingSlots(type);
        public bool HaveEmptySlot(CraftingType type) => Crafting.HaveEmptySlot(type);    
        public CraftingSlot GetCraftingSlot(CraftingType type, int slotIndex) => Crafting.GetCraftingSlot(type, slotIndex);  
        public ItemData GetCompletedItem(CraftingType type, int slotIndex) => Crafting.GetCompletedItem(type, slotIndex);
    }
}
