using System;



namespace GameUI
{
    public interface IShowable { void Show(); }
    public interface IHideable { void Hide(); }
    public interface IDestroyable { void DestroyUI(); }
    public interface IStatusBar { void UpdateStatusBar(float value); }
    public interface IButton { void BindButtonEvents(); }


    public interface ISetValue { }
    public interface ISetValue<T>:ISetValue { void SetValue(T value); }
    public interface ISelectable<T>
    {
        void Select(T value);
    }
    public interface ISettingPage
    {
        event Action OnSettingChanged;
        void LoadSetting();
        void ApplySetting();
    }

    public interface ISettingsPage<T> : ISettingPage
    {
        void Init(T data);
        T GetSubData();
    }
   
      
}