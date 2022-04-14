using System.Reflection;
using Game.Battle.Scripts.BattleCharacter.Abstract;
using Game.Battle.Scripts.BattleCharacter.Attributes;
using UnityEditor;
using UnityEngine;

namespace Game.Battle.Scripts.BattleCharacter.Editor
{
    [CustomEditor(typeof(BattleCharacterController), true)]
    public class BattleCharacterControllerEditor : UnityEditor.Editor
    {
        private BattleCharacterController _battleCharacterController;

        private void OnEnable()
        {
            _battleCharacterController = (BattleCharacterController) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var battleCharacterControllerPrivateMethods = _battleCharacterController
                .GetType()
                .GetMethods(BindingFlags.Default | BindingFlags.Instance | BindingFlags.NonPublic);
            
            foreach (var battleCharacterControllerPrivateMethod in battleCharacterControllerPrivateMethods)
            {
                var testMethodButtonAttribute = (TestMethodButtonAttribute) battleCharacterControllerPrivateMethod
                    .GetCustomAttribute(typeof(TestMethodButtonAttribute));
                if (testMethodButtonAttribute == null) continue;
                if (GUILayout.Button(testMethodButtonAttribute.Name))
                {
                    battleCharacterControllerPrivateMethod.Invoke(target, null);
                }
            }
        }
    }
}