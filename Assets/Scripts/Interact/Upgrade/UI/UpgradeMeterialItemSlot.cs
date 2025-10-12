using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMeterialItemSlot : MonoBehaviour
{
    Image itemGradeImg;
    [SerializeField] Image itmeImg;
    [SerializeField] TextMeshProUGUI cnt;

    // TODO: ItemData가 아닌 강화재료Data 필요
    public void Init(ItemData data, int cnt)
    {
        // TODO: 데이터에서 필요한 개수 가져오기 cnt.text = 
        // TODO: 강화재료 Data 받아서 해당 아이템의 itemData로 부터 itemGradeImg, itmeImg, cnt 설정하기
        this.cnt.text = cnt.ToString();
    }
}
