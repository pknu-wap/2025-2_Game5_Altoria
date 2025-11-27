using CustomEditor;
using GameInteract;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class LifeStatData
{
    public string LifeType;
    public int Level;
    public int Exp;

    public LifeStatData(string type, int level, int exp)
    {
        LifeType = type;
        Level = level;
        Exp = exp;
    }
}

[Serializable]
public class WrapperLifeStatDataList
{
    public List<LifeStatData> UserLifeData;
}

public class TotalLife { }

public class UserLifeData : Security, IUserData
{
    public Action OnSaveAction;

    string path = Path.Combine(Application.persistentDataPath, "lifeData.json");
    
    List<LifeStatData> userLifeData;

    public void SetDefaultData()
    {
        userLifeData = new ()
        {
            new(nameof(CollectInteractComponent), 0, 0),
            new(nameof(UpgradeInteractComponent), 0, 0),
            new(nameof(CraftInteractComponent), 0, 0),
            new(nameof(TotalLife), 0, 0),
        };
    }

    public bool LoadData()
    {
        bool result = false;

        try
        {
            if (!File.Exists(path)) // Create
            {
                SetDefaultData();
            }
            else // Load
            {
                string loadJson = File.ReadAllText(path);
                var wrapperData = JsonUtility.FromJson<WrapperLifeStatDataList>(Decrypt(loadJson, KEY));
                userLifeData = wrapperData.UserLifeData;
            }

            result = true;
        }
        catch (Exception e)
        {
            Debug.Log($"{GetType()} : Load failed ({e.Message})");
        }

        return result;
    }

    public bool SaveData()
    {
        bool result = false;

        try
        {
            var wrapperData = new WrapperLifeStatDataList();
            wrapperData.UserLifeData = userLifeData;
            string jsonData = JsonUtility.ToJson(wrapperData);
            File.WriteAllText(path, Encrypt(jsonData, KEY));

            result = true;
        }
        catch (Exception e)
        {
            Debug.Log($"{GetType()} : Save failed ({e.Message})");
        }

        return result;
    }

    public List<LifeStatData> GetUserLifeData() => userLifeData;

    public void SetSaveDataAndSave(List<LifeStatData> dataList) => userLifeData = dataList;
}
