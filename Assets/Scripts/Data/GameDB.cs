using GameInteract;
using ILoader;
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
        public static readonly CraftDatabase CraftingDB;
        public static readonly ItemDatabase ItemDB;
        public static readonly GradeDatabase GradeDB;

        static GameDB()
        {
            ItemDB = new ItemDatabase(new CsvLoader<ItemData>());
            CraftingDB = new CraftDatabase(new JsonLoader<CraftingData>());
            GradeDB = new GradeDatabase(new CsvLoader<GradeData>());
        }

        public static async Task LoadAll()
        {
            await CraftingDB.LoadAsync(nameof(CraftDatabase));
            await ItemDB.LoadAsync(nameof(ItemDatabase));
            await GradeDB.LoadAsync(nameof(GradeDatabase));
        }

        public static CustomDictionary<CraftingData>? GetCraftTypeData(CraftingType type)
            => CraftingDB.TryGetValue(type, out var dict) ? dict : null;
        public static ItemData? GetItemData(string itemId)
            => ItemDB.TryGetValue(itemId, out var data) ? data : null;
        public static GradeData? GetGradeData(int currentGrade)
            => GradeDB.TryGetValue(currentGrade, out var data) ? data : null;
    }
    public abstract class GameDatabase<TKey, TValue>
    {
        protected readonly Dictionary<TKey, TValue> values = new();
        readonly ILoadStrategy<TValue> loader;

        protected GameDatabase(ILoadStrategy<TValue> loader)
        {
            this.loader = loader;
        }

        public virtual async Task LoadAsync(string sourceKey)
        {
            var assets = await loader.LoadAsync(sourceKey);
            OnLoaded(assets);
        }

        protected abstract void OnLoaded(IList<TValue> assets);

        public bool TryGetValue(TKey key, out TValue value) => values.TryGetValue(key, out value);
    }

    public class CraftDatabase : GameDatabase<CraftingType, CustomDictionary<CraftingData>>
    {
         readonly ILoadStrategy<CraftingData> innerLoader;

        public CraftDatabase(ILoadStrategy<CraftingData> loader) : base(null!)
        {
            innerLoader = loader;
        }

        public override async Task LoadAsync(string sourceKey)
        {
            var craftingList = await innerLoader.LoadAsync(sourceKey);

            var dict = new CustomDictionary<CraftingData>();
            for (int i = 0; i < craftingList.Count; i++)
            {
                var data = craftingList[i];
                dict.Value[data.ID] = data;
            }

            OnLoaded(new List<CustomDictionary<CraftingData>> { dict });
        }

        protected override void OnLoaded(IList<CustomDictionary<CraftingData>> assets)
        {
            values.Clear();

            for (int i = 0; i < assets.Count; i++)
            {
                var currentDict = assets[i].Value;
                var keys = new List<string>(currentDict.Keys);

                for (int j = 0; j < keys.Count; j++)
                {
                    var key = keys[j];
                    var craftingData = currentDict[key];

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

    public class GradeDatabase : GameDatabase<int, GradeData>
    {
        public GradeDatabase(ILoadStrategy<GradeData> loader) : base(loader) { }

        protected override void OnLoaded(IList<GradeData> assets)
        {
            values.Clear();
            for (int i = 0; i < assets.Count; i++)
            {
                values[assets[i].CurrentGrade] = assets[i];
            }
        }
    }
}
