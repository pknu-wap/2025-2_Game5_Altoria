using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemCount;

    public void SetSlot(string item, int count)
    {
       // itemImage.sprite = item;
        itemCount.text = count.ToString();  
    }

}
