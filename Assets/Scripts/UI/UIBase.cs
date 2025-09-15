using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class UIBase : MonoBehaviour, IHideable, IShowable
    {
        private bool _init = false;
        private void Start()
        {
            Init();
        }
        public virtual bool Init()
        {
            if (_init) return false;

            _init = true;
            return true;
        }
        public virtual void Show() => gameObject.SetActive(true);
        public virtual void Hide() => gameObject.SetActive(false);
    }
    public class UIHUD:UIBase
    {
        [Header("고정된 위젯들 (인스펙터에서 세팅)")]
        [SerializeField] private List<UIWidget> fixedWidgets;

        private readonly Dictionary<System.Type, UIWidget> _widgets = new();
        public IReadOnlyDictionary<System.Type, UIWidget> Widgets => _widgets;

        public override bool Init()
        {
            if (!base.Init()) return false;
            InitializeFixedWidgets();
            return true;
        }
        void InitializeFixedWidgets()
        {
            var widgets = fixedWidgets.ToArray();
            for(int i =0; i<widgets.Length;i++)
            {
                var widget = widgets[i];
                if (widget == this) continue;
                widget.Init();

                var type = widget.GetType();
                if (!_widgets.ContainsKey(type))
                    _widgets[type] = widget;
            }
        }
        public void AddWidget(UIWidget widget)
        {
            var type = widget.GetType();
           
            if (_widgets.ContainsKey(type)) return;

            _widgets[type] = widget;
            widget.transform.SetParent(transform, false); 
            widget.Init();

        }

        public void RemoveWidget<T>() where T : UIWidget
        {
            var type = typeof(T);
            if (_widgets.TryGetValue(type, out var widget))
            {
                Destroy(widget.gameObject);
                _widgets.Remove(type);
            }
        }
        public T GetWidget<T>() where T : UIWidget
        {
            if (_widgets.TryGetValue(typeof(T), out var widget))
                return widget as T;

            return null;
        }

    }
    public class UIPopUp : UIBase,IDestroyable
    {
        public override bool Init()
        {
            if (!base.Init()) return false;
            Show();
            return true;
        }

        public void DestroyUI()
        {
            Destroy(gameObject);    
        }
    }
    public class UIWidget:UIBase
    {
        public override bool Init()
        {
            if (!base.Init()) return false;
            Show();
            return true;
        }
      
    }
}

