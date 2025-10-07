using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TopButtonSet
{
    public Image button;
    public Image focus;
    public GameObject page;
}

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryUI;

    [SerializeField] TopButtonSet[] topButtons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetButton()
    {
        for (int i = 0; i < topButtons.Length; i++)
        {
            int index = i;
            topButtons[i].button.GetComponent<Button>().onClick.AddListener(() =>
            {
                for (int j = 0; j < topButtons.Length; j++)
                {
                    if (j == index)
                    {
                        topButtons[j].focus.gameObject.SetActive(true);
                        topButtons[j].page.SetActive(true);
                    }
                    else
                    {
                        topButtons[j].focus.gameObject.SetActive(false);
                        topButtons[j].page.SetActive(false);
                    }
                }
                SoundManager.Instance.PlaySFX(SFX.ButtonClick);
            });
        }
    }

    public void OnClose()
    {
        inventoryUI.SetActive(false);
        SoundManager.Instance.PlaySFX(SFX.ButtonClick);
    }
}
