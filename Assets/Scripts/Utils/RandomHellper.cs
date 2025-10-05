using System.Collections.Generic;
using UnityEngine;

public class RandomHellper
{
    /// 확률(weight)을 가진 요소 중 하나를 선택.
    private T Pick<T>(IEnumerable<(T item, float weight)> items)
    {
        float total = 0;
        foreach (var i in items)
            total += i.weight;

        float rand = Random.Range(0, total);

        foreach (var i in items)
        {
            if (rand < i.weight)
                return i.item;
            rand -= i.weight;
        }

        return default;
    }

    // 채집 시스템
    public string GetCollectItem(CollectInteractSO data, CollectToolSO tool)
    {
        if (data == null || data.drops == null || data.drops.Count == 0)
        {
            Debug.LogWarning($"{GetType()} : 데이터가 없습니다.");
            return null;
        }

        var list = new List<(string, float)>();

        foreach (var drop in data.drops)
        {
            float prob = drop.baseProbability;
            if (tool != null && tool.type == data.collectType)
                prob += prob * tool.rareBonus;

            list.Add((drop.itemID, prob));
        }

        return Pick(list);
    }

    // 강화 시스템
    public string GetUpgradeResult() { return null; }
    // 재작 시스템
    public string GetCrateBigResult() { return null; }
}
