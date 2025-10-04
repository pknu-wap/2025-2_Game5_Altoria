using UnityEngine;
using UnityEngine.EventSystems;

namespace GameUI
{
    public  class UIHelper 
    {
        GameObject uiRoot;
        GameObject eventSystem;

     
        public  GameObject GetOrAddUIRoot()
        {
            if (uiRoot == null)
            {
                uiRoot = GameObject.Find("@UI_Root");
                if (uiRoot == null)
                {
                    uiRoot = new GameObject("@UI_Root");
                    Object.DontDestroyOnLoad(uiRoot);
                }
            }
            return uiRoot;
        }

        public  GameObject FindOrAddGameObject(string path)
        {
            if (eventSystem != null) return eventSystem;

            var existing = Object.FindAnyObjectByType<EventSystem>();
            if (existing != null)
            {
                eventSystem = existing.gameObject;
                Object.DontDestroyOnLoad(eventSystem);
                return eventSystem;
            }
            string prefabPath = path + "EventSystem";
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab != null)
            {
                eventSystem = Object.Instantiate(prefab);
            }
            else
            {
                eventSystem = new GameObject("@EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }

            Object.DontDestroyOnLoad(eventSystem);
            return eventSystem;
        }
    }
}