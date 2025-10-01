using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CSVToSO
{
    private const string COLLECT_SO_PATH = "Assets/Scripts/Interact/Collect";

    [MenuItem("Tools/CSVToSO/Generate CollectSO")]
    public static void GenerateCollectSO()
    {
        string path = Path.Combine(Application.dataPath, "CSV/collect_test.csv");
        string[] csvText = File.ReadAllLines(path);

        Dictionary<int, List<CollectItem>> sos = new ();

        for (var i = 1; i < csvText.Length; i++)
        {
            string[] datas = csvText[i].Split(',');

            CollectItem item = new();

            item.areaID = Convert.ToInt32(datas[0]);
            item.itemID = Convert.ToInt32(datas[1]);
            item.baseProbability = Convert.ToInt32(datas[2]);

            if (!sos.ContainsKey(item.areaID))
                sos[item.areaID] = new List<CollectItem>();
            sos[item.areaID].Add(item);
        }

        for (int id = 1; id <= sos.Count; id++)
        {
            CollectInteractSO so = ScriptableObject.CreateInstance<CollectInteractSO>();
            so.areaID = id;
            so.drops = sos[id];
            so.collectType = Define.CollectType.Fish;
            AssetDatabase.CreateAsset(so, $"{COLLECT_SO_PATH}/Collect{id}.asset");
            AssetDatabase.SaveAssets();
        }

    }
}
