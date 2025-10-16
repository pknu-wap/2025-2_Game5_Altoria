using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BamaoUIPack.Scripts
{
    // <summary>
    // Use To Change Image Color When Clicked Or Hover
    // </summary>
    public class ImageColorSwitcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public bool IsEnableOnClickColorSwitch = false;
        public Color OnClickColor1;
        public Color OnClickColor2;

        public bool IsEnableOnHoverColorSwitch = false;
        public Color OnHoverColor;

        public Image image;
        private int currentIndex = 1;
        private Color originalColor;

        private void Start()
        {
            if (image == null)
                image = GetComponent<Image>();

            originalColor = image.color;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsEnableOnClickColorSwitch)
            {
                image.color = currentIndex == 1 ? OnClickColor2 : OnClickColor1;
                currentIndex = currentIndex == 1 ? 2 : 1;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsEnableOnHoverColorSwitch)
                image.color = OnHoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsEnableOnHoverColorSwitch)
                image.color = originalColor;
        }
    }
}