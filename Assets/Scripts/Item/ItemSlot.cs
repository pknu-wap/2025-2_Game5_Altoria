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

    public void SetSlot(string spriteAddress, int count, ItemGrade grade = ItemGrade.None)
    {
        
        int gradeIndex = Mathf.Clamp((int)grade - 1, 0, itemBorders.Length - 1);

        if (itemBorders == null || itemBorders.Length == 0)
        {
            Debug.LogError($"[ItemSlot] itemBorders is null or empty on {gameObject.name}");
            return;
        }

        if (itemBorder != null)
            itemBorder.sprite = itemBorders[gradeIndex];

        itemCount.text = count.ToString();

  
       Sprite sprite = Manager.Resource.Load<Sprite>(spriteAddress);
        if (sprite != null)
            itemImage.sprite = sprite;
    }
}