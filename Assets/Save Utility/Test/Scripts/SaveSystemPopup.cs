using System.Collections.Generic;
using UI_System.Scripts.Popup.Abstract;
using UnityEngine;

namespace Save_System.Test.Scripts
{
    public class SaveSystemPopup : PopupBase
    {
        [Header("Prefab")]
        [SerializeField] private TestDataView viewPrefab;
        
        [Header("Data")]
        [SerializeField] private List<TestData> tests;
        
        protected override void OnInitialized()
        {
            var contentsRectTransform = ContentsTransform.GetComponent<RectTransform>();
            var unitHeight = contentsRectTransform.sizeDelta.y;
            var totalHeight = 0f;
            foreach (var test in tests)
            {
                test.Initialize();
                
                var view = Instantiate(viewPrefab, ContentsTransform);
                totalHeight += unitHeight;
                view.Construct(test);
                view.Initialize();
            }
            
            contentsRectTransform.sizeDelta = new Vector2(
                contentsRectTransform.sizeDelta.x,
                totalHeight);
        }

        protected override void OnTerminated()
        {
            //do nothing.
        }
    }
}
