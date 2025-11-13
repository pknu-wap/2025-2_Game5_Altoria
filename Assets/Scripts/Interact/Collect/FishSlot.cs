using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameInteract
{
    public class FishSlot : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] TextMeshProUGUI text;

        public void Init(string spriteAddress, string probablilty)
        {
            //image.sprite = spriteAddress;
            text.text = probablilty;
        }
    }

}