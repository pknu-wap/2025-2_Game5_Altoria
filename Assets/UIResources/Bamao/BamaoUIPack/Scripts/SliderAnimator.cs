using UnityEngine;
using UnityEngine.UI;

namespace BamaoUIPack.Scripts
{
    // <summary>
    // Simple Slider Animation Within a duration.
    // </summary>
    public class SliderAnimator : MonoBehaviour
    {
        public Slider slider;
        public float duration = 2f;

        private float startTime;

        private void Start()
        {
            if (slider == null)
            {
                slider = GetComponent<Slider>();
            }

            startTime = Time.time - (slider.value / slider.maxValue) * duration;
        }

        private void Update()
        {
            if (slider != null)
            {
                float elapsedTime = Time.time - startTime;
                slider.value = Mathf.PingPong(elapsedTime / duration, 1f) * slider.maxValue;
            }
        }
    }
}