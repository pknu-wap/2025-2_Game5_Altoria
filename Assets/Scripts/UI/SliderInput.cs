using UnityEngine;
using UnityEngine.UI;
using TMPro;

// UI) 슬라이더와 input field를 연동 (퍼센트 표시)
public class SliderInput : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_InputField input;

    public float Value
    {
        get => slider.value;
        set
        {
            slider.value = value;
            UpdateInputField(value);
        }
    }

    private void Awake()
    {
        slider.onValueChanged.AddListener(OnSliderChanged);
        input.onEndEdit.AddListener(OnInputChanged);
    }

    private void Start()
    {
        UpdateInputField(slider.value);
    }

    private void OnSliderChanged(float val)
    {
        UpdateInputField(val);
    }

    private void OnInputChanged(string str)
    {
        string clean = str.Replace("%", "");

        if (float.TryParse(clean, out float val))
        {
            slider.value = val / 100f;
        }
        else
        {
            UpdateInputField(slider.value);
        }
    }

    private void UpdateInputField(float val)
    {
        float percent = val * 100f; 
        input.text = percent.ToString("F2") + "%";
    }
}
