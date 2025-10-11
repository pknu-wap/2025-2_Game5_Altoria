using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneObjectData
{
    public int ID;
    public int parentID;
    public string PrefabAddress;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public int SiblingIndex;
}

[System.Serializable]
public class SerializationWrapper<T>
{
    public List<T> Items;
    public SerializationWrapper() { }
    public SerializationWrapper(List<T> items) => this.Items = items;
}
[System.Serializable]
public class SceneExportSettings
{
    public GameObject Target;
    public bool IncludeExport = true;

    public bool MakeAddressable = true;
    public bool ForcePrefabize = true;
 
    public SceneExportSettings(GameObject go)
    {
        Target = go;
    }


}