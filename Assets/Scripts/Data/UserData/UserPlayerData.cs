using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Define;

[Serializable]
public class CustomData
{
    public int Type;
    public string Id;

    public CustomData(int type, string id)
    {
        this.Type = type;
        this.Id = id;
    }
}

[Serializable]
public class WrapperClassCustomDataList
{
    public List<CustomData> UserCustomizingData;
}

[Serializable]
public class ColorData
{
    public int Index;
    public string Name;
    public Color Color;

    public ColorData(string name, int index)
    {
        Name = name;
        Color = Color.white;
        Index = index;
    }

    public ColorData(string name, int index, Color color)
    {
        Name = name;
        Color = color;
        Index = index;
    }
}

[Serializable]
public class WrapperColorData
{
    public List<ColorData> UserColorData;
}

[Serializable]
public class PlayerData
{
    public Vector3 PlayerTransform;
    public Quaternion Rotation;
    public float Time;
    public float Stemina;
    public int Gender;
    public bool Customed;
    public bool FirstGift;
    public bool RedTreasure;

    public PlayerData()
    {
        PlayerTransform = new Vector3(-35.0f, 4.0f, -10.01f);
        Rotation = Quaternion.identity;
        Time = 7f;
        Stemina = 100f;
        Gender = 0;
        Customed = false;
        FirstGift = false;
        RedTreasure = false;
    }
}

public class UserPlayerData : Security, IUserData
{
    string path = Path.Combine(Application.dataPath, "Data/playerData.json");
    string custom_path = Path.Combine(Application.dataPath, "Data/customData.json");
    string color_path = Path.Combine(Application.dataPath, "Data/colorData.json");

    PlayerData userPlayerData;
    List<CustomData> userCustomizingData;
    List<ColorData> userColorData;

    #region player
    public PlayerData GetPlayerData() => userPlayerData;


    public void SetTime(float time)
    {
        userPlayerData.Time = time;
        PlayerSaveData();
    }
    public float GetTime() => userPlayerData.Time;


    public void SetPlayerGPS(Vector3 position, Quaternion quaternion)
    {
        userPlayerData.PlayerTransform = position;
        userPlayerData.Rotation = quaternion;
        PlayerSaveData();
    }


    public Vector3 GetPlayerPosition() => userPlayerData.PlayerTransform;
    public Quaternion GetPlayerQuaternion() => userPlayerData.Rotation;


    public void SetGender(int value) => userPlayerData.Gender = value;
    public int GetGender() => userPlayerData.Gender;


    public void SetCustomed() => userPlayerData.Customed = true;
    public bool GetCustomed() => userPlayerData.Customed;


    public void SetFirstGift()
    {
        userPlayerData.FirstGift = true;
        PlayerSaveData();
    }
    public bool GetFirstGift() => userPlayerData.FirstGift;


    public void SetRedTreasure()
    {
        userPlayerData.RedTreasure = true;
        PlayerSaveData();
    }
    public bool GetRedTreasure() => userPlayerData.RedTreasure;
    #endregion

    #region Custom
    public string GetID(CustomizationType type)
    {
        for(int i = 0; i < userCustomizingData.Count; i++)
        {
            if(userCustomizingData[i].Type == (int)type)
                return userCustomizingData[i].Id;
        }
        return null;
    }
    public void SetID(CustomizationType type, string id)
    {
        bool found = false;

        for (int i = 0; i < userCustomizingData.Count; i++)
        {
            if (userCustomizingData[i].Type == (int)type)
            {
                userCustomizingData[i].Id = id;
                found = true;
                break;
            }
        }

        if (!found)
        {
            userCustomizingData.Add(new((int)type, id));
        }

        CustomSaveData();
    }
    #endregion

    #region Color

    public void SetColor(Color color, int index)
    {
        userColorData[index].Color = color;
        ColorSaveData();
    }

    public ColorData GetColor(int index) => userColorData[index];

    #endregion

    #region All Data
    public void SetDefaultData()
    {
        PlayerSetDefaultData();
        CustomSetDefaultData();
        ColorSetDefaultData();
    }

