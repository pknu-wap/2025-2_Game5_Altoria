using System;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public Vector3 PlayerTransform;
    public Quaternion Rotation;
    public float Stemina;
    public Define.SceneType LastScene;

    public PlayerData()
    {
        PlayerTransform = Vector3.zero;
        Rotation = Quaternion.identity;
        Stemina = 100f;
        LastScene = Define.SceneType.None; // TODO: 최초 시작 마을로 변경
    }
}

public class UserPlayerData : Security, IUserData
{
    string path = Path.Combine(Application.dataPath, "playerData.json");
    PlayerData userPlayerData;

    public PlayerData GetPlayerData() => userPlayerData;

    public void SetDefaultData()
    {
        userPlayerData = new ();
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
