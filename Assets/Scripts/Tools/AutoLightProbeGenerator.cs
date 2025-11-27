#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class AutoLightProbeGeneratorV2 : EditorWindow
{
    float fieldSpacing = 8f;
    float caveSpacing = 3f;
    float probeHeight = 1.7f;

    LayerMask groundMask;

    [MenuItem("Tools/Lighting/Generate Light Probes (v2)")]
    static void Init()
    {
        GetWindow<AutoLightProbeGeneratorV2>("Probe Generator v2");
    }

    private void OnGUI()
    {
        GUILayout.Label("Light Probe Auto Generator (v2)", EditorStyles.boldLabel);
        fieldSpacing = EditorGUILayout.FloatField("Field Spacing", fieldSpacing);
        caveSpacing = EditorGUILayout.FloatField("Cave Spacing", caveSpacing);
        probeHeight = EditorGUILayout.FloatField("Probe Height", probeHeight);

        if (GUILayout.Button("Generate Probes"))
        {
            Generate();
        }
    }

    void Generate()
    {
   
        var old = FindObjectOfType<LightProbeGroup>();
        if (old) DestroyImmediate(old.gameObject);


        GameObject groupObj = new GameObject("AutoLightProbes");
        LightProbeGroup group = groupObj.AddComponent<LightProbeGroup>();

        List<Vector3> list = new List<Vector3>();

        Bounds bounds = CalculateSceneBounds();

      
        groundMask = LayerMask.GetMask("Default", "Ground", "Terrain");

        float spacing;

        for (float x = bounds.min.x; x < bounds.max.x; x += fieldSpacing)
        {
            for (float z = bounds.min.z; z < bounds.max.z; z += fieldSpacing)
            {
                Vector3 start = new Vector3(x, bounds.max.y + 10f, z);

                if (Physics.Raycast(start, Vector3.down, out RaycastHit hit, 500f, groundMask))
                {
                    spacing = IsCave(hit.point) ? caveSpacing : fieldSpacing;

                    var pos = hit.point + Vector3.up * probeHeight;
                    list.Add(pos);
                }
                else
                {
                   
                    var meshPos = SampleMeshHeight(x, z);
                    if (meshPos.HasValue)
                    {
                        list.Add(meshPos.Value + Vector3.up * probeHeight);
                    }
                }
            }
        }

        group.probePositions = list.ToArray();

        Debug.Log($"[Auto Probe Generator v2] Probes: {list.Count}");
    }

    Bounds CalculateSceneBounds()
    {
        Renderer[] rs = Object.FindObjectsOfType<Renderer>();
        Bounds b = new Bounds(rs[0].bounds.center, Vector3.zero);
        foreach (var r in rs) b.Encapsulate(r.bounds);
        return b;
    }

    bool IsCave(Vector3 pos)
    {
        return Physics.Raycast(pos + Vector3.up * 0.5f, Vector3.up, 4f);
    }

    Vector3? SampleMeshHeight(float x, float z)
    {
       
        float y = 200f;
        Vector3 start = new Vector3(x, y, z);
        if (Physics.Raycast(start, Vector3.down, out RaycastHit hit, 500f))
            return hit.point;

        return null;
    }
}

#endif