using Common;
using GameInteract;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class LifeData
{
    public int Level;
    public int Exp;

    public LifeData()
    {
        Level = 1;
        Exp = 0;
    }

    public LifeData(int level, int exp)
    {
        this.Level = level;
        this.Exp = exp;
    }
}

[Serializable]
public class WrapperUserLifeData
{
    public List<LifeData> userLifeData;
}

public class UserLifeData : Security, IUserData
{
    string path = Path.Combine(Application.dataPath, "lifeData.json");
    List<LifeData> userLifeData;

    public void SetDefaultData()
    {
        userLifeData = new List<LifeData>()
        {
            (new ()),
            (new ()),
            (new ())
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
                var wrapperUserLifeData = JsonUtility.FromJson<WrapperUserLifeData>(loadJson);
                // var wrapperUserLifeData = JsonUtility.FromJson<WrapperUserLifeData>(Decrypt(loadJson, KEY));
                userLifeData = wrapperUserLifeData.userLifeData;
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

        var lifeStatList = GameSystem.Life.GetLifeStats();
        List<Type> keys = new List<Type>(lifeStatList.Keys);

        userLifeData.Clear();
        for (int i = 0; i < lifeStatList.Count; i++)
        {
            userLifeData.Add(new LifeData(lifeStatList[keys[i]].GetLevel(), lifeStatList[keys[i]].GetEXP()));
        }

        try
        {
            var wrapperUserLifeData = new WrapperUserLifeData();
            wrapperUserLifeData.userLifeData = userLifeData;
            string jsonData = JsonUtility.ToJson(wrapperUserLifeData);
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

    public List<LifeData> GetUserLifeData() => userLifeData;
}
