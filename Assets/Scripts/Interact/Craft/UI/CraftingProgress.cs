using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class CraftingProgress : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image progress;
    [SerializeField] private Image itemIcon;

    public int SlotIndex { get; private set; }
    public Sprite Icon => itemIcon.sprite;
    public float Progress => progress.fillAmount;
  
    public event Action<int> OnClicked;

    public void Init(int slotIndex) => SlotIndex = slotIndex;
    public void SetIcon(Sprite sprite) => itemIcon.sprite = sprite;
    public void FillProgress(float value) => progress.fillAmount = Mathf.Clamp01(value);

    public void CompleteProgress()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke(SlotIndex);
    }

}
