using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class UIController
    {
        #region Static
        static UIController _instance;
        public static UIController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UIController();
                return _instance;
            }
        }
        //public static WorldUI WorldUI => Instance.worldUI;
        #endregion

        public UIHUD MainHUD { get; private set; }
        public GameObject UIRoot => uiHelper.GetOrAddUIRoot();

        const string UI_PATH_PREFIX = "UI/";
        readonly UIHelper uiHelper = new();
        readonly Stack<UIPopUp> popUpStack = new();
        //readonly WorldUI worldUI = new();
        int order = 10;
       
        

        UIController() {  uiHelper.FindOrAddGameObject(UI_PATH_PREFIX);}
       
        void LoadUI<T>(string name, System.Action<T> onLoaded, Transform parent = null, bool sort = true) where T : UIBase
        {
            string path = UI_PATH_PREFIX + name;
            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogError($"[UIController] Failed to load UI from Resources: {path}");
                return;
            }

            var go = Object.Instantiate(prefab, parent ?? UIRoot.transform);
            SetCanvas(go, sort);

            var ui = go.GetComponent<T>();
            if (ui == null)
            {
                Debug.LogError($"[UIController] Missing component {typeof(T).Name} on prefab {path}");
                return;
            }

            ui.Init();
            onLoaded?.Invoke(ui);
        }

        void SetCanvas(GameObject obj, bool sort = true)
        {
            Canvas canvas = Utils.GetOrAddComponent<Canvas>(obj);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
            canvas.sortingOrder = sort ? order++ : 0;
        }

        public T ShowHUD<T>() where T : UIHUD
        {
            string hudPath = typeof(T).Name;

            if (MainHUD != null)
                Object.Destroy(MainHUD.gameObject);

            T result = null;
            LoadUI<T>(hudPath, hud =>
            {
                MainHUD = hud;
                result = hud;
            }, UIRoot.transform, false);

            return result;
        }

        public T ShowPopup<T>() where T : UIPopUp
        {
            string popupPath = typeof(T).Name;

            T result = null;
            LoadUI<T>(popupPath, popup =>
            {
                popUpStack.Push(popup);
                result = popup;
            });

            return result;
        }
        
        public void ClosePopup()
        {
            if (popUpStack.Count == 0) return;

            var popup = popUpStack.Pop();
            Object.Destroy(popup.gameObject);

            order--;
        }
       
        public void CloseAllPopup()
        {
            while (popUpStack.Count > 0) ClosePopup();
            order = 10;
        }

        public void ClearAllUI()
        {
            CloseAllPopup();
            Object.Destroy(MainHUD.gameObject);
            MainHUD = null;
        }
        public bool IsAnyPopUp() { return popUpStack.Count > 0; }
        public void HideHUD() => MainHUD.Hide();
    }
}
