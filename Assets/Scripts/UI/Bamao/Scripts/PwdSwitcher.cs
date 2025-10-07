using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BamaoUIPack.Scripts
{
    // <summary>
    // Use To Show/Hide Password When Button Clicked
    // </summary>
    public class PwdSwitcher : MonoBehaviour
    {
        public TMP_InputField passwordInputField;
        public Button toggleButton;
        private bool isPasswordVisible;

        private void Start()
        {
            isPasswordVisible = false;
            passwordInputField.contentType = TMP_InputField.ContentType.Password;
            passwordInputField.ForceLabelUpdate();
            toggleButton.onClick.AddListener(TogglePasswordVisibility);
        }

        private void TogglePasswordVisibility()
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                passwordInputField.contentType = TMP_InputField.ContentType.Standard;
            }
            else
            {
                passwordInputField.contentType = TMP_InputField.ContentType.Password;
            }

            passwordInputField.ForceLabelUpdate();
        }
    }
}