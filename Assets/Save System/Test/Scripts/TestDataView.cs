using System;
using TMPro;
using UI_System.Scripts.Popup.Header;
using UnityEngine;

namespace Save_System.Test.Scripts
{
    public class TestDataView : MonoBehaviour
    {
        [Serializable]
        public struct FieldView
        {
            [SerializeField] private TextMeshProUGUI label;
            [SerializeField] private TextMeshProUGUI value;

            public void Construct(string labelText, string valueText)
            {
                label.text = labelText + " :";
                value.text = valueText;
            }
        }
        [Header("UI Elements")]
        [SerializeField] private HeaderView headerView;

        [SerializeField] private FieldView fieldId;
        [SerializeField] private FieldView fieldName;
        [SerializeField] private FieldView fieldTestClass;

        private TestData _testData;

        private RectTransform _rectTransform;

        protected RectTransform RectTransform => _rectTransform == null 
            ? _rectTransform = GetComponent<RectTransform>() 
            : _rectTransform;

        public float Height => RectTransform.sizeDelta.y;

        public void Construct(TestData data)
        {
            _testData = data;
        }
        
        public void Initialize()
        {
            headerView.SetText(_testData.name);
            fieldId.Construct("Id", _testData.id.ToString());
            fieldName.Construct("Name", _testData.name);
            fieldTestClass.Construct("TestClass", _testData.testClass.ToString());
        }

        public void UpdateData()
        {
            _testData.UpdateData();
            Initialize();
        }
    }
}
