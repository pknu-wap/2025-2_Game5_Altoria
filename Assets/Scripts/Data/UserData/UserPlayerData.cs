using System;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public Vector3 PlayerTransform;
    public Quaternion Rotation;
    public float Stemina;
    public float time;

    public PlayerData()
    {
        PlayerTransform = Vector3.zero;
        Rotation = Quaternion.identity;
        Stemina = 100f;
        time = 7f;
    }
}

public class UserPlayerData : Security, IUserData
{
    string path = Path.Combine(Application.dataPath, "playerData.json");
    PlayerData userPlayerData;

    public PlayerData GetPlayerData() => userPlayerData;
    public void SetDataTime(float time) => userPlayerData.time = time;
    public void SetDataPlayerPosition(Vector3 position, Quaternion quaternion)
    {
        userPlayerData.PlayerTransform = position;
        userPlayerData.Rotation = quaternion;
    }
    public void SetDataStemina(float stemina) => userPlayerData.Stemina = stemina;

    public void SetDefaultData()
    {
        userPlayerData = new();
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
                userPlayerData = JsonUtility.FromJson<PlayerData>(loadJson);
                //playerData = JsonUtility.FromJson<PlayerData>(Decrypt(loadJson, KEY));
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
            string jsonData = JsonUtility.ToJson(userPlayerData);
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
}
