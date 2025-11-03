using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GameUI
{
    public class UIController
    {
        //public static WorldUI WorldUI => Instance.worldUI;

        public UIHUD MainHUD { get; private set; }
        public GameObject UIRoot => uiHelper.GetOrAddUIRoot();

        const string UI_PATH_PREFIX = "UI/";
        readonly UIHelper uiHelper = new();
        readonly Stack<UIPopUp> popUpStack = new();
        //readonly WorldUI worldUI = new();
        int order = 10;
       
        

        public UIController() {  /*uiHelper.FindOrAddEventSystem(UI_PATH_PREFIX);*/}
       
        void LoadUI<T>(string name, System.Action<T> onLoaded, Transform parent = null, bool sort = true) where T : UIBase
        {
            string path = UI_PATH_PREFIX + name;
            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogError($"[UIController] Failed to load UI from Resources: {path}");
                return;
            }

            var go = UnityEngine.Object.Instantiate(prefab, parent ?? UIRoot.transform);
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
        public T ShowHUD<T>() where T : UIHUD
        {
            string hudPath = typeof(T).Name;

            if (MainHUD != null)
                UnityEngine.Object.Destroy(MainHUD.gameObject);

            T result = null;
            LoadUI<T>(hudPath, hud =>
            {
                MainHUD = hud;
                result = hud;
            }, UIRoot.transform, false);

            return result;
        }
        void SetCanvas(GameObject obj, bool sort = true)
        {
            Canvas canvas = Utils.GetOrAddComponent<Canvas>(obj);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
            canvas.sortingOrder = sort ? order++ : 0;
        }

        public T ShowPopup<T>(Action onClosed = null) where T : UIPopUp
        {
            string popupPath = typeof(T).Name;

            T result = null;
            LoadUI<T>(popupPath, popup =>
            {
                popUpStack.Push(popup);
                result = popup;

                if (onClosed != null)
                    popup.OnClosed += onClosed;
            });

            return result;
        }
        public UIPopUp ShowPopup(Type popupType, Action onClosed = null)
        {
            if (popupType == null || !typeof(UIPopUp).IsAssignableFrom(popupType))
            {
                Debug.LogError($"[UIController] {popupType} is not a valid UIPopUp type");
                return null;
            }

            string popupPath = popupType.Name;
            UIPopUp result = null;

            LoadUI<UIPopUp>(popupPath, popup =>
            {
                popUpStack.Push(popup);
                result = popup;

                if (onClosed != null)
                    popup.OnClosed += onClosed;
            });

            return result;
        }

        public void ClosePopup()
        {
            if (popUpStack.Count == 0) return;

            var popup = popUpStack.Pop();

            popup.Close();
            order--;
           
        }
        public void ClosePopup(UIPopUp popup)
        {
            if (popUpStack.Count == 0) return;

            var top = popUpStack.Peek();
            if (top != popup)
            {
                Debug.LogWarning($"[UIController] Tried to close popup '{popup.name}', but top is '{top.name}'.");
                return;
            }

            popUpStack.Pop();
            if (popup != null && popup.gameObject != null)
                popup.Close();

            order = Mathf.Max(0, order - 1);
        }


        public void CloseAllPopup()
        {
            while (popUpStack.Count > 0) ClosePopup();
            order = 10;
        }

        public void ClearAllUI()
        {
            CloseAllPopup();
            UnityEngine.Object.Destroy(MainHUD.gameObject);
            MainHUD = null;
        }
        public bool IsAnyPopUp() { return popUpStack.Count > 0; }
        public bool HasThisPopUp(UIPopUp popup)
        {
            if (popup == null) return false;
            if (popUpStack == null || popUpStack.Count == 0) return false;

            return popUpStack.Contains(popup);
        }
        public void HideHUD() => MainHUD.Hide();
    }
}
