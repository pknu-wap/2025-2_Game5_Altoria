using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public enum GameScreenMode
{
    Fullscreen=0,
    Windowed=1
}

public class SettingData : MonoBehaviour
{
    public static SettingData Instance;
    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    [Serializable]
    public class KeyBindingData
    {
        public string actionName;  // 예: "MoveForward"
        public KeyCode key;        // 예 : W
        public KeyCode secondaryKey; // 예 : UpArrow (선택 사항)     // 현재 wasd, 상하좌우 키 둘 다 사용중인 것 같아서 action당 key를 두개 할당

        public KeyBindingData(string actionName, KeyCode key, KeyCode secondary = KeyCode.None)
        {
            this.actionName = actionName;
            this.key = key;
            this.secondaryKey = secondary;
        }
    }

    public float BGMVolume = 0.8f;
    public float SFXVolume = 0.8f;
    public GameScreenMode ScreenMode = GameScreenMode.Fullscreen;
    public int QualityLevel = 2;
    public float MouseSensitivity = 0.5f;  //cameracontroller의 sensitivity와 매칭

    [SerializeField] List<KeyBindingData> keyBindingList = new List<KeyBindingData>();

    public SettingData()
    {
        keyBindingList = new List<KeyBindingData>
        {
            new KeyBindingData("MoveForward", KeyCode.W, KeyCode.UpArrow),    // 두가지 키 할당
            new KeyBindingData("MoveBackward", KeyCode.S, KeyCode.DownArrow), // 두가지 키 할당
            new KeyBindingData("MoveLeft", KeyCode.A, KeyCode.LeftArrow),     // 두가지 키 할당
            new KeyBindingData("MoveRight", KeyCode.D, KeyCode.RightArrow),   // 두가지 키 할당
            new KeyBindingData("Jump", KeyCode.Space),
            new KeyBindingData("Interact", KeyCode.Mouse0),
            new KeyBindingData("Inventory", KeyCode.I),
            new KeyBindingData("Craft", KeyCode.C),
            new KeyBindingData("Upgrade", KeyCode.G),
            new KeyBindingData("Escape", KeyCode.Escape)
        };
    }

    // key down 가이드라인
    // example)
    // Input.GetKey(SettingData.GetKey("MoveForward"))
    //
    public KeyCode GetKey(string actionName)
    {
        var entry = keyBindingList.Find(k => k.actionName == actionName);
        if (entry == null) return KeyCode.None;
        return entry.key;
    }

    public List<KeyBindingData> GetkeyBindingList()
    {
        return keyBindingList;
    }

    public void ResetToDefault()
    {
        BGMVolume = 0.8f;
        SFXVolume = 0.8f;
        ScreenMode = GameScreenMode.Fullscreen;
        QualityLevel = 2;
        MouseSensitivity = 0.5f;

        keyBindingList.Clear();
        keyBindingList = new List<KeyBindingData>
        {
            new KeyBindingData("MoveForward", KeyCode.W, KeyCode.UpArrow),
            new KeyBindingData("MoveBackward", KeyCode.S, KeyCode.DownArrow),
            new KeyBindingData("MoveLeft", KeyCode.A, KeyCode.LeftArrow),
            new KeyBindingData("MoveRight", KeyCode.D, KeyCode.RightArrow),
            new KeyBindingData("Jump", KeyCode.Space),
            new KeyBindingData("Interact", KeyCode.Mouse0),
            new KeyBindingData("Inventory", KeyCode.I),
            new KeyBindingData("Craft", KeyCode.C),
            new KeyBindingData("Upgrade", KeyCode.G),
            new KeyBindingData("Escape", KeyCode.Escape)
        };
    }

    public bool ChangeKey(string actionName, KeyCode newKey, bool isPrimary = true)
    {
        var entry = keyBindingList.Find(k => k.actionName == actionName);
        if (entry != null)
        {
            if(isPrimary) entry.key = newKey;
            else entry.secondaryKey = newKey;
            Debug.Log($"[SettingData] : {actionName} -> {(isPrimary ? "주" : "보조")}키: {newKey}");
            return true;
        }
        return false;
    }

    #region Set
    public void SetScreenMode(GameScreenMode mode)
    {
        if (mode == GameScreenMode.Fullscreen)
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else
            Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    public void SetQuality(int level)
    {
        QualityLevel = Mathf.Clamp(level, 0, QualitySettings.names.Length - 1);
        QualitySettings.SetQualityLevel(QualityLevel);
    }
    public void SetMouseSensitivity(float sensitivity)
    {
        MouseSensitivity = Mathf.Clamp01(sensitivity);
    }

    // set bgm ? set sfx? soundmanager에서만 관리할지 

    #endregion

    #region get
    public float GetMouseSensitivity() => MouseSensitivity;
    #endregion


    // Save & Load는 SettingManager에서 처리?
}