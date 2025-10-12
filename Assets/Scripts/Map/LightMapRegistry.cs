using System.Collections.Generic;
using UnityEngine;

public static class LightmapRegistry
{
    static List<LightmapData> globalLightmaps = new List<LightmapData>();

    public static int RegisterLightmap(Texture2D color, Texture2D dir, Texture2D mask)
    {

        for (int i = 0; i < globalLightmaps.Count; i++)
        {
            if (globalLightmaps[i].lightmapColor == color)
                return i;
        }

        var data = new LightmapData
        {
            lightmapColor = color,
            lightmapDir = dir,
            shadowMask = mask
        };
        globalLightmaps.Add(data);
        LightmapSettings.lightmaps = globalLightmaps.ToArray();
        return globalLightmaps.Count - 1;
    }
   
}
