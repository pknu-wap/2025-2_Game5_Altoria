using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameInteract
{
    public class UpgradeSlot : MonoBehaviour
    {
        [SerializeField] Image itemGradeImg;
        [SerializeField] Image itmeImg;
        [SerializeField] TextMeshProUGUI gradeStep;
        int slotIndex;

        public Action<int> OnClickAction;

        public void Init(ItemData data, int slotIndex)
        {
            itemGradeImg.sprite = Resources.Load<Sprite>($"UI/ItemFrame/{data.Grade.ToString()}");
            // itmeImg.sprite = data.id 를 통해 이미지 가져오기
            gradeStep.text = "+" + Manager.UserData.GetUserData<UserToolData>().GetToolStep(data.ID).ToString();
            this.slotIndex = slotIndex;
        }

        public void OnClickSlot() => OnClickAction?.Invoke(slotIndex);
    }
}
