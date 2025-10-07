using GameUI;
using System;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class PagePopupEntry
{
    public int ID;
    public MonoScript popupScript;
}

[CreateAssetMenu(menuName = "GameUI/Page Popup", fileName = "PagePopup")]
public class PagePopUpSO : ScriptableObject
{
    public PagePopupEntry[] rows;

    public Type GetPopUpType(int index)
    {
        if (rows == null || index < 0 || index >= rows.Length)
            return null;

        var script = rows[index].popupScript;
        return script != null ? script.GetClass() : null;
    }
}
