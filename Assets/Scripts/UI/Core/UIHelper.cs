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

        public GameObject FindOrAddEventSystem(string path)
        {
    
            if (eventSystem != null)
                return eventSystem;

       
            string prefabPath = path + "EventSystem";
            GameObject prefab = Resources.Load<GameObject>(prefabPath);

            
           eventSystem = Object.Instantiate(prefab);
           Object.DontDestroyOnLoad(eventSystem);
            return eventSystem;
        }
            
          

    }
}