using System;
using UnityEngine;
using UnityEngine.UI;

namespace BamaoUIPack.Scripts
{
    // <summary>
    // Toggle The Navigation Tab GameObject When Pressing KeyCode
    // </summary>
    public class TabToggle : MonoBehaviour
    {
        public bool IsEnabled = true;
        public KeyCode KeyCode = KeyCode.Tab;
        public KeyCode NextPageKeyCode = KeyCode.RightArrow;
        public KeyCode PrevPageKeyCode = KeyCode.LeftArrow;
        public Toggle NavToggle;
        
        public GameObject targetObject;
        public PageNavDropdownView PageNavDropdownView;

        private void Start()
        {
            NavToggle.onValueChanged.AddListener((isOn) =>
            {
                targetObject.SetActive(isOn);
            });
        }

        private void Update()
        {
            if (IsEnabled == false) return;
            if (Input.GetKeyDown(KeyCode))
            {
                if (targetObject != null)
                {
                    targetObject.SetActive(!targetObject.activeSelf);
                    NavToggle.isOn = targetObject.activeSelf;
                }
            }
            
            if (Input.GetKeyDown(NextPageKeyCode))
            {
                PageNavDropdownView.OnNextButtonClicked();
            }
            
            if (Input.GetKeyDown(PrevPageKeyCode))
            {
                PageNavDropdownView.OnPreviousButtonClicked();
            }
        }
        
    }
}