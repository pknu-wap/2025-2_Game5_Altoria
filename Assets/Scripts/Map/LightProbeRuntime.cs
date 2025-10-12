using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class LightProbeRuntime : MonoBehaviour
{
    [Header("Lighting Settings")]
    [ColorUsage(false, true)]
    public Color ambient = Color.gray;

    [Tooltip("자동으로 찾은 라이트 목록 (수동으로도 지정 가능)")]
    [SerializeField]  List<Light> lights = new();

    [Tooltip("씬 시작 시 자동으로 프로브 업데이트할지 여부")]
    public bool autoUpdateOnStart = true;

    private bool initialized;


    private SphericalHarmonicsL2[] bakedProbes;
    private Vector3[] probePositions;
    private int probeCount;

    private void OnValidate()
    {
        if (!Application.isPlaying && initialized)
        {
            UpdateProbes();
        }
    }

    private IEnumerator Start()
    {
        yield return null;
        initialized = true;

        if (autoUpdateOnStart)
            UpdateProbes();
    }

    [ContextMenu("Update Probes")]
    public void UpdateProbes()
    {
        if (LightmapSettings.lightProbes == null)
        {
            Debug.LogWarning("No LightProbes found in the scene.");
            return;
        }

     
        probeCount = LightmapSettings.lightProbes.count;
        if (probeCount == 0) return;

        bakedProbes = LightmapSettings.lightProbes.bakedProbes;
        probePositions = LightmapSettings.lightProbes.positions;


        if (lights == null)
            lights = new List<Light>();
        else
            lights.Clear();

#if UNITY_2022_2_OR_NEWER
        lights.AddRange(Object.FindObjectsByType<Light>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
#else
        lights.AddRange(Object.FindObjectsOfType<Light>());
#endif


        for (int i = 0; i < probeCount; i++)
        {
            bakedProbes[i].Clear();
            bakedProbes[i].AddAmbientLight(ambient);
        }

    
        for (int l = 0; l < lights.Count; l++)
        {
            var light = lights[l];
            if (light == null || !light.enabled) continue;

            switch (light.type)
            {
                case LightType.Directional:
                    for (int p = 0; p < probeCount; p++)
                        bakedProbes[p].AddDirectionalLight(-light.transform.forward, light.color, light.intensity);
                    break;

                case LightType.Point:
                    for (int p = 0; p < probeCount; p++)
                        SHAddPointLight(probePositions[p], light.transform.position, light.range, light.color, light.intensity, ref bakedProbes[p]);
                    break;
            }
        }


        LightmapSettings.lightProbes.bakedProbes = bakedProbes;

        Debug.Log($"[LightProbeRuntime] {probeCount} probes updated from {lights.Count} lights.");
    }

   void SHAddPointLight(Vector3 probePosition, Vector3 lightPos, float range, Color color, float intensity, ref SphericalHarmonicsL2 sh)
    {
        Vector3 probeToLight = lightPos - probePosition;
        float sqrDist = probeToLight.sqrMagnitude;
        if (sqrDist > range * range) return; 
        float attenuation = 1.0f / (1.0f + 25.0f * sqrDist / (range * range));
        sh.AddDirectionalLight(probeToLight.normalized, color, intensity * attenuation);
    }
}
