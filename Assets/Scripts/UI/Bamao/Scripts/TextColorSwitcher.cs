using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BamaoUIPack.Scripts
{
    // <summary>
    // Use To Toggle TextMeshPro Text Color
    // </summary>
    public class TextColorSwitcher : MonoBehaviour, IPointerClickHandler
    {
        public Color Color1;
        public Color Color2;

        public TMP_Text _text;
        private int currentIndex = 1;

        private void Start()
        {
            if(_text == null)
                _text = GetComponent<TMP_Text>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _text.color = currentIndex == 1 ? Color2 : Color1;
            currentIndex = currentIndex == 1 ? 2 : 1;
        }
    }
}