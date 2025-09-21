using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BamaoUIPack.Scripts
{
    // <summary>
    // Use To Display Potion Input Number
    // </summary>
    public class PotionCountView : MonoBehaviour
    {
        public TMP_InputField InputText;
        public Button AddBtn;
        public Button MinusBtn;

        public int currentCount = 0;

        private void Start()
        {
            InputText.onValueChanged.AddListener((x)=>ChangeInputText(int.Parse(x)));
            MinusBtn.onClick.AddListener(()=>ChangeInputText(currentCount-1));
            AddBtn.onClick.AddListener(()=>ChangeInputText(currentCount+1));
        }

        private void ChangeInputText(int count)
        {
            currentCount = count;
            InputText.text = currentCount.ToString();
        }
    }
}