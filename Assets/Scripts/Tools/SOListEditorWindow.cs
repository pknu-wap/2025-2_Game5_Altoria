using System.Reflection;
using System;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class SOListEditorWindow : EditorWindow
{
    IList targetList;
    Type elementType;
    Vector2 scroll;

    ScriptableObject owner; 

    public static void Open(IList list, Type elementType, ScriptableObject owner)
    {
        var window = CreateInstance<SOListEditorWindow>();
        window.targetList = list;
        window.elementType = elementType;
        window.owner = owner; 
        window.titleContent = new GUIContent($"List<{elementType.Name}>");
        window.ShowUtility();
    }

    void OnGUI()
    {
        if (targetList == null || elementType == null)
        {
            EditorGUILayout.HelpBox("No list loaded.", MessageType.Warning);
            return;
        }

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Label("Index", GUILayout.Width(40));
        foreach (var f in elementType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (!f.IsPublic && f.GetCustomAttribute<SerializeField>() == null)
                continue;
            GUILayout.Label(f.Name, GUILayout.Width(150));
        }
        GUILayout.Label("Delete", GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();

        scroll = EditorGUILayout.BeginScrollView(scroll);

        for (int i = 0; i < targetList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal("box");
            GUILayout.Label($"[{i}]", GUILayout.Width(40));

            object element = targetList[i];

            foreach (var f in elementType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (!f.IsPublic && f.GetCustomAttribute<SerializeField>() == null)
                    continue;

                object value = f.GetValue(element);
                object newValue = DrawFieldCell(value, f.FieldType, 150);

                if (!Equals(value, newValue))
                {
                    f.SetValue(element, newValue);
                    EditorUtility.SetDirty(owner); 
                }
            }

            if (GUILayout.Button("X", GUILayout.Width(40)))
            {
                targetList.RemoveAt(i);
                EditorUtility.SetDirty(owner); 
                break;
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("+ Add Element"))
        {
            targetList.Add(Activator.CreateInstance(elementType));
            EditorUtility.SetDirty(owner);
        }
    }

    object DrawFieldCell(object value, Type type, float width)
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

        GUILayout.Label("(Unsupported)", GUILayout.Width(width));
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
            {
                nf.SetValue(value, newNestedValue);
                EditorUtility.SetDirty(owner); 
            }
        }
        EditorGUILayout.EndVertical();
        return value;
    }
}
