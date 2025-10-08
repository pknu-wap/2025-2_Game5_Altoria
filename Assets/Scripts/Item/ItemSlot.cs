using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemCount;


    public void SetSlot(string item, int count)
    {
       // itemImage.sprite = item;
        itemCount.text = count.ToString();  
    }

}
