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

        private AudioSource AudioSource;
        private Animator animator;

        private void Start()
        {
            AudioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioSource.clip = MouseClickClip;
            AudioSource.volume = MouseClickSoundVolume;
            AudioSource.Play();
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
            
            AudioSource.clip = MouseEnterClip;
            AudioSource.volume = MouseEnterSoundVolume;
            AudioSource.Play();
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

            AudioSource.clip = MouseExitClip;
            AudioSource.volume = MouseExitSoundVolume;
            AudioSource.Play();
        }
    }
}