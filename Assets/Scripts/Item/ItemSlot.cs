using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Sprite[] itemBorders;
    [SerializeField] Image itemBorder;
    [SerializeField] TextMeshProUGUI itemCount;

    public void SetSlot(string spriteAddress, int count, int grade = 0)
    {
        //itemImage.sprite = spriteAddress;
        itemBorder.sprite = itemBorders[grade]; 
        itemCount.text = count.ToString();  
    }
}