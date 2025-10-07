using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BamaoUIPack.Scripts
{
    public class EquipmentItemPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public List<Sprite> qualitySprites;

        public Sprite sprite;
        public int ItemQuality;
        public int ItemCategory;

        public GameObject FocusObj;
        public GameObject TooltipObj;

        public Vector3 TooltipsOffset = new Vector3(0, 0, 0);
        public Action OnClickAction;
        public bool isEquipped = false;

        private Image QualityBGImage;

        private void Start()
        {
            if (sprite == null)
            {
                sprite = GetComponentsInChildren<Image>()[1].sprite;
            }

            QualityBGImage = GetComponent<Image>();

            if (TooltipObj != null)
            {
                TooltipObj.SetActive(false);
            }
            
            GetComponent<Button>().onClick.AddListener(() =>
            {
                isEquipped = !isEquipped;
                FocusObj.SetActive(isEquipped);
                OnClickAction?.Invoke();
            });
        }

        private void Update()
        {
            if (TooltipObj != null && TooltipObj.activeSelf)
            {
                Vector3 mousePosition = Input.mousePosition;
                TooltipObj.transform.position = mousePosition + TooltipsOffset; 
            }
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (TooltipObj != null)
            {
                TooltipObj.SetActive(true); 
            }
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            if (TooltipObj != null)
            {
                TooltipObj.SetActive(false); // 隱藏 Tooltip
            }
        }

        public void SetTabVisibility(int category)
        {
            if (category == 0) // All
            {
                gameObject.SetActive(true);
                return;
            }

            gameObject.SetActive(category == ItemCategory);
        }

        public void SetEquipped(bool isEquipped)
        {
            this.isEquipped = isEquipped;
            FocusObj.SetActive(isEquipped);
        }


    }
}