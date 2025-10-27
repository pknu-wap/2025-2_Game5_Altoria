using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Sprite[] itemBorders; 
    [SerializeField] TextMeshProUGUI itemCount;

    // 등급 테두리 표시해야해서 인자로 itemGrade 추가하는거 어때? 띄울 정보가 이미지,테두리,갯수 세개임 
    public void SetSlot(string item, int count) //변수명 item에서 spriteAddress로 바꾸는거 어떰? 헷갈림 
    {
        //itemImage.sprite = item;
        //itemBorder.sprite = itemBorders[itemGrade]; 
        itemCount.text = count.ToString();  
    }
}
