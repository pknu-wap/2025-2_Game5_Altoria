using SceneLoad;
using UnityEngine;


    public class JsonSceneLoaderComponent : MonoBehaviour
    {
        JsonSceneLoader loader;


        private void OnEnable()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChanged;
#endif
        }
        private void OnDisable()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeChanged;
#endif
        }
        public void SetLoader(JsonSceneLoader loader)
        {
            this.loader = loader;
        }
#if UNITY_EDITOR
        private void OnPlayModeChanged(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            {
                Debug.Log("에디터 중단 Unload 호출");
                loader?.UnLoad();
            }
        }
#endif
    }
