using GameInteract;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GameInteract
{
    public class CollectDropHellper
    {
        public int? GetRandomItem(CollectInteractSO data, CollectToolSO tool)
        {
            float totalProb = 0;

            for (int i = 0; i < data.drops.Count; i++)
            {
                float finalProb = data.drops[i].baseProbability;

                // 도구 보정
                if (tool != null && tool.type == data.collectType)
                    finalProb += finalProb * tool.rareBonus;

                totalProb += finalProb;
                Debug.Log($"아이템 ID : {data.drops[i].itemID} 확률 : {finalProb}");
            }

            float ran = Random.Range(0, totalProb);

            for (int i = 0; i < data.drops.Count; i++)
            {
                float finalProb = data.drops[i].baseProbability;
                if (tool != null && tool.type == data.collectType)
                    finalProb += finalProb * tool.rareBonus;

                if (ran < finalProb)
                    return data.drops[i].itemID;

                ran -= finalProb;
            }

            return null;
        }

    }
}