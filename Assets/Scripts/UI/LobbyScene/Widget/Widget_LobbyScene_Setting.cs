using GameUI;
using UnityEngine;
using UnityEngine.EventSystems;

public class Widget_LobbyScene_Setting : UIWidget, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("설정 창 열기");

        // 팝업 UI 생성 및 표시
        UIController.Instance.ShowPopup<TestSettingPopUp>();
    }

}
