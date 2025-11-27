using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public enum EColorComponent
    {
        _Color1,
        _Color2,
        _CorneaColor,
        _LipColor,
    }

    public class ColorPickerControl : MonoBehaviour
    {
        [Header("Set Image")]
        [SerializeField] RawImage hueImg, satValImg, outputImg;

        [Header("Hue Slider")]
        [SerializeField] Slider hueSlider;

        [Header("Hex Input Field")]
        [SerializeField] TMP_InputField hexInputField;

        [Header("Set Change Color Target")]
        [SerializeField] SkinnedMeshRenderer[] changeThisColor;
        [SerializeField] SkinnedMeshRenderer[] bodyColor;

        [Header("Dropdown")]
        [SerializeField] TMP_Dropdown colorComponentDropdown;

        float CurrenrHue, CurrentSat, CurrentVal;

        Texture2D hueTexture, svTexture, outputTexture;
        EColorComponent colorComponent = EColorComponent._Color1;

        void Start()
        {
            CreateHueImage();
            CreatSVImage();
            CreateOutputImage();
            UpdateOutputImage();
        }

        void CreateHueImage()
        {
            hueTexture = new Texture2D(16, 1);
            hueTexture.wrapMode = TextureWrapMode.Clamp;
            hueTexture.name = "HueTexture";

            for (int i = 0; i < hueTexture.width; i++)
            {
                hueTexture.SetPixel(i, 0, Color.HSVToRGB((float)i / hueTexture.width, 1f, 0.85f));
            }

            hueTexture.Apply();
            CurrenrHue = 0f;

            hueImg.texture = hueTexture;
        }

        void CreatSVImage()
        {
            svTexture = new Texture2D(16, 16);
            svTexture.wrapMode = TextureWrapMode.Clamp;
            svTexture.name = "SatValTexture";

            for (int y = 0; y < svTexture.height; y++)
            {
                for (int x = 0; x < svTexture.width; x++)
                {

                    svTexture.SetPixel(x, y, Color.HSVToRGB(
                        CurrenrHue, (float)x / svTexture.width, (float)y / svTexture.height));
                }
            }

            svTexture.Apply();
            CurrentSat = 0f;
            CurrentVal = 0f;

            satValImg.texture = svTexture;
        }

        void CreateOutputImage()
        {
            outputTexture = new Texture2D(1, 16);
            outputTexture.wrapMode = TextureWrapMode.Clamp;
            outputTexture.name = "OutputTexture";

            Color currentColor = Color.HSVToRGB(CurrenrHue, CurrentSat, CurrentVal);

            for (int i = 0; i < outputTexture.height; i++)
            {
                outputTexture.SetPixel(0, i, currentColor);
            }

            outputTexture.Apply();

            outputImg.texture = outputTexture;
        }

        void UpdateOutputImage()
        {
            Color currentColor = Color.HSVToRGB(CurrenrHue, CurrentSat, CurrentVal);

            for (int i = 0; i < outputTexture.height; i++)
            {
                outputTexture.SetPixel(0, i, currentColor);
            }

            outputTexture.Apply();

            hexInputField.text = ColorUtility.ToHtmlStringRGB(currentColor);

            changeThisColor[colorComponentDropdown.value].material.SetColor(colorComponent.ToString(), currentColor);
            if(colorComponentDropdown.value == 0)
            {
                for(int i = 0; i < bodyColor.Length; i++)
                    bodyColor[i].material.SetColor(colorComponent.ToString(), currentColor);
            }

            Manager.UserData.GetUserData<UserPlayerData>().SetColor(currentColor, colorComponentDropdown.value); 
        }

        public void SetSV(float S, float V)
        {
            CurrentSat = S;
            CurrentVal = V;

            UpdateOutputImage();
        }


        #region Event
        public void UpdateSVImage()
        {
            CurrenrHue = hueSlider.value;

            for (int y = 0; y < svTexture.height; y++)
            {
                for (int x = 0; x < svTexture.width; x++)
                {

                    svTexture.SetPixel(x, y, Color.HSVToRGB(
                        CurrenrHue, (float)x / svTexture.width, (float)y / svTexture.height));
                }
            }

            svTexture.Apply();
            UpdateOutputImage();
        }

        public void OnTextInput()
        {
            if (hexInputField.text.Length < 6) return;

            Color newColor;

            if (ColorUtility.TryParseHtmlString("#" + hexInputField.text, out newColor))
                Color.RGBToHSV(newColor, out CurrenrHue, out CurrentSat, out CurrentVal);

            hueSlider.value = CurrenrHue;
            hexInputField.text = "";
            UpdateOutputImage();
        }

        public void OnChagedDropDown()
        {
            if (colorComponentDropdown.value > 3)
                colorComponent = EColorComponent._Color2;
            else
                colorComponent = (EColorComponent)colorComponentDropdown.value;
        }
        #endregion
    }
}