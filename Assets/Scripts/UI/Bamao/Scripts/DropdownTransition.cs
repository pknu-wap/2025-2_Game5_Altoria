using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BamaoUIPack.Scripts
{
    // <summary>
    // Use To Play Dropdown Transition Animation
    // </summary>
    public class DropdownTransition : TMP_Dropdown
    {
        Animator animatorList;
        Toggle[] toggleList;
        IEnumerator diableCoroutine;
        float disableTime = 0.4f;

        public new void Show()
        {
            Transform dropdownList = transform.Find("Dropdown List");
            if (dropdownList != null)
            {
                return;
            }

            base.Show();
            Transform contentTransform = transform.Find("Dropdown List/Viewport/Content");
            toggleList = contentTransform.GetComponentsInChildren<Toggle>(false);
            for (int i = 0; i < toggleList.Length; i++)
            {
                Toggle item = toggleList[i];
                item.onValueChanged.RemoveAllListeners();
                item.onValueChanged.AddListener(x => OnSelectItemCustom(item));
            }

            if (animatorList == null)
            {
                Transform listTransform = transform.Find("Dropdown List");
                animatorList = listTransform.gameObject.GetComponent<Animator>();
            }

            PlayAnimation(true);
        }

        void OnSelectItemCustom(Toggle toggle)
        {
            if (!toggle.isOn)
                toggle.isOn = true;

            int selectedIndex = -1;
            Transform tr = toggle.transform;
            Transform parent = tr.parent;
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i) == tr)
                {
                    selectedIndex = i - 1;
                    break;
                }
            }

            if (selectedIndex < 0)
                return;
            value = selectedIndex;
            Hide();
        }

        public new void Hide()
        {
            if (animatorList == null)
            {
                Transform listTransform = transform.Find("Dropdown List");
                animatorList = listTransform.gameObject.GetComponent<Animator>();
            }

            PlayAnimation(false);
            HideDropdown();
        }

        public void HideDropdown()
        {
            if (diableCoroutine != null)
            {
                StopCoroutine(diableCoroutine);
                diableCoroutine = null;
            }

            diableCoroutine = DisableTransition();
            StartCoroutine(diableCoroutine);
        }

        IEnumerator DisableTransition()
        {
            yield return new WaitForSeconds(disableTime);
            base.Hide();
        }

        void PlayAnimation(bool bShow)
        {
            if (animatorList != null)
            {
                if (animatorList.enabled == false)
                {
                    animatorList.enabled = true;
                }

                if (bShow)
                {
                    animatorList.Play("In", 0, 0);
                }
                else
                {
                    animatorList.Play("Out", 0, 0);
                }
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            Show();
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            Show();
        }

        public override void OnCancel(BaseEventData eventData)
        {
            Hide();
        }
    }
}