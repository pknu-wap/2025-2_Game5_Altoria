using UnityEngine;
using UnityEngine.UI;

namespace BamaoUIPack.Scripts
{
    public class ShopItemSoldOut : MonoBehaviour
    {
        public Button BuyBtn;
        public GameObject shopItemSoldOutObj;

        private void Start()
        {
            BuyBtn.onClick.AddListener(() =>
            {
                shopItemSoldOutObj.SetActive(true);
            });
        }
    }
}