using System;

namespace Game.Battle.Scripts.BattleCharacter.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TestMethodButtonAttribute : Attribute
    {
        public string Name { get; }

        public TestMethodButtonAttribute(string name)
        {
            this.Name = name;
        }
    }
}