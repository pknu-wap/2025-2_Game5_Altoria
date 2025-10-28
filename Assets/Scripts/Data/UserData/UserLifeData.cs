using Common;
using GameInteract;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class LifeDataDictionary
{
    public SerializableDictionary<Type, LifeStatData> LifeData;
}


public class UserLifeData : Security, IUserData
{
    string path = Path.Combine(Application.dataPath, "lifeData.json");
    
    Dictionary<Type, LifeStatData> userLifeDataDic;

    public void SetDefaultData()
    {
        userLifeDataDic = new ()
        {
            { typeof(CollectInteractComponent), new ()},
            { typeof(UpgradeInteractComponent), new ()},
            { typeof(TotalLife), new ()}
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
            var saveData = new LifeDataDictionary { LifeData = new (userLifeDataDic) };
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

    public Dictionary<Type, LifeStatData> GetUserLifeData() => userLifeDataDic;
}
