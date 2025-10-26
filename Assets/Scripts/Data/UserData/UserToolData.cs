using System.IO;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Overlays;

[Serializable]
public class ToolsDataDictionary
{
    public SerializableDictionary<string, int> ToolsData;
}

public class UserToolData : Security, IUserData
{
    string path = Path.Combine(Application.dataPath, "toolsData.json");

    Dictionary<string, int> userToolsDataDic;

    public int? GetToolStep(string id) => userToolsDataDic.TryGetValue(id, out var data) ? data : null;
    // 추후 새로운 key에 대해 value를 넣는 작업을 인덱스로 대입해도 추가가 되는지 확인 필요(2025.10.25_미확인)
    public void SetToolSteop(string id, int step) => userToolsDataDic[id] = step;

    public void SetDefaultData()
    {
        // TEST
        userToolsDataDic = new()
        {
            {"11", 11 },
            {"22", 12 },
            {"33", 110 }
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
                var loadData = JsonUtility.FromJson<ToolsDataDictionary>(loadJson);
                //var loadData = JsonUtility.FromJson<ToolsDataDictionary>(Decrypt(loadJson, KEY));
                userToolsDataDic = loadData.ToolsData.ToDictionary();
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

            var saveData = new ToolsDataDictionary { ToolsData = new SerializableDictionary<string, int>(userToolsDataDic) };
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
}
