using UnityEngine;
using UnityEngine.EventSystems;

namespace BamaoUIPack.Scripts
{
    // <summary>
    // Easy Drag UI Object
    // </summary>
    public class DragObj2D : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [Header("Set-up Variable")] 
        public bool EnableDrag = true;
        public string mainCanvasName = "Canvas";
        public bool isReturnPosition = true;
        public float alphaDrag = 0.6f;

        private Canvas m_Canvas;
        private CanvasGroup m_CanvasGroup;
        private RectTransform _rectTransform;
        private Transform _dropParent;
        private Vector2 m_InitialPosition;
        private int m_InitialSiblingIndex;
        private bool _initialIsReturnPos;

        private void Awake()
        {
            m_Canvas = GameObject.Find(mainCanvasName).GetComponent<Canvas>();
            m_CanvasGroup = gameObject.AddComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
            _dropParent = transform.parent;
            m_InitialPosition = _rectTransform.position;
            m_InitialSiblingIndex = transform.GetSiblingIndex();
            _initialIsReturnPos = isReturnPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!EnableDrag) return;

            m_CanvasGroup.alpha = alphaDrag;
            m_CanvasGroup.blocksRaycasts = false;
            transform.SetParent(m_Canvas.transform);
            isReturnPosition = _initialIsReturnPos;
            m_InitialPosition = _rectTransform.position;
            m_InitialSiblingIndex = transform.GetSiblingIndex();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!EnableDrag) return;    

            _rectTransform.anchoredPosition += eventData.delta / m_Canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!EnableDrag) return;

            if (isReturnPosition)
                SetInitialPosition();

            m_CanvasGroup.alpha = 1f;
            m_CanvasGroup.blocksRaycasts = true;
            transform.SetParent(_dropParent);
            transform.SetSiblingIndex(m_InitialSiblingIndex);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (!EnableDrag) return;

            if (!eventData.pointerDrag.TryGetComponent(out DragObj2D _))
                return;

            SetInitialPosition();
        }

        public void SetInitialPosition()
        {
            _rectTransform.position = m_InitialPosition;
        }
    }
}