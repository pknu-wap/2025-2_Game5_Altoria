using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BamaoUIPack.Scripts
{
    // <summary>
    // Use To Navigation Dropdown Page
    // </summary>
    public class PageNavDropdownView : MonoBehaviour
    {
        public TMP_Dropdown PageDropdown;
        public int NumberOfPages = 1; 
        public int CurrentPage = 0;
        public Button NextButton;
        public Button PreviousButton;

        public event Action<int> OnPageChanged;
        public event Action<int> NextPageAction;
        public event Action<int> PreviousPageAction;

        private void Start()
        {
            PageDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            NextButton.onClick.AddListener(OnNextButtonClicked);
            PreviousButton.onClick.AddListener(OnPreviousButtonClicked);

            UpdateButtonStates();
        }

        public void InitializeDropdown(List<string> pageNameList)   
        {
            NumberOfPages = pageNameList.Count;
            PageDropdown.options.Clear();
            
            foreach (var page in pageNameList)
            {
                PageDropdown.options.Add(new TMP_Dropdown.OptionData(page));
            }

            CurrentPage = 0;
            PageDropdown.value = CurrentPage;
            PageDropdown.captionText.text = PageDropdown.options[CurrentPage].text;
        }

        private void OnDropdownValueChanged(int index)
        {
            CurrentPage = index;
            OnPageChanged?.Invoke(CurrentPage);
            UpdateButtonStates();
        }

        public void OnNextButtonClicked()
        {
            if (CurrentPage < NumberOfPages - 1)
            {
                CurrentPage++;
                PageDropdown.value = CurrentPage;
                NextPageAction?.Invoke(CurrentPage);
            }
        }

        public void OnPreviousButtonClicked()
        {
            if (CurrentPage > 0)
            {
                CurrentPage--;
                PageDropdown.value = CurrentPage;
                PreviousPageAction?.Invoke(CurrentPage);
            }
        }

        private void UpdateButtonStates()
        {
            PreviousButton.interactable = CurrentPage > 0;
            NextButton.interactable = CurrentPage < NumberOfPages - 1;
        }

        public void AddNewPage(string pageName = null)
        {
            NumberOfPages++;
            string newPageName = pageName ?? $"Page {NumberOfPages}";
            PageDropdown.options.Add(new TMP_Dropdown.OptionData(newPageName));
            UpdateButtonStates();
        }

        public void RemovePage(int pageIndex)
        {
            if (pageIndex < 0 || pageIndex >= NumberOfPages)
            {
                Debug.LogWarning("Page index out of bounds");
                return;
            }

            PageDropdown.options.RemoveAt(pageIndex);
            NumberOfPages--;

            if (CurrentPage == pageIndex && CurrentPage > 0)
            {
                CurrentPage--;
            }

            PageDropdown.value = CurrentPage;
            OnPageChanged?.Invoke(CurrentPage);
            UpdateButtonStates();
        }

        public void SetPage(int page, bool isTriggerAction = false)
        {
            if (page < 0 || page >= NumberOfPages)
            {
                Debug.LogWarning("Page index out of bounds");
                return;
            }

            CurrentPage = page;
            PageDropdown.value = CurrentPage;
            UpdateButtonStates();

            if(isTriggerAction)
                OnPageChanged?.Invoke(CurrentPage);
        }
    }
}