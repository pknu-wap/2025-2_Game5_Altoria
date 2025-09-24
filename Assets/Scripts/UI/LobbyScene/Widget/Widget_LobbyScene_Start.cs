using UnityEngine;
using GameUI;
using UnityEngine.EventSystems;

public class Widget_LobbyScene_Start : UIWidget, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("게임 시작!");
    }
}
