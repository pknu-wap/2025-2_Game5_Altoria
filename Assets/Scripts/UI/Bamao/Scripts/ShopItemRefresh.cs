using UnityEngine;
using UnityEngine.UI;

namespace BamaoUIPack.Scripts
{
    // <summary>
    // Refresh Shop Item When Button Clicked
    // </summary>
    public class ShopItemRefresh : MonoBehaviour
    {
        public Transform[] shopItemContainer;
        public GameObject[] shopItemPrefabs;
        public Button RefreshBtn;
        public GameObject[] SoldOutItems;

        private void Start()
        {
            RefreshBtn.onClick.AddListener(RefreshItem);
        }

        private void RefreshItem()
        {
            foreach (var itemContainer in shopItemContainer)
            {
                foreach (Transform child in itemContainer)
                {
                    Destroy(child.gameObject);
                }

                int randomIndex = Random.Range(0, shopItemPrefabs.Length);
                Instantiate(shopItemPrefabs[randomIndex], itemContainer);
            }

            foreach (var item in SoldOutItems)
            {
                item.SetActive(false);
            }
        }
    }
}