using UnityEngine;
using UnityEngine.EventSystems;

namespace BamaoUIPack.Scripts
{
    // <summary>
    // Use To Play SoundFx When Pointer Hover, Clicked
    // </summary>
    [RequireComponent(typeof(AudioSource))]
    public class PointerSoundFx : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Enter Status")]
        public bool AvoidEnterSelectedStatus = true;
        public AudioClip MouseEnterClip;
        [Range(0, 1)] 
        public float MouseEnterSoundVolume = 1.0f;

        [Header("Exit Status")]
        public bool AvoidExitSelectedStatus = true;
        public AudioClip MouseExitClip;
        [Range(0, 1)] 
        public float MouseExitSoundVolume = 1.0f;

        [Header("Click Status")]
        public AudioClip MouseClickClip;
        [Range(0, 1)] 
        public float MouseClickSoundVolume = 1.0f;

        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SoundManager.Instance.PlaySFX(SFX.ButtonClick);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (animator != null)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Selected") && AvoidExitSelectedStatus)
                {
                    return;
                }
            }

            SoundManager.Instance.PlaySFX(SFX.ButtonClick);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (animator != null)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Selected") && AvoidExitSelectedStatus)
                {
                    return;
                }
            }

        }
    }
}