using Common;
using GameInteract;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class LifeDataDictionary
{
    public SerializableDictionary<string, LifeStatData> LifeData;
}


public class UserLifeData : Security, IUserData
{
    string path = Path.Combine(Application.dataPath, "lifeData.json");
    
    Dictionary<string, LifeStatData> userLifeDataDic;

    public void SetDefaultData()
    {
        userLifeDataDic = new ()
        {
            { nameof(CollectInteractComponent), new ()},
            { nameof(UpgradeInteractComponent), new ()},
            { nameof(TotalLife), new ()}
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
                var loadData = JsonUtility.FromJson<LifeDataDictionary>(loadJson);
                // var loadData = JsonUtility.FromJson<LifeDataDictionary>(Decrypt(loadJson, KEY));
                userLifeDataDic = loadData.LifeData.ToDictionary();
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
            var saveData = new LifeDataDictionary { LifeData = new (GameSystem.Life.GetLifeStats()) };
            string jsonData = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(path, jsonData);
            //File.WriteAllText(path, Encrypt(jsonData, KEY));

            result = true;
        }
        catch (Exception e)
        {
            Debug.Log($"{GetType()} : Save failed ({e.Message})");
        }

        return result;
    }

    public Dictionary<string, LifeStatData> GetUserLifeData() => userLifeDataDic;
}
