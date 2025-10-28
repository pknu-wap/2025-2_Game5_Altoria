using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomHellper
{
    public T Pick<T>(IList<(T item, float weight)> items)
    {
        float total = items.Sum(i => i.weight);
        float ran = Random.Range(0, total);
        float cumulative = 0;

        float rand = Random.Range(0, total);

        for(int i = 0; i < items.Count(); i++)
        {
            cumulative += items[i].weight;
            if (cumulative > ran)
                return items[i].item;
            cumulative += items[i].weight;
        }

        return default;
    }
}
