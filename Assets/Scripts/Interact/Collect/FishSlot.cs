using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishSlot : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;

    public void Init(string spriteAddress, string probablilty)
    {
        //image.sprite = 
        text.text = probablilty;
    }
}
