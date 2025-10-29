using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Sprite[] itemBorders;
    [SerializeField] Image itemBorder;
    [SerializeField] TextMeshProUGUI itemCount;

    public void SetSlot(string spriteAddress, int count, ItemGrade grade = 0)
    {
        //itemImage.sprite = spriteAddress;
        itemBorder.sprite = itemBorders[(int)grade-1]; 
        itemCount.text = count.ToString();  
    }
}