using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SODataTableEditor : EditorWindow
{
    private ScriptableObject targetSO;
    private Vector2 scrollPos;
    private FieldInfo rowsField;
    private IList rowsList;
    private Type rowType;

    [MenuItem("Tools/SO DataTable Editor")]
    public static void Init()
    {
        GetWindow<SODataTableEditor>("SO Data Table");
    }

    void OnGUI()
    {
        DrawTargetSelector();

        if (targetSO != null && rowsList != null && rowType != null)
        {
            DrawToolbar();
            GUILayout.Space(5);
            DrawTable();
        }
    }

    #region Target Selection
     void DrawTargetSelector()
    {
        var newSO = (ScriptableObject)EditorGUILayout.ObjectField("Target SO", targetSO, typeof(ScriptableObject), false);

        if (newSO != targetSO)
        {
            targetSO = newSO;
            if (targetSO != null)
            {
                FindRowsField();
                InitializeRowsList();
                DetermineRowType();
            }
        }
    }

     void FindRowsField()
    {
        rowsField = targetSO.GetType().GetField("rows", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (rowsField == null)
        {
            foreach (var f in targetSO.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (typeof(IList).IsAssignableFrom(f.FieldType) && f.GetCustomAttribute<SerializeField>() != null)
                {
                    rowsField = f;
                    break;
                }
            }
        }
    }

     void InitializeRowsList()
    {
        if (rowsField == null) return;

        rowsList = rowsField.GetValue(targetSO) as IList;
        if (rowsList == null)
        {
            var listType = rowsField.FieldType;
            rowsList = (IList)Activator.CreateInstance(listType);
            rowsField.SetValue(targetSO, rowsList);
        }
    }

     void DetermineRowType()
    {
        if (rowsList == null) return;

        if (rowsList.Count > 0)
        {
            rowType = rowsList[0].GetType();
        }
        else
        {
            if (rowsField.FieldType.IsArray)
                rowType = rowsField.FieldType.GetElementType();
            else if (rowsField.FieldType.IsGenericType)
                rowType = rowsField.FieldType.GetGenericArguments()[0];
        }
    }
    #endregion

    #region Toolbar
    //버튼 제작 
    void DrawToolbar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (GUILayout.Button("Load CSV", EditorStyles.toolbarButton))
            LoadCsv();

        if (GUILayout.Button("Save CSV", EditorStyles.toolbarButton))
            SaveCsv();

        if (GUILayout.Button("+ Add Row", EditorStyles.toolbarButton))
            AddRow();

        if (GUILayout.Button("Save SO", EditorStyles.toolbarButton))
            SaveSO();

        if (GUILayout.Button("Load JSON", EditorStyles.toolbarButton))
            LoadJson();

        if (GUILayout.Button("Save JSON", EditorStyles.toolbarButton))
            SaveJson();

        EditorGUILayout.EndHorizontal();
    }
     void LoadJson()
    {
        string path = EditorUtility.OpenFilePanel("Load JSON", Application.dataPath, "json");
        if (string.IsNullOrEmpty(path)) return;

        string json = File.ReadAllText(path);

      
        var wrapperType = typeof(ListWrapper<>).MakeGenericType(rowType);
        var wrapper = JsonUtility.FromJson(json, wrapperType);

        var dataField = wrapperType.GetField("items");
        var loadedList = dataField.GetValue(wrapper) as IList;

        rowsList.Clear();
        foreach (var item in loadedList)
            rowsList.Add(item);
    }

    void SaveJson()
    {
        string path = EditorUtility.SaveFilePanel(
         "Save JSON",
         Application.dataPath,                
         targetSO.name + ".json",
         "json"
         );
        if (string.IsNullOrEmpty(path)) return;

        var wrapperType = typeof(ListWrapper<>).MakeGenericType(rowType);
        var wrapper = Activator.CreateInstance(wrapperType);

        var dataField = wrapperType.GetField("items");
        dataField.SetValue(wrapper, rowsList);

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(path, json);
    }

 
   
     void LoadCsv()
    {
        string path = EditorUtility.OpenFilePanel("Load CSV", Application.dataPath, "csv");
        if (string.IsNullOrEmpty(path)) return;

        var method = typeof(CsvLoader).GetMethod("LoadCsv").MakeGenericMethod(rowType);
        var loadedList = method.Invoke(null, new object[] { path }) as IList;

        rowsList.Clear();
        foreach (var item in loadedList)
            rowsList.Add(item);
    }

     void SaveCsv()
    {
        string path = EditorUtility.SaveFilePanel("Save CSV", 
            Application.dataPath,
            targetSO.name + ".csv", 
            "csv");
        if (string.IsNullOrEmpty(path)) return;

        var method = typeof(CsvLoader).GetMethod("SaveCsv").MakeGenericMethod(rowType);
        method.Invoke(null, new object[] { path, rowsList });
    }

    void AddRow() { rowsList.Add(Activator.CreateInstance(rowType)); }
     void SaveSO()
    {
        EditorUtility.SetDirty(targetSO);
        AssetDatabase.SaveAssets();
    }
    #endregion

    #region Table
     void DrawTable()
    {
        if (rowsList == null || rowType == null) return;

        var fields = rowType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        DrawTableHeader(fields);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        int removeIndex = DrawTableRows(fields);
        if (removeIndex >= 0) rowsList.RemoveAt(removeIndex);
        EditorGUILayout.EndScrollView();
    }

    void DrawTableHeader(FieldInfo[] fields)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("ID", EditorStyles.boldLabel, GUILayout.Width(30));
        foreach (var f in fields)
        {
            if (!f.IsPublic && f.GetCustomAttribute<SerializeField>() == null)
                continue;
            GUILayout.Label(f.Name, EditorStyles.boldLabel, GUILayout.Width(150));
        }
        GUILayout.Label("Delete", EditorStyles.boldLabel, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();
    }

    int DrawTableRows(FieldInfo[] fields)
    {
        int removeIndex = -1;
        for (int i = 0; i < rowsList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(i.ToString(), GUILayout.Width(30));

            var row = rowsList[i];
            foreach (var f in fields)
            {
                if (!f.IsPublic && f.GetCustomAttribute<SerializeField>() == null)
                    continue;

                object value = f.GetValue(row);
                object newValue = DrawFieldCell(value, f.FieldType, 150);
                if (!Equals(value, newValue))
                {
                    f.SetValue(row, newValue);
                }
            }

            if (GUILayout.Button("X", GUILayout.Width(50)))
                removeIndex = i;

            EditorGUILayout.EndHorizontal();
        }
        return removeIndex;
    }
    #endregion

    #region Field Drawing
    private object DrawFieldCell(object value, Type type, float width)
    {
        if (type == typeof(int))
            return EditorGUILayout.IntField((int)(value ?? 0), GUILayout.Width(width));
        if (type == typeof(float))
            return EditorGUILayout.FloatField((float)(value ?? 0f), GUILayout.Width(width));
        if (type == typeof(string))
            return EditorGUILayout.TextField((string)(value ?? ""), GUILayout.Width(width));
        if (type == typeof(bool))
            return EditorGUILayout.Toggle((bool)(value ?? false), GUILayout.Width(width));
        if (type.IsEnum)
            return EditorGUILayout.EnumPopup((Enum)(value ?? Activator.CreateInstance(type)), GUILayout.Width(width));
        if (typeof(UnityEngine.Object).IsAssignableFrom(type))
            return EditorGUILayout.ObjectField((UnityEngine.Object)value, type, false, GUILayout.Width(width));

      
        if (!type.IsPrimitive && !type.IsEnum && !typeof(UnityEngine.Object).IsAssignableFrom(type))
            return DrawNestedObject(value, type, width);

        GUILayout.Label($"(Unsupported)", GUILayout.Width(width));
        return value;
    }

    object DrawNestedObject(object value, Type type, float width)
    {
        if (value == null)
            value = Activator.CreateInstance(type);

        EditorGUILayout.BeginVertical(GUILayout.Width(width));
        var nestedFields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var nf in nestedFields)
        {
            if (!nf.IsPublic && nf.GetCustomAttribute<SerializeField>() == null)
                continue;

            object nestedValue = nf.GetValue(value);
            object newNestedValue = DrawFieldCell(nestedValue, nf.FieldType, width - 10);
            if (!Equals(nestedValue, newNestedValue))
                nf.SetValue(value, newNestedValue);
        }
        EditorGUILayout.EndVertical();
        return value;
    }
    [Serializable]
     class ListWrapper<T>
    {
        public List<T> items;
    }
    #endregion
}
