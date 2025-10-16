using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BamaoUIPack.Scripts
{
    // <summary>
    // Use To Register Canvas Page
    // </summary>
    public class PageNavSwitcher : MonoBehaviour
    {
        public PageNavDropdownView PageNavDropdownView;
        public List<GameObject> PageCanvas;

        private void Start()
        {
            List<string> PageNameList = PageCanvas.Select(page => page.name).ToList();
            PageNavDropdownView.InitializeDropdown(PageNameList);
            PageNavDropdownView.OnPageChanged += PageNavDropdownViewOnOnPageChanged;
        }

        private void PageNavDropdownViewOnOnPageChanged(int index)
        {
            foreach (var page in PageCanvas)
            {
                page.SetActive(false);
            }
            
            PageCanvas[index].SetActive(true);
        }
    }
}