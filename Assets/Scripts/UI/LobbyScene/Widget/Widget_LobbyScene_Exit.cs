using GameUI;
using UnityEngine;
using UnityEngine.EventSystems;

public class Widget_LobbyScene_Exit : UIWidget, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("게임 종료");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

}
