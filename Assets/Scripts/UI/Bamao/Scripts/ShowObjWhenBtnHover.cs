using UnityEngine;
using UnityEngine.EventSystems;

namespace BamaoUIPack.Scripts
{
    // <summary>
    // Use To Show Focus GameObject When Pointer Hover
    // </summary>
    [RequireComponent(typeof(AudioSource))]
    public class ShowObjWhenBtnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public GameObject FocusObj;

        public AudioClip HoverSound;
        public AudioClip ClickSound;
        public AudioClip ExitSound;

        public AudioSource Source;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(FocusObj) FocusObj.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (FocusObj) FocusObj.SetActive(false);
            //SoundManager.Instance.PlaySFX(SFX.ButtonClick);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SoundManager.Instance.PlaySFX("CartoonPop");
        }
    }
}