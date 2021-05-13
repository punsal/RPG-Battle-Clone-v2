using System;
using Save_System.Scripts;
using Save_System.Scripts.Abstract;
using UnityEngine;

namespace Save_System.Test.Scripts
{
    [CreateAssetMenu(fileName = "TestData", menuName = "Save System/Tests/Create Test Data", order = 0)]
    public class TestData : SavableScriptableObject
    {
        public int id = 0;
        public new string name = "Test";
        public TestClass testClass;
        
        public override void Save()
        {
            SaveSystem.Save(this);
        }

        public override bool Load(out object result)
        {
            result = null;
            return false;
        }
    }

    [Serializable]
    public class TestClass
    {
        public string exampleData = "config";
    }
}