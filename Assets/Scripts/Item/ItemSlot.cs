using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Image itemBorder; 
    [SerializeField] TextMeshProUGUI itemCount;


    public void SetSlot(string item, int count)
    {
        //itemImage.sprite = item;
        //itemBorder.sprite = border; 아이템 등급을 가져옴
        itemCount.text = count.ToString();  
    }

}
