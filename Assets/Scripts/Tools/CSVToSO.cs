using System.Collections.Generic;
using System.IO;
using System;
using UnityEditor;
using UnityEngine;

namespace CSVToSO
{
    public class CSVToSO
    {
        private const string COLLECT_PATH = "Assets/ScriptableObject/Collect";

        [MenuItem("Tools/CSVToSO/Generate CollectSO")]
        public static void GenerateCollectSO()
        {
            string path = Path.Combine(Application.dataPath, "CSV/CollectDatabase.csv");
            string[] csvText = File.ReadAllLines(path);

            Dictionary<string, List<CollectData>> sos = new();

            for (var i = 1; i < csvText.Length; i++)
            {
                string[] datas = csvText[i].Split(',');

                CollectData item = new();

                item.ID = datas[0].ToString();
                item.Count = datas[1].ToString();
                item.Probability = Convert.ToInt32(datas[2]);

                if (!sos.ContainsKey(item.ID))
                    sos[item.ID] = new List<CollectData>();
                sos[item.ID].Add(item);
            }

            foreach(var data in sos)
            {
                CollectDataSO so = ScriptableObject.CreateInstance<CollectDataSO>();
                so.rows = new List<CollectData>();
                so.rows.AddRange(data.Value);
                AssetDatabase.CreateAsset(so, $"{COLLECT_PATH}/Collect{Convert.ToInt32(data.Key) % 100}.asset");
                AssetDatabase.SaveAssets();
            }
        }

        [MenuItem("Tools/CSVToSO/Generate FishSO")]
        public static void GenerateFishSO()
        {
            string path = Path.Combine(Application.dataPath, "CSV/FishSO.csv");
            string[] csvText = File.ReadAllLines(path);

            Dictionary<string, List<FishData>> sos = new();

            for (var i = 1; i < csvText.Length; i++)
            {
                string[] datas = csvText[i].Split(',');

                FishData item = new();

                item.ID = datas[0].ToString();
                item.Area = datas[2].ToString();
                item.Probability = Convert.ToInt32(datas[3]);

                if (!sos.ContainsKey(item.Area))
                    sos[item.Area] = new List<FishData>();
                sos[item.Area].Add(item);
            }

            for(int i = 0; i < sos.Count; i++)
            {
                FishDataSO so = ScriptableObject.CreateInstance<FishDataSO>();
                so.rows = new List<FishData>();
                so.rows = sos[(i+1).ToString()];
                AssetDatabase.CreateAsset(so, $"{COLLECT_PATH}/Fish/Fish{(Define.AreaType)(i+1)}.asset");
                AssetDatabase.SaveAssets();
            }
        }
    }
}


