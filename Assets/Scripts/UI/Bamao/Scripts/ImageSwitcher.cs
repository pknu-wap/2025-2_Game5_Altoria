using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BamaoUIPack.Scripts
{
    // <summary>
    // Use To Change Image Sprite When Clicked
    // </summary>
    public class ImageSwitcher : MonoBehaviour, IPointerClickHandler
    {
        public Sprite Sprite1;
        public Sprite Sprite2;

        public Image _image;
        private int currentIndex = 1;

        private void Start()
        {
            if(_image == null)
                _image = GetComponent<Image>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _image.sprite = currentIndex == 1 ? Sprite2 : Sprite1;
            currentIndex = currentIndex == 1 ? 2 : 1;
        }
    }
}