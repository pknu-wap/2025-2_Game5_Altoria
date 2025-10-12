using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CSVToSO
{
    private const string COLLECT_SO_PATH = "Assets/Scripts/Interact/Collect";

    [MenuItem("Tools/CSVToSO/Generate Fish CollectSO")]
    public static void GenerateFishCollectSO()
    {
        string path = Path.Combine(Application.dataPath, "CSV/collect_test.csv");
        string[] csvText = File.ReadAllLines(path);

        Dictionary<string, List<CollectItem>> sos = new ();

        for (var i = 1; i < csvText.Length; i++)
        {
            string[] datas = csvText[i].Split(',');

            CollectItem item = new();

            item.itemID = datas[0].ToString();
            item.content = datas[1].ToString();
            item.areaID = datas[3].ToString();
            item.baseProbability = Convert.ToInt32(datas[4]);

            
            if (!sos.ContainsKey(item.areaID))
                sos[item.areaID] = new List<CollectItem>();
            sos[item.areaID].Add(item);
        }

        for (int id = 1; id <= 5; id++)
        {
            CollectInteractSO so = ScriptableObject.CreateInstance<CollectInteractSO>();
            so.areaID = id.ToString();
            so.drops = sos[id.ToString()];
            so.collectType = Define.CollectType.Fish;
            AssetDatabase.CreateAsset(so, $"{COLLECT_SO_PATH}/Fish{id}.asset");
            AssetDatabase.SaveAssets();
        }
    }

    [MenuItem("Tools/CSVToSO/Generate CollectSO")]
    public static void GenerateCollectSO()
    {
        string path = Path.Combine(Application.dataPath, "CSV/collect_test.csv");
        string[] csvText = File.ReadAllLines(path);

        Dictionary<string, List<CollectItem>> sos = new();

        for (var i = 1; i < csvText.Length; i++)
        {
            string[] datas = csvText[i].Split(',');

            CollectItem item = new();

            item.itemID = datas[0].ToString();
            item.areaID = datas[3].ToString();
            item.baseProbability = Convert.ToInt32(datas[4]);

            if (!sos.ContainsKey(item.itemID))
                sos[item.itemID] = new List<CollectItem>();
            sos[item.itemID].Add(item);
        }

        for (int id = 1; id <= sos.Count; id++)
        {
            CollectInteractSO so = ScriptableObject.CreateInstance<CollectInteractSO>();
            so.areaID = id.ToString();
            so.drops = sos[id.ToString()];
            so.collectType = Define.CollectType.Fish;
            AssetDatabase.CreateAsset(so, $"{COLLECT_SO_PATH}/Collect{id}.asset");
            AssetDatabase.SaveAssets();
        }

    }
}
