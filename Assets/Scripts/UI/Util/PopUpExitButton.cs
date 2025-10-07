using GameUI;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PopUpExitButton : MonoBehaviour
{
    Button button;
    UIPopUp popup;

    void Awake()
    {
        button = GetComponent<Button>();
        popup = GetComponentInParent<UIPopUp>(); 

        if (button != null && popup != null) button.onClick.AddListener(ClosePopup);
        else Debug.LogWarning($"[PopUpExitButton] Missing Button or UIPopUp on {gameObject.name}");
    }

    void ClosePopup()=> Manager.UI.ClosePopup();
}
