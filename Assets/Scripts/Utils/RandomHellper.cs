using System.Collections.Generic;
using System.Linq;

public class RandomHellper
{
    public T Pick<T>(IList<(T item, float weight)> items, float bousProb = 0)
    {
        float minWeight = items.Min(i => i.weight);

        foreach (var i in items)
            UnityEngine.Debug.Log($"ÃÊ±â È®·ü {i.weight}\n");

        var adjustedItems = items.Select(i =>
        {
            float newWeight = i.weight;

            if (i.weight == minWeight)
                newWeight *= (1f + bousProb);

            return (i.item, newWeight);
        }).ToList();

        foreach (var i in adjustedItems)
            UnityEngine.Debug.Log($"Á¶Á¤µÈ È®·ü {i.newWeight}\n");

        float total = adjustedItems.Sum(i => i.newWeight);
        float rand = UnityEngine.Random.Range(0, total);

        float cumulative = 0f;
        foreach (var i in adjustedItems)
        {
            cumulative += i.newWeight;
            if (rand <= cumulative)
                return i.item;
        }

        return default;
    }
}
