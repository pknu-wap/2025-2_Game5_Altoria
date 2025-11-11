using System.Collections.Generic;
using UnityEngine;
using static Define;

[System.Serializable]
public class ContentPrefabEntry
{
    public ContentType Type;
    public GameObject Prefab;
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale = Vector3.one;
}

[CreateAssetMenu(fileName = "CraftPrefabDB", menuName = "Scriptable Objects/CraftPrefabDB")]
public class ContentPrefabDB : ScriptableObject
{
    [SerializeField] List<ContentPrefabEntry> entries = new();

    private Dictionary<ContentType, ContentPrefabEntry> cache = new();

    private void OnEnable()
    {
        InitializeCache();
    }

    public void InitializeCache()
    {
        cache.Clear();
        for(int i=0; i<entries.Count;  i++)
        {

            if (entries[i] == null || entries[i].Prefab == null)
                continue;

            if (!cache.ContainsKey(entries[i].Type))
                cache.Add(entries[i].Type, entries[i]);
            else
                Debug.LogWarning($"[CraftPrefabDB] 중복된 Type {entries[i].Type} 항목이 있습니다.");
        }

    }

    public ContentPrefabEntry GetEntry(ContentType type)
    {
        if (cache == null || cache.Count == 0)
            InitializeCache();

        if (cache.TryGetValue(type, out var entry))
            return entry;

        Debug.LogWarning($"[CraftPrefabDB] {type} 데이터가 존재하지 않습니다.");
        return null;
    }
}
