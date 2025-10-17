using GameInteract;
using ILoader;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static Define;

namespace GameData
{
    public static class GameDB
    {
        public static readonly CraftDatabase CraftingDB;
        public static readonly ItemDatabase ItemDB;
        public static readonly GradeDatabase GradeDB;
        public static readonly CollectDatabase CollectDB;
        public static readonly FishDatabase FishDB;

        static GameDB()
        {
            ItemDB = new ItemDatabase(new CsvLoader<ItemData>());
            CraftingDB = new CraftDatabase(new JsonLoader<CraftingData>());
            GradeDB = new GradeDatabase(new CsvLoader<GradeData>());
            CollectDB = new CollectDatabase(new JsonLoader<CollectData>());
            FishDB = new FishDatabase(new JsonLoader<FishData>());
        }

        public static async Task LoadAll()
        {
            await CraftingDB.LoadAsync(nameof(CraftDatabase));
            await ItemDB.LoadAsync(nameof(ItemDatabase));
            await GradeDB.LoadAsync(nameof(GradeDatabase));
            await CollectDB.LoadAsync(nameof(CollectDatabase));
            await FishDB.LoadAsync(nameof(FishDatabase));
        }

        public static CustomDictionary<CraftingData>? GetCraftTypeData(CraftingType type)
            => CraftingDB.TryGetValue(type, out var dict) ? dict : null;
        public static ItemData? GetItemData(string itemId)
            => ItemDB.TryGetValue(itemId, out var data) ? data : null;
        public static GradeData? GetGradeData(int currentGrade)
            => GradeDB.TryGetValue(currentGrade, out var data) ? data : null;
        public static CustomDictionary<CollectData>? GetCollectData(string id)
            => CollectDB.TryGetValue(id, out var data) ? data : null;
        public static CustomDictionary<FishData>? GetFishData(string areaType)
            => FishDB.TryGetValue(areaType, out var data) ? data : null;
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

    public class CollectDatabase : GameDatabase<string, CustomDictionary<CollectData>>
    {
        readonly ILoadStrategy<CollectData> innerLoader;

        public CollectDatabase(ILoadStrategy<CollectData> loader) : base(null!)
        {
            innerLoader = loader;
        }

        public override async Task LoadAsync(string sourceKey)
        {
            var collectList = await innerLoader.LoadAsync(sourceKey);

            var dict = new CustomDictionary<CollectData>();
            for (int i = 0; i < collectList.Count; i++)
            {
                var data = collectList[i];
                dict.Value[data.ID] = data;
            }

            OnLoaded(new List<CustomDictionary<CollectData>> { dict });
        }

        protected override void OnLoaded(IList<CustomDictionary<CollectData>> assets)
        {
            values.Clear();

            for (int i = 0; i < assets.Count; i++)
            {
                var currentDict = assets[i].Value;
                var keys = new List<string>(currentDict.Keys);

                for (int j = 0; j < keys.Count; j++)
                {
                    var key = keys[j];
                    var collectData = currentDict[key];

                    if (!values.TryGetValue(collectData.ID, out var collectDict))
                    {
                        collectDict = new CustomDictionary<CollectData>();
                        values[collectData.ID] = collectDict;
                    }

                    collectDict.Value[collectData.ID] = collectData;
                }
            }
        }
    }

    public class FishDatabase : GameDatabase<string, CustomDictionary<FishData>>
    {
        readonly ILoadStrategy<FishData> innerLoader;

        public FishDatabase(ILoadStrategy<FishData> loader) : base(null!)
        {
            innerLoader = loader;
        }

        public override async Task LoadAsync(string sourceKey)
        {
            var fishList = await innerLoader.LoadAsync(sourceKey);

            var dict = new CustomDictionary<FishData>();
            for (int i = 0; i < fishList.Count; i++)
            {
                var data = fishList[i];
                dict.Value[data.Area.ToString()] = data;
            }

            OnLoaded(new List<CustomDictionary<FishData>> { dict });
        }

        protected override void OnLoaded(IList<CustomDictionary<FishData>> assets)
        {
            values.Clear();

            for (int i = 0; i < assets.Count; i++)
            {
                var currentDict = assets[i].Value;
                var keys = new List<string>(currentDict.Keys);

                for (int j = 0; j < keys.Count; j++)
                {
                    var key = keys[j];
                    var fishData = currentDict[key];

                    if (!values.TryGetValue(fishData.Area, out var fishDict))
                    {
                        fishDict = new CustomDictionary<FishData>();
                        values[fishData.Area] = fishDict;
                    }

                    fishDict.Value[fishData.Area.ToString()] = fishData;
                }
            }
        }
    }
}
