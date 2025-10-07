using UnityEngine;
using UnityEngine.UI;

namespace BamaoUIPack.Scripts
{
    public class EquipmentSlotPrefab : MonoBehaviour
    {
        public Image QualitySpriteImage;
        public Image ItemImage;

        private void Start()
        {
            SetActive(false);
        }

        public void SetItem(Sprite sprite, Sprite qualitySprite)
        {
            ItemImage.sprite = sprite;
            QualitySpriteImage.sprite = qualitySprite;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
}
}