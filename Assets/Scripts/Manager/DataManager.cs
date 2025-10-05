using GameInteract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameData
{
    public static class GameDB
    {
        public static readonly CraftDatabase CraftingDataBase;
        public static readonly ItemDatabase ItemDatabase;

        static GameDB()
        {
            ItemDatabase = new ItemDatabase(new JsonLoader<ItemData>());
            CraftingDataBase = new CraftDatabase(new JsonLoader<CustomDictionary<CraftingData>>());
           
        }

        public static async Task LoadAll()
        {
            await CraftingDataBase.LoadAsync(nameof(CraftDatabase)); 
            await ItemDatabase.LoadAsync(nameof(ItemDatabase)); 
         
        }

        public static CustomDictionary<CraftingData>? GetCraftTypeData(CraftingType type)
            => CraftingDataBase.TryGetValue(type, out var dict) ? dict: null;
        public static ItemData? GetItemData(string itemId)
            => ItemDatabase.TryGetValue(itemId, out var data) ? data : null;
    }
    public abstract class GameDatabase<TKey, TValue>
    {
        protected readonly Dictionary<TKey, TValue> values = new();
        private readonly ILoadStrategy<TValue> loader;

        protected GameDatabase(ILoadStrategy<TValue> loader)
        {
            this.loader = loader;
        }

        public async Task LoadAsync(string sourceKey)
        {
            var assets = await loader.LoadAsync(sourceKey);
            OnLoaded(assets);
        }

        protected abstract void OnLoaded(IList<TValue> assets);

        public bool TryGetValue(TKey key, out TValue value) => values.TryGetValue(key, out value);
    }


    public class CraftDatabase : GameDatabase<CraftingType, CustomDictionary<CraftingData>>
    {
        public CraftDatabase(ILoadStrategy<CustomDictionary<CraftingData>> loader) : base(loader) { }

        protected override void OnLoaded(IList<CustomDictionary<CraftingData>> assets)
        {
            values.Clear();

            for (int i = 0; i < assets.Count; i++)
            {
                var dict = assets[i].Value;
                var keys = new List<string>(dict.Keys);

                for (int j = 0; j < keys.Count; j++)
                {
                    var key = keys[j];
                    var craftingData = dict[key];

                    if (!values.TryGetValue(craftingData.Type, out var typeDict))
                    {
                        typeDict = new CustomDictionary<CraftingData>();
                        values[craftingData.Type] = typeDict;
                    }

                    typeDict.Value[craftingData.ID] = craftingData;
                }
            }
        }


    }


    public class ItemDatabase : GameDatabase<string, ItemData>
    {
        public ItemDatabase(ILoadStrategy<ItemData> loader) : base(loader) { }

        protected override void OnLoaded(IList<ItemData> assets)
        {
            values.Clear();
            for (int i = 0; i < assets.Count; i++)
            {
                values[assets[i].ID] = assets[i];
            }
        }
    }
}
