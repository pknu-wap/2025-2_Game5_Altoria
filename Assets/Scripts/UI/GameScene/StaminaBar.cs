using GameUI;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : UIWidget
{
    [SerializeField] Image staminaBG;
    [SerializeField] Image stamina;
    void Start()
    {
        SetStamina();
    }
    public void SetStamina()
    {
        // UserPlayerData의 스테미나 변수를 불러올 부분
        // float value = userPlayerData.GetPlayerData().Stemina;
        // stamina.fillAmount = value;
    }
}