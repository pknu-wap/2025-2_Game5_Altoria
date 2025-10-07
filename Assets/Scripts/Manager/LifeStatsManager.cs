using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameInteract;

[Serializable]
public class LifeStatData
{
    [SerializeField] private int level = 1;
    [SerializeField] private int exp = 0;

    private readonly int[] levelThresholds = { 0, 100, 300, 700, 1500 };


    public bool AddExp(int amount)
    {
        exp += amount;
        bool leveledUp = false;

        while (level < levelThresholds.Length && exp >= levelThresholds[level])
        {
            level++;
            leveledUp = true;
        }

        return leveledUp;
    }

    public int GetLevel() => level;

    public int GetEXP() => exp;
}

public class TotalLife { }

public class LifeStatsManager
{
    private Dictionary<Type, LifeStatData> lifeStats = new ();
    private readonly Dictionary<Type, float> weights = new ()
    {
        { typeof(CollectInteractComponent), 1f },
    };

    public LifeStatsManager()
    {
        lifeStats[typeof(CollectInteractComponent)] = new LifeStatData();
        lifeStats[typeof(TotalLife)] = new LifeStatData();

    }

    public void AddExp<T>(int amount)
    {
        var type = typeof(T);
        if (!lifeStats.ContainsKey(type)) return;

        bool levelUp = lifeStats[type].AddExp(amount);
        if (levelUp)
        {
            Debug.Log($"{GetType()} : {type} 숙련도 레벨업! {GetLevel<T>() - 1} -> {GetLevel<T>()}");
        }

        Test(type, amount, levelUp);
    }

    public int GetLevel<T>() => lifeStats[typeof(T)].GetLevel();

    public int GetEXP<T>() => lifeStats[typeof(T)].GetEXP();

    private void Test(Type type, int amount, bool levelUp)
    {
        int totalLevel = GetLevel<TotalLife>();

        // 레벨이 같지 않으면 다른 숙련도를 올려야 생활력이 올라간다.
        // 하지만 방금 막 레벨업을 한 경우는 경험치를 집계한다.
        if (totalLevel != lifeStats[type].GetLevel() && !(levelUp && (totalLevel == lifeStats[type].GetLevel() - 1))) 
            return;

        if (!weights.ContainsKey(type))
        {
            Debug.LogError($"{GetType()} : not exit type({type})");
            return;
        }

        int addExp = Mathf.RoundToInt(weights[type] * amount);
        bool _levelUp = lifeStats[typeof(TotalLife)].AddExp(addExp);

        if (_levelUp)
        {
            Debug.Log($"{GetType()} : 생활력 레벨업! {GetLevel<TotalLife>() - 1} -> {GetLevel<TotalLife>()}");
        }
    }
}
