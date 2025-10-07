
using GameUI;
using UnityEngine;


namespace GameInteract
{
    public class PageHandler : UIPopUp
    {
        [SerializeField] UIPage[] pages;

        public override bool Init()
        {
            if (base.Init() == false) return false;
            OpenPage(0);
            return true;
        }
        void OpenPage(int index)
        {
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].gameObject.SetActive(i==index);
            }
        }
                
        #region ButtonEvent
        public void GetPage(int index) => OpenPage(index);
      
        #endregion
    }
}