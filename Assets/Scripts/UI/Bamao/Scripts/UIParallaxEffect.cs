using UnityEngine;

namespace BamaoUIPack.Scripts
{
    public class UIParallaxEffect : MonoBehaviour
    {
        [Header("Parallax Settings")] 
        public RectTransform targetUIElement; 
        public float speed = 10f; 
        public float range = 50f; 

        private Vector3 initialPosition;

        void Start()
        {
            if (targetUIElement == null)
            {
                targetUIElement = GetComponent<RectTransform>();
            }

            initialPosition = targetUIElement.localPosition;
        }

        void Update()
        {
            Vector2 mousePosition = new Vector2(
                (Input.mousePosition.x / Screen.width) - 0.5f,
                (Input.mousePosition.y / Screen.height) - 0.5f
            );

            Vector3 targetPosition = initialPosition + new Vector3(mousePosition.x * range, mousePosition.y * range, 0);

            targetUIElement.localPosition =
                Vector3.Lerp(targetUIElement.localPosition, targetPosition, speed * Time.deltaTime);
        }
    }
}