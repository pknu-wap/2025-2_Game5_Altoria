using System;
using System.Collections.Generic;
using UnityEngine;
using CustomEditor;
using GameInteract;

[Serializable]
public class LifeStatData
{
    [SerializeField] int level = 1;
    [SerializeField] int exp = 0;

    readonly List<int> levelThresholds = CsvLoader.LoadCsv<int>($"{Application.dataPath}/CSV/LevelThresholds.csv");

    public LifeStatData() { }

    public LifeStatData(int level, int exp)
    {
        this.level = level;
        this.exp = exp;
    }

    public bool AddExp(int amount)
    {
        exp += amount;
        bool leveledUp = false;

        while (level < levelThresholds.Count && exp >= levelThresholds[level])
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
    Dictionary<string, LifeStatData> lifeStats;
    readonly Dictionary<string, float> weights = new ()
    {
        { nameof(CollectInteractComponent), 0.7f },
        { nameof(UpgradeInteractComponent), 0.3f },
    };

    public LifeStatsManager()
    {
        lifeStats = new Dictionary<string, LifeStatData>()
{
    { nameof(CollectInteractComponent), new ()},
    { nameof(UpgradeInteractComponent), new ()},
    { nameof(TotalLife), new ()},
};
    }

    void SetData()
    {
        var userData = Manager.UserData.GetUserData<UserLifeData>();
        if (userData == null)
        {
            Debug.LogError("[LifeStatsManager] UserData not initialized!");
            return;
        }

        lifeStats = userData.GetUserLifeData();
        Debug.Log($"[LifeStatsManager] Loaded {lifeStats.Count} life stats.");
    }

    public void AddExp<T>(int amount)
    {
        var type = nameof(T);
        if (!lifeStats.ContainsKey(type)) return;

        bool levelUp = lifeStats[type].AddExp(amount);
        if (levelUp)
        {
            Debug.Log($"{GetType()} : {type} 숙련도 레벨업! {GetLevel<T>() - 1} -> {GetLevel<T>()}");
        }

        SetTotalStat(type, amount, levelUp);
    }

    public int GetLevel<T>() => lifeStats[nameof(T)].GetLevel();

    public int GetEXP<T>() => lifeStats[nameof(T)].GetEXP();

    void SetTotalStat(string type, int amount, bool levelUp)
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
        bool _levelUp = lifeStats[nameof(TotalLife)].AddExp(addExp);

        if (_levelUp)
        {
            Debug.Log($"{GetType()} : 생활력 레벨업! {GetLevel<TotalLife>() - 1} -> {GetLevel<TotalLife>()}");
        }
    }

    public Dictionary<string, LifeStatData> GetLifeStats() => lifeStats;
}
