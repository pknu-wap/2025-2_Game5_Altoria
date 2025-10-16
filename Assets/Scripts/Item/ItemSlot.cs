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
        //itemBorder.sprite = border; 
        itemCount.text = count.ToString();  
    }

}
