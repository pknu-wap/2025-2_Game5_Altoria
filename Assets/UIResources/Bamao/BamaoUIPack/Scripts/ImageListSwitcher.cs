using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BamaoUIPack.Scripts
{
    // <summary>
    // Switched Image Sprite List Sequence When Clicked
    // </summary>
    public class ImageListSwitcher : MonoBehaviour, IPointerClickHandler
    {
        public Sprite[] SpriteList;

        public Image _image;
        private int currentIndex = 0;

        private void Start()
        {
            if (_image == null)
                _image = GetComponent<Image>();

            if (SpriteList.Length > 0)
                _image.sprite = SpriteList[0];
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (SpriteList.Length == 0)
                return;

            currentIndex = (currentIndex + 1) % SpriteList.Length;
            _image.sprite = SpriteList[currentIndex];
        }
    }
}