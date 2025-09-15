using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

public static class CsvLoader
{
  
    public static List<T> LoadCsv<T>(string fullPath) where T : new()
    {
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"CSV file not found: {fullPath}");
            return new List<T>();
        }
        string csvText = File.ReadAllText(fullPath);
        return ParseCsvText<T>(csvText);
    }

    
    public static List<T> ParseCsvText<T>(string csvText) where T : new()
    {
        var lines = csvText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length <= 1)
            return new List<T>();

        var headers = lines[0].Split(',');
        var list = new List<T>();

        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            var values = line.Split(',');
            var obj = new T();

            for (int headerIndex = 0; headerIndex < headers.Length; headerIndex++)
            {
                string header = headers[headerIndex].Trim();
                string value = values.Length > headerIndex ? values[headerIndex].Trim() : string.Empty;

                var field = typeof(T).GetField(header, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                {
                    try
                    {
                        object convertedValue = ConvertValue(field.FieldType, value);
                        field.SetValue(obj, convertedValue);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"CSV Parse Error: {header} - {value} - {ex.Message}");
                    }
                }
            }
            list.Add(obj);
        }
        return list;
    }
    public static void SaveCsv<T>(string fullPath, IList<T> dataList)
    {
        if (dataList == null || dataList.Count == 0)
        {
            Debug.LogWarning("No data to save.");
            return;
        }

        var sb = new StringBuilder();

        // 필드 목록 가져오기
        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        // CSV 헤더 작성
        for (int i = 0; i < fields.Length; i++)
        {
            if (!fields[i].IsPublic && fields[i].GetCustomAttribute<SerializeField>() == null)
                continue;

            sb.Append(fields[i].Name);
            if (i < fields.Length - 1) sb.Append(",");
        }
        sb.AppendLine();

        // 데이터 작성
        foreach (var item in dataList)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (!fields[i].IsPublic && fields[i].GetCustomAttribute<SerializeField>() == null)
                    continue;

                object value = fields[i].GetValue(item);
                string strValue = value != null ? value.ToString() : string.Empty;

                // 값에 콤마, 줄바꿈이 있으면 따옴표로 감싸기
                if (strValue.Contains(",") || strValue.Contains("\n") || strValue.Contains("\r"))
                {
                    strValue = $"\"{strValue.Replace("\"", "\"\"")}\"";
                }

                sb.Append(strValue);
                if (i < fields.Length - 1) sb.Append(",");
            }
            sb.AppendLine();
        }

        // 디렉토리 생성 후 파일 저장
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        File.WriteAllText(fullPath, sb.ToString(), Encoding.UTF8);

        Debug.Log($"CSV saved: {fullPath}");
    }


    private static object ConvertValue(Type type, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            if (type.IsValueType) return Activator.CreateInstance(type);
            return null;
        }

        if (type.IsEnum) return Enum.Parse(type, value, true);
        if (type == typeof(int)) return int.Parse(value);
        if (type == typeof(float)) return float.Parse(value);
        if (type == typeof(bool)) return bool.Parse(value);
        if (type == typeof(string)) return value;
        return null;
    }
}
