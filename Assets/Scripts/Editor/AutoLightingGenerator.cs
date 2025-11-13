using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

#if  UNITY_EDITOR
[InitializeOnLoad]
public static class AutoLightingGenerator
{
    static AutoLightingGenerator()
    {
        // 씬이 열릴 때마다 자동 실행
        EditorSceneManager.sceneOpened += OnSceneOpened;
    }

    private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
    {
        if (Application.isPlaying)
            return;

        // Lighting Settings가 없으면 테스트 씬일 가능성이 높으므로 스킵
        var lightingSettings = Lightmapping.lightingSettings;
        if (lightingSettings == null)
        {
            Debug.Log($"[AutoLightingGenerator] Lighting Settings 없음 → 베이크 생략 ({scene.name})");
            return;
        }

        // GI 워크플로 확인 후 베이크 수행
        if (Lightmapping.giWorkflowMode == Lightmapping.GIWorkflowMode.OnDemand)
        {
            Debug.Log($"[AutoLightingGenerator] 씬 열림 감지: {scene.name}, 라이트맵 자동 생성 및 베이크 시작!");
            Lightmapping.BakeAsync();
        }
        else
        {
            Debug.Log($"[AutoLightingGenerator] Auto Generate 켜져 있음 → 수동 베이크 생략 ({scene.name})");
        }
    }
}
#endif