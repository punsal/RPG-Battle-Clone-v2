using System;
using Save_Utility.Scripts;
using Save_Utility.Scripts.Abstract;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Save_Utility.Test.Scripts
{
    [CreateAssetMenu(fileName = "TestData", menuName = "Save System/Tests/Create Test Data", order = 0)]
    public class TestData : SavableScriptableObject
    {
        public int id = 0;
        public new string name = "Test";
        public TestClass testClass;

        public void Initialize()
        {
           Load();
        }

        public void UpdateData()
        {
            id = Random.Range(0, 100);
            Save();
        }
        
        public override void Save()
        {
            SaveUtility.Save(this);
        }

        public override void Load()
        {
            if (SaveUtility.Load(this, out var result))
            {
                var testDataResult = (TestData) result;
                id = testDataResult.id;
                name = testDataResult.name;
                testClass = testDataResult.testClass;
            }
            else
            {
                ResetDefaults();
            }
        }

        public override void ResetDefaults()
        {
            // ReSharper disable once ConvertIfStatementToNullCoalescingAssignment
            if (testClass == null)
            {
                testClass = new TestClass();
            }
            
            Save();
        }
    }

    [Serializable]
    public class TestClass
    {
        [SerializeField] private string exampleData = "config";

        public override string ToString()
        {
            return exampleData;
        }
    }
}