using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public static class ItemSorter
{
    public static List<ItemData> SortItems(List<ItemData> items, SortType sortType)
    {
        switch (sortType)
        {
            case SortType.GradeHighToLow:
                //////
                break;
            case SortType.GradeLowToHigh:
                //////
                break;
            default:
                break;
        }
        return items;
    }
}
