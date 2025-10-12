using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameInteract
{
    public class UpgradeSlot : MonoBehaviour
    {
        Image itemGradeImg;
        [SerializeField] Image itmeImg;
        int slotIndex;

        public Action<int> OnClickAction;

        public void Init(ItemData data, int slotIndex)
        {
            // itemGrade.GetComponent<Image>().sprite = data.SpriteAddress를 등급에 맞는 틀 설정
            // itmeImg.sprite = data.id 를 통해 이미지 가져오기
            this.slotIndex = slotIndex;
        }

        public void OnClickSlot() => OnClickAction?.Invoke(slotIndex);
    }
}