    public bool LoadData()
    {
        var playerResult = PlayerLoadData();
        var customResult = CustomLoadData();
        var colorResult = ColorLoadData();

        return playerResult && customResult && colorResult;
    }

    public bool SaveData()
    {
        var playerResult = PlayerSaveData();
        var customResult = CustomSaveData();
        var colorResult = ColorSaveData();

        return playerResult && customResult && colorResult;
    }
    #endregion

    #region Player Data
    public void PlayerSetDefaultData()
    {
        userPlayerData = new();
    }

    public bool PlayerLoadData()
    {
        bool result = false;

        try
        {
            if (!File.Exists(path)) // Create
            {
                PlayerSetDefaultData();
            }
            else // Load
            {
                string loadJson = File.ReadAllText(path);
                userPlayerData = JsonUtility.FromJson<PlayerData>(Decrypt(loadJson, KEY));
            }

            result = true;
        }
        catch (Exception e)
        {
            Debug.Log($"{GetType()} : Load failed ({e.Message})");
        }

        return result;
    }

    public bool PlayerSaveData()
    {
        bool result = false;

        try
        {
            string jsonData = JsonUtility.ToJson(userPlayerData);
            File.WriteAllText(path, Encrypt(jsonData, KEY));

            result = true;
        }
        catch (Exception e)
        {
            Debug.Log($"{GetType()} : Save failed ({e.Message})");
        }

        return result;
    }
    #endregion

    #region Custom Data
    public void CustomSetDefaultData()
    {
        userCustomizingData = new()
        {
            new (0, "Male[M_eyebrows0]"),
            new (1, "Male[M_eyes0]"),
            new (2, "Male[M_mouth0]"),
            new (3, "Male[facialHair_1]"),
            new (4, "Male[M_hair_1]"),
        };
    }

    public bool CustomLoadData()
    {
        bool result = false;

        try
        {
            if (!File.Exists(custom_path)) // Create
            {
                CustomSetDefaultData();
            }
            else // Load
            {
                string loadJson = File.ReadAllText(custom_path);
                var wrapper = JsonUtility.FromJson<WrapperClassCustomDataList>(Decrypt(loadJson, KEY));
                userCustomizingData = wrapper.UserCustomizingData;
            }

            result = true;
        }
        catch (Exception e)
        {
            Debug.Log($"{GetType()} : Load failed ({e.Message})");
        }

        return result;
    }

    public bool CustomSaveData()
    {
        bool result = false;

        try
        {
            WrapperClassCustomDataList wrapper = new();
            wrapper.UserCustomizingData = userCustomizingData;
            string jsonData = JsonUtility.ToJson(wrapper);
            File.WriteAllText(custom_path, Encrypt(jsonData, KEY));
            result = true;
        }
        catch (Exception e)
        {
            Debug.Log($"{GetType()} : Save failed ({e.Message})");
        }

        return result;
    }
    #endregion

    #region Color Data
    public void ColorSetDefaultData()
    {
        userColorData = new()
        {
            new("_Color1", 0, Color.black),
            new("_Color2", 1),
            new("_CorneaColor", 2),
            new("_LipColor", 3),
            new("_Color2", 4),
            new("_Color2", 5)
        };               
    }                    
                         
    public bool ColorLoadData()
    {
        bool result = false;

        try
        {
            if (!File.Exists(color_path)) // Create
            {
                ColorSetDefaultData();
            }
            else // Load
            {
                string loadJson = File.ReadAllText(color_path);
                var wrapper = JsonUtility.FromJson<WrapperColorData>(Decrypt(loadJson, KEY));
                userColorData = wrapper.UserColorData;
            }

            result = true;
        }
        catch (Exception e)
        {
            Debug.Log($"{GetType()} : Load failed ({e.Message})");
        }

        return result;
    }

    public bool ColorSaveData()
    {
        bool result = false;

        try
        {
            WrapperColorData wrapper = new();
            wrapper.UserColorData = userColorData;
            string jsonData = JsonUtility.ToJson(wrapper);
            File.WriteAllText(color_path, Encrypt(jsonData, KEY));
            result = true;
        }
        catch (Exception e)
        {
            Debug.Log($"{GetType()} : Save failed ({e.Message})");
        }

        return result;
    }
    #endregion
}
