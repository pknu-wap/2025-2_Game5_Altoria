using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[ExecuteAlways]
public class PrefabLightmapData : MonoBehaviour
{
    [Tooltip("Reassigns shaders when applying baked lightmaps. Might conflict with transparent HDRP shaders.")]
    public bool releaseShaders = true;

    [System.Serializable]
    struct RendererInfo
    {
        public Renderer renderer;
        public int lightmapIndex;
        public Vector4 lightmapOffsetScale;
    }

    [System.Serializable]
    struct LightInfo
    {
        public Light light;
        public int lightmapBaketype;
        public int mixedLightingMode;
    }

    [SerializeField] RendererInfo[] m_RendererInfo;
    [SerializeField] Texture2D[] m_Lightmaps;
    [SerializeField] Texture2D[] m_LightmapsDir;
    [SerializeField] Texture2D[] m_ShadowMasks;
    [SerializeField] LightInfo[] m_LightInfo;

    void Awake() => Init();
    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) => Init();

    /// <summary>
    /// Registers this prefab's baked lightmaps and applies renderer mapping.
    /// </summary>
    void Init()
    {
#if UNITY_EDITOR
        // Prevent executing Init while editing scene (avoid editor preview flicker)
        if (!Application.isPlaying)
            return;
#endif

        if (m_RendererInfo == null || m_RendererInfo.Length == 0)
            return;

        int[] mappedIndexes = new int[m_Lightmaps.Length];
        for (int i = 0; i < m_Lightmaps.Length; i++)
        {
            mappedIndexes[i] = LightmapRegistry.RegisterLightmap(
                m_Lightmaps[i],
                (m_LightmapsDir.Length == m_Lightmaps.Length) ? m_LightmapsDir[i] : null,
                (m_ShadowMasks.Length == m_Lightmaps.Length) ? m_ShadowMasks[i] : null
            );
        }

        ApplyRendererInfo(m_RendererInfo, mappedIndexes, m_LightInfo);
    }

    void ApplyRendererInfo(RendererInfo[] infos, int[] lightmapIndexes, LightInfo[] lightsInfo)
    {
        foreach (var info in infos)
        {
            if (info.renderer == null) continue;

            info.renderer.lightmapIndex = lightmapIndexes[info.lightmapIndex];
            info.renderer.lightmapScaleOffset = info.lightmapOffsetScale;

            if (releaseShaders)
            {
                var mats = info.renderer.sharedMaterials;
                for (int j = 0; j < mats.Length; j++)
                {
                    if (mats[j] != null && Shader.Find(mats[j].shader.name) != null)
                        mats[j].shader = Shader.Find(mats[j].shader.name);
                }
            }
        }

        foreach (var l in lightsInfo)
        {
            if (l.light == null) continue;

            var bake = new LightBakingOutput
            {
                isBaked = true,
                lightmapBakeType = (LightmapBakeType)l.lightmapBaketype,
                mixedLightingMode = (MixedLightingMode)l.mixedLightingMode
            };
            l.light.bakingOutput = bake;
        }
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Bake Prefab Lightmaps")]
    static void GenerateLightmapInfo()
    {
#if UNITY_6000_0_OR_NEWER
        // Unity 6: GI workflow mode is gone, always uses LightingSettings.AutoGenerate
        var lightingSettings = Lightmapping.lightingSettings;
        if (lightingSettings == null)
        {
            Debug.LogError("Lighting Settings asset not found in the scene.");
            return;
        }

        if (Lightmapping.giWorkflowMode != Lightmapping.GIWorkflowMode.OnDemand)
        {
            Debug.LogError("Please disable 'Auto Generate' in Lighting Settings before baking.");
            return;
        }
#else
        if (Lightmapping.giWorkflowMode != Lightmapping.GIWorkflowMode.OnDemand)
        {
            Debug.LogError("Please disable Auto Generate in Lighting Settings before baking.");
            return;
        }
#endif

        Lightmapping.Bake();

        // Unity 6.0 이후 FindObjectsOfType 기본적으로 inactive 포함 가능
#if UNITY_2023_1_OR_NEWER
        var instances = Object.FindObjectsByType<PrefabLightmapData>(FindObjectsSortMode.None);
#else
        var instances = Object.FindObjectsOfType<PrefabLightmapData>();
#endif

        foreach (var instance in instances)
        {
            var go = instance.gameObject;
            var renderers = new List<RendererInfo>();
            var lightmaps = new List<Texture2D>();
            var lightmapsDir = new List<Texture2D>();
            var shadowMasks = new List<Texture2D>();
            var lights = new List<LightInfo>();

            GenerateLightmapInfo(go, renderers, lightmaps, lightmapsDir, shadowMasks, lights);

            instance.m_RendererInfo = renderers.ToArray();
            instance.m_Lightmaps = lightmaps.ToArray();
            instance.m_LightmapsDir = lightmapsDir.ToArray();
            instance.m_ShadowMasks = shadowMasks.ToArray();
            instance.m_LightInfo = lights.ToArray();

#if UNITY_2018_3_OR_NEWER
            var prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(go) as GameObject;
            if (prefab != null)
                PrefabUtility.ApplyPrefabInstance(go, InteractionMode.AutomatedAction);
#endif
        }

        Debug.Log(" Prefab lightmap baking complete!");
    }

    static void GenerateLightmapInfo(GameObject root, List<RendererInfo> rendererInfos, List<Texture2D> lightmaps,
        List<Texture2D> lightmapsDir, List<Texture2D> shadowMasks, List<LightInfo> lightsInfo)
    {
        foreach (var renderer in root.GetComponentsInChildren<MeshRenderer>(true))
        {
            if (renderer.lightmapIndex < 0 || renderer.lightmapScaleOffset == Vector4.zero)
                continue;

            var info = new RendererInfo
            {
                renderer = renderer,
                lightmapOffsetScale = renderer.lightmapScaleOffset
            };

            var data = LightmapSettings.lightmaps[renderer.lightmapIndex];
            var lightmap = data.lightmapColor;
            var dir = data.lightmapDir;
            var mask = data.shadowMask;

            info.lightmapIndex = lightmaps.IndexOf(lightmap);
            if (info.lightmapIndex == -1)
            {
                info.lightmapIndex = lightmaps.Count;
                lightmaps.Add(lightmap);
                lightmapsDir.Add(dir);
                shadowMasks.Add(mask);
            }

            rendererInfos.Add(info);
        }

        foreach (var l in root.GetComponentsInChildren<Light>(true))
        {
            var info = new LightInfo
            {
                light = l,
                lightmapBaketype = (int)l.lightmapBakeType,
#if UNITY_6000_0_OR_NEWER
                mixedLightingMode = (int)Lightmapping.lightingSettings.mixedBakeMode
#elif UNITY_2020_1_OR_NEWER
                mixedLightingMode = (int)Lightmapping.lightingSettings.mixedBakeMode
#else
                mixedLightingMode = (int)LightmapEditorSettings.mixedBakeMode
#endif
            };
            lightsInfo.Add(info);
        }
    }
#endif
}
