using SceneLoad;
using UnityEngine;


    public class JsonMapLoaderComponent : MonoBehaviour
    {
        JsonMapLoader loader;


        void OnEnable()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChanged;
#endif
        }
         void OnDisable()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeChanged;
#endif
        }
        public void SetLoader(JsonMapLoader loader)
        {
            this.loader = loader;
        }
#if UNITY_EDITOR
        void OnPlayModeChanged(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            {
                Debug.Log("에디터 중단 Unload 호출");
                loader?.UnLoad();
            }
        }
#endif
    }
